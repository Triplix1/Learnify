import json
import threading
import time

import pika

from Services.audioService import extract_audio
from Services.blobService import download_video_from_blob, upload_to_blob
from Services.subtitlesService import generate_subtitles
from Services.translateService import translate_subtitles, translate_transcription

# Load RabbitMQ Configuration (same as C# app)
RABBITMQ_HOST = "localhost"
RABBITMQ_USERNAME = "guest"
RABBITMQ_PASSWORD = "guest"
RABBITMQ_QUEUE = "subtitles-generate-request-event"  # Follow MassTransit naming conventions
RABBITMQ_QUEUE_RESPONSE = "main-service-subtitles-generated-response"  # Define response queue

# Azure Storage Configuration
SUBTITLES_CONTAINER = "learnify-subtitles"
TRANSCRIPTIONS_CONTAINER = "learnify-transcriptions"


def establish_rabbitmq_connection():
    """Establishes a RabbitMQ connection with retries."""
    attempt = 0
    while attempt < 5:
        try:
            credentials = pika.PlainCredentials(RABBITMQ_USERNAME, RABBITMQ_PASSWORD)
            connection = pika.BlockingConnection(
                pika.ConnectionParameters(host=RABBITMQ_HOST, credentials=credentials, virtual_host="/")
            )
            return connection
        except pika.exceptions.AMQPConnectionError as e:
            print(f"RabbitMQ connection failed: {e}. Retrying in 10 seconds...")
            attempt += 1
            time.sleep(10)
    raise Exception("Failed to connect to RabbitMQ after multiple attempts.")


def publish_subtitles_generated_response(subtitle_id, subtitle_blob_name, transcription_blob_name):
    """Publishes a MassTransit-compatible SubtitlesGeneratedResponse message to RabbitMQ."""

    response_message = {
        "message": {  # ✅ Wrap inside "message" key for MassTransit
            "SubtitleId": subtitle_id,
            "SubtitleFileInfo": {
                "ContentType": "text/vtt",
                "ContainerName": SUBTITLES_CONTAINER,
                "BlobName": subtitle_blob_name
            },
            "TranscriptionFileInfo": {
                "ContentType": "text/plain",
                "ContainerName": TRANSCRIPTIONS_CONTAINER,
                "BlobName": transcription_blob_name
            },
        },
        "messageType": ["urn:message:Learnify.Contracts:SubtitlesGeneratedResponse"]
    }

    try:
        channel = RABBITMQ_CONNECTION.channel()

        # ✅ Declare the exchange and bind to the queue
        exchange_name = "main-service-subtitles-generated-response"  # ✅ Use MassTransit naming

        # ✅ Publish the message to the correct exchange
        properties = pika.BasicProperties(
            delivery_mode=2,  # Persistent message
            headers={
                "messageType": ["urn:message:Learnify.Contracts:SubtitlesGeneratedResponse"]
            }
        )

        # ✅ Publish the message to the correct exchange
        channel.basic_publish(
            exchange=exchange_name,
            routing_key="",  # Fanout exchange ignores this
            body=json.dumps(response_message),
            properties=properties
        )
        print(f"✅ Published SubtitlesGeneratedResponse: {response_message}")

    except pika.exceptions.AMQPConnectionError as e:
        print(f"❌ RabbitMQ Connection Error: {str(e)}")
    except Exception as e:
        print(f"❌ Error Publishing to RabbitMQ: {str(e)}")

def process_video_message(message):
    """Processes a RabbitMQ message for generating subtitles and translations."""
    try:
        msg_data = json.loads(message)

        # ✅ Extract actual message payload
        if "message" in msg_data:
            msg_data = msg_data["message"]

        video_container = msg_data["videoContainerName"]
        video_blob_name = msg_data["videoBlobName"]
        primary_language = msg_data["primaryLanguage"]
        subtitle_info = msg_data["subtitleInfo"]  # List of SubtitleInfo objects

        print(f"Processing subtitles for video: {video_blob_name}")

        # ✅ Find the SubtitleInfo where language matches primaryLanguage
        primary_subtitle = next(
            (s for s in subtitle_info if s["language"] == primary_language),
            None
        )

        if primary_subtitle is None:
            raise ValueError(f"No SubtitleInfo found for primary language: {primary_language}")

        # ✅ Download video
        print("Downloading video from Azure Blob Storage...")
        video_path = download_video_from_blob(video_container, video_blob_name)

        # ✅ Extract audio
        print("Extracting audio from video...")
        audio_path = extract_audio(video_path)

        # ✅ Generate primary language subtitles & transcription
        print(f"Generating primary subtitles in {primary_language}...")
        original_srt, original_txt, segments = generate_subtitles(audio_path, primary_language)

        # ✅ Upload primary language subtitles & transcription
        subtitle_blob_name = video_blob_name + f"_{primary_language}.vtt"
        transcription_blob_name = video_blob_name + f"_{primary_language}.txt"

        upload_to_blob(SUBTITLES_CONTAINER, original_srt, subtitle_blob_name)
        upload_to_blob(TRANSCRIPTIONS_CONTAINER, original_txt, transcription_blob_name)

        # ✅ Publish primary language subtitles response
        publish_subtitles_generated_response(
            primary_subtitle["id"], subtitle_blob_name, transcription_blob_name
        )

        # ✅ Translate subtitles & transcriptions into other languages
        for subtitle in subtitle_info:
            target_language = subtitle["language"]
            subtitle_id = subtitle["id"]

            if target_language != primary_language:
                print(f"Translating subtitles & transcription to {target_language} (ID: {subtitle_id})...")
                translated_srt = translate_subtitles(segments, target_language)
                translated_txt = translate_transcription(original_txt, target_language)

                # Upload translated subtitles & transcription
                translated_srt_blob_name = video_blob_name.replace(".mp4", f"_{target_language}.srt")
                translated_txt_blob_name = video_blob_name.replace(".mp4", f"_{target_language}.txt")

                upload_to_blob(SUBTITLES_CONTAINER, translated_srt, translated_srt_blob_name)
                upload_to_blob(TRANSCRIPTIONS_CONTAINER, translated_txt, translated_txt_blob_name)

                # ✅ Publish translated subtitles response
                publish_subtitles_generated_response(subtitle_id, translated_srt_blob_name, translated_txt_blob_name)

        print(f"✅ Subtitles processing completed for {video_blob_name}")

    except Exception as e:
        print(f"❌ Error processing video message: {str(e)}")

def rabbitmq_callback(ch, method, properties, body):
    """Callback function for RabbitMQ message consumption."""
    print("Received message from RabbitMQ")
    try:
        process_video_message(body.decode())
        ch.basic_ack(delivery_tag=method.delivery_tag)  # ✅ Acknowledge message
    except Exception as e:
        print(f"❌ Error processing message: {e}")
        ch.basic_nack(delivery_tag=method.delivery_tag, requeue=True)  # ✅ Requeue message if error occurs


def listen_to_rabbitmq():
    """Connects to RabbitMQ and listens for messages without closing connection."""
    connection = establish_rabbitmq_connection()
    channel = connection.channel()
    channel.queue_declare(queue=RABBITMQ_QUEUE, durable=True)

    # ✅ Use manual acknowledgment to keep connection active
    channel.basic_consume(queue=RABBITMQ_QUEUE, on_message_callback=rabbitmq_callback, auto_ack=False)

    print("Listening for messages from RabbitMQ...")
    try:
        channel.start_consuming()
    except KeyboardInterrupt:
        print("❌ Stopping RabbitMQ listener.")
        channel.stop_consuming()
    except Exception as e:
        print(f"❌ Error in RabbitMQ Consumer: {e}")

def send_heartbeat():
    """Sends a periodic heartbeat to keep the RabbitMQ connection alive."""
    while True:
        try:
            if RABBITMQ_CONNECTION and not RABBITMQ_CONNECTION.is_closed:
                RABBITMQ_CONNECTION.process_data_events()  # ✅ Ping RabbitMQ
            time.sleep(30)  # ✅ Send heartbeat every 30 seconds
        except Exception as e:
            print(f"❌ Heartbeat failed: {e}")

def get_rabbitmq_connection():
    """Creates a persistent RabbitMQ connection and auto-reconnects if needed."""
    global RABBITMQ_CONNECTION

    while True:
        try:
            credentials = pika.PlainCredentials(RABBITMQ_USERNAME, RABBITMQ_PASSWORD)
            parameters = pika.ConnectionParameters(
                host=RABBITMQ_HOST,
                credentials=credentials,
                virtual_host="/",
                heartbeat=600,  # ✅ Keeps connection alive
                blocked_connection_timeout=300
            )
            connection = pika.BlockingConnection(parameters)
            print("✅ RabbitMQ Connected Successfully!")

            # ✅ Start Keep-Alive Thread
            threading.Thread(target=send_heartbeat, daemon=True).start()

            return connection
        except pika.exceptions.AMQPConnectionError as e:
            print(f"❌ RabbitMQ Connection Failed: {e}. Retrying in 5 seconds...")
            time.sleep(5)

if __name__ == "__main__":
    global RABBITMQ_CONNECTION  # ✅ Declare `global` before using it
    RABBITMQ_CONNECTION = get_rabbitmq_connection()
    listen_to_rabbitmq()

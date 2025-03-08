import json
import os
import threading
import time

import pika

from Services.azureService import get_sas_url, upload_to_blob
from Services.fileService import generate_temporary_file
from Services.gemini import get_summary

# Load RabbitMQ Configuration (same as C# app)
RABBITMQ_HOST = "localhost"
RABBITMQ_USERNAME = "guest"
RABBITMQ_PASSWORD = "guest"
RABBITMQ_QUEUE = "summary-generate-request-event"  # Follow MassTransit naming conventions
RABBITMQ_QUEUE_RESPONSE = "main-service-summary-generated-response"  # Define response queue

# Azure Storage Configuration
SUMMARIES_CONTAINER = "learnify-summaries"


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


def publish_summary_generated_response(file_id, subtitle_blob_name):
    """Publishes a MassTransit-compatible SubtitlesGeneratedResponse message to RabbitMQ."""

    response_message = {
        "message": {  # ✅ Wrap inside "message" key for MassTransit
            "FileId": file_id,
            "SummaryFileInfo": {
                "ContentType": "text/plain",
                "ContainerName": SUMMARIES_CONTAINER,
                "BlobName": subtitle_blob_name
            },
        },
        "messageType": ["urn:message:Learnify.Contracts:SummaryGeneratedResponse"]
    }

    try:
        channel = RABBITMQ_CONNECTION.channel()

        # ✅ Declare the exchange and bind to the queue
        exchange_name = "main-service-summary-generated-response"  # ✅ Use MassTransit naming

        # ✅ Publish the message to the correct exchange
        channel.basic_publish(
            exchange=exchange_name,
            routing_key="",  # Fanout exchange ignores this
            body=json.dumps(response_message)
        )
        print(f"✅ Published SummaryGeneratedResponse: {response_message}")

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

        file_id = msg_data["fileId"]
        video_container = msg_data["videoContainerName"]
        video_blob_name = msg_data["videoBlobName"]
        content_type = msg_data["contentType"]
        language = msg_data["language"]

        print(f"Processing subtitles for video: {video_blob_name}")


        # ✅ Download video
        print("generating sas url:")
        video_path = get_sas_url(video_container, video_blob_name)

        # ✅ Extract audio
        print("generating summary:")
        summary = get_summary(video_path, content_type, language)

        # ✅ Upload primary language subtitles & transcription
        summary_blob_name = video_blob_name + f"summary.txt"

        temp_file_path = generate_temporary_file(summary, summary_blob_name)

        upload_to_blob(SUMMARIES_CONTAINER, temp_file_path, summary_blob_name)

        # ✅ Publish primary language subtitles response
        publish_summary_generated_response(
            file_id, summary_blob_name
        )
        print(f"✅ Summary processing completed for {video_blob_name}")

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

﻿import json
import os
import time

import pika

from Services.assemblyService import generate_subtitles
from Services.audioService import extract_audio
from Services.blobService import download_video_from_blob, upload_to_blob
from dotenv import load_dotenv

load_dotenv()

RABBITMQ_HOST = os.getenv("RABBITMQ_HOST")
RABBITMQ_USERNAME = os.getenv("RABBITMQ_USERNAME")
RABBITMQ_PASSWORD = os.getenv("RABBITMQ_PASSWORD")
RABBITMQ_QUEUE = os.getenv("RABBITMQ_SUBTITLES_QUEUE")
RABBITMQ_QUEUE_RESPONSE = os.getenv("RABBITMQ_SUBTITLES_QUEUE_RESPONSE")
RABBITMQ_VIRTUAL_HOST = os.getenv("RABBITMQ_VIRTUAL_HOST")

SUBTITLES_CONTAINER = os.getenv("SUBTITLES_CONTAINER")

def publish_subtitles_generated_response(lesson_id, subtitle_id, subtitle_blob_name):

    response_message = {
        "message": {
            "LessonId": lesson_id,
            "SubtitleId": subtitle_id,
            "SubtitleFileInfo": {
                "ContentType": "text/vtt",
                "ContainerName": SUBTITLES_CONTAINER,
                "BlobName": subtitle_blob_name
            },
        },
        "messageType": ["urn:message:Learnify.Contracts:SubtitlesGeneratedResponse"]
    }

    properties = pika.BasicProperties(
        delivery_mode=2,
        headers={
            "messageType": ["urn:message:Learnify.Contracts:SubtitlesGeneratedResponse"]
        }
    )

    channel = rabbitmq_connection.channel()

    channel.basic_publish(
        exchange=RABBITMQ_QUEUE_RESPONSE,
        routing_key="",
        body=json.dumps(response_message),
        properties=properties
    )
    print(f"Published SubtitlesGeneratedResponse: {response_message}")

def process_video_message(message):
    video_file_path = None
    audio_file_path = None
    subtitles_file = None

    try:
        msg_data = json.loads(message)

        if "message" in msg_data:
            msg_data = msg_data["message"]

        lesson_id = msg_data["lessonId"]
        video_container = msg_data["videoContainerName"]
        video_blob_name = msg_data["videoBlobName"]
        subtitle_info = msg_data["subtitleInfo"]

        print(f"Received message. Start generating subtitles for: {video_blob_name}")

        video_file_path = download_video_from_blob(video_container, video_blob_name)

        audio_file_path = extract_audio(video_file_path)

        subtitles_file = generate_subtitles(audio_file_path, subtitle_info["language"])
        subtitle_blob_name = video_blob_name + f"_{lesson_id}" + f"_{subtitle_info['language']}.vtt"

        upload_to_blob(SUBTITLES_CONTAINER, subtitles_file, subtitle_blob_name)

        publish_subtitles_generated_response(lesson_id, subtitle_info["id"], subtitle_blob_name)

        print(f"Subtitles successfully generated for: {video_blob_name}")

    except Exception as e:
        print(f"Subtitles generation failed: {str(e)}")

    finally:
        for file_path in [video_file_path, audio_file_path, subtitles_file]:
            if file_path and os.path.exists(file_path):
                try:
                    os.remove(file_path)
                    print(f"Deleted temp file: {file_path}")
                except Exception as err:
                    print(f"Failed to delete {file_path}: {err}")

def rabbitmq_callback(ch, method, properties, body):
    print("Received message from RabbitMQ")
    try:
        process_video_message(body.decode())
        ch.basic_ack(delivery_tag=method.delivery_tag)
    except Exception as e:
        print(f"Error processing message: {e}")
        ch.basic_nack(delivery_tag=method.delivery_tag, requeue=True)

def establish_rabbitmq_connection():
    attempt = 0
    global rabbitmq_connection
    while attempt < 5:
        try:
            credentials = pika.PlainCredentials(RABBITMQ_USERNAME, RABBITMQ_PASSWORD)
            connection = pika.BlockingConnection(
                pika.ConnectionParameters(host=RABBITMQ_HOST, credentials=credentials, virtual_host=RABBITMQ_VIRTUAL_HOST)
            )
            rabbitmq_connection = connection
            return
        except pika.exceptions.AMQPConnectionError as e:
            print(f"RabbitMQ connection failed: {e}. Retrying in 10 seconds...")
            attempt += 1
            time.sleep(10)
    raise Exception("Failed to connect to RabbitMQ after multiple attempts.")

def listen_to_rabbitmq():
    establish_rabbitmq_connection()
    channel = rabbitmq_connection.channel()
    channel.queue_declare(queue=RABBITMQ_QUEUE, durable=True)

    channel.basic_consume(queue=RABBITMQ_QUEUE, on_message_callback=rabbitmq_callback, auto_ack=False)

    print("Listening for messages from RabbitMQ...")
    try:
        channel.start_consuming()
    except KeyboardInterrupt:
        print("Stopping RabbitMQ listener.")
        channel.stop_consuming()

    return channel

if __name__ == "__main__":
    global rabbitmq_connection
    listen_to_rabbitmq()



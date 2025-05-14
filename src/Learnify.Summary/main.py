import json
import os
import time

import pika

from Services.azureService import get_sas_url, upload_to_blob
from Services.fileService import generate_temporary_file
from Services.gemini import get_summary
from dotenv import load_dotenv

load_dotenv()

RABBITMQ_HOST = os.getenv("RABBITMQ_HOST")
RABBITMQ_USERNAME = os.getenv("RABBITMQ_USERNAME")
RABBITMQ_PASSWORD = os.getenv("RABBITMQ_PASSWORD")
RABBITMQ_QUEUE = os.getenv("RABBITMQ_SUMMARIES_QUEUE")
RABBITMQ_QUEUE_RESPONSE = os.getenv("RABBITMQ_SUMMARIES_QUEUE_RESPONSE")
SUMMARIES_CONTAINER = os.getenv("SUMMARIES_CONTAINER")
RABBITMQ_VIRTUAL_HOST = os.getenv("RABBITMQ_VIRTUAL_HOST")

def publish_summary_generated_response(file_id, subtitle_blob_name):

    response_message = {
        "message": {
            "FileId": file_id,
            "SummaryFileInfo": {
                "ContentType": "text/plain",
                "ContainerName": SUMMARIES_CONTAINER,
                "BlobName": subtitle_blob_name
            },
        },
        "messageType": ["urn:message:Learnify.Contracts:SummaryGeneratedResponse"]
    }

    channel = rabbit_mq_connection.channel()

    channel.basic_publish(
        exchange=RABBITMQ_QUEUE_RESPONSE,
        routing_key="",
        body=json.dumps(response_message)
    )
    print(f"Published SummaryGeneratedResponse: {response_message}")

def process_video_message(message):
    temporary_summary_file_path = None

    try:
        msg_data = json.loads(message)

        if "message" in msg_data:
            msg_data = msg_data["message"]

        file_id = msg_data["fileId"]
        video_container = msg_data["videoContainerName"]
        video_blob_name = msg_data["videoBlobName"]
        content_type = msg_data["contentType"]
        language = msg_data["language"]

        print(f"Processing subtitles for video: {video_blob_name}")

        video_url = get_sas_url(video_container, video_blob_name)

        resulted_summary = get_summary(video_url, content_type, language)

        summary_blob_name = video_blob_name + f"_{language}_{file_id}" + f"summary.txt"

        temporary_summary_file_path = generate_temporary_file(resulted_summary, summary_blob_name)

        upload_to_blob(SUMMARIES_CONTAINER, temporary_summary_file_path, summary_blob_name)

        publish_summary_generated_response(file_id, summary_blob_name)
        print(f"Summary processing completed for {video_blob_name}")

    except Exception as e:
        print(f"Error while summary generation: {str(e)}")

    finally:
        if temporary_summary_file_path and os.path.exists(temporary_summary_file_path):
            try:
                os.remove(temporary_summary_file_path)
                print(f"Deleted temp file: {temporary_summary_file_path}")
            except Exception as err:
                print(f"Failed to delete {temporary_summary_file_path}: {err}")



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
    while attempt < 5:
        try:
            global rabbit_mq_connection
            credentials = pika.PlainCredentials(RABBITMQ_USERNAME, RABBITMQ_PASSWORD)
            connection = pika.BlockingConnection(
                pika.ConnectionParameters(host=RABBITMQ_HOST, credentials=credentials, virtual_host=RABBITMQ_VIRTUAL_HOST)
            )
            rabbit_mq_connection = connection
            return
        except pika.exceptions.AMQPConnectionError as e:
            print(f"RabbitMQ connection failed: {e}. Retrying in 10 seconds...")
            attempt += 1
            time.sleep(10)
    raise Exception("Failed to connect to RabbitMQ after multiple attempts.")


def listen_to_rabbitmq():
    establish_rabbitmq_connection()
    channel = rabbit_mq_connection.channel()
    channel.queue_declare(queue=RABBITMQ_QUEUE, durable=True)

    channel.basic_consume(queue=RABBITMQ_QUEUE, on_message_callback=rabbitmq_callback, auto_ack=False)

    print("Listening for messages from RabbitMQ...")
    try:
        channel.start_consuming()
    except KeyboardInterrupt:
        print("Stopping RabbitMQ listener.")
        channel.stop_consuming()
    except Exception as e:
        print(f"Error in RabbitMQ Consumer: {e}")

if __name__ == "__main__":
    global rabbit_mq_connection
    listen_to_rabbitmq()

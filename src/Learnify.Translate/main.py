import json
import mimetypes
import os
import time
import pika
from Services.blobService import upload_to_blob, download_blob_text
from Services.translateService import translate
from dotenv import load_dotenv

load_dotenv()

RABBITMQ_HOST = os.getenv("RABBITMQ_HOST")
RABBITMQ_USERNAME = os.getenv("RABBITMQ_USERNAME")
RABBITMQ_PASSWORD = os.getenv("RABBITMQ_PASSWORD")
RABBITMQ_QUEUE = os.getenv("RABBITMQ_TRANSLATE_QUEUE")
RABBITMQ_QUEUE_RESPONSE = os.getenv("RABBITMQ_TRANSLATE_QUEUE_RESPONSE")
RABBITMQ_VIRTUAL_HOST = os.getenv("RABBITMQ_VIRTUAL_HOST")

def publish_translated_response(file_id, content_type, file_container_name, file_blob_name):

    response_message = {
        "message": {
            "FileId": file_id,
            "FileInfo": {
                "ContentType": content_type,
                "ContainerName": file_container_name,
                "BlobName": file_blob_name
            },
        },
        "messageType": ["urn:message:Learnify.Contracts:TranslatedResponse"]
    }

    try:
        channel = rabbit_mq_connection.channel()

        channel.basic_publish(
            exchange=RABBITMQ_QUEUE_RESPONSE,
            routing_key="",
            body=json.dumps(response_message)
        )
        print(f"Published TranslatedResponse: {response_message}")

    except pika.exceptions.AMQPConnectionError as e:
        print(f"RabbitMQ Connection Error: {str(e)}")
    except Exception as e:
        print(f"Error Publishing to RabbitMQ: {str(e)}")


def process_translate_request_message(message):
    try:
        msg_data = json.loads(message)

        if "message" in msg_data:
            msg_data = msg_data["message"]

        main_file_container_name = msg_data["mainFileContainerName"]
        main_file_blob_name = msg_data["mainFileBlobName"]
        main_language = msg_data["mainLanguage"]
        translate_requests = msg_data["translateRequests"]
        content_type = get_content_type(main_file_blob_name)

        print(f"Processing translation for file: {main_file_blob_name}")

        print("Downloading text from Azure Blob Storage...")
        original_text = download_blob_text(main_file_container_name, main_file_blob_name)

        for request in translate_requests:
            target_language = request["language"]
            file_id = request["fileId"]
            print("Translating text...")
            translated_text = translate(original_text, main_language, target_language, content_type)

            translated_file_path = insert_unique_before_extension(main_file_blob_name, target_language, file_id)
            with open(translated_file_path, "w", encoding="utf-8") as translated_file:
                translated_file.write(translated_text)

            translated_blob_name = translated_file_path
            upload_to_blob(main_file_container_name, translated_blob_name, translated_file_path)
            os.remove(translated_file_path)
            publish_translated_response(file_id, content_type, main_file_container_name, translated_blob_name)
            print(f"Uploaded translated file: {translated_blob_name}")

        print(f"translation processing completed for {main_file_blob_name}")

    except Exception as e:
        print(f"Error: {str(e)}")


def get_content_type(file_path):
    mime_type, _ = mimetypes.guess_type(file_path)
    return mime_type


def insert_unique_before_extension(filename, language, file_id):
    name_parts = filename.rsplit(".", 1)
    if len(name_parts) == 2:
        return f"{name_parts[0]}_{language}_{file_id}.{name_parts[1]}"
    return f"{filename}_{language}"  # In case there's no extension


def establish_rabbitmq_connection():
    attempt = 0
    global rabbit_mq_connection
    while attempt < 5:
        try:
            print(f"Adjusting the RabbitMq connection")
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


def rabbitmq_callback(ch, method, properties, body):
    print("Received message from RabbitMQ")
    try:
        process_translate_request_message(body.decode())
        ch.basic_ack(delivery_tag=method.delivery_tag)
    except Exception as e:
        print(f"Error processing message: {e}")
        ch.basic_nack(delivery_tag=method.delivery_tag, requeue=True)


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

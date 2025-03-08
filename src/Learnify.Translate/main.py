import json
import mimetypes
import os
import threading
import time

import pika

from Services.blobService import upload_to_blob, download_blob_text

from Services.translateService import translate

# Load RabbitMQ Configuration (same as C# app)
RABBITMQ_HOST = "localhost"
RABBITMQ_USERNAME = "guest"
RABBITMQ_PASSWORD = "guest"
RABBITMQ_QUEUE = "translate-request-event"  # Follow MassTransit naming conventions
RABBITMQ_QUEUE_RESPONSE = "main-service-translated-response"  # Define response queue

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


def publish_translated_response(file_id, content_type, file_container_name, file_blob_name):

    response_message = {
        "message": {  # ✅ Wrap inside "message" key for MassTransit
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
        channel = RABBITMQ_CONNECTION.channel()

        # ✅ Declare the exchange and bind to the queue
        exchange_name = "main-service-file-translated"  # ✅ Use MassTransit naming

        # ✅ Publish the message to the correct exchange
        channel.basic_publish(
            exchange=exchange_name,
            routing_key="",  # Fanout exchange ignores this
            body=json.dumps(response_message)
        )
        print(f"✅ Published TranslatedResponse: {response_message}")

    except pika.exceptions.AMQPConnectionError as e:
        print(f"❌ RabbitMQ Connection Error: {str(e)}")
    except Exception as e:
        print(f"❌ Error Publishing to RabbitMQ: {str(e)}")

def process_translate_request_message(message):
    try:
        msg_data = json.loads(message)

        # ✅ Extract actual message payload
        if "message" in msg_data:
            msg_data = msg_data["message"]

        main_file_container_name = msg_data["mainFileContainerName"]
        main_file_blob_name = msg_data["mainFileBlobName"]
        main_language = msg_data["mainLanguage"]
        translate_requests = msg_data["translateRequests"]  # List of SubtitleInfo objects
        content_type = get_content_type(main_file_blob_name)

        print(f"Processing translation for file: {main_file_blob_name}")

        # ✅ Download file
        print("Downloading text from Azure Blob Storage...")
        original_text = download_blob_text(main_file_container_name, main_file_blob_name)

        # ✅ Process translation requests
        for request in translate_requests:
            target_language = request["language"]
            print("Translating text...")
            translated_text = translate(original_text, main_language, target_language, content_type)

            # Save translated file
            translated_file_path = insert_language_before_extension(main_file_blob_name, request["language"])
            with open(translated_file_path, "w", encoding="utf-8") as translated_file:
                translated_file.write(translated_text)

            # ✅ Upload translated file back to Azure Blob Storage
            translated_blob_name = translated_file_path
            upload_to_blob(main_file_container_name, translated_blob_name, translated_file_path)
            os.remove(translated_file_path)
            publish_translated_response(request["fileId"], content_type, main_file_container_name, translated_blob_name)
            print(f"✅ Uploaded translated file: {translated_blob_name}")

        print(f"✅ translation processing completed for {main_file_blob_name}")

    except Exception as e:
        print(f"❌ Error: {str(e)}")

def get_content_type(file_path):
    """Returns the MIME type of a given file."""
    mime_type, _ = mimetypes.guess_type(file_path)
    return mime_type


def insert_language_before_extension(filename, language):
    name_parts = filename.rsplit(".", 1)
    if len(name_parts) == 2:
        return f"{name_parts[0]}_{language}.{name_parts[1]}"
    return f"{filename}_{language}"  # In case there's no extension


def rabbitmq_callback(ch, method, properties, body):
    """Callback function for RabbitMQ message consumption."""
    print("Received message from RabbitMQ")
    try:
        process_translate_request_message(body.decode())
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

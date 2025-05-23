﻿import os
import tempfile
from azure.storage.blob import BlobServiceClient
from dotenv import load_dotenv

load_dotenv()

AZURE_CONNECTION_STRING = os.getenv("AZURE_CONNECTION_STRING")

def download_video_from_blob(container_name, blob_name):
    print("Downloading video")
    temp_video_path = tempfile.NamedTemporaryFile(delete=False, suffix=".mp4").name
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING)
    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)

    with open(temp_video_path, "wb") as video_file:
        video_file.write(blob_client.download_blob().readall())

    return temp_video_path


def upload_to_blob(container_name, file_path, blob_name):
    print(f"Uploading subtitle: {blob_name}")
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING, api_version="2021-06-08")

    container_client = blob_service_client.get_container_client(container_name)
    if not container_client.exists():
        container_client.create_container()

    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)
    with open(file_path, "rb") as data:
        blob_client.upload_blob(data, overwrite=True)

    print(f"File uploaded: {container_name}/{blob_name}")

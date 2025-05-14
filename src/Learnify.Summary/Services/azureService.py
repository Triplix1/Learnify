import os
from datetime import datetime, timedelta, timezone
from azure.storage.blob import BlobServiceClient, generate_blob_sas, BlobSasPermissions
from dotenv import load_dotenv

load_dotenv()

AZURE_CONNECTION_STRING = os.getenv("AZURE_CONNECTION_STRING")


def get_sas_url(container_name, blob_name):
    print("generating sas url:")
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING)

    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)

    sas_token = generate_blob_sas(
        account_name=blob_service_client.account_name,
        blob_name=blob_name,
        container_name=container_name,
        account_key=blob_service_client.credential.account_key,
        permission=BlobSasPermissions(read=True),
        expiry=datetime.now(timezone.utc) + timedelta(hours=1)
    )

    blob_url = f"{blob_client.url}?{sas_token}"

    print("One-time URL:", blob_url)
    return blob_url

def upload_to_blob(container_name, file_path, blob_name):
    print(f"Uploading file: {blob_name}")
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING, api_version="2021-06-08")

    container_client = blob_service_client.get_container_client(container_name)
    if not container_client.exists():
        container_client.create_container()

    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)
    with open(file_path, "rb") as data:
        blob_client.upload_blob(data, overwrite=True)

    print(f"File uploaded: {container_name}/{blob_name}")

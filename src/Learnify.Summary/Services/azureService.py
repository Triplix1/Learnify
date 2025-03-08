import os
from datetime import datetime, timedelta
from azure.storage.blob import BlobServiceClient, generate_blob_sas, BlobSasPermissions
from dotenv import load_dotenv

load_dotenv()

AZURE_CONNECTION_STRING = os.getenv("AZURE_CONNECTION_STRING")


def get_sas_url(container_name, blob_name):
    # Create a SAS token valid for 1 hour
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING)

    # Get the BlobClient
    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)

    # Generate a SAS token
    sas_token = generate_blob_sas(
        account_name=blob_service_client.account_name,
        container_name=container_name,
        blob_name=blob_name,
        account_key=blob_service_client.credential.account_key,  # Fetch account key from client
        permission=BlobSasPermissions(read=True),  # Read-only access
        expiry=datetime.utcnow() + timedelta(hours=1)  # URL expires in 1 hour
    )

    # Append SAS token to the blob URL
    blob_url = f"{blob_client.url}?{sas_token}"

    print("One-time URL:", blob_url)
    return blob_url

def upload_to_blob(container_name, file_path, blob_name):
    """Uploads a file to Azure Blob Storage."""
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING, api_version="2021-06-08")

    # Ensure container exists
    container_client = blob_service_client.get_container_client(container_name)
    if not container_client.exists():
        print(f"Container '{container_name}' does not exist. Creating it...")
        container_client.create_container()

    # Upload blob
    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)
    with open(file_path, "rb") as data:
        blob_client.upload_blob(data, overwrite=True)

    print(f"Uploaded {file_path} to Azure Blob Storage: {container_name}/{blob_name}")

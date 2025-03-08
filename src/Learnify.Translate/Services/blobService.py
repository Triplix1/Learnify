from azure.storage.blob import BlobServiceClient

AZURE_CONNECTION_STRING = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;"

def download_blob_text(container_name, blob_name, encoding="utf-8"):
    """Downloads and returns the text content from Azure Blob Storage without creating a local file."""
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING)
    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)

    # Read blob content directly into memory
    blob_content = blob_client.download_blob().readall()

    # Decode the bytes into a string
    text_content = blob_content.decode(encoding)

    return text_content

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

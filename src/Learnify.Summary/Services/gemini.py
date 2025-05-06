import os
import time
import google.generativeai as genai
import requests
from dotenv import load_dotenv
from io import BytesIO

load_dotenv()

GEMINI_KEY = os.getenv("GEMINI_KEY")
genai.configure(api_key=GEMINI_KEY)


def stream_to_seekable_bytesio(url):
    response = requests.get(url, stream=True)
    response.raise_for_status()

    buffer = BytesIO()
    for chunk in response.iter_content(chunk_size=8192):  # Read in chunks
        buffer.write(chunk)

    buffer.seek(0)
    return buffer


def upload_to_gemini(file_stream, mime_type, file_name="uploaded_file"):
    file = genai.upload_file(file_stream, display_name=file_name, mime_type=mime_type)
    print(f"Uploaded file '{file.display_name}' as: {file.uri}")
    return file


def wait_for_files_active(files):
    print("Waiting for file processing...")
    for name in (file.name for file in files):
        file = genai.get_file(name)
        while file.state.name == "PROCESSING":
            print(".", end="", flush=True)
            time.sleep(10)
            file = genai.get_file(name)
        if file.state.name != "ACTIVE":
            raise Exception(f"File {file.name} failed to process")
    print("...all files ready")
    print()


def get_summary(file_url, mime_type, language):
    file_stream = stream_to_seekable_bytesio(file_url)
    files = [upload_to_gemini(file_stream, mime_type)]
    wait_for_files_active(files)

    generation_config = {
        "temperature": 1,
        "top_p": 0.95,
        "top_k": 40,
        "max_output_tokens": 8192,
        "response_mime_type": "text/plain",
    }

    model = genai.GenerativeModel(
        model_name="gemini-1.5-flash-8b",
        generation_config=generation_config,
    )

    chat_session = model.start_chat(
        history=[
            {
                "role": "user",
                "parts": [files[0]],
            }
        ]
    )

    response = chat_session.send_message(
        f"Write summary with key points from from this lesson. In response provide me only summary and key points, any other messages are not acceptable. Response should be in {language}"
    )

    return response.text

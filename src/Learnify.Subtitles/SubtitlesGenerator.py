import os
import tempfile
from azure.storage.blob import BlobServiceClient
import whisper
from pydub import AudioSegment

# Azure Blob Storage Configuration
AZURE_CONNECTION_STRING = "your_connection_string"
CONTAINER_NAME = "your_container_name"
BLOB_NAME = "your_video.mp4"

# Step 1: Download video from Azure Blob Storage
def download_video_from_blob(container_name, blob_name):
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_CONNECTION_STRING)
    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)

    temp_video_path = tempfile.NamedTemporaryFile(delete=False, suffix=".mp4").name
    with open(temp_video_path, "wb") as video_file:
        video_file.write(blob_client.download_blob().readall())

    return temp_video_path

# Step 2: Extract audio from video
def extract_audio(video_path):
    temp_audio_path = tempfile.NamedTemporaryFile(delete=False, suffix=".wav").name
    audio = AudioSegment.from_file(video_path, format="mp4")
    audio.export(temp_audio_path, format="wav")
    return temp_audio_path

# Step 3: Transcribe audio using Whisper
def generate_subtitles(audio_path, language):
    model = whisper.load_model("medium")  # Change to "large" for better accuracy
    result = model.transcribe(audio_path, language=language)

    subtitle_path = audio_path.replace(".wav", ".srt")
    with open(subtitle_path, "w", encoding="utf-8") as srt_file:
        for i, segment in enumerate(result["segments"]):
            start = segment["start"]
            end = segment["end"]
            text = segment["text"]

            srt_file.write(f"{i+1}\n")
            srt_file.write(f"{format_time(start)} --> {format_time(end)}\n")
            srt_file.write(f"{text}\n\n")

    return subtitle_path

# Step 4: Format time for SRT format
def format_time(seconds):
    millisec = int((seconds - int(seconds)) * 1000)
    return f"{int(seconds // 3600):02}:{int((seconds % 3600) // 60):02}:{int(seconds % 60):02},{millisec:03}"

if __name__ == "__main__":
    language = input("Enter the language code (e.g., 'en' for English, 'es' for Spanish): ").strip()

    print("Downloading video from Azure Blob Storage...")
    video_path = download_video_from_blob(CONTAINER_NAME, BLOB_NAME)

    print("Extracting audio from video...")
    audio_path = extract_audio(video_path)

    print("Generating subtitles...")
    subtitle_path = generate_subtitles(audio_path, language)

    print(f"Subtitles saved at: {subtitle_path}")

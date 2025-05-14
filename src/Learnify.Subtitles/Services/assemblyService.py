import os

import assemblyai
from dotenv import load_dotenv

from Services.fileService import get_vtt_file_name

load_dotenv()

ASSEMBLYAI_API_KEY = os.getenv("ASSEMBLYAI_API_KEY")

assemblyai.settings.api_key = ASSEMBLYAI_API_KEY

languages = {"English": "en", "Ukrainian": "uk", "Spanish": "es", "French": "fr"}

def generate_subtitles(audio_path, primary_language):
    print(f"Generating subtitles in {primary_language}")
    primary_language = languages.get(primary_language, primary_language)

    transcriber = assemblyai.Transcriber()
    config = assemblyai.TranscriptionConfig(language_code=primary_language)
    transcript = transcriber.transcribe(audio_path, config=config)

    file_path = get_vtt_file_name(audio_path, ".vtt")

    with open(file_path, "w", encoding="utf-8") as f:
        f.write(transcript.export_subtitles_vtt())

    print("Subtitles saved to " + file_path)
    return file_path
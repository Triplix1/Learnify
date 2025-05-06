import whisper

def format_time_vtt(seconds):
    hours = int(seconds // 3600)
    minutes = int((seconds % 3600) // 60)
    secs = seconds % 60
    return f"{hours:02}:{minutes:02}:{secs:06.3f}".replace(",", ".")

def generate_subtitles(audio_path, primary_language):
    lang_map = {"English": "en", "Spanish": "es", "French": "fr", "German": "de"}
    primary_language = lang_map.get(primary_language, primary_language)

    model = whisper.load_model("base")
    result = model.transcribe(audio_path, language=primary_language)

    vtt_path = audio_path.replace(".wav", f"_{primary_language}.vtt")

    segments = result["segments"]
    with open(vtt_path, "w", encoding="utf-8") as vtt_file:
        vtt_file.write("WEBVTT\n\n")  # VTT header

        for segment in segments:
            start = format_time_vtt(segment["start"])
            end = format_time_vtt(segment["end"])
            text = segment["text"]

            vtt_file.write(f"{start} --> {end}\n")
            vtt_file.write(f"{text}\n\n")

    return vtt_path, segments
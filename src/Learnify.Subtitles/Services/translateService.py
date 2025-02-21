from deep_translator import GoogleTranslator
import tempfile

from Helpers.timeFormatter import format_time


def translate_subtitles(segments, target_language):
    """Translates subtitles into a target language."""
    translator = GoogleTranslator(source="auto", target=target_language)

    translated_srt_path = tempfile.NamedTemporaryFile(delete=False, suffix=f"_{target_language}.srt").name
    with open(translated_srt_path, "w", encoding="utf-8") as srt_file:
        for i, segment in enumerate(segments):
            start = segment["start"]
            end = segment["end"]
            translated_text = translator.translate(segment["text"])

            srt_file.write(f"{i + 1}\n")
            srt_file.write(f"{format_time(start)} --> {format_time(end)}\n")
            srt_file.write(f"{translated_text}\n\n")

    return translated_srt_path

def translate_transcription(transcription_path, target_language):
    """Translates a transcription into the target language."""
    translator = GoogleTranslator(source="auto", target=target_language)

    translated_txt_path = tempfile.NamedTemporaryFile(delete=False, suffix=f"_{target_language}.txt").name

    with open(transcription_path, "r", encoding="utf-8") as original_txt, open(translated_txt_path, "w", encoding="utf-8") as translated_txt:
        for line in original_txt:
            translated_txt.write(translator.translate(line) + "\n")

    return translated_txt_path
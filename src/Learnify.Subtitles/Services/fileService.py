import os


def get_vtt_file_name(audio_path, extension):
    base, _ = os.path.splitext(audio_path)
    file_path = base + extension
    return file_path

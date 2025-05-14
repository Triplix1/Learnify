from pydub import AudioSegment

from Services.fileService import get_vtt_file_name


def extract_audio(video_path):
    print(f"Extracting audio from video: {video_path}")
    temporary_audio_path = get_vtt_file_name(video_path, ".wav")
    audio = AudioSegment.from_file(video_path)
    audio = audio.set_channels(1).set_frame_rate(16000)
    audio.export(temporary_audio_path, format="wav", bitrate="32k")
    print(f"Audio extracted for: {video_path}")

    return temporary_audio_path

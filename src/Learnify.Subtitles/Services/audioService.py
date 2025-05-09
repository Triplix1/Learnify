from pydub import AudioSegment

from Services.fileService import get_vtt_file_name


def extract_audio(video_path):
    temp_audio_path = get_vtt_file_name(video_path, ".wav")
    audio = AudioSegment.from_file(video_path)
    audio = audio.set_channels(1).set_frame_rate(16000)
    audio.export(temp_audio_path, format="wav", bitrate="32k")

    return temp_audio_path

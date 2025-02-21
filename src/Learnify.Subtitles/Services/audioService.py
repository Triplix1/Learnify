import tempfile
from pydub import AudioSegment

def extract_audio(video_path):
    """Extracts audio from the video file and optimizes it for Whisper."""
    temp_audio_path = tempfile.NamedTemporaryFile(delete=False, suffix=".wav").name

    audio = AudioSegment.from_file(video_path, format="mp4")

    # ✅ Convert to mono, 16kHz sample rate (optimized for Whisper)
    audio = audio.set_channels(1).set_frame_rate(16000)

    # ✅ Reduce bitrate if needed
    audio.export(temp_audio_path, format="wav", bitrate="32k")

    return temp_audio_path

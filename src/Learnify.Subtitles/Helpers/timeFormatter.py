def format_time(seconds):
    """Formats time in SRT timecode format (HH:MM:SS,MS)."""
    millisec = int((seconds - int(seconds)) * 1000)
    return f"{int(seconds // 3600):02}:{int((seconds % 3600) // 60):02}:{int(seconds % 60):02},{millisec:03}"

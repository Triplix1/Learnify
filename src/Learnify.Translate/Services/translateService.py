from deep_translator import GoogleTranslator

names_map = {'english': 'en', 'french': 'fr', 'spanish': 'es', 'ukrainian': 'uk',}


def split_text(text, max_length=4999):
    lines = text.splitlines()
    chunks = []
    current_chunk = ""

    for line in lines:
        if len(current_chunk) + len(line) + 1 > max_length:
            chunks.append(current_chunk.strip())
            current_chunk = line
        else:
            current_chunk += "\n" + line if current_chunk else line

    if current_chunk:
        chunks.append(current_chunk.strip())

    return chunks


def extract_vtt_segments(vtt_content):
    segments = []
    lines = vtt_content.strip().splitlines()
    current_segment = []
    i = 0
    for line in lines:

        if i % 3 == 2:
            if current_segment:
                segments.append(current_segment)
                current_segment = []
        current_segment.append(line)

        i += 1

    if current_segment:
        segments.append(current_segment)

    return segments

def translate_chunked_text(text_chunks, source_language, target_language):
    translator = GoogleTranslator(source=source_language, target=target_language)
    translated_text = ""
    for text_chunk in text_chunks:
        translated_text += translator.translate(text_chunk)
        translated_text += "\n"
    return [line.strip() for line in translated_text.split('\n')]


def translate(content, source_language, target_language, content_type):
    print(f"Translating from: {source_language} to {target_language}")

    source_language = map_language_name(source_language)
    target_language = map_language_name(target_language)

    if content_type == "text/vtt":
        segments = extract_vtt_segments(content)
        timestamps = []

        text_without_timestamps = ""

        for segment in segments[1:]:
            timestamp = segment[0]
            text = ' '.join(segment[1:])
            text_without_timestamps += text + "\n"
            timestamps.append(timestamp)

        text_split = split_text(text_without_timestamps)
        translated_chunks = translate_chunked_text(text_split, source_language, target_language)

        translated_subtitles_chunks = [line for text in translated_chunks for line in text.splitlines()]

        translated_segments = []
        for timestamp, translated_text in zip(timestamps, translated_subtitles_chunks):
            if timestamp:
                translated_segments.append(f"{timestamp}\n{translated_text}")
            else:
                translated_segments.append(translated_text)

        print(f"Successfully translated to {target_language}")

        return segments[0][0] + "\n\n" + "\n\n".join(translated_segments)

def map_language_name(language: str):
    return names_map[language.lower()]
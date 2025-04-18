﻿import re
from deep_translator import GoogleTranslator

names_map = {'afrikaans': 'af', 'albanian': 'sq', 'amharic': 'am', 'arabic': 'ar', 'armenian': 'hy', 'assamese': 'as',
             'aymara': 'ay', 'azerbaijani': 'az', 'bambara': 'bm', 'basque': 'eu', 'belarusian': 'be', 'bengali': 'bn',
             'bhojpuri': 'bho', 'bosnian': 'bs', 'bulgarian': 'bg', 'catalan': 'ca', 'cebuano': 'ceb', 'chichewa': 'ny',
             'chinese (simplified)': 'zh-CN', 'chinese (traditional)': 'zh-TW', 'corsican': 'co', 'croatian': 'hr',
             'czech': 'cs', 'danish': 'da', 'dhivehi': 'dv', 'dogri': 'doi', 'dutch': 'nl', 'english': 'en',
             'esperanto': 'eo',
             'estonian': 'et', 'ewe': 'ee', 'filipino': 'tl', 'finnish': 'fi', 'french': 'fr', 'frisian': 'fy',
             'galician': 'gl', 'georgian': 'ka', 'german': 'de', 'greek': 'el', 'guarani': 'gn', 'gujarati': 'gu',
             'haitian creole': 'ht', 'hausa': 'ha', 'hawaiian': 'haw', 'hebrew': 'iw', 'hindi': 'hi', 'hmong': 'hmn',
             'hungarian': 'hu', 'icelandic': 'is', 'igbo': 'ig', 'ilocano': 'ilo', 'indonesian': 'id', 'irish': 'ga',
             'italian': 'it', 'japanese': 'ja', 'javanese': 'jw', 'kannada': 'kn', 'kazakh': 'kk', 'khmer': 'km',
             'kinyarwanda': 'rw', 'konkani': 'gom', 'korean': 'ko', 'krio': 'kri', 'kurdish (kurmanji)': 'ku',
             'kurdish (sorani)': 'ckb', 'kyrgyz': 'ky', 'lao': 'lo', 'latin': 'la', 'latvian': 'lv', 'lingala': 'ln',
             'lithuanian': 'lt', 'luganda': 'lg', 'luxembourgish': 'lb', 'macedonian': 'mk', 'maithili': 'mai',
             'malagasy': 'mg', 'malay': 'ms', 'malayalam': 'ml', 'maltese': 'mt', 'maori': 'mi', 'marathi': 'mr',
             'meiteilon (manipuri)': 'mni-Mtei', 'mizo': 'lus', 'mongolian': 'mn', 'myanmar': 'my', 'nepali': 'ne',
             'norwegian': 'no', 'odia (oriya)': 'or', 'oromo': 'om', 'pashto': 'ps', 'persian': 'fa', 'polish': 'pl',
             'portuguese': 'pt', 'punjabi': 'pa', 'quechua': 'qu', 'romanian': 'ro', 'russian': 'ru', 'samoan': 'sm',
             'sanskrit': 'sa', 'scots gaelic': 'gd', 'sepedi': 'nso', 'serbian': 'sr', 'sesotho': 'st', 'shona': 'sn',
             'sindhi': 'sd', 'sinhala': 'si', 'slovak': 'sk', 'slovenian': 'sl', 'somali': 'so', 'spanish': 'es',
             'sundanese': 'su', 'swahili': 'sw', 'swedish': 'sv', 'tajik': 'tg', 'tamil': 'ta', 'tatar': 'tt',
             'telugu': 'te',
             'thai': 'th', 'tigrinya': 'ti', 'tsonga': 'ts', 'turkish': 'tr', 'turkmen': 'tk', 'twi': 'ak',
             'ukrainian': 'uk',
             'urdu': 'ur', 'uyghur': 'ug', 'uzbek': 'uz', 'vietnamese': 'vi', 'welsh': 'cy', 'xhosa': 'xh',
             'yiddish': 'yi',
             'yoruba': 'yo', 'zulu': 'zu'}


def split_text(text, max_length=5000):
    """Splits text into chunks without breaking words."""
    words = text.split()  # Split by spaces to keep words intact
    chunks = []
    current_chunk = ""

    for word in words:
        if len(current_chunk) + len(word) + 1 > max_length:  # +1 for the space
            chunks.append(current_chunk.strip())  # Save the chunk and start a new one
            current_chunk = word
        else:
            current_chunk += " " + word if current_chunk else word

    if current_chunk:  # Append the last chunk
        chunks.append(current_chunk.strip())

    return chunks


def extract_vtt_segments(vtt_content):
    """Extracts timestamped segments from VTT subtitles."""
    segments = []
    lines = vtt_content.strip().split('\r\n')
    current_segment = []

    for line in lines:
        if re.match(r'^[0-9]+$', line) or re.match(r'^[0-9]+:[0-9]+:[0-9]+', line):
            if current_segment:
                segments.append(current_segment)
                current_segment = []
        current_segment.append(line)

    if current_segment:
        segments.append(current_segment)

    return segments


def batch_translate(text_chunks, source_language, target_language):
    """Translates multiple text chunks in a single request where possible."""
    translator = GoogleTranslator(source=source_language, target=target_language)
    joined_text = '\n'.join(text_chunks)
    translated_text = translator.translate(joined_text)
    return [line.strip() for line in translated_text.split('\n')]


def translate(content, source_language, target_language, content_type):
    """Translates text or subtitle content from the source language into the target language."""
    if not content:
        raise ValueError("Content to translate cannot be empty.")

    source_language = map_language_name(source_language)
    target_language = map_language_name(target_language)

    if content_type == "text/vtt":
        segments = extract_vtt_segments(content)
        text_chunks = []
        timestamps = []

        for segment in segments[1:]:
            timestamp = segment[0] if re.match(r'^[0-9]+:[0-9]+:[0-9]+', segment[0]) else None
            text = ' '.join(segment[1:]) if timestamp else ' '.join(segment)
            text_chunks.append(text)
            timestamps.append(timestamp)

        translated_chunks = batch_translate(text_chunks, source_language, target_language)

        translated_segments = []
        for timestamp, translated_text in zip(timestamps, translated_chunks):
            if timestamp:
                translated_segments.append(f"{timestamp}\n{translated_text}")
            else:
                translated_segments.append(translated_text)

        return segments[0][0] + "\n\n" + "\n\n".join(translated_segments)

    # Split text into sections while keeping words intact
    text_chunks = split_text(content, max_length=5000)
    translated_chunks = batch_translate(text_chunks, source_language, target_language)

    return "\n".join(translated_chunks)

def map_language_name(language: str):
    return names_map[language.lower()]
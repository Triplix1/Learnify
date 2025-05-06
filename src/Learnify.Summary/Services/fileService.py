import tempfile

def generate_temporary_file(summary, summary_blob_name) :
    summary_file_path = tempfile.NamedTemporaryFile(delete=False, suffix=summary_blob_name).name

    with open(summary_file_path, "w", encoding="utf-8") as translated_txt:
       translated_txt.write(summary)

    return summary_file_path
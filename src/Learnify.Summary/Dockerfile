FROM python:3.11-slim

ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

WORKDIR /app

RUN pip install --no-cache-dir --upgrade pip \
    && pip install --no-cache-dir \
    pika \
    python-dotenv \
    google-generativeai \
    requests \
    azure-storage-blob \
    && apt-get update \
    && apt-get install -y libmagic1 \
    && apt-get clean

COPY . .

CMD ["python", "main.py"]
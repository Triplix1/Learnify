FROM python:3.11-slim

ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

WORKDIR /app

RUN pip install --no-cache-dir --upgrade pip \
    && pip install --no-cache-dir \
    azure-storage-blob \
    deep-translator \
    pika \
    python-dotenv \
    && apt-get update \
    && apt-get install -y libmagic1 \
    && apt-get clean

COPY . .

CMD ["python", "main.py"]
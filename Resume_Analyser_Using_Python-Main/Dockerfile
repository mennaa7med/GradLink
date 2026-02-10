FROM python:3.11-slim

WORKDIR /app

# Install dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy application
COPY . .

# Create uploads directory
RUN mkdir -p uploads

# Railway uses PORT env variable
ENV PORT=5005
EXPOSE 5005

CMD ["python", "main.py"]


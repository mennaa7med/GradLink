from flask import Flask, request, jsonify
from flask_cors import CORS
import PyPDF2
from analyse_pdf import analyse_resume_gemini
import os

app = Flask(__name__)
# Enable CORS - Allow all origins
CORS(app, resources={r"/*": {"origins": "*"}}, supports_credentials=False)
app.config['UPLOAD_FOLDER'] = 'uploads'
app.config['MAX_CONTENT_LENGTH'] = 16 * 1024 * 1024  # 16MB max file size
os.makedirs(app.config['UPLOAD_FOLDER'], exist_ok=True)


def extract_text_from_resume(pdf_path):
    """Extract text content from PDF resume using PyPDF2"""
    try:
        text = ""
        with open(pdf_path, "rb") as f:
            reader = PyPDF2.PdfReader(f)
            for page in reader.pages:
                text += page.extract_text()
        return text
    except Exception as e:
        raise Exception(f"Error extracting text from PDF: {str(e)}")


@app.route("/", methods=["GET"])
def index():
    """Health check endpoint"""
    api_key = os.environ.get("GEMINI_API_KEY", "")
    key_status = "configured" if api_key else "NOT SET"
    key_preview = f"{api_key[:10]}..." if api_key and len(api_key) > 10 else "none"
    
    # Try to list available models
    available_models = []
    if api_key:
        try:
            import google.generativeai as genai
            genai.configure(api_key=api_key)
            for model in genai.list_models():
                if 'generateContent' in model.supported_generation_methods:
                    available_models.append(model.name)
        except Exception as e:
            available_models = [f"Error listing models: {str(e)}"]
    
    return jsonify({
        "status": "success",
        "message": "Resume Analyzer API is running",
        "gemini_api_key_status": key_status,
        "gemini_api_key_preview": key_preview,
        "available_models": available_models[:5],
        "current_model": "models/gemini-2.0-flash",
        "endpoints": {
            "analyze": "/api/analyze (POST)",
            "health": "/ (GET)"
        }
    })


@app.route("/api/analyze", methods=["POST"])
def analyze_resume():
    """Analyze resume against job description - Returns JSON"""
    try:
        # Validate request
        if 'resume' not in request.files:
            return jsonify({
                "status": "error",
                "message": "No resume file provided"
            }), 400

        if 'job_description' not in request.form:
            return jsonify({
                "status": "error",
                "message": "No job description provided"
            }), 400

        resume_file = request.files["resume"]
        job_description = request.form["job_description"]

        # Validate file
        if resume_file.filename == '':
            return jsonify({
                "status": "error",
                "message": "No file selected"
            }), 400

        if not resume_file.filename.endswith(".pdf"):
            return jsonify({
                "status": "error",
                "message": "Only PDF files are supported"
            }), 400

        # Process resume
        pdf_path = os.path.join(app.config['UPLOAD_FOLDER'], resume_file.filename)
        resume_file.save(pdf_path)

        # Extract text and analyze
        resume_content = extract_text_from_resume(pdf_path)
        result = analyse_resume_gemini(resume_content, job_description)

        # Clean up uploaded file
        if os.path.exists(pdf_path):
            os.remove(pdf_path)

        return jsonify({
            "status": "success",
            "result": result,
            "filename": resume_file.filename
        })

    except Exception as e:
        return jsonify({
            "status": "error",
            "message": str(e)
        }), 500


if __name__ == "__main__":
    port = int(os.environ.get("PORT", 5005))
    app.run(debug=False, host='0.0.0.0', port=port)

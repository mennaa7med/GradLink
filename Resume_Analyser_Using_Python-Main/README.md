# Resume Analyzer Flask API

This Flask application provides a JSON API for analyzing resumes against job descriptions using AI (Google Gemini).

## 🚀 Setup Instructions

### 1. Install Python Dependencies

Navigate to the Flask app directory and install required packages:

```bash
cd Resume_Analyser_Using_Python-Main
pip install -r requirements.txt
```

Or install packages individually:

```bash
pip install Flask==3.0.0
pip install flask-cors==4.0.0
pip install PyPDF2==3.0.1
pip install google-generativeai==0.3.2
pip install python-dotenv==1.0.0
```

### 2. Configure API Key

Make sure you have your Google Gemini API key configured in the `analyse_pdf.py` file.

### 3. Run the Flask Server

```bash
python main.py
```

The server will start on `http://localhost:5000`

## 📡 API Endpoints

### 1. Health Check
**GET** `/`

Returns API status and available endpoints.

**Response:**
```json
{
  "status": "success",
  "message": "Resume Analyzer API is running",
  "endpoints": {
    "analyze": "/api/analyze (POST)",
    "health": "/ (GET)"
  }
}
```

### 2. Analyze Resume
**POST** `/api/analyze`

Analyzes a resume PDF against a job description.

**Request:**
- Content-Type: `multipart/form-data`
- Fields:
  - `resume`: PDF file (required)
  - `job_description`: Text string (required)

**Success Response (200):**
```json
{
  "status": "success",
  "result": "AI analysis result text...",
  "filename": "resume.pdf"
}
```

**Error Response (400/500):**
```json
{
  "status": "error",
  "message": "Error description"
}
```

## 🔧 Integration with React

The React component `FlaskResumeAnalyzer.jsx` is already configured to communicate with this API.

### Example Usage in React:

```javascript
const formData = new FormData();
formData.append('resume', pdfFile);
formData.append('job_description', jobDescText);

const response = await fetch('http://localhost:5000/api/analyze', {
  method: 'POST',
  body: formData,
});

const data = await response.json();
console.log(data.result);
```

## 🛡️ CORS Configuration

CORS is enabled for all origins to allow React app integration. In production, you should restrict this to specific domains:

```python
CORS(app, resources={r"/api/*": {"origins": "http://localhost:5173"}})
```

## 📁 File Structure

```
Resume_Analyser_Using_Python-Main/
├── main.py                 # Flask API server
├── analyse_pdf.py          # Gemini AI integration
├── requirements.txt        # Python dependencies
├── uploads/               # Temporary file storage
└── README.md              # This file
```

## 🔍 Features

- ✅ PDF text extraction using PyPDF2
- ✅ AI-powered resume analysis with Google Gemini
- ✅ JSON API responses for easy integration
- ✅ CORS enabled for React integration
- ✅ Error handling and validation
- ✅ Automatic file cleanup after analysis
- ✅ 16MB file size limit

## 🐛 Troubleshooting

### Issue: CORS errors
**Solution:** Make sure Flask server is running and CORS is properly configured.

### Issue: "Module not found" errors
**Solution:** Install all dependencies from requirements.txt

### Issue: API not responding
**Solution:** Check if Flask server is running on port 5000

### Issue: PDF extraction fails
**Solution:** Ensure the uploaded file is a valid PDF

## 📝 Notes

- The API automatically deletes uploaded files after analysis
- Maximum file size is 16MB
- Only PDF files are supported
- The server runs in debug mode by default (disable in production)

## 🔐 Security Recommendations for Production

1. Disable debug mode
2. Add authentication/authorization
3. Restrict CORS to specific origins
4. Implement rate limiting
5. Add input sanitization
6. Use environment variables for sensitive data
7. Add file type validation
8. Implement request size limits


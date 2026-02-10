# 🚀 Flask API Integration Setup Guide

## ✅ What Was Changed

### 1. **Flask Backend (Resume_Analyser_Using_Python-Main/main.py)**
   - ✅ Changed from HTML rendering to **JSON API responses**
   - ✅ Added **CORS support** for React integration
   - ✅ Created new endpoint: `/api/analyze` (POST)
   - ✅ Added health check endpoint: `/` (GET)
   - ✅ Improved error handling with JSON responses
   - ✅ Added automatic file cleanup after analysis

### 2. **React Frontend**
   - ✅ Created `FlaskResumeAnalyzer.jsx` - New React component
   - ✅ Created `FlaskResumeAnalyzer.css` - Styled with GradLink theme
   - ✅ Updated `ScreeningResume.jsx` - Now uses React component instead of iframe
   - ✅ Integrated with Flask API using fetch

### 3. **Dependencies**
   - ✅ Created `requirements.txt` for Flask dependencies
   - ✅ Added `flask-cors` for cross-origin requests

---

## 📦 Installation Steps

### Step 1: Install Flask Dependencies

Open a terminal and navigate to the Flask directory:

```bash
cd Resume_Analyser_Using_Python-Main
pip install -r requirements.txt
```

This will install:
- Flask 3.0.0
- flask-cors 4.0.0
- PyMuPDF 1.23.8
- google-generativeai 0.3.2

### Step 2: Start the Flask Server

```bash
python main.py
```

You should see:
```
 * Running on http://127.0.0.1:5000
 * Debug mode: on
```

### Step 3: Start Your React App

In a **new terminal**, navigate to your project root and run:

```bash
npm run dev
```

---

## 🎯 How to Use

1. **Navigate to the app** in your browser (usually `http://localhost:5173`)

2. **Click "Screening Resume"** in the navbar

3. **Upload a PDF resume** by clicking the upload box

4. **Enter a job description** in the text area

5. **Click "Analyze Resume"** to get AI-powered analysis

6. **View results** displayed in GradLink style on the right side

---

## 🔄 API Flow

```
React Component (FlaskResumeAnalyzer.jsx)
    ↓
    POST request to http://localhost:5000/api/analyze
    ↓
Flask API (main.py)
    ↓
    Extract PDF text (PyMuPDF)
    ↓
    Analyze with Gemini AI (analyse_pdf.py)
    ↓
    Return JSON response
    ↓
React Component displays result
```

---

## 📡 API Endpoints

### Health Check
```
GET http://localhost:5000/
```

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

### Analyze Resume
```
POST http://localhost:5000/api/analyze
```

**Request Body (multipart/form-data):**
- `resume`: PDF file
- `job_description`: Text string

**Success Response:**
```json
{
  "status": "success",
  "result": "AI analysis result...",
  "filename": "resume.pdf"
}
```

**Error Response:**
```json
{
  "status": "error",
  "message": "Error description"
}
```

---

## 🎨 Features

### React Component Features:
- ✨ Beautiful GradLink-themed UI
- 📤 Drag-and-drop file upload
- 📝 Job description input
- ⏳ Loading states with spinner
- ✅ Success/error messages
- 📱 Responsive design
- 🎯 Real-time validation

### Flask API Features:
- 🔒 CORS enabled
- 📄 PDF text extraction
- 🤖 AI-powered analysis
- ✅ Input validation
- 🗑️ Automatic file cleanup
- 📊 JSON responses
- 🛡️ Error handling

---

## 🐛 Troubleshooting

### Problem: "Failed to connect to the analyzer"
**Solution:** Make sure Flask server is running on port 5000
```bash
cd Resume_Analyser_Using_Python-Main
python main.py
```

### Problem: CORS errors in browser console
**Solution:** Ensure `flask-cors` is installed
```bash
pip install flask-cors
```

### Problem: "Module not found" errors
**Solution:** Install all dependencies
```bash
pip install -r requirements.txt
```

### Problem: PDF upload fails
**Solution:** 
- Check file is a valid PDF
- File size must be under 16MB
- Check Flask server logs for errors

---

## 🔍 Testing the API

You can test the API using curl:

```bash
# Health check
curl http://localhost:5000/

# Analyze resume
curl -X POST http://localhost:5000/api/analyze \
  -F "resume=@path/to/resume.pdf" \
  -F "job_description=Software Engineer with Python experience"
```

---

## 📝 Key Files Modified/Created

### Created:
- ✅ `src/components/FlaskResumeAnalyzer.jsx`
- ✅ `src/components/FlaskResumeAnalyzer.css`
- ✅ `Resume_Analyser_Using_Python-Main/requirements.txt`
- ✅ `Resume_Analyser_Using_Python-Main/README.md`

### Modified:
- ✅ `Resume_Analyser_Using_Python-Main/main.py`
- ✅ `src/pages/ScreeningResume.jsx`

---

## 🎉 Benefits of This Approach

1. **Consistent UI**: Everything uses GradLink styling
2. **Better UX**: No iframe, smoother experience
3. **More Control**: Can customize how results are displayed
4. **API Reusability**: Can use the same API from other components
5. **Error Handling**: Better error messages and user feedback
6. **Modern Architecture**: Proper separation of frontend/backend

---

## 🚀 Next Steps

1. ✅ Install Flask dependencies
2. ✅ Start Flask server
3. ✅ Start React app
4. ✅ Test the integration
5. 🎯 Customize the result display if needed
6. 🎯 Add more features (save results, history, etc.)

---

## 💡 Tips

- Keep both servers running (Flask on 5000, React on 5173)
- Check browser console for any errors
- Check Flask terminal for API logs
- The Flask server auto-reloads when you change Python files
- The React app auto-reloads when you change JSX/CSS files

---

**Enjoy your integrated Resume Analyzer! 🎊**


import google.generativeai as genai
from dotenv import load_dotenv
import os

# تحميل متغيرات البيئة من .env (للتطوير المحلي)
load_dotenv()

def get_gemini_model():
    """Initialize and return Gemini model with API key"""
    api_key = os.environ.get("GEMINI_API_KEY") or os.getenv("GEMINI_API_KEY")
    
    if not api_key:
        raise ValueError("GEMINI_API_KEY environment variable is not set")
    
    # تهيئة المكتبة
    genai.configure(api_key=api_key)
    
    # إعدادات التوليد
    configuration = {
        "temperature": 0.7,
        "top_p": 0.95,
        "top_k": 40,
        "max_output_tokens": 8192
    }
    
    # إنشاء الموديل
    model = genai.GenerativeModel(
        model_name="gemini-1.5-flash",
        generation_config=configuration
    )
    
    return model

def analyse_resume_gemini(resume_content, job_description):
    """Analyze resume against job description using Gemini AI"""
    
    # Get model (this ensures API key is loaded)
    model = get_gemini_model()
    
    prompt = f"""
    You are a professional resume analyzer. Analyze the following resume against the job description.

    Resume:
    {resume_content}

    Job Description:
    {job_description}

    Task:
    1. Calculate a match score (0-100) based on how well the resume matches the job requirements.
    2. Identify missing skills or experiences that are mentioned in the job description but not in the resume.
    3. Provide specific, actionable suggestions to improve the resume.
    4. Write a brief summary of the candidate's strengths and weaknesses.

    IMPORTANT: Return the result in EXACTLY this format (do not add extra text before or after):

    Match Score: [number only, e.g., 85]

    Missing Skills:
    - [skill 1]
    - [skill 2]
    - [skill 3]

    Suggestions:
    - [suggestion 1]
    - [suggestion 2]
    - [suggestion 3]

    Summary:
    [Write a 2-3 sentence summary here]
    """
    
    # استدعاء الموديل
    response = model.generate_content(prompt)
    return response.text

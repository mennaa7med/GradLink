import google.generativeai as genai
from dotenv import load_dotenv
import os

# تحميل متغيرات البيئة
load_dotenv()

# قراءة مفتاح API من .env
api_key = os.getenv("GEMINI_API_KEY")

# تهيئة المكتبة
genai.configure(api_key=api_key)

# إعدادات التوليد - متوافقة مع Gemini SDK الحالي
configuration = {
    "temperature": 0.7,
    "top_p": 0.95,
    "top_k": 40,
    "max_output_tokens": 8192
}

# استخدام موديل Gemini المستقر
model = genai.GenerativeModel(
    model_name="gemini-1.5-flash",
    generation_config=configuration
)

def analyse_resume_gemini(resume_content, job_description):
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

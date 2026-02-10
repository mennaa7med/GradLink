import os

# Disable Google Cloud credentials - use API key only
os.environ["GOOGLE_APPLICATION_CREDENTIALS"] = ""

import google.generativeai as genai

def analyse_resume_gemini(resume_content, job_description):
    """Analyze resume against job description using Gemini AI"""
    
    # Get API key from environment
    api_key = os.environ.get("GEMINI_API_KEY")
    
    if not api_key:
        raise ValueError("GEMINI_API_KEY environment variable is not set. Please set it in Railway Variables.")
    
    # Configure Gemini with API key (not Google Cloud credentials)
    genai.configure(api_key=api_key)
    
    # Generation configuration
    generation_config = {
        "temperature": 0.7,
        "top_p": 0.95,
        "top_k": 40,
        "max_output_tokens": 8192
    }
    
    # Create model instance
    model = genai.GenerativeModel(
        model_name="gemini-1.5-flash",
        generation_config=generation_config
    )
    
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
    
    # Generate response
    response = model.generate_content(prompt)
    return response.text

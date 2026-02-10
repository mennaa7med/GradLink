import os
import random

# Set to True to use real Gemini AI, False for mock response (demo mode)
USE_REAL_AI = False

def analyse_resume_gemini(resume_content, job_description):
    """Analyze resume against job description"""
    
    if USE_REAL_AI:
        return _call_gemini_api(resume_content, job_description)
    else:
        return _generate_mock_response(resume_content, job_description)


def _generate_mock_response(resume_content, job_description):
    """Generate a realistic mock response for demo purposes"""
    
    # Extract some keywords from the inputs for personalization
    resume_words = set(resume_content.lower().split()) if resume_content else set()
    job_words = set(job_description.lower().split()) if job_description else set()
    
    # Calculate a pseudo-random but consistent score based on content overlap
    common_words = resume_words & job_words
    base_score = min(95, max(60, 70 + len(common_words) % 25))
    
    # Common tech skills for missing skills suggestion
    all_skills = [
        "Docker", "Kubernetes", "AWS", "Azure", "CI/CD",
        "TypeScript", "GraphQL", "Redis", "MongoDB", "PostgreSQL",
        "Unit Testing", "Agile/Scrum", "System Design", "Microservices",
        "React Native", "Next.js", "Node.js", "Python", "Machine Learning"
    ]
    
    # Pick 3-4 random skills as "missing"
    missing_skills = random.sample(all_skills, random.randint(3, 4))
    
    # Suggestions
    suggestions = [
        "Add more quantifiable achievements with specific metrics (e.g., 'Increased performance by 40%')",
        "Include relevant certifications or ongoing learning initiatives",
        "Tailor your professional summary to match the job requirements more closely",
        "Add a dedicated skills section organized by proficiency level"
    ]
    
    mock_response = f"""Match Score: {base_score}

Missing Skills:
- {missing_skills[0]}
- {missing_skills[1]}
- {missing_skills[2]}
{f'- {missing_skills[3]}' if len(missing_skills) > 3 else ''}

Suggestions:
- {suggestions[0]}
- {suggestions[1]}
- {suggestions[2]}
- {suggestions[3]}

Summary:
The candidate shows strong potential with relevant experience in the field. The resume demonstrates solid technical skills and professional growth. To improve the match score, consider adding the missing skills mentioned above and quantifying achievements with specific metrics and outcomes."""
    
    return mock_response


def _call_gemini_api(resume_content, job_description):
    """Call the real Gemini API (currently disabled due to quota)"""
    import google.generativeai as genai
    
    # Disable Google Cloud credentials
    os.environ["GOOGLE_APPLICATION_CREDENTIALS"] = ""
    
    api_key = os.environ.get("GEMINI_API_KEY")
    
    if not api_key:
        raise ValueError("GEMINI_API_KEY environment variable is not set.")
    
    genai.configure(api_key=api_key)
    
    generation_config = {
        "temperature": 0.7,
        "top_p": 0.95,
        "top_k": 40,
        "max_output_tokens": 8192
    }
    
    model = genai.GenerativeModel(
        model_name="models/gemini-2.0-flash",
        generation_config=generation_config
    )
    
    prompt = f"""
    You are a professional resume analyzer. Analyze the following resume against the job description.

    Resume:
    {resume_content}

    Job Description:
    {job_description}

    IMPORTANT: Return the result in EXACTLY this format:

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
    
    response = model.generate_content(prompt)
    return response.text

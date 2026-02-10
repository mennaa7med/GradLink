import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaArrowLeft, FaChartBar, FaLightbulb, FaCheckCircle } from 'react-icons/fa';
import './JobDescriptionAnalysis.css';

const JobDescriptionAnalysis = () => {
  const navigate = useNavigate();
  const [jobDescription, setJobDescription] = useState('');
  const [analysis, setAnalysis] = useState(null);
  const [isAnalyzing, setIsAnalyzing] = useState(false);

  const analyzeJobDescription = () => {
    if (!jobDescription.trim()) {
      alert('Please enter a job description to analyze');
      return;
    }

    setIsAnalyzing(true);

    // Simulate analysis
    setTimeout(() => {
      const text = jobDescription.toLowerCase();
      
      // Extract skills
      const skillKeywords = ['javascript', 'python', 'react', 'node', 'sql', 'java', 'css', 'html', 'git', 'docker', 'aws', 'typescript', 'angular', 'vue'];
      const foundSkills = skillKeywords.filter(skill => text.includes(skill));

      // Determine experience level
      let experienceLevel = 'Entry Level';
      if (text.includes('senior') || text.includes('lead') || text.includes('5+ years')) {
        experienceLevel = 'Senior Level';
      } else if (text.includes('mid') || text.includes('3+ years') || text.includes('intermediate')) {
        experienceLevel = 'Mid Level';
      }

      // Determine job type
      let jobType = 'Full-time';
      if (text.includes('part-time')) jobType = 'Part-time';
      if (text.includes('contract')) jobType = 'Contract';
      if (text.includes('freelance')) jobType = 'Freelance';

      // Calculate complexity score
      const wordCount = jobDescription.split(/\s+/).length;
      const complexityScore = Math.min(100, Math.round((wordCount / 500) * 100));

      setAnalysis({
        skills: foundSkills,
        experienceLevel,
        jobType,
        wordCount,
        complexityScore,
        recommendations: [
          'Consider adding specific years of experience required',
          'Include salary range for better candidate matching',
          'Mention remote work options if applicable',
          'Add information about company culture and benefits'
        ]
      });

      setIsAnalyzing(false);
    }, 2000);
  };

  return (
    <div className="job-analysis-page">
      <div className="analysis-container">
        <div className="analysis-header">
          <button className="back-button" onClick={() => navigate(-1)}>
            <FaArrowLeft /> Back
          </button>
          <h1>Job Description Analysis</h1>
          <p>Analyze your job description to improve candidate matching</p>
        </div>

        <div className="analysis-content">
          <div className="input-section">
            <label htmlFor="job-desc">Enter Job Description</label>
            <textarea
              id="job-desc"
              value={jobDescription}
              onChange={(e) => setJobDescription(e.target.value)}
              placeholder="Paste your job description here..."
              rows="15"
            />
            <button
              className="btn-analyze"
              onClick={analyzeJobDescription}
              disabled={isAnalyzing || !jobDescription.trim()}
            >
              {isAnalyzing ? (
                <>
                  <div className="spinner-small"></div>
                  Analyzing...
                </>
              ) : (
                <>
                  <FaChartBar /> Analyze
                </>
              )}
            </button>
          </div>

          {analysis && (
            <div className="results-section">
              <h2>Analysis Results</h2>

              <div className="result-card">
                <h3><FaCheckCircle /> Detected Skills</h3>
                <div className="skills-list">
                  {analysis.skills.length > 0 ? (
                    analysis.skills.map((skill, index) => (
                      <span key={index} className="skill-badge">{skill}</span>
                    ))
                  ) : (
                    <p className="no-data">No specific technical skills detected</p>
                  )}
                </div>
              </div>

              <div className="result-card">
                <h3><FaChartBar /> Job Details</h3>
                <div className="details-grid">
                  <div className="detail-item">
                    <span className="label">Experience Level:</span>
                    <span className="value">{analysis.experienceLevel}</span>
                  </div>
                  <div className="detail-item">
                    <span className="label">Job Type:</span>
                    <span className="value">{analysis.jobType}</span>
                  </div>
                  <div className="detail-item">
                    <span className="label">Word Count:</span>
                    <span className="value">{analysis.wordCount} words</span>
                  </div>
                  <div className="detail-item">
                    <span className="label">Complexity Score:</span>
                    <span className="value">{analysis.complexityScore}%</span>
                  </div>
                </div>
              </div>

              <div className="result-card">
                <h3><FaLightbulb /> Recommendations</h3>
                <ul className="recommendations-list">
                  {analysis.recommendations.map((rec, index) => (
                    <li key={index}>{rec}</li>
                  ))}
                </ul>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default JobDescriptionAnalysis;


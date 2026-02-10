import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { FaArrowLeft, FaCheckCircle, FaTimesCircle, FaFileAlt } from 'react-icons/fa';
import './ResumeScreener.css';

const ResumeScreener = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [resumes, setResumes] = useState([]);
  const [jobDescription, setJobDescription] = useState('');
  const [minScore, setMinScore] = useState(0);

  useEffect(() => {
    // Load data from localStorage or location state
    const storedResumes = JSON.parse(localStorage.getItem('uploadedResumes') || '[]');
    const storedJobDesc = localStorage.getItem('jobDescription') || location.state?.jobDescription || '';
    
    setResumes(storedResumes);
    setJobDescription(storedJobDesc);
  }, [location]);

  const calculateMatchScore = (resumeText, jobDesc) => {
    if (!resumeText || !jobDesc) return 0;

    const resumeLower = resumeText.toLowerCase();
    const jobLower = jobDesc.toLowerCase();
    
    // Extract keywords from job description
    const keywords = jobLower
      .split(/\s+/)
      .filter(word => word.length > 3)
      .filter(word => !['this', 'that', 'with', 'from', 'have', 'will', 'your'].includes(word));
    
    if (keywords.length === 0) return 0;

    // Count matching keywords
    const matches = keywords.filter(keyword => resumeLower.includes(keyword)).length;
    
    // Calculate percentage
    return Math.round((matches / keywords.length) * 100);
  };

  const getMissingSkills = (resumeText, jobDesc) => {
    const commonSkills = ['javascript', 'python', 'react', 'node', 'sql', 'java', 'css', 'html', 'git', 'docker'];
    const resumeLower = resumeText.toLowerCase();
    const jobLower = jobDesc.toLowerCase();
    
    return commonSkills.filter(skill => 
      jobLower.includes(skill) && !resumeLower.includes(skill)
    );
  };

  const submitApplication = (resumeId) => {
    alert(`Application submitted successfully for resume ${resumeId}!`);
  };

  const filteredResumes = resumes
    .map(resume => ({
      ...resume,
      score: calculateMatchScore(resume.text, jobDescription),
      missingSkills: getMissingSkills(resume.text, jobDescription)
    }))
    .filter(resume => resume.score >= minScore)
    .sort((a, b) => b.score - a.score);

  return (
    <div className="resume-screener-page">
      <div className="screener-header">
        <button className="back-button" onClick={() => navigate(-1)}>
          <FaArrowLeft /> Back
        </button>
        <h1>Resume Screening Results</h1>
      </div>

      <div className="screener-container">
        <div className="filter-section">
          <label>
            Minimum Match Score: {minScore}%
            <input
              type="range"
              min="0"
              max="100"
              value={minScore}
              onChange={(e) => setMinScore(Number(e.target.value))}
              className="score-slider"
            />
          </label>
          <p className="results-count">
            Showing {filteredResumes.length} of {resumes.length} resumes
          </p>
        </div>

        <div className="resumes-grid">
          {filteredResumes.length === 0 ? (
            <div className="no-results">
              <FaFileAlt size={60} />
              <p>No resumes match your criteria</p>
            </div>
          ) : (
            filteredResumes.map((resume, index) => (
              <div key={index} className="resume-card">
                <div className="resume-header">
                  <FaFileAlt className="file-icon" />
                  <h3>{resume.name}</h3>
                </div>
                
                <div className="score-section">
                  <div className="score-circle" style={{
                    background: `conic-gradient(#4CAF50 ${resume.score * 3.6}deg, #e0e0e0 0deg)`
                  }}>
                    <div className="score-inner">
                      <span className="score-value">{resume.score}%</span>
                    </div>
                  </div>
                  <p className="score-label">Match Score</p>
                </div>

                {resume.missingSkills.length > 0 && (
                  <div className="missing-skills">
                    <h4>Missing Skills:</h4>
                    <div className="skills-tags">
                      {resume.missingSkills.map((skill, i) => (
                        <span key={i} className="skill-tag">{skill}</span>
                      ))}
                    </div>
                  </div>
                )}

                <div className="resume-actions">
                  {resume.score >= 85 ? (
                    <button 
                      className="btn-submit"
                      onClick={() => submitApplication(resume.name)}
                    >
                      <FaCheckCircle /> Submit Application
                    </button>
                  ) : (
                    <button className="btn-improve" disabled>
                      <FaTimesCircle /> Needs Improvement
                    </button>
                  )}
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default ResumeScreener;


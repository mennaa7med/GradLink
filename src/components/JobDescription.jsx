import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaArrowLeft, FaArrowRight, FaBriefcase } from 'react-icons/fa';
import './JobDescription.css';

const JobDescription = () => {
  const navigate = useNavigate();
  const [jobDescription, setJobDescription] = useState('');
  const [requiredSkills, setRequiredSkills] = useState('');

  const handleBack = () => {
    navigate('/upload-resume');
  };

  const handleStartScreening = () => {
    if (!jobDescription.trim()) {
      alert('Please enter a job description');
      return;
    }

    // Save to localStorage
    localStorage.setItem('jobDescription', jobDescription);
    localStorage.setItem('requiredSkills', requiredSkills);

    // Navigate to screening page
    navigate('/resume-screener');
  };

  return (
    <div className="job-description-page">
      <div className="job-desc-container">
        <div className="job-desc-header">
          <button className="back-btn" onClick={handleBack}>
            <FaArrowLeft /> Back
          </button>
          <div className="header-content">
            <FaBriefcase size={40} />
            <h1>Job Description</h1>
            <p>Enter the job requirements to match against resumes</p>
          </div>
        </div>

        <div className="job-desc-form">
          <div className="form-group">
            <label htmlFor="job-description">
              Job Description *
            </label>
            <textarea
              id="job-description"
              value={jobDescription}
              onChange={(e) => setJobDescription(e.target.value)}
              placeholder="Enter the complete job description including:
- Job title and role
- Required qualifications
- Key responsibilities
- Experience level
- Technical skills needed
- Soft skills required"
              rows="12"
              required
            />
            <span className="char-count">{jobDescription.length} characters</span>
          </div>

          <div className="form-group">
            <label htmlFor="required-skills">
              Required Skills (Optional)
            </label>
            <input
              type="text"
              id="required-skills"
              value={requiredSkills}
              onChange={(e) => setRequiredSkills(e.target.value)}
              placeholder="e.g., JavaScript, React, Node.js, Python (comma-separated)"
            />
            <span className="help-text">
              Separate skills with commas for better matching
            </span>
          </div>

          <div className="form-actions">
            <button
              className="btn-start-screening"
              onClick={handleStartScreening}
              disabled={!jobDescription.trim()}
            >
              Start Screening <FaArrowRight />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default JobDescription;


import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaFileAlt, FaPlay, FaArrowLeft } from 'react-icons/fa';
import './JobForm.css';

const JobForm = () => {
  const [jobDescription, setJobDescription] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!jobDescription.trim()) {
      alert('Please enter a job description before starting screening.');
      return;
    }

    setIsSubmitting(true);
    
    try {
      // Simulate processing time
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Navigate to ResumeScreener with job description data
      navigate('/resume-screener', { 
        state: { 
          jobDescription: jobDescription.trim(),
          fromJobForm: true 
        } 
      });
    } catch (error) {
      alert('An error occurred. Please try again.');
      setIsSubmitting(false);
    }
  };

  const handleGoBack = () => {
    window.history.back();
  };

  const handleTextareaChange = (e) => {
    setJobDescription(e.target.value);
  };

  return (
    <div className="job-form-page">
      <div className="container">
        {/* Header */}
        <header className="page-header">
          <button className="back-button" onClick={handleGoBack}>
            <FaArrowLeft />
            Back
          </button>
          <h1 className="page-title">Job Description Form</h1>
        </header>

        {/* Main Content */}
        <main className="job-form-content">
          <div className="form-container">
            <div className="form-header">
              <FaFileAlt className="form-icon" />
              <h2>Enter Job Description</h2>
              <p>Provide a detailed job description to help us screen resumes more effectively.</p>
            </div>

            <form onSubmit={handleSubmit} className="job-form">
              <div className="form-group">
                <label htmlFor="job-description" className="form-label">
                  Job Description *
                </label>
                <textarea
                  id="job-description"
                  name="jobDescription"
                  value={jobDescription}
                  onChange={handleTextareaChange}
                  placeholder="Enter the job description here. Include details about:
- Required skills and qualifications
- Job responsibilities and duties
- Experience level required
- Education requirements
- Any specific certifications or licenses needed
- Company culture or values to consider"
                  className="job-description-textarea"
                  rows="12"
                  required
                />
                <div className="character-count">
                  {jobDescription.length} characters
                </div>
              </div>

              <div className="form-actions">
                <button 
                  type="submit" 
                  className="start-screening-btn"
                  disabled={!jobDescription.trim() || isSubmitting}
                >
                  {isSubmitting ? (
                    <>
                      <div className="loading-spinner"></div>
                      Processing...
                    </>
                  ) : (
                    <>
                      <FaPlay />
                      Start Screening
                    </>
                  )}
                </button>
              </div>
            </form>

            {/* Help Text */}
            <div className="help-section">
              <h3>Tips for Better Screening Results:</h3>
              <ul>
                <li>Be specific about required technical skills</li>
                <li>Include years of experience needed</li>
                <li>Mention preferred education background</li>
                <li>Describe key responsibilities clearly</li>
                <li>Include any special requirements or certifications</li>
              </ul>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default JobForm;

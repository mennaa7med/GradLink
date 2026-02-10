import React, { useState, useEffect } from 'react';
import { FaUpload, FaSpinner, FaCheckCircle, FaTimesCircle, FaFileAlt, FaBrain, FaChartLine, FaExclamationTriangle, FaLightbulb, FaClipboardList } from 'react-icons/fa';
import './FlaskResumeAnalyzer.css';

const FlaskResumeAnalyzer = () => {
  const [resumeFile, setResumeFile] = useState(null);
  const [jobDescription, setJobDescription] = useState('');
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [result, setResult] = useState(null);
  const [parsedResult, setParsedResult] = useState(null);
  const [error, setError] = useState(null);
  const [progress, setProgress] = useState(0);
  const [currentStep, setCurrentStep] = useState('');

  // Parse the result text to extract structured data
  const parseAnalysisResult = (resultText) => {
    try {
      console.log('Raw result text:', resultText); // Debug log

      const parsed = {
        matchScore: 0,
        missingSkills: [],
        suggestions: [],
        summary: ''
      };

      // Extract Match Score - support multiple formats
      // Format 1: "Match Score: 85%"
      // Format 2: "Match Score: 85/100"
      // Format 3: "Match Score: 85"
      const matchScoreRegex = /Match Score[:\s]*(\d+)(?:%|\/100)?/i;
      const matchScoreMatch = resultText.match(matchScoreRegex);
      if (matchScoreMatch) {
        parsed.matchScore = parseInt(matchScoreMatch[1]);
        console.log('Extracted Match Score:', parsed.matchScore); // Debug log
      } else {
        console.log('No match score found'); // Debug log
      }

      // Extract Missing Skills
      const missingSkillsRegex = /Missing Skills[:\s]*([\s\S]*?)(?=\n\s*Suggestions?:|Summary:|$)/i;
      const missingSkillsMatch = resultText.match(missingSkillsRegex);
      if (missingSkillsMatch) {
        const skillsText = missingSkillsMatch[1];
        parsed.missingSkills = skillsText
          .split(/[\n\r]+/)
          .map(line => line.trim())
          .filter(line => line && line.length > 2)
          .map(line => line.replace(/^[-*•\d.)\s]+/, '').trim())
          .filter(skill => skill.length > 2 && !skill.match(/^(Suggestions?|Summary)/i));

        console.log('Extracted Missing Skills:', parsed.missingSkills); // Debug log
      }

      // Extract Suggestions
      const suggestionsRegex = /Suggestions?[:\s]*([\s\S]*?)(?=\n\s*Summary:|Missing Skills:|$)/i;
      const suggestionsMatch = resultText.match(suggestionsRegex);
      if (suggestionsMatch) {
        const suggestionsText = suggestionsMatch[1];
        parsed.suggestions = suggestionsText
          .split(/[\n\r]+/)
          .map(line => line.trim())
          .filter(line => line && line.length > 2)
          .map(line => line.replace(/^[-*•\d.)\s]+/, '').trim())
          .filter(suggestion => suggestion.length > 2 && !suggestion.match(/^(Summary|Missing Skills?)/i));

        console.log('Extracted Suggestions:', parsed.suggestions); // Debug log
      }

      // Extract Summary
      const summaryRegex = /Summary[:\s]*([\s\S]*?)(?=\n\s*Missing Skills?:|Suggestions?:|$)/i;
      const summaryMatch = resultText.match(summaryRegex);
      if (summaryMatch) {
        parsed.summary = summaryMatch[1]
          .trim()
          .replace(/^[-*•\s]+/, '')
          .trim();

        console.log('Extracted Summary:', parsed.summary); // Debug log
      }

      console.log('Final parsed result:', parsed); // Debug log
      return parsed;
    } catch (err) {
      console.error('Error parsing result:', err);
      return null;
    }
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file && file.type === 'application/pdf') {
      setResumeFile(file);
      setError(null);
    } else {
      setError('Please select a PDF file');
      setResumeFile(null);
    }
  };

  // Simulate progress animation
  useEffect(() => {
    if (isAnalyzing) {
      setProgress(0);
      setCurrentStep('Uploading resume...');

      const steps = [
        { progress: 25, step: 'Uploading resume...', delay: 500 },
        { progress: 50, step: 'Extracting text from PDF...', delay: 1500 },
        { progress: 75, step: 'Analyzing skills and experience...', delay: 2500 },
        { progress: 90, step: 'Generating AI insights...', delay: 3500 },
        { progress: 95, step: 'Finalizing report...', delay: 4500 }
      ];

      steps.forEach(({ progress: prog, step, delay }) => {
        setTimeout(() => {
          if (isAnalyzing) {
            setProgress(prog);
            setCurrentStep(step);
          }
        }, delay);
      });
    }
  }, [isAnalyzing]);

  const handleAnalyze = async () => {
    if (!resumeFile) {
      setError('Please upload a resume');
      return;
    }

    if (!jobDescription.trim()) {
      setError('Please enter a job description');
      return;
    }

    setIsAnalyzing(true);
    setError(null);
    setResult(null);
    setProgress(0);
    setCurrentStep('Uploading resume...');

    try {
      // Send directly to Flask API on port 5005
      const flaskUrl = 'http://127.0.0.1:5005';
      const formData = new FormData();
      formData.append('resume', resumeFile);
      formData.append('job_description', jobDescription);

      setProgress(25);
      setCurrentStep('Uploading resume...');

      const response = await fetch(`${flaskUrl}/api/analyze`, {
        method: 'POST',
        body: formData
      });

      setProgress(50);
      setCurrentStep('Extracting text from PDF...');

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Analysis failed');
      }

      setProgress(75);
      setCurrentStep('Analyzing skills and experience...');

      const data = await response.json();

      setProgress(90);
      setCurrentStep('Generating AI insights...');

      if (data.status === 'success') {
        setProgress(100);
        setCurrentStep('Analysis complete!');
        
        setTimeout(() => {
          setResult(data.result);
          const parsed = parseAnalysisResult(data.result || '');
          setParsedResult(parsed);
          setIsAnalyzing(false);
        }, 500);
      } else {
        throw new Error(data.message || 'Analysis failed');
      }
    } catch (err) {
      setError(`Failed to connect to the analyzer. Make sure Flask server is running on port 5005. Error: ${err.message}`);
      console.error('Error:', err);
      setIsAnalyzing(false);
    }
  };

  const handleReset = () => {
    setResumeFile(null);
    setJobDescription('');
    setResult(null);
    setParsedResult(null);
    setError(null);
    setProgress(0);
    setCurrentStep('');
  };

  return (
    <div className="flask-analyzer-page">
      <div className="flask-analyzer-container">
        <div className="analyzer-header">
          <h1>AI Resume Analyzer</h1>
          <p>Upload your resume and job description for intelligent analysis</p>
        </div>

        <div className="analyzer-main-card">
          {/* Show Progress Section when analyzing */}
          {isAnalyzing ? (
            <div className="progress-section">
              <div className="progress-icon-wrapper">
                <FaBrain className="progress-brain-icon" />
              </div>
              <h2 className="progress-title">Resume Analysis Progress</h2>
              <p className="progress-subtitle">Please wait while we analyze your resume</p>

              <div className="progress-bar-container">
                <div className="progress-bar-wrapper">
                  <div
                    className="progress-bar-fill"
                    style={{ width: `${progress}%` }}
                  >
                    <span className="progress-percentage">{progress}%</span>
                  </div>
                </div>
              </div>

              <div className="progress-steps">
                <div className={`progress-step ${progress >= 25 ? 'active' : ''}`}>
                  <FaFileAlt className="step-icon" />
                  <span>Uploading resume</span>
                </div>
                <div className={`progress-step ${progress >= 50 ? 'active' : ''}`}>
                  <FaSpinner className="step-icon spinner-icon" />
                  <span>Extracting text</span>
                </div>
                <div className={`progress-step ${progress >= 75 ? 'active' : ''}`}>
                  <FaBrain className="step-icon" />
                  <span>Analyzing skills</span>
                </div>
                <div className={`progress-step ${progress >= 95 ? 'active' : ''}`}>
                  <FaChartLine className="step-icon" />
                  <span>Generating report</span>
                </div>
              </div>

              <div className="current-step-text">
                <div className="step-indicator"></div>
                {currentStep}
              </div>
            </div>
          ) : (
            <>
              {/* Input Section */}
              <div className="input-section">
                <div className="upload-box">
                  <label htmlFor="resume-upload" className="upload-label">
                    <FaUpload size={30} />
                    <span>{resumeFile ? resumeFile.name : 'Upload Resume (PDF)'}</span>
                  </label>
                  <input
                    id="resume-upload"
                    type="file"
                    accept=".pdf"
                    onChange={handleFileChange}
                    style={{ display: 'none' }}
                  />
                </div>

                <div className="job-desc-box">
                  <label htmlFor="job-desc">Job Description</label>
                  <textarea
                    id="job-desc"
                    value={jobDescription}
                    onChange={(e) => setJobDescription(e.target.value)}
                    placeholder="Paste the job description here..."
                    rows="8"
                  />
                </div>

                <div className="action-buttons">
                  <button
                    className="btn-analyze"
                    onClick={handleAnalyze}
                    disabled={isAnalyzing || !resumeFile || !jobDescription.trim()}
                  >
                    <FaCheckCircle /> Analyze Resume
                  </button>
                  <button className="btn-reset" onClick={handleReset}>
                    Reset
                  </button>
                </div>

                {error && (
                  <div className="error-message">
                    <FaTimesCircle /> {error}
                  </div>
                )}
              </div>

              {/* Results Section */}
              {result && (
                <div className="results-section">
                  <div className="results-header">
                    <FaCheckCircle className="success-icon" />
                    <h2>Analysis Results</h2>
                  </div>

                  {parsedResult && parsedResult.matchScore > 0 ? (
                    <>
                      {/* Match Score Progress Bar */}
                      <div className="match-score-card">
                    <div className="card-header">
                      <FaChartLine className="card-icon" />
                      <h3>Match Score</h3>
                    </div>
                    <div className="match-score-content">
                      <div className="score-circle">
                        <svg className="score-svg" viewBox="0 0 120 120">
                          <circle
                            className="score-bg"
                            cx="60"
                            cy="60"
                            r="50"
                          />
                          <circle
                            className="score-progress"
                            cx="60"
                            cy="60"
                            r="50"
                            style={{
                              strokeDasharray: `${2 * Math.PI * 50}`,
                              strokeDashoffset: `${2 * Math.PI * 50 * (1 - parsedResult.matchScore / 100)}`
                            }}
                          />
                        </svg>
                        <div className="score-text">
                          <span className="score-number">{parsedResult.matchScore}</span>
                          <span className="score-percent">%</span>
                        </div>
                      </div>
                      <div className="score-bar-wrapper">
                        <div className="score-label">
                          <span>Resume Match</span>
                          <span className="score-value">{parsedResult.matchScore}%</span>
                        </div>
                        <div className="score-bar">
                          <div
                            className="score-bar-fill"
                            style={{
                              width: `${parsedResult.matchScore}%`,
                              background: parsedResult.matchScore >= 70
                                ? 'linear-gradient(90deg, #4CAF50, #66BB6A)'
                                : parsedResult.matchScore >= 50
                                ? 'linear-gradient(90deg, #FFCB66, #ffa726)'
                                : 'linear-gradient(90deg, #f44336, #e57373)'
                            }}
                          />
                        </div>
                        <div className="score-description">
                          {parsedResult.matchScore >= 70 && '🎉 Excellent match! Strong candidate.'}
                          {parsedResult.matchScore >= 50 && parsedResult.matchScore < 70 && '👍 Good match with room for improvement.'}
                          {parsedResult.matchScore < 50 && '⚠️ Needs significant improvement.'}
                        </div>
                      </div>
                    </div>
                  </div>

                  {/* Missing Skills Card */}
                  {parsedResult.missingSkills && parsedResult.missingSkills.length > 0 && (
                    <div className="info-card missing-skills-card">
                      <div className="card-header">
                        <FaExclamationTriangle className="card-icon warning-icon" />
                        <h3>Missing Skills</h3>
                        <span className="badge">{parsedResult.missingSkills.length}</span>
                      </div>
                      <div className="card-content">
                        <ul className="skills-list">
                          {parsedResult.missingSkills.map((skill, index) => (
                            <li key={index} className="skill-item">
                              <span className="skill-bullet">•</span>
                              <span className="skill-text">{skill}</span>
                            </li>
                          ))}
                        </ul>
                      </div>
                    </div>
                  )}

                  {/* Suggestions Card */}
                  {parsedResult.suggestions && parsedResult.suggestions.length > 0 && (
                    <div className="info-card suggestions-card">
                      <div className="card-header">
                        <FaLightbulb className="card-icon suggestion-icon" />
                        <h3>Suggestions for Improvement</h3>
                      </div>
                      <div className="card-content">
                        <ul className="suggestions-list">
                          {parsedResult.suggestions.map((suggestion, index) => (
                            <li key={index} className="suggestion-item">
                              <span className="suggestion-number">{index + 1}</span>
                              <span className="suggestion-text">{suggestion}</span>
                            </li>
                          ))}
                        </ul>
                      </div>
                    </div>
                  )}

                  {/* Summary Card */}
                  {parsedResult.summary && (
                    <div className="info-card summary-card">
                      <div className="card-header">
                        <FaClipboardList className="card-icon summary-icon" />
                        <h3>Summary</h3>
                      </div>
                      <div className="card-content">
                        <p className="summary-text">{parsedResult.summary}</p>
                      </div>
                    </div>
                  )}

                    </>
                  ) : (
                    /* Raw Result (Fallback) */
                    <div className="results-content">
                      <div className="fallback-notice">
                        <FaExclamationTriangle />
                        <p>Showing raw analysis result. The structured format could not be parsed.</p>
                      </div>
                      <pre>{result}</pre>
                    </div>
                  )}
                </div>
              )}
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default FlaskResumeAnalyzer;


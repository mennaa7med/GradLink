import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaTrash, FaArrowLeft, FaExclamationTriangle } from 'react-icons/fa';
import './DeleteResumes.css';

const DeleteResumes = () => {
  const navigate = useNavigate();
  const [resumes, setResumes] = useState([]);
  const [selectedResumes, setSelectedResumes] = useState([]);

  useEffect(() => {
    // Load resumes from localStorage
    const storedResumes = JSON.parse(localStorage.getItem('uploadedResumes') || '[]');
    setResumes(storedResumes);
  }, []);

  const toggleSelectResume = (index) => {
    if (selectedResumes.includes(index)) {
      setSelectedResumes(selectedResumes.filter(i => i !== index));
    } else {
      setSelectedResumes([...selectedResumes, index]);
    }
  };

  const selectAll = () => {
    if (selectedResumes.length === resumes.length) {
      setSelectedResumes([]);
    } else {
      setSelectedResumes(resumes.map((_, index) => index));
    }
  };

  const deleteSelected = () => {
    if (selectedResumes.length === 0) {
      alert('Please select at least one resume to delete');
      return;
    }

    if (window.confirm(`Are you sure you want to delete ${selectedResumes.length} resume(s)?`)) {
      const updatedResumes = resumes.filter((_, index) => !selectedResumes.includes(index));
      setResumes(updatedResumes);
      localStorage.setItem('uploadedResumes', JSON.stringify(updatedResumes));
      setSelectedResumes([]);
      
      if (updatedResumes.length === 0) {
        alert('All resumes deleted. Redirecting to upload page...');
        navigate('/upload-resume');
      }
    }
  };

  const deleteAll = () => {
    if (window.confirm('Are you sure you want to delete ALL resumes? This action cannot be undone.')) {
      localStorage.removeItem('uploadedResumes');
      localStorage.removeItem('jobDescription');
      localStorage.removeItem('requiredSkills');
      setResumes([]);
      setSelectedResumes([]);
      alert('All data cleared. Redirecting to upload page...');
      navigate('/upload-resume');
    }
  };

  const formatFileSize = (bytes) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  };

  return (
    <div className="delete-resumes-page">
      <div className="delete-container">
        <div className="delete-header">
          <button className="back-button" onClick={() => navigate(-1)}>
            <FaArrowLeft /> Back
          </button>
          <h1>Manage Resumes</h1>
          <p>Select and delete resumes from your collection</p>
        </div>

        {resumes.length === 0 ? (
          <div className="no-resumes">
            <FaExclamationTriangle size={60} />
            <h2>No Resumes Found</h2>
            <p>Upload some resumes to get started</p>
            <button className="btn-upload" onClick={() => navigate('/upload-resume')}>
              Upload Resumes
            </button>
          </div>
        ) : (
          <>
            <div className="actions-bar">
              <button className="btn-select-all" onClick={selectAll}>
                {selectedResumes.length === resumes.length ? 'Deselect All' : 'Select All'}
              </button>
              <span className="selected-count">
                {selectedResumes.length} of {resumes.length} selected
              </span>
              <div className="action-buttons">
                <button
                  className="btn-delete-selected"
                  onClick={deleteSelected}
                  disabled={selectedResumes.length === 0}
                >
                  <FaTrash /> Delete Selected
                </button>
                <button className="btn-delete-all" onClick={deleteAll}>
                  <FaTrash /> Delete All
                </button>
              </div>
            </div>

            <div className="resumes-list">
              {resumes.map((resume, index) => (
                <div
                  key={index}
                  className={`resume-item ${selectedResumes.includes(index) ? 'selected' : ''}`}
                  onClick={() => toggleSelectResume(index)}
                >
                  <input
                    type="checkbox"
                    checked={selectedResumes.includes(index)}
                    onChange={() => toggleSelectResume(index)}
                    onClick={(e) => e.stopPropagation()}
                  />
                  <div className="resume-info">
                    <h3>{resume.name}</h3>
                    <p>{formatFileSize(resume.size)} • {resume.type}</p>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default DeleteResumes;


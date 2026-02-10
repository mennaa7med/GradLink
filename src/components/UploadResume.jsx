import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaUpload, FaFileAlt, FaTrash, FaArrowRight } from 'react-icons/fa';
import * as pdfjsLib from 'pdfjs-dist';
import './UploadResume.css';

// Set up PDF.js worker
pdfjsLib.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjsLib.version}/pdf.worker.min.js`;

const UploadResume = () => {
  const navigate = useNavigate();
  const [files, setFiles] = useState([]);
  const [isProcessing, setIsProcessing] = useState(false);

  const extractTextFromPDF = async (file) => {
    try {
      const arrayBuffer = await file.arrayBuffer();
      const pdf = await pdfjsLib.getDocument({ data: arrayBuffer }).promise;
      let fullText = '';

      for (let i = 1; i <= pdf.numPages; i++) {
        const page = await pdf.getPage(i);
        const textContent = await page.getTextContent();
        const pageText = textContent.items.map(item => item.str).join(' ');
        fullText += pageText + '\n';
      }

      return fullText;
    } catch (error) {
      console.error('Error extracting PDF text:', error);
      return '';
    }
  };

  const handleFileSelect = async (e) => {
    const selectedFiles = Array.from(e.target.files);
    setIsProcessing(true);

    const processedFiles = await Promise.all(
      selectedFiles.map(async (file) => {
        let text = '';
        
        if (file.type === 'application/pdf') {
          text = await extractTextFromPDF(file);
        }

        return {
          name: file.name,
          size: file.size,
          type: file.type,
          text: text,
          file: file
        };
      })
    );

    setFiles([...files, ...processedFiles]);
    setIsProcessing(false);
  };

  const removeFile = (index) => {
    setFiles(files.filter((_, i) => i !== index));
  };

  const handleNext = () => {
    if (files.length === 0) {
      alert('Please upload at least one resume');
      return;
    }

    // Save to localStorage
    localStorage.setItem('uploadedResumes', JSON.stringify(files));
    navigate('/job-description');
  };

  const formatFileSize = (bytes) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  };

  return (
    <div className="upload-resume-page">
      <div className="upload-container">
        <div className="upload-header">
          <h1>Upload Resumes</h1>
          <p>Upload PDF, DOC, or DOCX files to start the screening process</p>
        </div>

        <div className="upload-area">
          <input
            type="file"
            id="file-input"
            multiple
            accept=".pdf,.doc,.docx"
            onChange={handleFileSelect}
            style={{ display: 'none' }}
          />
          <label htmlFor="file-input" className="upload-label">
            <FaUpload size={50} />
            <h3>Click to upload or drag and drop</h3>
            <p>PDF, DOC, DOCX (Max 10MB each)</p>
          </label>
        </div>

        {isProcessing && (
          <div className="processing-indicator">
            <div className="spinner"></div>
            <p>Processing files...</p>
          </div>
        )}

        {files.length > 0 && (
          <div className="files-list">
            <h3>Uploaded Files ({files.length})</h3>
            {files.map((file, index) => (
              <div key={index} className="file-item">
                <FaFileAlt className="file-icon" />
                <div className="file-info">
                  <p className="file-name">{file.name}</p>
                  <p className="file-size">{formatFileSize(file.size)}</p>
                </div>
                <button
                  className="remove-btn"
                  onClick={() => removeFile(index)}
                >
                  <FaTrash />
                </button>
              </div>
            ))}
          </div>
        )}

        <div className="upload-actions">
          <button
            className="btn-next"
            onClick={handleNext}
            disabled={files.length === 0}
          >
            Next: Job Description <FaArrowRight />
          </button>
        </div>
      </div>
    </div>
  );
};

export default UploadResume;


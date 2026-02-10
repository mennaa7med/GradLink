import React, { useState, useRef } from 'react';
import api from '../../api/client';
import './ProfilePictureUpload.css';

const ProfilePictureUpload = ({ currentImage, onUploadSuccess }) => {
  const [uploading, setUploading] = useState(false);
  const [preview, setPreview] = useState(currentImage);
  const [error, setError] = useState('');
  const fileInputRef = useRef(null);

  const handleFileSelect = (e) => {
    const file = e.target.files[0];
    if (!file) return;

    // Validate file type
    const validTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
    if (!validTypes.includes(file.type)) {
      setError('Please select a valid image file (JPEG, PNG, GIF, or WebP)');
      return;
    }

    // Validate file size (max 5MB)
    if (file.size > 5 * 1024 * 1024) {
      setError('Image size must be less than 5MB');
      return;
    }

    setError('');
    
    // Preview
    const reader = new FileReader();
    reader.onloadend = () => {
      setPreview(reader.result);
    };
    reader.readAsDataURL(file);

    // Upload
    uploadFile(file);
  };

  const uploadFile = async (file) => {
    setUploading(true);
    setError('');

    const formData = new FormData();
    formData.append('file', file);

    try {
      const { data } = await api.post('/api/users/profile-picture', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
      
      setPreview(data.profilePictureUrl);
      if (onUploadSuccess) {
        onUploadSuccess(data.profilePictureUrl);
      }
    } catch (err) {
      setError(err.response?.data?.error || 'Failed to upload image');
      setPreview(currentImage); // Revert to current image
    } finally {
      setUploading(false);
    }
  };

  const handleClick = () => {
    fileInputRef.current?.click();
  };

  const handleRemove = async () => {
    try {
      await api.delete('/api/users/profile-picture');
      setPreview(null);
      if (onUploadSuccess) {
        onUploadSuccess(null);
      }
    } catch (err) {
      setError('Failed to remove image');
    }
  };

  return (
    <div className="profile-picture-upload">
      <div 
        className={`profile-picture-container ${uploading ? 'uploading' : ''}`}
        onClick={handleClick}
      >
        {preview ? (
          <img src={preview} alt="Profile" className="profile-image" />
        ) : (
          <div className="profile-placeholder">
            <span className="placeholder-icon">👤</span>
          </div>
        )}
        
        <div className="upload-overlay">
          {uploading ? (
            <div className="upload-spinner"></div>
          ) : (
            <span className="upload-icon">📷</span>
          )}
        </div>
      </div>

      <input
        ref={fileInputRef}
        type="file"
        accept="image/jpeg,image/png,image/gif,image/webp"
        onChange={handleFileSelect}
        className="file-input"
      />

      <div className="profile-picture-actions">
        <button 
          type="button" 
          className="upload-btn" 
          onClick={handleClick}
          disabled={uploading}
        >
          {uploading ? 'Uploading...' : 'Change Photo'}
        </button>
        {preview && (
          <button 
            type="button" 
            className="remove-btn" 
            onClick={handleRemove}
            disabled={uploading}
          >
            Remove
          </button>
        )}
      </div>

      {error && <div className="upload-error">{error}</div>}
    </div>
  );
};

export default ProfilePictureUpload;


















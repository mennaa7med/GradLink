import React, { useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { motion, AnimatePresence } from 'framer-motion';
import { 
  FiUser, FiCamera, FiCheck, FiArrowRight, FiArrowLeft,
  FiBookOpen, FiBriefcase, FiGlobe, FiLinkedin, FiGithub
} from 'react-icons/fi';
import * as usersApi from '../../api/users';
import './CompleteProfile.css';

const CompleteProfile = () => {
  const navigate = useNavigate();
  const fileInputRef = useRef(null);
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);
  const [profileImage, setProfileImage] = useState(null);
  const [profileImageFile, setProfileImageFile] = useState(null);
  
  const [formData, setFormData] = useState({
    bio: '',
    skills: '',
    location: '',
    linkedInUrl: '',
    gitHubUrl: '',
    websiteUrl: '',
    interests: []
  });

  const interestOptions = [
    'Web Development', 'Mobile Development', 'AI/Machine Learning',
    'Data Science', 'Cybersecurity', 'Cloud Computing', 'DevOps',
    'UI/UX Design', 'Game Development', 'Blockchain', 'IoT',
    'Project Management', 'Entrepreneurship', 'Research'
  ];

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setProfileImageFile(file);
      setProfileImage(URL.createObjectURL(file));
    }
  };

  const handleChange = (e) => {
    setFormData(prev => ({
      ...prev,
      [e.target.name]: e.target.value
    }));
  };

  const toggleInterest = (interest) => {
    setFormData(prev => ({
      ...prev,
      interests: prev.interests.includes(interest)
        ? prev.interests.filter(i => i !== interest)
        : [...prev.interests, interest]
    }));
  };

  const handleSubmit = async () => {
    setLoading(true);
    try {
      // Upload profile picture first
      if (profileImageFile) {
        await usersApi.uploadAvatar(profileImageFile);
      }

      // Update profile
      await usersApi.updateProfile({
        ...formData,
        skills: formData.skills
      });

      // Mark profile as complete
      localStorage.setItem('profileCompleted', 'true');
      
      // Redirect to dashboard
      navigate('/dashboard');
    } catch (error) {
      console.error('Failed to complete profile:', error);
      alert('Failed to save profile. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const nextStep = () => setStep(prev => Math.min(prev + 1, 3));
  const prevStep = () => setStep(prev => Math.max(prev - 1, 1));

  return (
    <div className="complete-profile-page">
      <div className="profile-container">
        <div className="profile-header">
          <h1>Complete Your Profile</h1>
          <p>Help us personalize your experience</p>
        </div>

        {/* Progress Steps */}
        <div className="progress-steps">
          {[1, 2, 3].map(num => (
            <div 
              key={num} 
              className={`step ${step >= num ? 'active' : ''} ${step > num ? 'completed' : ''}`}
            >
              <div className="step-number">
                {step > num ? <FiCheck /> : num}
              </div>
              <span className="step-label">
                {num === 1 ? 'Photo & Bio' : num === 2 ? 'Skills' : 'Links'}
              </span>
            </div>
          ))}
        </div>

        {/* Step Content */}
        <AnimatePresence mode="wait">
          {step === 1 && (
            <motion.div 
              key="step1"
              className="step-content"
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
            >
              <div className="photo-upload-section">
                <div 
                  className="photo-preview"
                  onClick={() => fileInputRef.current?.click()}
                >
                  {profileImage ? (
                    <img src={profileImage} alt="Profile" />
                  ) : (
                    <div className="photo-placeholder">
                      <FiCamera />
                      <span>Add Photo</span>
                    </div>
                  )}
                  <div className="photo-overlay">
                    <FiCamera />
                  </div>
                </div>
                <input
                  ref={fileInputRef}
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                  style={{ display: 'none' }}
                />
                <p className="photo-hint">Click to upload a profile photo</p>
              </div>

              <div className="form-group">
                <label>
                  <FiUser /> About You
                </label>
                <textarea
                  name="bio"
                  value={formData.bio}
                  onChange={handleChange}
                  placeholder="Tell us a bit about yourself, your goals, and what you're passionate about..."
                  rows="4"
                />
              </div>

              <div className="form-group">
                <label>
                  <FiGlobe /> Location
                </label>
                <input
                  type="text"
                  name="location"
                  value={formData.location}
                  onChange={handleChange}
                  placeholder="e.g., Cairo, Egypt"
                />
              </div>
            </motion.div>
          )}

          {step === 2 && (
            <motion.div 
              key="step2"
              className="step-content"
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
            >
              <div className="form-group">
                <label>
                  <FiBookOpen /> Skills
                </label>
                <input
                  type="text"
                  name="skills"
                  value={formData.skills}
                  onChange={handleChange}
                  placeholder="e.g., JavaScript, React, Python, Node.js"
                />
                <p className="input-hint">Separate skills with commas</p>
              </div>

              <div className="interests-section">
                <label>
                  <FiBriefcase /> Areas of Interest
                </label>
                <p className="section-hint">Select topics you're interested in</p>
                <div className="interests-grid">
                  {interestOptions.map(interest => (
                    <button
                      key={interest}
                      type="button"
                      className={`interest-tag ${formData.interests.includes(interest) ? 'selected' : ''}`}
                      onClick={() => toggleInterest(interest)}
                    >
                      {interest}
                      {formData.interests.includes(interest) && <FiCheck />}
                    </button>
                  ))}
                </div>
              </div>
            </motion.div>
          )}

          {step === 3 && (
            <motion.div 
              key="step3"
              className="step-content"
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
            >
              <p className="step-description">
                Add your professional links to help others find and connect with you.
              </p>

              <div className="form-group">
                <label>
                  <FiLinkedin /> LinkedIn Profile
                </label>
                <input
                  type="url"
                  name="linkedInUrl"
                  value={formData.linkedInUrl}
                  onChange={handleChange}
                  placeholder="https://linkedin.com/in/yourprofile"
                />
              </div>

              <div className="form-group">
                <label>
                  <FiGithub /> GitHub Profile
                </label>
                <input
                  type="url"
                  name="gitHubUrl"
                  value={formData.gitHubUrl}
                  onChange={handleChange}
                  placeholder="https://github.com/yourusername"
                />
              </div>

              <div className="form-group">
                <label>
                  <FiGlobe /> Personal Website
                </label>
                <input
                  type="url"
                  name="websiteUrl"
                  value={formData.websiteUrl}
                  onChange={handleChange}
                  placeholder="https://yourwebsite.com"
                />
              </div>
            </motion.div>
          )}
        </AnimatePresence>

        {/* Navigation Buttons */}
        <div className="navigation-buttons">
          {step > 1 && (
            <button className="btn-back" onClick={prevStep}>
              <FiArrowLeft /> Back
            </button>
          )}
          
          {step < 3 ? (
            <button className="btn-next" onClick={nextStep}>
              Next <FiArrowRight />
            </button>
          ) : (
            <button 
              className="btn-complete" 
              onClick={handleSubmit}
              disabled={loading}
            >
              {loading ? 'Saving...' : 'Complete Profile'}
            </button>
          )}
        </div>

        <button 
          className="skip-link"
          onClick={() => {
            localStorage.setItem('profileCompleted', 'true');
            navigate('/dashboard');
          }}
        >
          Skip for now
        </button>
      </div>
    </div>
  );
};

export default CompleteProfile;
















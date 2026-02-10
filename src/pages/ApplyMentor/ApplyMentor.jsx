import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { motion, AnimatePresence } from 'framer-motion';
import axios from 'axios';
import './ApplyMentor.css';

const API_URL = (import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000') + '/api';

const ApplyMentor = () => {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);
  const [specializations, setSpecializations] = useState([]);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(null);
  
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    phoneNumber: '',
    specialization: '',
    yearsOfExperience: '',
    linkedInUrl: '',
    bio: '',
    currentPosition: '',
    company: ''
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    fetchSpecializations();
  }, []);

  const fetchSpecializations = async () => {
    try {
      const response = await axios.get(`${API_URL}/mentorapplications/specializations`);
      setSpecializations(response.data);
    } catch (err) {
      // Fallback specializations
      setSpecializations([
        'Software Engineering',
        'Data Science',
        'Machine Learning',
        'Web Development',
        'Mobile Development',
        'UI/UX Design',
        'DevOps',
        'Cybersecurity',
        'Cloud Computing',
        'Project Management',
        'Product Management',
        'Business Analysis',
        'Digital Marketing',
        'Other'
      ]);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    
    // Clear error when user types
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const validateStep = (stepNum) => {
    const newErrors = {};
    
    if (stepNum === 1) {
      if (!formData.fullName.trim()) newErrors.fullName = 'Full name is required';
      if (!formData.email.trim()) {
        newErrors.email = 'Email is required';
      } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
        newErrors.email = 'Invalid email format';
      }
      if (formData.phoneNumber && !/^[+]?[\d\s-]{8,20}$/.test(formData.phoneNumber)) {
        newErrors.phoneNumber = 'Invalid phone number';
      }
    }
    
    if (stepNum === 2) {
      if (!formData.specialization) newErrors.specialization = 'Please select a specialization';
      if (!formData.yearsOfExperience) {
        newErrors.yearsOfExperience = 'Years of experience is required';
      } else if (isNaN(formData.yearsOfExperience) || formData.yearsOfExperience < 0) {
        newErrors.yearsOfExperience = 'Please enter a valid number';
      }
    }
    
    if (stepNum === 3) {
      if (!formData.bio.trim()) {
        newErrors.bio = 'Please tell us about yourself';
      } else if (formData.bio.length < 50) {
        newErrors.bio = 'Bio should be at least 50 characters';
      }
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const nextStep = () => {
    if (validateStep(step)) {
      setStep(prev => Math.min(prev + 1, 4));
    }
  };

  const prevStep = () => {
    setStep(prev => Math.max(prev - 1, 1));
  };

  const handleSubmit = async () => {
    if (!validateStep(3)) return;
    
    setLoading(true);
    setError('');
    
    try {
      const response = await axios.post(`${API_URL}/mentorapplications/apply`, {
        fullName: formData.fullName,
        email: formData.email,
        phoneNumber: formData.phoneNumber || null,
        specialization: formData.specialization,
        yearsOfExperience: parseInt(formData.yearsOfExperience),
        linkedInUrl: formData.linkedInUrl || null,
        bio: formData.bio,
        currentPosition: formData.currentPosition || null,
        company: formData.company || null
      });
      
      setSuccess(response.data);
      setStep(4);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to submit application. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const renderStep1 = () => (
    <motion.div
      key="step1"
      initial={{ opacity: 0, x: 20 }}
      animate={{ opacity: 1, x: 0 }}
      exit={{ opacity: 0, x: -20 }}
      className="form-step"
    >
      <h3>Personal Information</h3>
      <p className="step-description">Let's start with your basic information</p>
      
      <div className="form-group">
        <label htmlFor="fullName">Full Name *</label>
        <input
          type="text"
          id="fullName"
          name="fullName"
          value={formData.fullName}
          onChange={handleChange}
          placeholder="Enter your full name"
          className={errors.fullName ? 'error' : ''}
        />
        {errors.fullName && <span className="error-text">{errors.fullName}</span>}
      </div>
      
      <div className="form-group">
        <label htmlFor="email">Email Address *</label>
        <input
          type="email"
          id="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          placeholder="Enter your email"
          className={errors.email ? 'error' : ''}
        />
        {errors.email && <span className="error-text">{errors.email}</span>}
      </div>
      
      <div className="form-group">
        <label htmlFor="phoneNumber">Phone Number (Optional)</label>
        <input
          type="tel"
          id="phoneNumber"
          name="phoneNumber"
          value={formData.phoneNumber}
          onChange={handleChange}
          placeholder="+20 xxx xxx xxxx"
          className={errors.phoneNumber ? 'error' : ''}
        />
        {errors.phoneNumber && <span className="error-text">{errors.phoneNumber}</span>}
      </div>
    </motion.div>
  );

  const renderStep2 = () => (
    <motion.div
      key="step2"
      initial={{ opacity: 0, x: 20 }}
      animate={{ opacity: 1, x: 0 }}
      exit={{ opacity: 0, x: -20 }}
      className="form-step"
    >
      <h3>Professional Background</h3>
      <p className="step-description">Tell us about your expertise</p>
      
      <div className="form-group">
        <label htmlFor="specialization">Specialization *</label>
        <select
          id="specialization"
          name="specialization"
          value={formData.specialization}
          onChange={handleChange}
          className={errors.specialization ? 'error' : ''}
        >
          <option value="">Select your specialization</option>
          {specializations.map(spec => (
            <option key={spec} value={spec}>{spec}</option>
          ))}
        </select>
        {errors.specialization && <span className="error-text">{errors.specialization}</span>}
      </div>
      
      <div className="form-group">
        <label htmlFor="yearsOfExperience">Years of Experience *</label>
        <input
          type="number"
          id="yearsOfExperience"
          name="yearsOfExperience"
          value={formData.yearsOfExperience}
          onChange={handleChange}
          placeholder="0"
          min="0"
          max="50"
          className={errors.yearsOfExperience ? 'error' : ''}
        />
        {errors.yearsOfExperience && <span className="error-text">{errors.yearsOfExperience}</span>}
      </div>
      
      <div className="form-row">
        <div className="form-group">
          <label htmlFor="currentPosition">Current Position</label>
          <input
            type="text"
            id="currentPosition"
            name="currentPosition"
            value={formData.currentPosition}
            onChange={handleChange}
            placeholder="e.g. Senior Developer"
          />
        </div>
        
        <div className="form-group">
          <label htmlFor="company">Company</label>
          <input
            type="text"
            id="company"
            name="company"
            value={formData.company}
            onChange={handleChange}
            placeholder="e.g. Google"
          />
        </div>
      </div>
      
      <div className="form-group">
        <label htmlFor="linkedInUrl">LinkedIn Profile (Optional)</label>
        <input
          type="url"
          id="linkedInUrl"
          name="linkedInUrl"
          value={formData.linkedInUrl}
          onChange={handleChange}
          placeholder="https://linkedin.com/in/yourprofile"
        />
      </div>
    </motion.div>
  );

  const renderStep3 = () => (
    <motion.div
      key="step3"
      initial={{ opacity: 0, x: 20 }}
      animate={{ opacity: 1, x: 0 }}
      exit={{ opacity: 0, x: -20 }}
      className="form-step"
    >
      <h3>About You</h3>
      <p className="step-description">Tell us why you want to be a mentor</p>
      
      <div className="form-group">
        <label htmlFor="bio">Your Bio *</label>
        <textarea
          id="bio"
          name="bio"
          value={formData.bio}
          onChange={handleChange}
          placeholder="Tell us about yourself, your experience, and why you want to mentor students..."
          rows={6}
          className={errors.bio ? 'error' : ''}
        />
        <div className="char-count">
          {formData.bio.length}/2000 characters
          {formData.bio.length < 50 && <span className="warning"> (minimum 50)</span>}
        </div>
        {errors.bio && <span className="error-text">{errors.bio}</span>}
      </div>
      
      <div className="info-box">
        <h4>📝 What happens next?</h4>
        <ul>
          <li>You'll receive an email with a test link</li>
          <li>Complete the 20-minute assessment (15 questions)</li>
          <li>Score 70% or higher to become a mentor</li>
          <li>Get instant access to the Mentor Dashboard</li>
        </ul>
      </div>
    </motion.div>
  );

  const renderStep4 = () => (
    <motion.div
      key="step4"
      initial={{ opacity: 0, scale: 0.9 }}
      animate={{ opacity: 1, scale: 1 }}
      className="form-step success-step"
    >
      <div className="success-icon">✉️</div>
      <h3>Application Submitted!</h3>
      <p className="success-message">
        We've sent a test link to <strong>{formData.email}</strong>
      </p>
      
      <div className="success-info">
        <h4>Next Steps:</h4>
        <ol>
          <li>Check your email inbox (and spam folder)</li>
          <li>Click the test link within 48 hours</li>
          <li>Complete the 20-minute assessment</li>
          <li>Score 70%+ to become a GradLink Mentor!</li>
        </ol>
      </div>
      
      <div className="success-actions">
        <button 
          className="btn-primary"
          onClick={() => navigate('/')}
        >
          Back to Home
        </button>
        <button 
          className="btn-secondary"
          onClick={() => window.open('https://mail.google.com', '_blank')}
        >
          Open Gmail
        </button>
      </div>
    </motion.div>
  );

  return (
    <div className="apply-mentor-page">
      <div className="apply-mentor-container">
        <motion.div 
          className="apply-mentor-header"
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
        >
          <h1>🎓 Become a Mentor</h1>
          <p>Share your knowledge and help shape the next generation of professionals</p>
        </motion.div>

        {step < 4 && (
          <div className="progress-bar">
            <div className="progress-steps">
              {[1, 2, 3].map(num => (
                <div 
                  key={num}
                  className={`progress-step ${step >= num ? 'active' : ''} ${step > num ? 'completed' : ''}`}
                >
                  <div className="step-number">
                    {step > num ? '✓' : num}
                  </div>
                  <span className="step-label">
                    {num === 1 ? 'Personal' : num === 2 ? 'Professional' : 'About You'}
                  </span>
                </div>
              ))}
            </div>
            <div className="progress-line">
              <div 
                className="progress-fill"
                style={{ width: `${((step - 1) / 2) * 100}%` }}
              />
            </div>
          </div>
        )}

        {error && (
          <motion.div 
            className="error-banner"
            initial={{ opacity: 0, y: -10 }}
            animate={{ opacity: 1, y: 0 }}
          >
            {error}
          </motion.div>
        )}

        <div className="form-container">
          <AnimatePresence mode="wait">
            {step === 1 && renderStep1()}
            {step === 2 && renderStep2()}
            {step === 3 && renderStep3()}
            {step === 4 && renderStep4()}
          </AnimatePresence>
        </div>

        {step < 4 && (
          <div className="form-navigation">
            {step > 1 && (
              <button 
                className="btn-back"
                onClick={prevStep}
                disabled={loading}
              >
                ← Back
              </button>
            )}
            
            {step < 3 ? (
              <button 
                className="btn-next"
                onClick={nextStep}
              >
                Next →
              </button>
            ) : (
              <button 
                className="btn-submit"
                onClick={handleSubmit}
                disabled={loading}
              >
                {loading ? (
                  <>
                    <span className="spinner"></span>
                    Submitting...
                  </>
                ) : (
                  'Submit Application'
                )}
              </button>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default ApplyMentor;
















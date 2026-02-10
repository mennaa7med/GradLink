import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import './Mentors.css';
import * as mentorsApi from '../api/mentors';

const Mentors = () => {
  const { user } = useAuth();
  const navigate = useNavigate();
  
  const [mentors, setMentors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedMentor, setSelectedMentor] = useState(null);
  const [popupType, setPopupType] = useState('');
  const [showPopup, setShowPopup] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [applyForm, setApplyForm] = useState({
    fullName: '',
    email: '',
    specialization: '',
    bio: '',
    jobTitle: '',
    skills: '',
    experienceYears: '',
    whatsApp: '',
    linkedInUrl: ''
  });
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    loadMentors();
  }, []);

  // Pre-fill form with user data when logged in
  useEffect(() => {
    if (user && popupType === 'apply') {
      setApplyForm(prev => ({
        ...prev,
        fullName: prev.fullName || user.fullName || '',
        email: prev.email || user.email || ''
      }));
    }
  }, [user, popupType]);

  const loadMentors = async () => {
    try {
      setLoading(true);
      const data = await mentorsApi.listMentors();
      setMentors(data);
      setError(null);
    } catch (err) {
      console.error('Failed to load mentors:', err);
      setError('Failed to load mentors. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    if (!searchQuery.trim()) {
      loadMentors();
      return;
    }
    
    try {
      setLoading(true);
      const data = await mentorsApi.searchMentors(searchQuery);
      setMentors(data);
    } catch (err) {
      console.error('Search failed:', err);
    } finally {
      setLoading(false);
    }
  };

  const openPopup = (mentor, type) => {
    // Check if user is logged in when trying to apply
    if (type === 'apply' && !user) {
      if (window.confirm('You need to be logged in to apply as a mentor. Would you like to log in now?')) {
        navigate('/login');
      }
      return;
    }
    
    setSelectedMentor(mentor);
    setPopupType(type);
    setShowPopup(true);
  };

  const closePopup = () => {
    setShowPopup(false);
    setSelectedMentor(null);
    setPopupType('');
  };

  const handleApplyChange = (e) => {
    const { name, value } = e.target;
    setApplyForm(prev => ({ ...prev, [name]: value }));
  };

  const handleApplySubmit = async (e) => {
    e.preventDefault();
    
    // Double check user is logged in
    if (!user) {
      alert('You must be logged in to apply as a mentor.');
      navigate('/login');
      return;
    }
    
    setSubmitting(true);
    
    try {
      await mentorsApi.applyAsMentor({
        ...applyForm,
        experienceYears: applyForm.experienceYears ? parseInt(applyForm.experienceYears) : null
      });
      alert('Application submitted successfully! We will review your profile.');
      closePopup();
      setApplyForm({
        fullName: '',
        email: '',
        specialization: '',
        bio: '',
        jobTitle: '',
        skills: '',
        experienceYears: '',
        whatsApp: '',
        linkedInUrl: ''
      });
      loadMentors();
    } catch (err) {
      console.error('Failed to apply:', err);
      
      // Handle specific error cases
      if (err.response?.status === 401) {
        alert('Your session has expired. Please log in again.');
        navigate('/login');
      } else if (err.response?.status === 400) {
        alert(err.response?.data?.error || 'You have already applied as a mentor or this email is already in use.');
      } else {
        alert(err.response?.data?.error || 'Failed to submit application. Please try again.');
      }
    } finally {
      setSubmitting(false);
    }
  };

  const getDefaultImage = (name) => {
    return `https://ui-avatars.com/api/?name=${encodeURIComponent(name)}&background=random&size=200`;
  };

  return (
    <div className="mentors-container">
      <div className="mentors-header">
        <h1>Meet Our Mentors</h1>
        <p>Connect with experienced professionals ready to support your project journey.</p>
        
        <div className="search-box">
          <input
            type="text"
            placeholder="Search by name, skills, or specialization..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
          />
          <button onClick={handleSearch}>Search</button>
        </div>
      </div>

      {loading ? (
        <div className="loading">Loading mentors...</div>
      ) : error ? (
        <div className="error">{error}</div>
      ) : mentors.length === 0 ? (
        <div className="no-mentors">
          <p>No mentors found. Be the first to become a mentor!</p>
        </div>
      ) : (
        <div className="mentor-cards">
          {mentors.map((mentor) => (
            <div className="mentor-card" key={mentor.id}>
              <img 
                src={mentor.profilePicture || getDefaultImage(mentor.fullName)} 
                alt={mentor.fullName} 
                className="mentor-img" 
              />
              <h2>{mentor.fullName}</h2>
              <p className="mentor-field">{mentor.specialization || 'General'}</p>
              {mentor.jobTitle && <p className="mentor-title">{mentor.jobTitle}</p>}
              <p className="mentor-desc">{mentor.bio || 'Experienced mentor ready to help.'}</p>
              {mentor.experienceYears && (
                <p className="mentor-exp">{mentor.experienceYears} years of experience</p>
              )}
              <div className="mentor-buttons">
                <button onClick={() => openPopup(mentor, 'profile')} className="btn profile-btn">View Profile</button>
                <button onClick={() => openPopup(mentor, 'chat')} className="btn chat-btn">Contact</button>
              </div>
            </div>
          ))}
        </div>
      )}

      <div className="become-mentor">
        <h2>Want to become a Mentor?</h2>
        <p>Share your knowledge and guide the next generation of innovators.</p>
        <button className="btn apply-btn" onClick={() => navigate('/apply-mentor')}>
          Apply Now
        </button>
      </div>

      {/* Pop-up Modal */}
      {showPopup && (
        <div className="popup-overlay" onClick={closePopup}>
          <div className="popup-content" onClick={(e) => e.stopPropagation()}>
            <button className="popup-close" onClick={closePopup}>✖</button>

            {popupType === 'profile' && selectedMentor && (
              <>
                <img 
                  src={selectedMentor.profilePicture || getDefaultImage(selectedMentor.fullName)} 
                  alt={selectedMentor.fullName}
                  className="profile-img"
                />
                <h2>{selectedMentor.fullName}</h2>
                {selectedMentor.jobTitle && <p><strong>Position:</strong> {selectedMentor.jobTitle}</p>}
                <p><strong>Specialization:</strong> {selectedMentor.specialization || 'General'}</p>
                {selectedMentor.experienceYears && (
                  <p><strong>Experience:</strong> {selectedMentor.experienceYears} years</p>
                )}
                {selectedMentor.skills && <p><strong>Skills:</strong> {selectedMentor.skills}</p>}
                <p>{selectedMentor.bio}</p>
                <p><strong>Email:</strong> {selectedMentor.email}</p>
              </>
            )}

            {popupType === 'chat' && selectedMentor && (
              <>
                <h2>Contact {selectedMentor.fullName}</h2>
                <ul className="contact-list">
                  {selectedMentor.facebookUrl && (
                    <li><a href={selectedMentor.facebookUrl} target="_blank" rel="noreferrer">Facebook</a></li>
                  )}
                  {selectedMentor.whatsApp && (
                    <li><a href={`https://wa.me/${selectedMentor.whatsApp.replace(/\D/g, '')}`} target="_blank" rel="noreferrer">WhatsApp</a></li>
                  )}
                  {selectedMentor.linkedInUrl && (
                    <li><a href={selectedMentor.linkedInUrl} target="_blank" rel="noreferrer">LinkedIn</a></li>
                  )}
                  <li><a href={`mailto:${selectedMentor.email}`}>Email: {selectedMentor.email}</a></li>
                </ul>
              </>
            )}

            {popupType === 'apply' && (
              <>
                <h2>Mentor Application Form</h2>
                <form className="apply-form" onSubmit={handleApplySubmit}>
                  <input 
                    type="text" 
                    name="fullName"
                    placeholder="Full Name" 
                    value={applyForm.fullName}
                    onChange={handleApplyChange}
                    required 
                  />
                  <input 
                    type="email" 
                    name="email"
                    placeholder="Email Address" 
                    value={applyForm.email}
                    onChange={handleApplyChange}
                    required 
                  />
                  <input 
                    type="text" 
                    name="jobTitle"
                    placeholder="Job Title" 
                    value={applyForm.jobTitle}
                    onChange={handleApplyChange}
                  />
                  <input 
                    type="text" 
                    name="specialization"
                    placeholder="Field of Expertise" 
                    value={applyForm.specialization}
                    onChange={handleApplyChange}
                    required 
                  />
                  <input 
                    type="text" 
                    name="skills"
                    placeholder="Skills (comma-separated)" 
                    value={applyForm.skills}
                    onChange={handleApplyChange}
                  />
                  <input 
                    type="number" 
                    name="experienceYears"
                    placeholder="Years of Experience" 
                    value={applyForm.experienceYears}
                    onChange={handleApplyChange}
                    min="0"
                    max="50"
                  />
                  <input 
                    type="text" 
                    name="whatsApp"
                    placeholder="WhatsApp Number (optional)" 
                    value={applyForm.whatsApp}
                    onChange={handleApplyChange}
                  />
                  <input 
                    type="url" 
                    name="linkedInUrl"
                    placeholder="LinkedIn URL (optional)" 
                    value={applyForm.linkedInUrl}
                    onChange={handleApplyChange}
                  />
                  <textarea 
                    name="bio"
                    placeholder="Brief Bio / Experience" 
                    value={applyForm.bio}
                    onChange={handleApplyChange}
                    required
                  ></textarea>
                  <button type="submit" className="btn submit-btn" disabled={submitting}>
                    {submitting ? 'Submitting...' : 'Submit Application'}
                  </button>
                </form>
              </>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default Mentors;

import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { motion } from 'framer-motion';
import { 
  FiMail, FiMapPin, FiCalendar, FiBriefcase, FiBook,
  FiGithub, FiLinkedin, FiGlobe, FiMessageSquare, FiArrowLeft
} from 'react-icons/fi';
import api from '../../api/client';
import './PublicProfile.css';

const PublicProfile = () => {
  const { userId } = useParams();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadProfile();
  }, [userId]);

  const loadProfile = async () => {
    try {
      const { data } = await api.get(`/api/users/${userId}/public`);
      setProfile(data);
    } catch (err) {
      if (err.response?.status === 404) {
        setError('User not found');
      } else if (err.response?.status === 403) {
        setError('This profile is private');
      } else {
        setError('Failed to load profile');
      }
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="public-profile-page">
        <div className="profile-loading">
          <div className="loading-spinner"></div>
          <p>Loading profile...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="public-profile-page">
        <div className="profile-error">
          <h2>😔 {error}</h2>
          <Link to="/" className="back-btn">
            <FiArrowLeft /> Back to Home
          </Link>
        </div>
      </div>
    );
  }

  const skills = profile?.skills?.split(',').map(s => s.trim()).filter(Boolean) || [];

  return (
    <div className="public-profile-page">
      <div className="profile-container">
        <Link to="/" className="back-link">
          <FiArrowLeft /> Back
        </Link>

        {/* Header Section */}
        <motion.div 
          className="profile-header"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
        >
          <div className="profile-cover"></div>
          <div className="profile-avatar">
            {profile?.profilePicture ? (
              <img src={profile.profilePicture} alt={profile.fullName} />
            ) : (
              <span>{profile?.fullName?.charAt(0) || 'U'}</span>
            )}
          </div>
          
          <div className="profile-info">
            <h1>{profile?.fullName}</h1>
            <p className="role-badge">{profile?.role}</p>
            
            <div className="profile-meta">
              {profile?.location && (
                <span><FiMapPin /> {profile.location}</span>
              )}
              {profile?.university && (
                <span><FiBook /> {profile.university}</span>
              )}
              {profile?.company && (
                <span><FiBriefcase /> {profile.company}</span>
              )}
              {profile?.createdAt && (
                <span><FiCalendar /> Joined {new Date(profile.createdAt).toLocaleDateString()}</span>
              )}
            </div>

            <div className="profile-social">
              {profile?.linkedInUrl && (
                <a href={profile.linkedInUrl} target="_blank" rel="noopener noreferrer">
                  <FiLinkedin />
                </a>
              )}
              {profile?.gitHubUrl && (
                <a href={profile.gitHubUrl} target="_blank" rel="noopener noreferrer">
                  <FiGithub />
                </a>
              )}
              {profile?.websiteUrl && (
                <a href={profile.websiteUrl} target="_blank" rel="noopener noreferrer">
                  <FiGlobe />
                </a>
              )}
            </div>
          </div>
        </motion.div>

        <div className="profile-content">
          {/* Bio Section */}
          {profile?.bio && (
            <motion.section 
              className="profile-section"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.1 }}
            >
              <h2>About</h2>
              <p className="bio-text">{profile.bio}</p>
            </motion.section>
          )}

          {/* Skills Section */}
          {skills.length > 0 && (
            <motion.section 
              className="profile-section"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.2 }}
            >
              <h2>Skills</h2>
              <div className="skills-grid">
                {skills.map((skill, index) => (
                  <span key={index} className="skill-tag">{skill}</span>
                ))}
              </div>
            </motion.section>
          )}

          {/* Stats Section */}
          <motion.section 
            className="profile-section stats-section"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.3 }}
          >
            <h2>Activity</h2>
            <div className="stats-grid">
              <div className="stat-card">
                <span className="stat-value">{profile?.projectsCount || 0}</span>
                <span className="stat-label">Projects</span>
              </div>
              <div className="stat-card">
                <span className="stat-value">{profile?.applicationsCount || 0}</span>
                <span className="stat-label">Applications</span>
              </div>
              <div className="stat-card">
                <span className="stat-value">{profile?.completedProjectsCount || 0}</span>
                <span className="stat-label">Completed</span>
              </div>
            </div>
          </motion.section>

          {/* Education Section (for students) */}
          {profile?.role === 'Student' && (profile?.university || profile?.major) && (
            <motion.section 
              className="profile-section"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.4 }}
            >
              <h2>Education</h2>
              <div className="education-card">
                {profile?.university && <h3>{profile.university}</h3>}
                {profile?.faculty && <p>{profile.faculty}</p>}
                {profile?.major && <p>Major: {profile.major}</p>}
                {profile?.graduationYear && <p>Class of {profile.graduationYear}</p>}
              </div>
            </motion.section>
          )}

          {/* Contact Button */}
          <motion.div 
            className="contact-action"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.5 }}
          >
            <button className="contact-btn">
              <FiMessageSquare /> Send Message
            </button>
          </motion.div>
        </div>
      </div>
    </div>
  );
};

export default PublicProfile;
















import React, { useState } from 'react';
import { motion } from 'framer-motion';
import './Applicants.css';

const Applicants = () => {
  const [filterStatus, setFilterStatus] = useState('all');
  const [applicants] = useState([
    {
      id: 1,
      name: 'Ahmed Hassan',
      field: 'Software Engineering',
      matchPercentage: 95,
      status: 'accepted',
      project: 'Mobile App Development',
      skills: ['React Native', 'JavaScript', 'Firebase'],
      experience: '2 years',
      location: 'Cairo, Egypt',
      appliedDate: '2024-01-20'
    },
    {
      id: 2,
      name: 'Sarah Mohamed',
      field: 'UI/UX Design',
      matchPercentage: 87,
      status: 'pending',
      project: 'Web Development Internship',
      skills: ['Figma', 'Adobe XD', 'Sketch'],
      experience: '1 year',
      location: 'Alexandria, Egypt',
      appliedDate: '2024-01-18'
    },
    {
      id: 3,
      name: 'ahmed ali',
      field: 'Data Science',
      matchPercentage: 92,
      status: 'accepted',
      project: 'Data Science Position',
      skills: ['Python', 'TensorFlow', 'SQL'],
      experience: '3 years',
      location: 'Giza, Egypt',
      appliedDate: '2024-01-15'
    },
    {
      id: 4,
      name: 'Fatma Ibrahim',
      field: 'Frontend Development',
      matchPercentage: 78,
      status: 'pending',
      project: 'UI/UX Design Role',
      skills: ['React', 'CSS', 'JavaScript'],
      experience: '1.5 years',
      location: 'Cairo, Egypt',
      appliedDate: '2024-01-12'
    },
    {
      id: 5,
      name: 'Mahmoud Khalil',
      field: 'Backend Development',
      matchPercentage: 65,
      status: 'rejected',
      project: 'Mobile App Development',
      skills: ['Node.js', 'MongoDB', 'Express'],
      experience: '2 years',
      location: 'Alexandria, Egypt',
      appliedDate: '2024-01-10'
    }
  ]);

  const getMatchLevel = (percentage) => {
    if (percentage >= 90) return 'high';
    if (percentage >= 75) return 'medium';
    if (percentage >= 60) return 'low';
    return 'very-low';
  };

  const getStatusIcon = (status) => {
    const icons = {
      accepted: '✅',
      pending: '⏳',
      rejected: '❌'
    };
    return icons[status] || icons.pending;
  };

  const filteredApplicants = filterStatus === 'all' 
    ? applicants 
    : applicants.filter(applicant => applicant.status === filterStatus);

  const stats = {
    total: applicants.length,
    accepted: applicants.filter(a => a.status === 'accepted').length,
    pending: applicants.filter(a => a.status === 'pending').length,
    rejected: applicants.filter(a => a.status === 'rejected').length
  };

  return (
    <div className="applicants-page">
      {/* Page Header */}
      <div className="applicants-page-header">
        <div>
          <h1 className="applicants-page-title">Applicants</h1>
          <p className="applicants-page-description">Review and manage job applicants</p>
        </div>
        
        {/* Filter Buttons */}
        <div className="applicants-filters">
          <button
            className={`applicants-filter-button ${filterStatus === 'all' ? 'active' : ''}`}
            onClick={() => setFilterStatus('all')}
          >
            All ({stats.total})
          </button>
          <button
            className={`applicants-filter-button ${filterStatus === 'accepted' ? 'active' : ''}`}
            onClick={() => setFilterStatus('accepted')}
          >
            Accepted ({stats.accepted})
          </button>
          <button
            className={`applicants-filter-button ${filterStatus === 'pending' ? 'active' : ''}`}
            onClick={() => setFilterStatus('pending')}
          >
            Pending ({stats.pending})
          </button>
          <button
            className={`applicants-filter-button ${filterStatus === 'rejected' ? 'active' : ''}`}
            onClick={() => setFilterStatus('rejected')}
          >
            Rejected ({stats.rejected})
          </button>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="applicants-stats-grid">
        <div className="applicants-stat-card">
          <div className="applicants-stat-value">{stats.total}</div>
          <div className="applicants-stat-label">Total Applicants</div>
        </div>
        <div className="applicants-stat-card">
          <div className="applicants-stat-value">{stats.accepted}</div>
          <div className="applicants-stat-label">Accepted</div>
        </div>
        <div className="applicants-stat-card">
          <div className="applicants-stat-value">{stats.pending}</div>
          <div className="applicants-stat-label">Pending Review</div>
        </div>
        <div className="applicants-stat-card">
          <div className="applicants-stat-value">{stats.rejected}</div>
          <div className="applicants-stat-label">Rejected</div>
        </div>
      </div>

      {/* Applicants Grid */}
      {filteredApplicants.length > 0 ? (
        <div className="applicants-grid">
          {filteredApplicants.map((applicant, index) => {
            const matchLevel = getMatchLevel(applicant.matchPercentage);
            
            return (
              <motion.div
                key={applicant.id}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: index * 0.1 }}
                className="applicant-card"
              >
                {/* Applicant Header */}
                <div className="applicant-header">
                  <div className="applicant-info">
                    <div className="applicant-avatar">
                      {applicant.name.split(' ').map(n => n[0]).join('')}
                    </div>
                    <div className="applicant-details">
                      <div className="applicant-name">{applicant.name}</div>
                      <div className="applicant-field">{applicant.field}</div>
                    </div>
                  </div>
                  <div className={`applicant-status ${applicant.status}`}>
                    <span className="applicant-status-icon">
                      {getStatusIcon(applicant.status)}
                    </span>
                    <span>{applicant.status.charAt(0).toUpperCase() + applicant.status.slice(1)}</span>
                  </div>
                </div>

                {/* Match Percentage */}
                <div className="applicant-match">
                  <div className="applicant-match-header">
                    <span className="applicant-match-label">Match Score</span>
                    <span className={`applicant-match-percentage ${matchLevel}`}>
                      {applicant.matchPercentage}%
                    </span>
                  </div>
                  <div className="applicant-match-bar">
                    <div 
                      className={`applicant-match-progress ${matchLevel}`}
                      style={{ width: `${applicant.matchPercentage}%` }}
                    />
                  </div>
                </div>

                {/* Applicant Details */}
                <div className="applicant-details-section">
                  <div className="applicant-detail-item">
                    <span className="applicant-detail-icon">📁</span>
                    <span className="applicant-project">
                      Applied for: <span className="applicant-project-name">{applicant.project}</span>
                    </span>
                  </div>
                  <div className="applicant-detail-item">
                    <span className="applicant-detail-icon">💼</span>
                    <span>{applicant.experience} experience</span>
                  </div>
                  <div className="applicant-detail-item">
                    <span className="applicant-detail-icon">📍</span>
                    <span>{applicant.location}</span>
                  </div>
                </div>

                {/* Skills */}
                <div className="applicant-skills">
                  <div className="applicant-skills-label">Skills</div>
                  <div className="applicant-skills-list">
                    {applicant.skills.map((skill, idx) => (
                      <span key={idx} className="applicant-skill-tag">
                        {skill}
                      </span>
                    ))}
                  </div>
                </div>

                {/* Actions */}
                <div className="applicant-actions">
                  <motion.button
                    whileHover={{ scale: 1.02 }}
                    whileTap={{ scale: 0.98 }}
                    className="applicant-action-button primary"
                  >
                    <span className="applicant-action-icon">👁️</span>
                    <span>View Resume</span>
                  </motion.button>
                  <motion.button
                    whileHover={{ scale: 1.02 }}
                    whileTap={{ scale: 0.98 }}
                    className="applicant-action-button secondary"
                  >
                    <span className="applicant-action-icon">💬</span>
                    <span>Contact</span>
                  </motion.button>
                </div>
              </motion.div>
            );
          })}
        </div>
      ) : (
        <div className="applicants-empty">
          <div className="applicants-empty-icon">👥</div>
          <h3 className="applicants-empty-title">No applicants found</h3>
          <p className="applicants-empty-description">
            No applicants match the current filter criteria.
          </p>
        </div>
      )}
    </div>
  );
};

export default Applicants;
import React, { useState, useEffect } from 'react';
import './Matchmaking.css';
import { FiPlus, FiSearch, FiUsers, FiMail, FiX, FiCode, FiPenTool, FiDatabase, FiSmartphone, FiGlobe, FiCpu, FiLoader } from 'react-icons/fi';
import { getTeamRequests, createTeamRequest, sendJoinRequest, getStats } from '../api/teamRequests';

const Matchmaking = () => {
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showContactModal, setShowContactModal] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [filterSkill, setFilterSkill] = useState('all');
  
  // Form states
  const [createForm, setCreateForm] = useState({
    projectName: '',
    description: '',
    category: '',
    membersNeeded: 1,
    skillsNeeded: '',
    contactEmail: ''
  });
  
  const [joinForm, setJoinForm] = useState({
    applicantName: '',
    applicantEmail: '',
    applicantSkills: '',
    message: ''
  });

  // Data states
  const [teamRequests, setTeamRequests] = useState([]);
  const [stats, setStats] = useState({ ActiveRequests: 0, MembersNeeded: 0, SkillsRequired: 0 });
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  // Check if user is logged in
  const isLoggedIn = !!localStorage.getItem('accessToken');

  // Fetch data on mount
  useEffect(() => {
    fetchTeamRequests();
    fetchStats();
  }, []);

  const fetchTeamRequests = async () => {
    try {
      setLoading(true);
      const data = await getTeamRequests();
      setTeamRequests(data);
      setError(null);
    } catch (err) {
      console.error('Error fetching team requests:', err);
      setError('Failed to load team requests');
    } finally {
      setLoading(false);
    }
  };

  const fetchStats = async () => {
    try {
      const data = await getStats();
      setStats(data);
    } catch (err) {
      console.error('Error fetching stats:', err);
    }
  };

  const skillIcons = {
    'Machine Learning': <FiCpu />,
    'Python': <FiCode />,
    'Computer Vision': <FiCpu />,
    'React Native': <FiSmartphone />,
    'Node.js': <FiDatabase />,
    'UI/UX Design': <FiPenTool />,
    'React': <FiCode />,
    'PostgreSQL': <FiDatabase />,
    'Backend Development': <FiDatabase />,
    'IoT': <FiGlobe />,
    'Embedded Systems': <FiCpu />,
    'Mobile Development': <FiSmartphone />,
    'Unity': <FiCode />,
    'Game Design': <FiPenTool />,
    '3D Modeling': <FiPenTool />
  };

  const allSkills = [...new Set(teamRequests.flatMap(r => r.skillsNeeded || []))];

  const filteredRequests = teamRequests.filter(request => {
    const matchesSearch = 
      request.projectName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      request.description?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      (request.skillsNeeded || []).some(skill => skill.toLowerCase().includes(searchTerm.toLowerCase()));
    const matchesFilter = filterSkill === 'all' || (request.skillsNeeded || []).includes(filterSkill);
    return matchesSearch && matchesFilter;
  });

  const handleContactClick = (request) => {
    if (!isLoggedIn) {
      alert('Please sign in to send a join request');
      return;
    }
    setSelectedRequest(request);
    setShowContactModal(true);
  };

  const handleCreateClick = () => {
    if (!isLoggedIn) {
      alert('Please sign in to post a team request');
      return;
    }
    setShowCreateModal(true);
  };

  const handleSendMessage = async (e) => {
    e.preventDefault();
    if (!selectedRequest) return;

    try {
      setSubmitting(true);
      await sendJoinRequest(selectedRequest.id, {
        applicantName: joinForm.applicantName,
        applicantEmail: joinForm.applicantEmail,
        applicantSkills: joinForm.applicantSkills,
        message: joinForm.message
      });
      
      alert(`Your join request has been sent to ${selectedRequest.ownerName}!`);
      setJoinForm({ applicantName: '', applicantEmail: '', applicantSkills: '', message: '' });
      setShowContactModal(false);
    } catch (err) {
      console.error('Error sending join request:', err);
      alert(err.response?.data || 'Failed to send join request. Please try again.');
    } finally {
      setSubmitting(false);
    }
  };

  const handleCreateTeam = async (e) => {
    e.preventDefault();
    
    try {
      setSubmitting(true);
      const newRequest = await createTeamRequest({
        projectName: createForm.projectName,
        description: createForm.description,
        category: createForm.category,
        membersNeeded: parseInt(createForm.membersNeeded),
        skillsNeeded: createForm.skillsNeeded,
        contactEmail: createForm.contactEmail
      });
      
      // Add new request to the list
      setTeamRequests(prev => [newRequest, ...prev]);
      
      // Update stats
      fetchStats();
      
      alert('Team request created successfully!');
      setCreateForm({
        projectName: '',
        description: '',
        category: '',
        membersNeeded: 1,
        skillsNeeded: '',
        contactEmail: ''
      });
      setShowCreateModal(false);
    } catch (err) {
      console.error('Error creating team request:', err);
      alert(err.response?.data || 'Failed to create team request. Please try again.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="matchmaking-page">
      {/* Hero Section */}
      <div className="matchmaking-hero">
        <div className="hero-content">
          <h1>Find Your Dream Team</h1>
          <p>Connect with talented individuals who share your vision. Post your project idea or join an existing team to bring amazing projects to life.</p>
          <button className="create-request-btn" onClick={handleCreateClick}>
            <FiPlus /> Post Team Request
          </button>
        </div>
        <div className="hero-stats">
          <div className="stat-item">
            <span className="stat-number">{stats.ActiveRequests || teamRequests.length}</span>
            <span className="stat-label">Active Requests</span>
          </div>
          <div className="stat-item">
            <span className="stat-number">{stats.MembersNeeded || teamRequests.reduce((acc, r) => acc + (r.membersNeeded || 0), 0)}</span>
            <span className="stat-label">Members Needed</span>
          </div>
          <div className="stat-item">
            <span className="stat-number">{stats.SkillsRequired || allSkills.length}+</span>
            <span className="stat-label">Skills Required</span>
          </div>
        </div>
      </div>

      {/* Search & Filter Section */}
      <div className="search-filter-section">
        <div className="search-box">
          <FiSearch className="search-icon" />
          <input
            type="text"
            placeholder="Search projects, skills, or keywords..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
        <div className="filter-box">
          <select value={filterSkill} onChange={(e) => setFilterSkill(e.target.value)}>
            <option value="all">All Skills</option>
            {allSkills.map(skill => (
              <option key={skill} value={skill}>{skill}</option>
            ))}
          </select>
        </div>
      </div>

      {/* Team Requests Grid */}
      <div className="requests-section">
        <h2 className="section-title">
          <FiUsers /> Team Requests
          <span className="count-badge">{filteredRequests.length}</span>
        </h2>

        {loading ? (
          <div className="loading-state">
            <FiLoader className="spinner" />
            <p>Loading team requests...</p>
          </div>
        ) : error ? (
          <div className="error-state">
            <span className="error-icon">⚠️</span>
            <h3>{error}</h3>
            <button onClick={fetchTeamRequests}>Try Again</button>
          </div>
        ) : filteredRequests.length === 0 ? (
          <div className="no-results">
            <span className="no-results-icon">🔍</span>
            <h3>No requests found</h3>
            <p>Try adjusting your search or filter criteria, or be the first to post a team request!</p>
          </div>
        ) : (
          <div className="requests-grid">
            {filteredRequests.map(request => (
              <div key={request.id} className="request-card">
                <div className="card-header">
                  <span className="category-badge">{request.category}</span>
                  <span className="posted-date">{request.postedDate}</span>
                </div>

                <h3 className="project-title">{request.projectName}</h3>
                <p className="project-description">{request.description}</p>

                <div className="skills-section">
                  <h4>Skills Needed:</h4>
                  <div className="skills-list">
                    {(request.skillsNeeded || []).map(skill => (
                      <span key={skill} className="skill-tag">
                        {skillIcons[skill] || <FiCode />}
                        {skill}
                      </span>
                    ))}
                  </div>
                </div>

                <div className="team-info">
                  <div className="members-needed">
                    <FiUsers />
                    <span>{request.currentMembers}/{request.currentMembers + request.membersNeeded} members</span>
                    <span className="spots-left">({request.membersNeeded} spots left)</span>
                  </div>
                </div>

                <div className="card-footer">
                  <div className="owner-info">
                    {request.ownerAvatar && request.ownerAvatar.startsWith('http') ? (
                      <img src={request.ownerAvatar} alt={request.ownerName} className="owner-avatar-img" />
                    ) : (
                      <span className="owner-avatar">{request.ownerAvatar || '👨‍💻'}</span>
                    )}
                    <span className="owner-name">{request.ownerName}</span>
                  </div>
                  <button className="contact-btn" onClick={() => handleContactClick(request)}>
                    <FiMail /> Join Team
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Create Team Request Modal */}
      {showCreateModal && (
        <div className="modal-overlay" onClick={() => setShowCreateModal(false)}>
          <div className="modal-content create-modal" onClick={(e) => e.stopPropagation()}>
            <button className="close-btn" onClick={() => setShowCreateModal(false)}>
              <FiX />
            </button>
            <div className="modal-header">
              <span className="modal-icon">🚀</span>
              <h2>Post a Team Request</h2>
              <p>Describe your project and find the perfect teammates</p>
            </div>
            <form onSubmit={handleCreateTeam}>
              <div className="form-group">
                <label>Project Name *</label>
                <input 
                  type="text" 
                  placeholder="Enter your project name" 
                  value={createForm.projectName}
                  onChange={(e) => setCreateForm({...createForm, projectName: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Project Description *</label>
                <textarea 
                  placeholder="Describe your project idea, goals, and what you're building..." 
                  rows="4" 
                  value={createForm.description}
                  onChange={(e) => setCreateForm({...createForm, description: e.target.value})}
                  required
                ></textarea>
              </div>
              <div className="form-row">
                <div className="form-group">
                  <label>Category *</label>
                  <select 
                    value={createForm.category}
                    onChange={(e) => setCreateForm({...createForm, category: e.target.value})}
                    required
                  >
                    <option value="">Select category</option>
                    <option value="Web">Web Development</option>
                    <option value="Mobile">Mobile App</option>
                    <option value="AI/ML">AI / Machine Learning</option>
                    <option value="IoT">IoT</option>
                    <option value="Gaming">Gaming</option>
                    <option value="Other">Other</option>
                  </select>
                </div>
                <div className="form-group">
                  <label>Team Size Needed *</label>
                  <input 
                    type="number" 
                    min="1" 
                    max="10" 
                    placeholder="How many members?" 
                    value={createForm.membersNeeded}
                    onChange={(e) => setCreateForm({...createForm, membersNeeded: e.target.value})}
                    required 
                  />
                </div>
              </div>
              <div className="form-group">
                <label>Skills Required *</label>
                <input 
                  type="text" 
                  placeholder="e.g., React, Python, UI/UX Design (comma separated)" 
                  value={createForm.skillsNeeded}
                  onChange={(e) => setCreateForm({...createForm, skillsNeeded: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Your Contact Email *</label>
                <input 
                  type="email" 
                  placeholder="your.email@example.com" 
                  value={createForm.contactEmail}
                  onChange={(e) => setCreateForm({...createForm, contactEmail: e.target.value})}
                  required 
                />
              </div>
              <button type="submit" className="submit-btn" disabled={submitting}>
                {submitting ? <><FiLoader className="spinner" /> Creating...</> : <><FiPlus /> Post Request</>}
              </button>
            </form>
          </div>
        </div>
      )}

      {/* Contact/Join Modal */}
      {showContactModal && selectedRequest && (
        <div className="modal-overlay" onClick={() => setShowContactModal(false)}>
          <div className="modal-content contact-modal" onClick={(e) => e.stopPropagation()}>
            <button className="close-btn" onClick={() => setShowContactModal(false)}>
              <FiX />
            </button>
            <div className="modal-header">
              <span className="modal-icon">✉️</span>
              <h2>Request to Join Team</h2>
              <p>Send a message to {selectedRequest.ownerName}</p>
            </div>
            <div className="project-preview">
              <h4>{selectedRequest.projectName}</h4>
              <div className="preview-skills">
                {(selectedRequest.skillsNeeded || []).map(skill => (
                  <span key={skill} className="skill-tag small">{skill}</span>
                ))}
              </div>
            </div>
            <form onSubmit={handleSendMessage}>
              <div className="form-group">
                <label>Your Name *</label>
                <input 
                  type="text" 
                  placeholder="Enter your full name" 
                  value={joinForm.applicantName}
                  onChange={(e) => setJoinForm({...joinForm, applicantName: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Your Email *</label>
                <input 
                  type="email" 
                  placeholder="your.email@example.com" 
                  value={joinForm.applicantEmail}
                  onChange={(e) => setJoinForm({...joinForm, applicantEmail: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Your Skills *</label>
                <input 
                  type="text" 
                  placeholder="e.g., React, Node.js, UI Design" 
                  value={joinForm.applicantSkills}
                  onChange={(e) => setJoinForm({...joinForm, applicantSkills: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Message to Team Owner *</label>
                <textarea 
                  placeholder="Introduce yourself, explain why you're interested in this project, and what you can contribute..."
                  rows="4"
                  value={joinForm.message}
                  onChange={(e) => setJoinForm({...joinForm, message: e.target.value})}
                  required
                ></textarea>
              </div>
              <button type="submit" className="submit-btn" disabled={submitting}>
                {submitting ? <><FiLoader className="spinner" /> Sending...</> : <><FiMail /> Send Request</>}
              </button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Matchmaking;

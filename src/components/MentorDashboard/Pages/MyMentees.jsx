import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { 
  FiSearch, 
  FiFilter, 
  FiGrid, 
  FiList,
  FiMessageSquare,
  FiCalendar,
  FiUser,
  FiX,
  FiMail,
  FiPhone,
  FiMapPin,
  FiGithub,
  FiLinkedin,
  FiTrendingUp
} from 'react-icons/fi';
import './MyMentees.css';

const MyMentees = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [viewMode, setViewMode] = useState('grid');
  const [filterOpen, setFilterOpen] = useState(false);
  const [selectedMentee, setSelectedMentee] = useState(null);
  const [filters, setFilters] = useState({
    progress: 'all',
    activity: 'all'
  });

  // Mock mentees data
  const mentees = [
    {
      id: 1,
      name: 'Ahmed Hassan',
      email: 'ahmed.hassan@university.edu',
      phone: '+20 100 123 4567',
      avatar: null,
      university: 'Cairo University',
      major: 'Computer Science',
      year: '4th Year',
      project: 'AI-Powered Learning Platform',
      progress: 85,
      lastActive: '2 hours ago',
      status: 'active',
      skills: ['Python', 'TensorFlow', 'React'],
      location: 'Cairo, Egypt',
      github: 'github.com/ahmedhassan',
      linkedin: 'linkedin.com/in/ahmedhassan'
    },
    {
      id: 2,
      name: 'Sarah Mohamed',
      email: 'sarah.m@university.edu',
      phone: '+20 101 234 5678',
      avatar: null,
      university: 'Alexandria University',
      major: 'Software Engineering',
      year: '4th Year',
      project: 'E-Commerce Mobile App',
      progress: 72,
      lastActive: '1 day ago',
      status: 'active',
      skills: ['Flutter', 'Firebase', 'Node.js'],
      location: 'Alexandria, Egypt',
      github: 'github.com/sarahm',
      linkedin: 'linkedin.com/in/sarahm'
    },
    {
      id: 3,
      name: 'ahmed ali',
      email: 'omar.ali@university.edu',
      phone: '+20 102 345 6789',
      avatar: null,
      university: 'Ain Shams University',
      major: 'Data Science',
      year: '5th Year',
      project: 'Healthcare Analytics Dashboard',
      progress: 60,
      lastActive: '3 days ago',
      status: 'inactive',
      skills: ['Python', 'SQL', 'Tableau'],
      location: 'Cairo, Egypt',
      github: 'github.com/omarali',
      linkedin: 'linkedin.com/in/omarali'
    },
    {
      id: 4,
      name: 'Fatma Ibrahim',
      email: 'fatma.i@university.edu',
      phone: '+20 103 456 7890',
      avatar: null,
      university: 'Cairo University',
      major: 'Information Systems',
      year: '4th Year',
      project: 'Smart Inventory Management',
      progress: 45,
      lastActive: 'Just now',
      status: 'active',
      skills: ['Java', 'Spring Boot', 'MySQL'],
      location: 'Giza, Egypt',
      github: 'github.com/fatmai',
      linkedin: 'linkedin.com/in/fatmai'
    },
    {
      id: 5,
      name: 'Mahmoud Khalil',
      email: 'mahmoud.k@university.edu',
      phone: '+20 104 567 8901',
      avatar: null,
      university: 'Helwan University',
      major: 'Computer Engineering',
      year: '4th Year',
      project: 'IoT Home Automation',
      progress: 90,
      lastActive: '5 hours ago',
      status: 'active',
      skills: ['C++', 'Arduino', 'React Native'],
      location: 'Cairo, Egypt',
      github: 'github.com/mahmoudk',
      linkedin: 'linkedin.com/in/mahmoudk'
    },
    {
      id: 6,
      name: 'Nour Elsayed',
      email: 'nour.e@university.edu',
      phone: '+20 105 678 9012',
      avatar: null,
      university: 'Mansoura University',
      major: 'Cybersecurity',
      year: '5th Year',
      project: 'Network Security Monitor',
      progress: 55,
      lastActive: '1 week ago',
      status: 'inactive',
      skills: ['Python', 'Kali Linux', 'Wireshark'],
      location: 'Mansoura, Egypt',
      github: 'github.com/nourse',
      linkedin: 'linkedin.com/in/nourse'
    }
  ];

  // Filter and search mentees
  const filteredMentees = mentees.filter(mentee => {
    const matchesSearch = mentee.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         mentee.project.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         mentee.university.toLowerCase().includes(searchQuery.toLowerCase());
    
    const matchesProgress = filters.progress === 'all' ||
                           (filters.progress === 'high' && mentee.progress >= 70) ||
                           (filters.progress === 'medium' && mentee.progress >= 40 && mentee.progress < 70) ||
                           (filters.progress === 'low' && mentee.progress < 40);
    
    const matchesActivity = filters.activity === 'all' ||
                           (filters.activity === 'active' && mentee.status === 'active') ||
                           (filters.activity === 'inactive' && mentee.status === 'inactive');
    
    return matchesSearch && matchesProgress && matchesActivity;
  });

  const getProgressColor = (progress) => {
    if (progress >= 70) return '#22c55e';
    if (progress >= 40) return '#f59e0b';
    return '#ef4444';
  };

  return (
    <div className="my-mentees-page">
      {/* Header */}
      <div className="mentees-header">
        <div>
          <h1 className="page-title">My Mentees</h1>
          <p className="page-subtitle">Manage and track your students' progress</p>
        </div>
        <div className="mentees-stats">
          <div className="stat-item">
            <span className="stat-value">{mentees.length}</span>
            <span className="stat-label">Total Mentees</span>
          </div>
          <div className="stat-item">
            <span className="stat-value">{mentees.filter(m => m.status === 'active').length}</span>
            <span className="stat-label">Active</span>
          </div>
        </div>
      </div>

      {/* Search and Filters */}
      <div className="mentees-toolbar">
        <div className="search-box">
          <FiSearch className="search-icon" />
          <input
            type="text"
            placeholder="Search mentees by name, project, or university..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>

        <div className="toolbar-actions">
          <button 
            className={`filter-btn ${filterOpen ? 'active' : ''}`}
            onClick={() => setFilterOpen(!filterOpen)}
          >
            <FiFilter /> Filters
          </button>
          
          <div className="view-toggle">
            <button 
              className={viewMode === 'grid' ? 'active' : ''}
              onClick={() => setViewMode('grid')}
            >
              <FiGrid />
            </button>
            <button 
              className={viewMode === 'list' ? 'active' : ''}
              onClick={() => setViewMode('list')}
            >
              <FiList />
            </button>
          </div>
        </div>
      </div>

      {/* Filter Panel */}
      <AnimatePresence>
        {filterOpen && (
          <motion.div 
            className="filter-panel"
            initial={{ opacity: 0, height: 0 }}
            animate={{ opacity: 1, height: 'auto' }}
            exit={{ opacity: 0, height: 0 }}
          >
            <div className="filter-group">
              <label>Progress Level</label>
              <select 
                value={filters.progress}
                onChange={(e) => setFilters({...filters, progress: e.target.value})}
              >
                <option value="all">All Levels</option>
                <option value="high">High (70%+)</option>
                <option value="medium">Medium (40-70%)</option>
                <option value="low">Low (&lt;40%)</option>
              </select>
            </div>
            <div className="filter-group">
              <label>Activity Status</label>
              <select 
                value={filters.activity}
                onChange={(e) => setFilters({...filters, activity: e.target.value})}
              >
                <option value="all">All Status</option>
                <option value="active">Active</option>
                <option value="inactive">Inactive</option>
              </select>
            </div>
            <button 
              className="clear-filters"
              onClick={() => setFilters({ progress: 'all', activity: 'all' })}
            >
              Clear Filters
            </button>
          </motion.div>
        )}
      </AnimatePresence>

      {/* Mentees Grid/List */}
      <div className={`mentees-container ${viewMode}`}>
        {filteredMentees.map((mentee, index) => (
          <motion.div
            key={mentee.id}
            className="mentee-card"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: index * 0.05 }}
            onClick={() => setSelectedMentee(mentee)}
          >
            <div className="mentee-avatar">
              {mentee.name.charAt(0)}
            </div>
            <div className="mentee-info">
              <h3 className="mentee-name">{mentee.name}</h3>
              <p className="mentee-university">{mentee.university}</p>
              <p className="mentee-major">{mentee.major} • {mentee.year}</p>
              <p className="mentee-project">{mentee.project}</p>
            </div>
            <div className="mentee-progress">
              <div className="progress-header">
                <span>Progress</span>
                <span style={{ color: getProgressColor(mentee.progress) }}>
                  {mentee.progress}%
                </span>
              </div>
              <div className="progress-bar">
                <div 
                  className="progress-fill"
                  style={{ 
                    width: `${mentee.progress}%`,
                    background: getProgressColor(mentee.progress)
                  }}
                />
              </div>
            </div>
            <div className="mentee-footer">
              <span className={`status-badge ${mentee.status}`}>
                {mentee.status}
              </span>
              <span className="last-active">Last active: {mentee.lastActive}</span>
            </div>
            <div className="mentee-actions">
              <button className="action-btn message" title="Send Message">
                <FiMessageSquare />
              </button>
              <button className="action-btn schedule" title="Schedule Session">
                <FiCalendar />
              </button>
              <button className="action-btn profile" title="View Profile">
                <FiUser />
              </button>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Empty State */}
      {filteredMentees.length === 0 && (
        <div className="empty-state">
          <FiSearch className="empty-icon" />
          <h3>No mentees found</h3>
          <p>Try adjusting your search or filters</p>
        </div>
      )}

      {/* Mentee Detail Modal */}
      <AnimatePresence>
        {selectedMentee && (
          <motion.div 
            className="mentee-modal-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={() => setSelectedMentee(null)}
          >
            <motion.div 
              className="mentee-modal"
              initial={{ scale: 0.9, opacity: 0 }}
              animate={{ scale: 1, opacity: 1 }}
              exit={{ scale: 0.9, opacity: 0 }}
              onClick={(e) => e.stopPropagation()}
            >
              <button className="close-modal" onClick={() => setSelectedMentee(null)}>
                <FiX />
              </button>
              
              <div className="modal-header">
                <div className="modal-avatar">
                  {selectedMentee.name.charAt(0)}
                </div>
                <div className="modal-title">
                  <h2>{selectedMentee.name}</h2>
                  <p>{selectedMentee.university}</p>
                  <span className={`status-badge ${selectedMentee.status}`}>
                    {selectedMentee.status}
                  </span>
                </div>
              </div>

              <div className="modal-body">
                <div className="modal-section">
                  <h4>Contact Information</h4>
                  <div className="info-grid">
                    <div className="info-item">
                      <FiMail /> {selectedMentee.email}
                    </div>
                    <div className="info-item">
                      <FiPhone /> {selectedMentee.phone}
                    </div>
                    <div className="info-item">
                      <FiMapPin /> {selectedMentee.location}
                    </div>
                    <div className="info-item">
                      <FiGithub /> {selectedMentee.github}
                    </div>
                    <div className="info-item">
                      <FiLinkedin /> {selectedMentee.linkedin}
                    </div>
                  </div>
                </div>

                <div className="modal-section">
                  <h4>Academic Information</h4>
                  <p><strong>Major:</strong> {selectedMentee.major}</p>
                  <p><strong>Year:</strong> {selectedMentee.year}</p>
                  <p><strong>Project:</strong> {selectedMentee.project}</p>
                </div>

                <div className="modal-section">
                  <h4>Skills</h4>
                  <div className="skills-list">
                    {selectedMentee.skills.map((skill, i) => (
                      <span key={i} className="skill-tag">{skill}</span>
                    ))}
                  </div>
                </div>

                <div className="modal-section">
                  <h4>Progress</h4>
                  <div className="progress-large">
                    <div className="progress-circle">
                      <span className="progress-value">{selectedMentee.progress}%</span>
                    </div>
                    <div className="progress-details">
                      <p>Current project completion</p>
                      <div className="progress-bar large">
                        <div 
                          className="progress-fill"
                          style={{ 
                            width: `${selectedMentee.progress}%`,
                            background: getProgressColor(selectedMentee.progress)
                          }}
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div className="modal-footer">
                <button className="mentor-btn mentor-btn-secondary">
                  <FiMessageSquare /> Send Message
                </button>
                <button className="mentor-btn mentor-btn-primary">
                  <FiCalendar /> Schedule Session
                </button>
              </div>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
};

export default MyMentees;







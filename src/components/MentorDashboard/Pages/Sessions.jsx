import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { 
  FiCalendar, 
  FiPlus, 
  FiClock, 
  FiVideo,
  FiMapPin,
  FiEdit2,
  FiTrash2,
  FiCheck,
  FiBell,
  FiX,
  FiUser
} from 'react-icons/fi';
import './Sessions.css';

const Sessions = () => {
  const [viewMode, setViewMode] = useState('list'); // 'list' or 'calendar'
  const [showModal, setShowModal] = useState(false);
  const [selectedSession, setSelectedSession] = useState(null);
  const [filter, setFilter] = useState('all');

  // Mock sessions data
  const [sessions, setSessions] = useState([
    {
      id: 1,
      student: 'Ahmed Hassan',
      topic: 'Project Progress Review',
      date: '2024-12-12',
      time: '15:00',
      duration: 60,
      type: 'online',
      status: 'scheduled',
      notes: 'Discuss AI model implementation',
      meetingLink: 'https://meet.google.com/abc-defg-hij'
    },
    {
      id: 2,
      student: 'Sarah Mohamed',
      topic: 'Career Guidance & Resume Review',
      date: '2024-12-13',
      time: '10:00',
      duration: 45,
      type: 'online',
      status: 'scheduled',
      notes: 'Review updated resume, discuss job opportunities'
    },
    {
      id: 3,
      student: 'ahmed ali',
      topic: 'Technical Architecture Discussion',
      date: '2024-12-14',
      time: '14:00',
      duration: 90,
      type: 'in-person',
      status: 'scheduled',
      location: 'University Library, Room 205'
    },
    {
      id: 4,
      student: 'Fatma Ibrahim',
      topic: 'Database Design Review',
      date: '2024-12-10',
      time: '11:00',
      duration: 60,
      type: 'online',
      status: 'completed',
      notes: 'Completed successfully'
    },
    {
      id: 5,
      student: 'Mahmoud Khalil',
      topic: 'Final Project Presentation',
      date: '2024-12-08',
      time: '09:00',
      duration: 120,
      type: 'in-person',
      status: 'completed',
      location: 'Engineering Building'
    },
    {
      id: 6,
      student: 'Nour Elsayed',
      topic: 'Security Assessment',
      date: '2024-12-05',
      time: '16:00',
      duration: 45,
      type: 'online',
      status: 'cancelled',
      notes: 'Rescheduled due to conflict'
    }
  ]);

  const [formData, setFormData] = useState({
    student: '',
    topic: '',
    date: '',
    time: '',
    duration: 60,
    type: 'online',
    notes: '',
    location: '',
    meetingLink: ''
  });

  // Filter sessions
  const filteredSessions = sessions.filter(session => {
    if (filter === 'all') return true;
    return session.status === filter;
  });

  // Sort sessions by date
  const sortedSessions = [...filteredSessions].sort((a, b) => {
    return new Date(b.date + ' ' + b.time) - new Date(a.date + ' ' + a.time);
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    if (selectedSession) {
      // Edit session
      setSessions(sessions.map(s => 
        s.id === selectedSession.id ? { ...s, ...formData } : s
      ));
    } else {
      // Add new session
      setSessions([...sessions, { 
        ...formData, 
        id: Date.now(),
        status: 'scheduled'
      }]);
    }
    closeModal();
  };

  const openModal = (session = null) => {
    if (session) {
      setSelectedSession(session);
      setFormData(session);
    } else {
      setSelectedSession(null);
      setFormData({
        student: '',
        topic: '',
        date: '',
        time: '',
        duration: 60,
        type: 'online',
        notes: '',
        location: '',
        meetingLink: ''
      });
    }
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setSelectedSession(null);
  };

  const deleteSession = (id) => {
    if (confirm('Are you sure you want to delete this session?')) {
      setSessions(sessions.filter(s => s.id !== id));
    }
  };

  const markCompleted = (id) => {
    setSessions(sessions.map(s => 
      s.id === id ? { ...s, status: 'completed' } : s
    ));
  };

  const cancelSession = (id) => {
    setSessions(sessions.map(s => 
      s.id === id ? { ...s, status: 'cancelled' } : s
    ));
  };

  const formatDate = (dateStr) => {
    const date = new Date(dateStr);
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    
    if (dateStr === today.toISOString().split('T')[0]) return 'Today';
    if (dateStr === tomorrow.toISOString().split('T')[0]) return 'Tomorrow';
    
    return date.toLocaleDateString('en-US', { 
      weekday: 'short', 
      month: 'short', 
      day: 'numeric' 
    });
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'scheduled': return 'blue';
      case 'completed': return 'green';
      case 'cancelled': return 'red';
      default: return 'gray';
    }
  };

  return (
    <div className="sessions-page">
      {/* Header */}
      <div className="sessions-header">
        <div>
          <h1 className="page-title">Sessions</h1>
          <p className="page-subtitle">Manage your mentorship meetings</p>
        </div>
        <button className="mentor-btn mentor-btn-primary" onClick={() => openModal()}>
          <FiPlus /> Create Session
        </button>
      </div>

      {/* Toolbar */}
      <div className="sessions-toolbar">
        <div className="filter-tabs">
          {['all', 'scheduled', 'completed', 'cancelled'].map(f => (
            <button
              key={f}
              className={`filter-tab ${filter === f ? 'active' : ''}`}
              onClick={() => setFilter(f)}
            >
              {f.charAt(0).toUpperCase() + f.slice(1)}
              <span className="tab-count">
                {f === 'all' ? sessions.length : sessions.filter(s => s.status === f).length}
              </span>
            </button>
          ))}
        </div>

        <div className="view-toggle">
          <button 
            className={viewMode === 'list' ? 'active' : ''}
            onClick={() => setViewMode('list')}
          >
            List
          </button>
          <button 
            className={viewMode === 'calendar' ? 'active' : ''}
            onClick={() => setViewMode('calendar')}
          >
            Calendar
          </button>
        </div>
      </div>

      {/* Sessions List */}
      <div className="sessions-list">
        {sortedSessions.map((session, index) => (
          <motion.div
            key={session.id}
            className={`session-card ${session.status}`}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: index * 0.05 }}
          >
            <div className="session-date-badge">
              <span className="date-day">{new Date(session.date).getDate()}</span>
              <span className="date-month">
                {new Date(session.date).toLocaleDateString('en-US', { month: 'short' })}
              </span>
            </div>

            <div className="session-main">
              <div className="session-info">
                <div className="session-top">
                  <h3 className="session-topic">{session.topic}</h3>
                  <span className={`status-badge ${getStatusColor(session.status)}`}>
                    {session.status}
                  </span>
                </div>
                <div className="session-meta">
                  <span className="meta-item">
                    <FiUser /> {session.student}
                  </span>
                  <span className="meta-item">
                    <FiClock /> {session.time} ({session.duration} min)
                  </span>
                  <span className="meta-item">
                    {session.type === 'online' ? <FiVideo /> : <FiMapPin />}
                    {session.type === 'online' ? 'Online' : session.location}
                  </span>
                </div>
                {session.notes && (
                  <p className="session-notes">{session.notes}</p>
                )}
              </div>

              <div className="session-actions">
                {session.status === 'scheduled' && (
                  <>
                    <button 
                      className="action-btn join"
                      title={session.type === 'online' ? 'Join Meeting' : 'View Details'}
                    >
                      {session.type === 'online' ? <FiVideo /> : <FiMapPin />}
                    </button>
                    <button 
                      className="action-btn remind"
                      title="Send Reminder"
                    >
                      <FiBell />
                    </button>
                    <button 
                      className="action-btn complete"
                      title="Mark as Completed"
                      onClick={() => markCompleted(session.id)}
                    >
                      <FiCheck />
                    </button>
                    <button 
                      className="action-btn edit"
                      title="Edit"
                      onClick={() => openModal(session)}
                    >
                      <FiEdit2 />
                    </button>
                    <button 
                      className="action-btn cancel"
                      title="Cancel"
                      onClick={() => cancelSession(session.id)}
                    >
                      <FiX />
                    </button>
                  </>
                )}
                {session.status !== 'scheduled' && (
                  <button 
                    className="action-btn delete"
                    title="Delete"
                    onClick={() => deleteSession(session.id)}
                  >
                    <FiTrash2 />
                  </button>
                )}
              </div>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Empty State */}
      {sortedSessions.length === 0 && (
        <div className="empty-state">
          <FiCalendar className="empty-icon" />
          <h3>No sessions found</h3>
          <p>Create a new session to get started</p>
          <button className="mentor-btn mentor-btn-primary" onClick={() => openModal()}>
            <FiPlus /> Create Session
          </button>
        </div>
      )}

      {/* Create/Edit Modal */}
      <AnimatePresence>
        {showModal && (
          <motion.div 
            className="modal-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={closeModal}
          >
            <motion.div 
              className="session-modal"
              initial={{ scale: 0.9, opacity: 0 }}
              animate={{ scale: 1, opacity: 1 }}
              exit={{ scale: 0.9, opacity: 0 }}
              onClick={(e) => e.stopPropagation()}
            >
              <div className="modal-header">
                <h2>{selectedSession ? 'Edit Session' : 'Create New Session'}</h2>
                <button className="close-btn" onClick={closeModal}>
                  <FiX />
                </button>
              </div>

              <form onSubmit={handleSubmit} className="session-form">
                <div className="form-row">
                  <div className="form-group">
                    <label>Student Name</label>
                    <input
                      type="text"
                      value={formData.student}
                      onChange={(e) => setFormData({...formData, student: e.target.value})}
                      placeholder="Select or enter student name"
                      required
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label>Session Topic</label>
                  <input
                    type="text"
                    value={formData.topic}
                    onChange={(e) => setFormData({...formData, topic: e.target.value})}
                    placeholder="What will you discuss?"
                    required
                  />
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label>Date</label>
                    <input
                      type="date"
                      value={formData.date}
                      onChange={(e) => setFormData({...formData, date: e.target.value})}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label>Time</label>
                    <input
                      type="time"
                      value={formData.time}
                      onChange={(e) => setFormData({...formData, time: e.target.value})}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label>Duration (min)</label>
                    <select
                      value={formData.duration}
                      onChange={(e) => setFormData({...formData, duration: parseInt(e.target.value)})}
                    >
                      <option value={30}>30 min</option>
                      <option value={45}>45 min</option>
                      <option value={60}>1 hour</option>
                      <option value={90}>1.5 hours</option>
                      <option value={120}>2 hours</option>
                    </select>
                  </div>
                </div>

                <div className="form-group">
                  <label>Session Type</label>
                  <div className="type-toggle">
                    <button
                      type="button"
                      className={formData.type === 'online' ? 'active' : ''}
                      onClick={() => setFormData({...formData, type: 'online'})}
                    >
                      <FiVideo /> Online
                    </button>
                    <button
                      type="button"
                      className={formData.type === 'in-person' ? 'active' : ''}
                      onClick={() => setFormData({...formData, type: 'in-person'})}
                    >
                      <FiMapPin /> In-Person
                    </button>
                  </div>
                </div>

                {formData.type === 'online' && (
                  <div className="form-group">
                    <label>Meeting Link (optional)</label>
                    <input
                      type="url"
                      value={formData.meetingLink}
                      onChange={(e) => setFormData({...formData, meetingLink: e.target.value})}
                      placeholder="https://meet.google.com/..."
                    />
                  </div>
                )}

                {formData.type === 'in-person' && (
                  <div className="form-group">
                    <label>Location</label>
                    <input
                      type="text"
                      value={formData.location}
                      onChange={(e) => setFormData({...formData, location: e.target.value})}
                      placeholder="Where will you meet?"
                    />
                  </div>
                )}

                <div className="form-group">
                  <label>Notes (optional)</label>
                  <textarea
                    value={formData.notes}
                    onChange={(e) => setFormData({...formData, notes: e.target.value})}
                    placeholder="Any additional notes for this session..."
                    rows={3}
                  />
                </div>

                <div className="form-actions">
                  <button type="button" className="mentor-btn mentor-btn-secondary" onClick={closeModal}>
                    Cancel
                  </button>
                  <button type="submit" className="mentor-btn mentor-btn-primary">
                    {selectedSession ? 'Save Changes' : 'Create Session'}
                  </button>
                </div>
              </form>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
};

export default Sessions;







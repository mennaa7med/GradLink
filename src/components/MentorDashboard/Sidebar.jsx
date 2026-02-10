import React from 'react';
import { motion } from 'framer-motion';
import { 
  FiHome, 
  FiUsers, 
  FiCalendar, 
  FiMessageSquare, 
  FiUser,
  FiLogOut,
  FiAward
} from 'react-icons/fi';
import { useNavigate } from 'react-router-dom';
import './Sidebar.css';

const Sidebar = ({ activePage, setActivePage, isOpen }) => {
  const navigate = useNavigate();
  
  // Get mentor info from localStorage
  const mentorName = localStorage.getItem('userName') || 'Mentor';
  const mentorEmail = localStorage.getItem('userEmail') || 'mentor@gradlink.com';

  const menuItems = [
    { id: 'overview', icon: <FiHome />, label: 'Overview' },
    { id: 'mentees', icon: <FiUsers />, label: 'My Mentees' },
    { id: 'sessions', icon: <FiCalendar />, label: 'Sessions' },
    { id: 'messages', icon: <FiMessageSquare />, label: 'Messages' },
    { id: 'profile', icon: <FiUser />, label: 'Profile & Settings' },
  ];

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
    localStorage.removeItem('userRole');
    navigate('/signin');
  };

  return (
    <div className="mentor-sidebar">
      {/* Logo Section */}
      <div className="mentor-sidebar-logo" onClick={() => navigate('/')} style={{ cursor: 'pointer' }}>
        <div className="mentor-logo-icon">
          <FiAward />
        </div>
        {isOpen && (
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            className="mentor-logo-text"
          >
            <span className="mentor-logo-title">GradLink</span>
            <span className="mentor-logo-subtitle">Mentor Portal</span>
          </motion.div>
        )}
      </div>

      {/* Navigation Menu */}
      <nav className="mentor-sidebar-nav">
        {menuItems.map((item) => (
          <motion.button
            key={item.id}
            onClick={() => setActivePage(item.id)}
            className={`mentor-nav-item ${activePage === item.id ? 'active' : ''}`}
            whileHover={{ x: 4 }}
            whileTap={{ scale: 0.98 }}
          >
            <span className="mentor-nav-icon">{item.icon}</span>
            {isOpen && (
              <motion.span
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                className="mentor-nav-label"
              >
                {item.label}
              </motion.span>
            )}
            {activePage === item.id && (
              <motion.div
                layoutId="mentor-active-indicator"
                className="mentor-nav-indicator"
              />
            )}
          </motion.button>
        ))}
      </nav>

      {/* Mentor Profile Card */}
      {isOpen && (
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          className="mentor-sidebar-profile"
        >
          <div className="mentor-profile-avatar">
            {mentorName.charAt(0).toUpperCase()}
          </div>
          <div className="mentor-profile-info">
            <span className="mentor-profile-name">{mentorName}</span>
            <span className="mentor-profile-role">Mentor</span>
          </div>
        </motion.div>
      )}

      {/* Logout Button */}
      <button className="mentor-logout-btn" onClick={handleLogout}>
        <FiLogOut />
        {isOpen && <span>Logout</span>}
      </button>
    </div>
  );
};

export default Sidebar;







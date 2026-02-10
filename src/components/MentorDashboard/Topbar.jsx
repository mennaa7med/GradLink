import React, { useState } from 'react';
import { motion } from 'framer-motion';
import { 
  FiMenu, 
  FiSearch, 
  FiBell, 
  FiChevronDown,
  FiSettings,
  FiUser,
  FiLogOut
} from 'react-icons/fi';
import './Topbar.css';

const Topbar = ({ toggleSidebar, sidebarOpen }) => {
  const [showNotifications, setShowNotifications] = useState(false);
  const [showProfileMenu, setShowProfileMenu] = useState(false);
  
  const mentorName = localStorage.getItem('userName') || 'Mentor';

  // Mock notifications
  const notifications = [
    { id: 1, text: 'New mentee request from Ahmed Hassan', time: '5 min ago', unread: true },
    { id: 2, text: 'Session reminder: Meeting with Sarah in 1 hour', time: '30 min ago', unread: true },
    { id: 3, text: 'Omar completed the assigned task', time: '2 hours ago', unread: false },
  ];

  const unreadCount = notifications.filter(n => n.unread).length;

  // Get current greeting
  const getGreeting = () => {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good Morning';
    if (hour < 18) return 'Good Afternoon';
    return 'Good Evening';
  };

  return (
    <div className="mentor-topbar">
      <div className="mentor-topbar-left">
        <button className="mentor-menu-toggle" onClick={toggleSidebar}>
          <FiMenu />
        </button>
        
        <div className="mentor-topbar-greeting">
          <span className="greeting-text">{getGreeting()},</span>
          <span className="greeting-name">{mentorName}! 👋</span>
        </div>
      </div>

      <div className="mentor-topbar-center">
        <div className="mentor-topbar-search">
          <FiSearch className="search-icon" />
          <input 
            type="text" 
            placeholder="Search mentees, sessions, messages..." 
          />
        </div>
      </div>

      <div className="mentor-topbar-right">
        {/* Notifications */}
        <div className="mentor-notification-wrapper">
          <motion.button 
            className="mentor-notification-btn"
            onClick={() => setShowNotifications(!showNotifications)}
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
          >
            <FiBell />
            {unreadCount > 0 && (
              <span className="notification-badge">{unreadCount}</span>
            )}
          </motion.button>

          {showNotifications && (
            <motion.div 
              className="mentor-notification-dropdown"
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
            >
              <div className="notification-header">
                <span>Notifications</span>
                <button className="mark-read-btn">Mark all as read</button>
              </div>
              <div className="notification-list">
                {notifications.map(notif => (
                  <div 
                    key={notif.id} 
                    className={`notification-item ${notif.unread ? 'unread' : ''}`}
                  >
                    <div className="notification-dot"></div>
                    <div className="notification-content">
                      <p>{notif.text}</p>
                      <span className="notification-time">{notif.time}</span>
                    </div>
                  </div>
                ))}
              </div>
              <div className="notification-footer">
                <button>View All Notifications</button>
              </div>
            </motion.div>
          )}
        </div>

        {/* Profile Menu */}
        <div className="mentor-profile-wrapper">
          <motion.button 
            className="mentor-profile-btn"
            onClick={() => setShowProfileMenu(!showProfileMenu)}
            whileHover={{ scale: 1.02 }}
          >
            <div className="profile-avatar">
              {mentorName.charAt(0).toUpperCase()}
            </div>
            <span className="profile-name">{mentorName}</span>
            <FiChevronDown className={`chevron ${showProfileMenu ? 'open' : ''}`} />
          </motion.button>

          {showProfileMenu && (
            <motion.div 
              className="mentor-profile-dropdown"
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
            >
              <button className="dropdown-item">
                <FiUser /> My Profile
              </button>
              <button className="dropdown-item">
                <FiSettings /> Settings
              </button>
              <hr />
              <button className="dropdown-item logout">
                <FiLogOut /> Logout
              </button>
            </motion.div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Topbar;







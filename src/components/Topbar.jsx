import React, { useState, useEffect, useRef, useCallback } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import './Topbar.css';
import { FiSearch, FiMail, FiBell, FiCheck, FiX, FiMessageSquare, FiUser, FiSettings, FiLogOut, FiChevronDown } from 'react-icons/fi';
import defaultAvatar from '../assets/images/mask.png';
import { useAuth } from '../contexts/AuthContext';
import { getUserIdFromToken } from '../utils/jwt';
import { getUser } from '../api/users';
import { getNotifications, getNotificationCount, markAsRead, markAllAsRead } from '../api/notifications';
import { getUnreadMessagesCount, getUnreadMessages } from '../api/messages';
import { listTasks } from '../api/tasks';

const Topbar = () => {
  const navigate = useNavigate();
  const { user: authUser, logout } = useAuth();
  const [userProfile, setUserProfile] = useState(null);
  const [notificationCount, setNotificationCount] = useState(0);
  const [messageCount, setMessageCount] = useState(0);
  const [notificationsList, setNotificationsList] = useState([]);
  const [messagesList, setMessagesList] = useState([]);
  const [showNotifications, setShowNotifications] = useState(false);
  const [showMessages, setShowMessages] = useState(false);
  const [showUserMenu, setShowUserMenu] = useState(false);
  const [loadingNotifications, setLoadingNotifications] = useState(false);
  const [loadingMessages, setLoadingMessages] = useState(false);
  
  // Search state
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [showSearchResults, setShowSearchResults] = useState(false);
  const [searchLoading, setSearchLoading] = useState(false);
  
  const notificationsRef = useRef(null);
  const messagesRef = useRef(null);
  const userMenuRef = useRef(null);
  const searchRef = useRef(null);

  // Close dropdowns when clicking outside
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (notificationsRef.current && !notificationsRef.current.contains(event.target)) {
        setShowNotifications(false);
      }
      if (messagesRef.current && !messagesRef.current.contains(event.target)) {
        setShowMessages(false);
      }
      if (userMenuRef.current && !userMenuRef.current.contains(event.target)) {
        setShowUserMenu(false);
      }
      if (searchRef.current && !searchRef.current.contains(event.target)) {
        setShowSearchResults(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  // Search functionality
  const handleSearch = useCallback(async (query) => {
    if (!query.trim()) {
      setSearchResults([]);
      setShowSearchResults(false);
      return;
    }

    setSearchLoading(true);
    setShowSearchResults(true);

    try {
      const results = [];
      const lowerQuery = query.toLowerCase();

      // Search tasks
      try {
        const tasks = await listTasks();
        const matchedTasks = tasks
          .filter(t => 
            t.title?.toLowerCase().includes(lowerQuery) || 
            t.description?.toLowerCase().includes(lowerQuery)
          )
          .slice(0, 3)
          .map(t => ({
            type: 'task',
            icon: '✅',
            title: t.title,
            subtitle: `Task - ${t.status}`,
            path: '/dashboard/tasks'
          }));
        results.push(...matchedTasks);
      } catch (e) { /* ignore */ }

      // Add static navigation items that match
      const navItems = [
        { title: 'Dashboard', subtitle: 'Overview', path: '/dashboard', icon: '📊' },
        { title: 'Tasks', subtitle: 'Task Manager', path: '/dashboard/tasks', icon: '✅' },
        { title: 'Materials', subtitle: 'Learning Resources', path: '/dashboard/materials', icon: '📚' },
        { title: 'Teams', subtitle: 'Team Collaboration', path: '/dashboard/teams', icon: '👥' },
        { title: 'Mentors', subtitle: 'Find Mentors', path: '/dashboard/mentors', icon: '🎓' },
        { title: 'Sponsors', subtitle: 'Sponsorship', path: '/dashboard/sponsors', icon: '💼' },
        { title: 'Settings', subtitle: 'Account Settings', path: '/dashboard/settings', icon: '⚙️' },
        { title: 'Help', subtitle: 'Help & Support', path: '/dashboard/help', icon: '❓' },
        { title: 'Career', subtitle: 'Job Opportunities', path: '/career', icon: '🚀' },
        { title: 'Profile', subtitle: 'Your Profile', path: '/dashboard/settings', icon: '👤' },
      ];

      const matchedNav = navItems
        .filter(item => 
          item.title.toLowerCase().includes(lowerQuery) || 
          item.subtitle.toLowerCase().includes(lowerQuery)
        )
        .map(item => ({ ...item, type: 'navigation' }));
      
      results.push(...matchedNav);

      setSearchResults(results.slice(0, 8));
    } catch (error) {
      console.error('Search error:', error);
    } finally {
      setSearchLoading(false);
    }
  }, []);

  // Debounced search
  useEffect(() => {
    const timer = setTimeout(() => {
      handleSearch(searchQuery);
    }, 300);
    return () => clearTimeout(timer);
  }, [searchQuery, handleSearch]);

  // Handle search result click
  const handleSearchResultClick = (result) => {
    navigate(result.path);
    setSearchQuery('');
    setShowSearchResults(false);
  };

  // Handle logout
  const handleLogout = () => {
    logout();
    navigate('/signin');
  };

  // Load user profile
  useEffect(() => {
    const loadUserProfile = async () => {
      try {
        const userId = getUserIdFromToken();
        if (userId) {
          const profile = await getUser(userId);
          setUserProfile(profile);
        }
      } catch (error) {
        console.error('Error loading user profile:', error);
      }
    };

    loadUserProfile();
  }, []);

  // Load notification and message counts
  const loadCounts = useCallback(async () => {
    try {
      const [notifData, msgData] = await Promise.all([
        getNotificationCount().catch(() => ({ unread: 0 })),
        getUnreadMessagesCount().catch(() => ({ unreadCount: 0 }))
      ]);
      setNotificationCount(notifData?.unread || notifData?.Unread || 0);
      setMessageCount(msgData?.unreadCount || 0);
    } catch (error) {
      console.error('Error loading counts:', error);
    }
  }, []);

  useEffect(() => {
    loadCounts();
    // Refresh counts every 30 seconds
    const interval = setInterval(loadCounts, 30000);
    return () => clearInterval(interval);
  }, [loadCounts]);

  // Load notifications when dropdown opens
  const handleNotificationsClick = async () => {
    setShowNotifications(!showNotifications);
    setShowMessages(false);
    
    if (!showNotifications) {
      setLoadingNotifications(true);
      try {
        const data = await getNotifications({ unreadOnly: false, pageSize: 10 });
        setNotificationsList(data?.notifications || []);
      } catch (error) {
        console.error('Error loading notifications:', error);
      } finally {
        setLoadingNotifications(false);
      }
    }
  };

  // Load messages when dropdown opens
  const handleMessagesClick = async () => {
    setShowMessages(!showMessages);
    setShowNotifications(false);
    
    if (!showMessages) {
      setLoadingMessages(true);
      try {
        const messages = await getUnreadMessages(10);
        setMessagesList(messages || []);
      } catch (error) {
        console.error('Error loading messages:', error);
      } finally {
        setLoadingMessages(false);
      }
    }
  };

  // Mark notification as read
  const handleMarkAsRead = async (notifId) => {
    try {
      await markAsRead(notifId);
      setNotificationsList(prev => 
        prev.map(n => n.id === notifId ? { ...n, isRead: true } : n)
      );
      setNotificationCount(prev => Math.max(0, prev - 1));
    } catch (error) {
      console.error('Error marking notification as read:', error);
    }
  };

  // Mark all notifications as read
  const handleMarkAllAsRead = async () => {
    try {
      await markAllAsRead();
      setNotificationsList(prev => prev.map(n => ({ ...n, isRead: true })));
      setNotificationCount(0);
    } catch (error) {
      console.error('Error marking all as read:', error);
    }
  };

  // Format time ago
  const formatTimeAgo = (dateStr) => {
    const date = new Date(dateStr);
    const now = new Date();
    const diffMs = now - date;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins}m ago`;
    if (diffHours < 24) return `${diffHours}h ago`;
    if (diffDays < 7) return `${diffDays}d ago`;
    return date.toLocaleDateString();
  };

  // Get notification icon based on type
  const getNotificationIcon = (type) => {
    const iconMap = {
      'application': '📋',
      'job': '💼',
      'internship': '🎓',
      'project': '🚀',
      'message': '💬',
      'task': '✅',
      'system': '⚙️',
      'success': '✨',
      'warning': '⚠️',
      'error': '❌',
    };
    return <span className="notif-type-icon">{iconMap[type?.toLowerCase()] || '🔔'}</span>;
  };

  const displayName = userProfile?.fullName || userProfile?.name || authUser?.email || 'User';
  const displayEmail = userProfile?.email || authUser?.email || 'user@gradlink.com';
  const profilePicture = userProfile?.profilePicture || defaultAvatar;

  return (
    <motion.div 
      className="top-bar"
      initial={{ y: -50, opacity: 0 }}
      animate={{ y: 0, opacity: 1 }}
      transition={{ duration: 0.5 }}
    >
      <div className="left-section">
        <div className="search-container" ref={searchRef}>
          <motion.div 
            className="search-input-wrapper"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ delay: 0.3 }}
          >
            <FiSearch className="search-icon" />
            <input 
              type="text" 
              placeholder="Search tasks, pages..." 
              className="search-input"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              onFocus={() => searchQuery && setShowSearchResults(true)}
            />
            {searchQuery && (
              <button 
                className="search-clear-btn"
                onClick={() => { setSearchQuery(''); setShowSearchResults(false); }}
              >
                <FiX />
              </button>
            )}
          </motion.div>
          
          <AnimatePresence>
            {showSearchResults && (
              <motion.div 
                className="search-dropdown"
                initial={{ opacity: 0, y: -10 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: -10 }}
              >
                {searchLoading ? (
                  <div className="search-loading">
                    <div className="loading-spinner"></div>
                    <span>Searching...</span>
                  </div>
                ) : searchResults.length === 0 ? (
                  <div className="search-empty">
                    <span>No results found for "{searchQuery}"</span>
                  </div>
                ) : (
                  searchResults.map((result, index) => (
                    <motion.div 
                      key={index}
                      className="search-result-item"
                      onClick={() => handleSearchResultClick(result)}
                      whileHover={{ backgroundColor: '#f8f9fa' }}
                    >
                      <span className="search-result-icon">{result.icon}</span>
                      <div className="search-result-content">
                        <div className="search-result-title">{result.title}</div>
                        <div className="search-result-subtitle">{result.subtitle}</div>
                      </div>
                      <span className="search-result-type">{result.type}</span>
                    </motion.div>
                  ))
                )}
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </div>
      
      <motion.div 
        className="right-section"
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ delay: 0.2 }}
      >
        <div className="icon-container">
          {/* Messages Icon */}
          <div className="icon-dropdown-wrapper" ref={messagesRef}>
            <motion.div 
              className="icon-wrapper"
              whileHover={{ scale: 1.1, rotate: 5 }}
              whileTap={{ scale: 0.95 }}
              onClick={handleMessagesClick}
            >
              <FiMail className="icon" />
              {messageCount > 0 && (
                <motion.span 
                  className="notification-badge"
                  initial={{ scale: 0 }}
                  animate={{ scale: 1 }}
                  transition={{ delay: 0.5, type: "spring" }}
                >
                  {messageCount > 99 ? '99+' : messageCount}
                </motion.span>
              )}
            </motion.div>
            
            <AnimatePresence>
              {showMessages && (
                <motion.div 
                  className="dropdown-menu messages-dropdown"
                  initial={{ opacity: 0, y: -10, scale: 0.95 }}
                  animate={{ opacity: 1, y: 0, scale: 1 }}
                  exit={{ opacity: 0, y: -10, scale: 0.95 }}
                  transition={{ duration: 0.2 }}
                >
                  <div className="dropdown-header">
                    <h4>Messages</h4>
                    <span className="dropdown-count">{messageCount} unread</span>
                  </div>
                  <div className="dropdown-content">
                    {loadingMessages ? (
                      <div className="dropdown-loading">
                        <div className="loading-spinner"></div>
                        <span>Loading...</span>
                      </div>
                    ) : messagesList.length === 0 ? (
                      <div className="dropdown-empty">
                        <FiMessageSquare className="empty-icon" />
                        <p>No unread messages</p>
                      </div>
                    ) : (
                      messagesList.map((msg) => (
                        <motion.div 
                          key={msg.id}
                          className="dropdown-item message-item"
                          whileHover={{ backgroundColor: '#f8f9fa' }}
                        >
                          <div className="message-avatar">
                            {msg.senderAvatar ? (
                              <img src={msg.senderAvatar} alt="" />
                            ) : (
                              <span>{msg.senderName?.charAt(0) || 'U'}</span>
                            )}
                          </div>
                          <div className="message-content">
                            <div className="message-sender">{msg.senderName}</div>
                            <div className="message-preview">{msg.content?.substring(0, 50)}...</div>
                            <div className="message-time">{formatTimeAgo(msg.sentAt)}</div>
                          </div>
                        </motion.div>
                      ))
                    )}
                  </div>
                  <div className="dropdown-footer">
                    <button 
                      className="view-all-btn"
                      onClick={() => { navigate('/dashboard/messages'); setShowMessages(false); }}
                    >
                      View All Messages
                    </button>
                  </div>
                </motion.div>
              )}
            </AnimatePresence>
          </div>
          
          {/* Notifications Icon */}
          <div className="icon-dropdown-wrapper" ref={notificationsRef}>
            <motion.div 
              className="icon-wrapper"
              whileHover={{ scale: 1.1, rotate: -5 }}
              whileTap={{ scale: 0.95 }}
              onClick={handleNotificationsClick}
            >
              <FiBell className="icon" />
              {notificationCount > 0 && (
                <motion.span 
                  className="notification-badge"
                  initial={{ scale: 0 }}
                  animate={{
                    scale: [1, 1.2, 1],
                  }}
                  transition={{
                    duration: 2,
                    repeat: Infinity,
                    repeatDelay: 3
                  }}
                >
                  {notificationCount > 99 ? '99+' : notificationCount}
                </motion.span>
              )}
            </motion.div>
            
            <AnimatePresence>
              {showNotifications && (
                <motion.div 
                  className="dropdown-menu notifications-dropdown"
                  initial={{ opacity: 0, y: -10, scale: 0.95 }}
                  animate={{ opacity: 1, y: 0, scale: 1 }}
                  exit={{ opacity: 0, y: -10, scale: 0.95 }}
                  transition={{ duration: 0.2 }}
                >
                  <div className="dropdown-header">
                    <h4>Notifications</h4>
                    {notificationCount > 0 && (
                      <button 
                        className="mark-all-read-btn"
                        onClick={handleMarkAllAsRead}
                      >
                        <FiCheck /> Mark all read
                      </button>
                    )}
                  </div>
                  <div className="dropdown-content">
                    {loadingNotifications ? (
                      <div className="dropdown-loading">
                        <div className="loading-spinner"></div>
                        <span>Loading...</span>
                      </div>
                    ) : notificationsList.length === 0 ? (
                      <div className="dropdown-empty">
                        <FiBell className="empty-icon" />
                        <p>No notifications yet</p>
                      </div>
                    ) : (
                      notificationsList.map((notif) => (
                        <motion.div 
                          key={notif.id}
                          className={`dropdown-item notification-item ${!notif.isRead ? 'unread' : ''}`}
                          whileHover={{ backgroundColor: '#f8f9fa' }}
                          onClick={() => !notif.isRead && handleMarkAsRead(notif.id)}
                        >
                          <div className="notification-icon-wrapper">
                            {getNotificationIcon(notif.type)}
                          </div>
                          <div className="notification-content">
                            <div className="notification-title">{notif.title}</div>
                            <div className="notification-message">{notif.message}</div>
                            <div className="notification-time">{formatTimeAgo(notif.createdAt)}</div>
                          </div>
                          {!notif.isRead && (
                            <div className="unread-indicator"></div>
                          )}
                        </motion.div>
                      ))
                    )}
                  </div>
                  <div className="dropdown-footer">
                    <button 
                      className="view-all-btn"
                      onClick={() => { navigate('/dashboard/notifications'); setShowNotifications(false); }}
                    >
                      View All Notifications
                    </button>
                  </div>
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
        
        {/* User Dropdown */}
        <div className="user-dropdown-wrapper" ref={userMenuRef}>
          <motion.div 
            className="user-section"
            onClick={() => setShowUserMenu(!showUserMenu)}
            whileHover={{ scale: 1.02 }}
            transition={{ type: "spring", stiffness: 300 }}
          >
            <motion.img
              src={profilePicture}
              alt="User"
              className="user-photo"
              onError={(e) => { e.target.src = defaultAvatar; }}
              whileHover={{ scale: 1.1 }}
              transition={{ type: "spring", stiffness: 300 }}
            />
            <div className="user-info">
              <div className="username">{displayName}</div>
              <div className="email">{displayEmail}</div>
            </div>
            <motion.div
              animate={{ rotate: showUserMenu ? 180 : 0 }}
              transition={{ duration: 0.2 }}
            >
              <FiChevronDown className="dropdown-arrow" />
            </motion.div>
          </motion.div>
          
          <AnimatePresence>
            {showUserMenu && (
              <motion.div 
                className="user-menu-dropdown"
                initial={{ opacity: 0, y: -10, scale: 0.95 }}
                animate={{ opacity: 1, y: 0, scale: 1 }}
                exit={{ opacity: 0, y: -10, scale: 0.95 }}
                transition={{ duration: 0.2 }}
              >
                <div className="user-menu-header">
                  <img src={profilePicture} alt="" onError={(e) => { e.target.src = defaultAvatar; }} />
                  <div>
                    <div className="user-menu-name">{displayName}</div>
                    <div className="user-menu-email">{displayEmail}</div>
                  </div>
                </div>
                <div className="user-menu-divider"></div>
                <motion.div 
                  className="user-menu-item"
                  onClick={() => { navigate('/dashboard/settings'); setShowUserMenu(false); }}
                  whileHover={{ backgroundColor: '#f8f9fa' }}
                >
                  <FiUser className="user-menu-icon" />
                  <span>My Profile</span>
                </motion.div>
                <motion.div 
                  className="user-menu-item"
                  onClick={() => { navigate('/dashboard/settings'); setShowUserMenu(false); }}
                  whileHover={{ backgroundColor: '#f8f9fa' }}
                >
                  <FiSettings className="user-menu-icon" />
                  <span>Settings</span>
                </motion.div>
                <div className="user-menu-divider"></div>
                <motion.div 
                  className="user-menu-item logout"
                  onClick={handleLogout}
                  whileHover={{ backgroundColor: '#fef2f2' }}
                >
                  <FiLogOut className="user-menu-icon" />
                  <span>Logout</span>
                </motion.div>
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </motion.div>
    </motion.div>
  );
};

export default Topbar;

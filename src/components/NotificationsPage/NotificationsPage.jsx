import React, { useState, useEffect, useCallback } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiBell, FiCheck, FiTrash2, FiFilter, FiRefreshCw } from 'react-icons/fi';
import { getNotifications, markAsRead, markAllAsRead, deleteNotification, clearReadNotifications } from '../../api/notifications';
import './NotificationsPage.css';

const NotificationsPage = () => {
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('all'); // all, unread, read
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const loadNotifications = useCallback(async () => {
    setLoading(true);
    try {
      const unreadOnly = filter === 'unread' ? true : filter === 'read' ? false : undefined;
      const data = await getNotifications({ 
        unreadOnly: filter === 'unread' ? true : undefined, 
        page, 
        pageSize: 20 
      });
      
      let filteredNotifications = data?.notifications || [];
      if (filter === 'read') {
        filteredNotifications = filteredNotifications.filter(n => n.isRead);
      }
      
      setNotifications(filteredNotifications);
      setTotalPages(Math.ceil((data?.total || 0) / 20));
    } catch (error) {
      console.error('Failed to load notifications:', error);
    } finally {
      setLoading(false);
    }
  }, [filter, page]);

  useEffect(() => {
    loadNotifications();
  }, [loadNotifications]);

  const handleMarkAsRead = async (id) => {
    try {
      await markAsRead(id);
      setNotifications(prev => 
        prev.map(n => n.id === id ? { ...n, isRead: true } : n)
      );
    } catch (error) {
      console.error('Failed to mark as read:', error);
    }
  };

  const handleMarkAllAsRead = async () => {
    try {
      await markAllAsRead();
      setNotifications(prev => prev.map(n => ({ ...n, isRead: true })));
    } catch (error) {
      console.error('Failed to mark all as read:', error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await deleteNotification(id);
      setNotifications(prev => prev.filter(n => n.id !== id));
    } catch (error) {
      console.error('Failed to delete notification:', error);
    }
  };

  const handleClearRead = async () => {
    if (!window.confirm('Are you sure you want to delete all read notifications?')) return;
    try {
      await clearReadNotifications();
      setNotifications(prev => prev.filter(n => !n.isRead));
    } catch (error) {
      console.error('Failed to clear read notifications:', error);
    }
  };

  const formatTimeAgo = (dateStr) => {
    const date = new Date(dateStr);
    const now = new Date();
    const diffMs = now - date;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins} minutes ago`;
    if (diffHours < 24) return `${diffHours} hours ago`;
    if (diffDays < 7) return `${diffDays} days ago`;
    return date.toLocaleDateString();
  };

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
    return iconMap[type?.toLowerCase()] || '🔔';
  };

  const unreadCount = notifications.filter(n => !n.isRead).length;

  return (
    <div className="notifications-page">
      <div className="notifications-header">
        <div className="notifications-title-section">
          <h1>
            <FiBell className="title-icon" />
            Notifications
          </h1>
          {unreadCount > 0 && (
            <span className="unread-badge">{unreadCount} unread</span>
          )}
        </div>
        
        <div className="notifications-actions">
          <button 
            className="action-btn refresh-btn"
            onClick={loadNotifications}
            disabled={loading}
          >
            <FiRefreshCw className={loading ? 'spinning' : ''} />
            Refresh
          </button>
          
          {unreadCount > 0 && (
            <button 
              className="action-btn"
              onClick={handleMarkAllAsRead}
            >
              <FiCheck />
              Mark All Read
            </button>
          )}
          
          <button 
            className="action-btn danger"
            onClick={handleClearRead}
          >
            <FiTrash2 />
            Clear Read
          </button>
        </div>
      </div>

      <div className="notifications-filters">
        <button 
          className={`filter-btn ${filter === 'all' ? 'active' : ''}`}
          onClick={() => { setFilter('all'); setPage(1); }}
        >
          All
        </button>
        <button 
          className={`filter-btn ${filter === 'unread' ? 'active' : ''}`}
          onClick={() => { setFilter('unread'); setPage(1); }}
        >
          Unread
        </button>
        <button 
          className={`filter-btn ${filter === 'read' ? 'active' : ''}`}
          onClick={() => { setFilter('read'); setPage(1); }}
        >
          Read
        </button>
      </div>

      <div className="notifications-list">
        {loading ? (
          <div className="notifications-loading">
            <div className="loading-spinner"></div>
            <p>Loading notifications...</p>
          </div>
        ) : notifications.length === 0 ? (
          <div className="notifications-empty">
            <FiBell className="empty-icon" />
            <h3>No notifications</h3>
            <p>You're all caught up! Check back later for updates.</p>
          </div>
        ) : (
          <AnimatePresence>
            {notifications.map((notification, index) => (
              <motion.div
                key={notification.id}
                className={`notification-card ${!notification.isRead ? 'unread' : ''}`}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, x: -100 }}
                transition={{ delay: index * 0.05 }}
              >
                <div className="notification-icon">
                  {getNotificationIcon(notification.type)}
                </div>
                
                <div className="notification-body">
                  <div className="notification-title">{notification.title}</div>
                  <div className="notification-message">{notification.message}</div>
                  <div className="notification-meta">
                    <span className="notification-time">{formatTimeAgo(notification.createdAt)}</span>
                    {notification.type && (
                      <span className="notification-type">{notification.type}</span>
                    )}
                  </div>
                </div>
                
                <div className="notification-actions">
                  {!notification.isRead && (
                    <button 
                      className="notif-action-btn"
                      onClick={() => handleMarkAsRead(notification.id)}
                      title="Mark as read"
                    >
                      <FiCheck />
                    </button>
                  )}
                  <button 
                    className="notif-action-btn delete"
                    onClick={() => handleDelete(notification.id)}
                    title="Delete"
                  >
                    <FiTrash2 />
                  </button>
                </div>
              </motion.div>
            ))}
          </AnimatePresence>
        )}
      </div>

      {totalPages > 1 && (
        <div className="notifications-pagination">
          <button 
            disabled={page === 1}
            onClick={() => setPage(p => p - 1)}
          >
            Previous
          </button>
          <span>Page {page} of {totalPages}</span>
          <button 
            disabled={page === totalPages}
            onClick={() => setPage(p => p + 1)}
          >
            Next
          </button>
        </div>
      )}
    </div>
  );
};

export default NotificationsPage;
















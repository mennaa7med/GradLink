import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import { FiUsers, FiFileText, FiBriefcase, FiSettings, FiAlertCircle, FiSearch, FiEdit2, FiTrash2, FiCheck, FiX, FiRefreshCw } from 'react-icons/fi';
import api from '../../api/client';
import './AdminDashboard.css';

const AdminDashboard = () => {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState('overview');
  const [stats, setStats] = useState(null);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [page, setPage] = useState(1);
  const [totalUsers, setTotalUsers] = useState(0);
  const [searchTerm, setSearchTerm] = useState('');
  const [pendingContent, setPendingContent] = useState([]);
  const [reports, setReports] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
  const [showUserModal, setShowUserModal] = useState(false);

  useEffect(() => {
    loadStats();
  }, []);

  useEffect(() => {
    if (activeTab === 'users') {
      loadUsers();
    }
  }, [activeTab, page]);

  const loadStats = async () => {
    try {
      const { data } = await api.get('/api/admin/stats');
      setStats(data);
    } catch (err) {
      if (err.response?.status === 403) {
        setError('Access denied. Admin privileges required.');
      } else {
        setError('Failed to load statistics');
      }
    } finally {
      setLoading(false);
    }
  };

  const loadUsers = async () => {
    try {
      const { data } = await api.get('/api/admin/users', { 
        params: { page, pageSize: 20, search: searchTerm } 
      });
      setUsers(data.users);
      setTotalUsers(data.total);
    } catch (err) {
      console.error('Failed to load users:', err);
    }
  };

  const loadPendingContent = async () => {
    try {
      const { data } = await api.get('/api/admin/pending-content');
      setPendingContent(data);
    } catch (err) {
      console.error('Failed to load pending content:', err);
    }
  };

  const updateUserRole = async (userId, role) => {
    try {
      await api.put(`/api/admin/users/${userId}/role`, { role });
      loadUsers();
      setShowUserModal(false);
    } catch (err) {
      console.error('Failed to update user role:', err);
    }
  };

  const toggleUserStatus = async (userId, isActive) => {
    try {
      await api.put(`/api/admin/users/${userId}/status`, { isActive });
      loadUsers();
    } catch (err) {
      console.error('Failed to update user status:', err);
    }
  };

  const approveContent = async (type, id) => {
    try {
      await api.put(`/api/admin/${type}/${id}/approve`);
      loadPendingContent();
    } catch (err) {
      console.error('Failed to approve content:', err);
    }
  };

  const rejectContent = async (type, id) => {
    try {
      await api.delete(`/api/admin/${type}/${id}`);
      loadPendingContent();
    } catch (err) {
      console.error('Failed to reject content:', err);
    }
  };

  const filteredUsers = users.filter(user => 
    user.fullName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    user.email?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (error) {
    return (
      <div className="admin-dashboard">
        <div className="admin-error">
          <span className="error-icon">🔒</span>
          <h2>{error}</h2>
          <button onClick={() => navigate('/')} className="back-btn">Go Home</button>
        </div>
      </div>
    );
  }

  return (
    <div className="admin-dashboard">
      <div className="admin-header">
        <h1>🛡️ Admin Dashboard</h1>
        <p>Manage and monitor GradLink platform</p>
      </div>

      <div className="admin-tabs">
        <button 
          className={`tab ${activeTab === 'overview' ? 'active' : ''}`}
          onClick={() => setActiveTab('overview')}
        >
          📊 Overview
        </button>
        <button 
          className={`tab ${activeTab === 'users' ? 'active' : ''}`}
          onClick={() => setActiveTab('users')}
        >
          <FiUsers /> Users
        </button>
        <button 
          className={`tab ${activeTab === 'content' ? 'active' : ''}`}
          onClick={() => setActiveTab('content')}
        >
          <FiFileText /> Content
        </button>
        <button 
          className={`tab ${activeTab === 'jobs' ? 'active' : ''}`}
          onClick={() => setActiveTab('jobs')}
        >
          <FiBriefcase /> Jobs
        </button>
        <button 
          className={`tab ${activeTab === 'reports' ? 'active' : ''}`}
          onClick={() => setActiveTab('reports')}
        >
          <FiAlertCircle /> Reports
        </button>
        <button 
          className={`tab ${activeTab === 'settings' ? 'active' : ''}`}
          onClick={() => setActiveTab('settings')}
        >
          <FiSettings /> Settings
        </button>
      </div>

      {loading ? (
        <div className="loading-container">
          <div className="loader"></div>
          <p>Loading dashboard...</p>
        </div>
      ) : (
        <div className="admin-content">
          {activeTab === 'overview' && stats && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              className="stats-grid"
            >
              <div className="stat-card">
                <span className="stat-icon">👤</span>
                <div className="stat-info">
                  <h3>{stats.totalUsers}</h3>
                  <p>Total Users</p>
                </div>
              </div>
              <div className="stat-card">
                <span className="stat-icon">📁</span>
                <div className="stat-info">
                  <h3>{stats.totalProjects}</h3>
                  <p>Projects</p>
                </div>
              </div>
              <div className="stat-card">
                <span className="stat-icon">💼</span>
                <div className="stat-info">
                  <h3>{stats.totalJobs}</h3>
                  <p>Job Postings</p>
                </div>
              </div>
              <div className="stat-card">
                <span className="stat-icon">📄</span>
                <div className="stat-info">
                  <h3>{stats.totalResumes}</h3>
                  <p>Resumes</p>
                </div>
              </div>
              <div className="stat-card">
                <span className="stat-icon">🤝</span>
                <div className="stat-info">
                  <h3>{stats.totalMatches}</h3>
                  <p>Matches</p>
                </div>
              </div>
              <div className="stat-card">
                <span className="stat-icon">💬</span>
                <div className="stat-info">
                  <h3>{stats.totalConversations}</h3>
                  <p>Conversations</p>
                </div>
              </div>

              <div className="recent-users">
                <h3>Recent Users</h3>
                <div className="users-list">
                  {stats.recentUsers?.map(user => (
                    <div key={user.id} className="user-item">
                      <div className="user-avatar">{user.fullName?.charAt(0) || '?'}</div>
                      <div className="user-info">
                        <strong>{user.fullName || 'No name'}</strong>
                        <span>{user.email}</span>
                      </div>
                      <span className="user-date">
                        {new Date(user.createdAt).toLocaleDateString()}
                      </span>
                    </div>
                  ))}
                </div>
              </div>
            </motion.div>
          )}

          {activeTab === 'users' && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              className="users-section"
            >
              <div className="users-header">
                <h3>All Users ({totalUsers})</h3>
                <div className="search-box">
                  <FiSearch />
                  <input 
                    type="text" 
                    placeholder="Search users..." 
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                  />
                </div>
              </div>
              <table className="users-table">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Role</th>
                    <th>Status</th>
                    <th>Joined</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredUsers.map(user => (
                    <tr key={user.id}>
                      <td>
                        <div className="user-cell">
                          <div className="user-avatar-small">{user.fullName?.charAt(0) || '?'}</div>
                          {user.fullName || '-'}
                        </div>
                      </td>
                      <td>{user.email}</td>
                      <td>
                        <span className={`role-badge ${user.roles?.[0]?.toLowerCase() || 'student'}`}>
                          {user.roles?.[0] || 'Student'}
                        </span>
                      </td>
                      <td>
                        <span className={`status-badge ${user.isActive !== false ? 'active' : 'inactive'}`}>
                          {user.isActive !== false ? 'Active' : 'Inactive'}
                        </span>
                      </td>
                      <td>{user.createdAt ? new Date(user.createdAt).toLocaleDateString() : '-'}</td>
                      <td>
                        <div className="action-buttons">
                          <button 
                            className="action-btn edit"
                            onClick={() => { setSelectedUser(user); setShowUserModal(true); }}
                            title="Edit"
                          >
                            <FiEdit2 />
                          </button>
                          <button 
                            className="action-btn toggle"
                            onClick={() => toggleUserStatus(user.id, user.isActive === false)}
                            title={user.isActive !== false ? 'Deactivate' : 'Activate'}
                          >
                            {user.isActive !== false ? <FiX /> : <FiCheck />}
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              <div className="pagination">
                <button disabled={page === 1} onClick={() => setPage(p => p - 1)}>← Previous</button>
                <span>Page {page} of {Math.ceil(totalUsers / 20)}</span>
                <button disabled={page * 20 >= totalUsers} onClick={() => setPage(p => p + 1)}>Next →</button>
              </div>
            </motion.div>
          )}

          {activeTab === 'content' && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              className="content-section"
            >
              <div className="section-header">
                <h3>Pending Content Approval</h3>
                <button className="refresh-btn" onClick={loadPendingContent}><FiRefreshCw /> Refresh</button>
              </div>
              {pendingContent.length === 0 ? (
                <div className="empty-state">
                  <FiCheck />
                  <p>No pending content to review</p>
                </div>
              ) : (
                <div className="pending-list">
                  {pendingContent.map(item => (
                    <div key={item.id} className="pending-item">
                      <div className="pending-info">
                        <span className="pending-type">{item.type}</span>
                        <h4>{item.title}</h4>
                        <p>{item.description?.substring(0, 100)}...</p>
                        <span className="pending-author">By: {item.authorName}</span>
                      </div>
                      <div className="pending-actions">
                        <button className="approve-btn" onClick={() => approveContent(item.type, item.id)}>
                          <FiCheck /> Approve
                        </button>
                        <button className="reject-btn" onClick={() => rejectContent(item.type, item.id)}>
                          <FiX /> Reject
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </motion.div>
          )}

          {activeTab === 'settings' && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              className="settings-section"
            >
              <h3>System Settings</h3>
              <div className="settings-grid">
                <div className="setting-card">
                  <h4>Email Configuration</h4>
                  <p>Configure SMTP settings for sending emails</p>
                  <span className="setting-status configured">Configured</span>
                </div>
                <div className="setting-card">
                  <h4>OAuth Providers</h4>
                  <p>Google, GitHub authentication settings</p>
                  <span className="setting-status pending">Pending Setup</span>
                </div>
                <div className="setting-card">
                  <h4>Payment Gateway</h4>
                  <p>Stripe integration for sponsorships</p>
                  <span className="setting-status pending">Pending Setup</span>
                </div>
                <div className="setting-card">
                  <h4>Push Notifications</h4>
                  <p>VAPID keys for web push</p>
                  <span className="setting-status pending">Pending Setup</span>
                </div>
              </div>
            </motion.div>
          )}
        </div>
      )}

      {/* User Edit Modal */}
      {showUserModal && selectedUser && (
        <div className="modal-overlay" onClick={() => setShowUserModal(false)}>
          <div className="modal-content" onClick={e => e.stopPropagation()}>
            <h3>Edit User: {selectedUser.fullName}</h3>
            <div className="form-group">
              <label>Email</label>
              <input type="text" value={selectedUser.email} disabled />
            </div>
            <div className="form-group">
              <label>Role</label>
              <select defaultValue={selectedUser.roles?.[0] || 'Student'}>
                <option value="Student">Student</option>
                <option value="Company">Company</option>
                <option value="Mentor">Mentor</option>
                <option value="Sponsor">Sponsor</option>
                <option value="Admin">Admin</option>
              </select>
            </div>
            <div className="modal-actions">
              <button onClick={() => setShowUserModal(false)}>Cancel</button>
              <button className="primary" onClick={() => updateUserRole(selectedUser.id, 'Admin')}>
                Save Changes
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default AdminDashboard;




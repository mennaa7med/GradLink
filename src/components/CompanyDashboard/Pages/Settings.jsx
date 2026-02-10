import React, { useState } from 'react';
import { motion } from 'framer-motion';
import './Settings.css';

const Settings = () => {
  const [activeTab, setActiveTab] = useState('profile');
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [companyLogo, setCompanyLogo] = useState(() => {
    return localStorage.getItem('companyLogo') || null;
  });
  const [settings, setSettings] = useState({
    companyName: 'TechCorp Inc.',
    industry: 'Technology',
    email: 'contact@techcorp.com',
    website: 'https://techcorp.com',
    description: 'Leading technology company specializing in innovative solutions.',
    notifications: {
      newApplications: true,
      projectUpdates: true,
      weeklyReports: false,
      emailDigest: true
    }
  });

  const handleInputChange = (field, value) => {
    setSettings(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const handleNotificationChange = (key, value) => {
    setSettings(prev => ({
      ...prev,
      notifications: {
        ...prev.notifications,
        [key]: value
      }
    }));
  };

  const handleLogoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      // Check file size (max 2MB)
      if (file.size > 2 * 1024 * 1024) {
        alert('File size must be less than 2MB');
        return;
      }

      // Check file type
      if (!file.type.match('image.*')) {
        alert('Please select an image file (JPG, PNG, or GIF)');
        return;
      }

      // Read file as base64 and save to localStorage
      const reader = new FileReader();
      reader.onload = (event) => {
        const logoData = event.target.result;
        setCompanyLogo(logoData);
        localStorage.setItem('companyLogo', logoData);
        
        // Dispatch custom event to notify Sidebar
        window.dispatchEvent(new Event('logoChanged'));
        
        alert('Logo updated successfully!');
      };
      reader.readAsDataURL(file);
    }
  };

  const handleRemoveLogo = () => {
    setCompanyLogo(null);
    localStorage.removeItem('companyLogo');
    
    // Dispatch custom event to notify Sidebar
    window.dispatchEvent(new Event('logoChanged'));
    
    alert('Logo removed successfully!');
  };

  const handleSave = () => {
    // Save settings logic here
    console.log('Settings saved:', settings);
    alert('Settings saved successfully!');
  };

  const handleDeleteAccount = () => {
    // Delete account logic here
    console.log('Account deletion requested');
    setShowDeleteModal(false);
  };

  const DeleteModal = () => (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      className="settings-modal-overlay"
      onClick={() => setShowDeleteModal(false)}
    >
      <motion.div
        initial={{ scale: 0.9, opacity: 0 }}
        animate={{ scale: 1, opacity: 1 }}
        className="settings-modal-content"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="settings-modal-header">
          <div className="settings-modal-icon">⚠️</div>
          <h2 className="settings-modal-title">Delete Account</h2>
        </div>
        <p className="settings-modal-description">
          Are you sure you want to delete your account? This action cannot be undone and will permanently remove all your data, projects, and applicant information.
        </p>
        <div className="settings-modal-buttons">
          <button
            onClick={() => setShowDeleteModal(false)}
            className="settings-modal-button cancel"
          >
            Cancel
          </button>
          <button
            onClick={handleDeleteAccount}
            className="settings-modal-button delete"
          >
            Delete Account
          </button>
        </div>
      </motion.div>
    </motion.div>
  );

  const tabs = [
    { id: 'profile', label: 'Company Profile', icon: '🏢' },
    { id: 'notifications', label: 'Notifications', icon: '🔔' },
    { id: 'security', label: 'Security', icon: '🔒' }
  ];

  return (
    <div className="settings-page">
      {/* Page Header */}
      <div className="settings-page-header">
        <h1 className="settings-page-title">Settings</h1>
        <p className="settings-page-description">Manage your company account settings and preferences</p>
      </div>

      {/* Settings Container */}
      <div className="settings-container">
        {/* Settings Tabs */}
        <div className="settings-tabs">
          <nav className="settings-tabs-nav">
            {tabs.map((tab) => (
              <button
                key={tab.id}
                className={`settings-tab-button ${activeTab === tab.id ? 'active' : ''}`}
                onClick={() => setActiveTab(tab.id)}
              >
                <span className="settings-tab-icon">{tab.icon}</span>
                <span className="settings-tab-text">{tab.label}</span>
              </button>
            ))}
          </nav>
        </div>

        {/* Settings Content */}
        <div className="settings-content">
          {activeTab === 'profile' && (
            <div className="settings-section">
              {/* Company Logo */}
              <div className="settings-logo-section">
                <div className="settings-logo-container">
                  {companyLogo ? (
                    <img 
                      src={companyLogo} 
                      alt="Company Logo" 
                      style={{ width: '100%', height: '100%', objectFit: 'cover', borderRadius: '12px' }}
                    />
                  ) : (
                    'TC'
                  )}
                </div>
                <div className="settings-logo-actions">
                  <input
                    type="file"
                    id="logo-upload"
                    accept="image/*"
                    style={{ display: 'none' }}
                    onChange={handleLogoChange}
                  />
                  <button 
                    className="settings-logo-button"
                    onClick={() => document.getElementById('logo-upload').click()}
                  >
                    <span className="settings-logo-icon">📷</span>
                    <span>Change Logo</span>
                  </button>
                  {companyLogo && (
                    <button 
                      className="settings-logo-button remove"
                      onClick={handleRemoveLogo}
                      style={{ 
                        backgroundColor: '#ff4444', 
                        marginLeft: '10px',
                        border: 'none',
                        color: 'white',
                        padding: '8px 16px',
                        borderRadius: '8px',
                        cursor: 'pointer'
                      }}
                    >
                      <span className="settings-logo-icon">🗑️</span>
                      <span>Remove Logo</span>
                    </button>
                  )}
                  <p className="settings-logo-hint">JPG, PNG or GIF. Max size 2MB.</p>
                </div>
              </div>

              {/* Company Information */}
              <div className="settings-form-grid">
                <div className="settings-form-field">
                  <label className="settings-form-label">Company Name</label>
                  <input
                    type="text"
                    className="settings-form-input"
                    value={settings.companyName}
                    onChange={(e) => handleInputChange('companyName', e.target.value)}
                  />
                </div>
                <div className="settings-form-field">
                  <label className="settings-form-label">Industry</label>
                  <select
                    className="settings-form-select"
                    value={settings.industry}
                    onChange={(e) => handleInputChange('industry', e.target.value)}
                  >
                    <option value="Technology">Technology</option>
                    <option value="Healthcare">Healthcare</option>
                    <option value="Finance">Finance</option>
                    <option value="Education">Education</option>
                    <option value="Manufacturing">Manufacturing</option>
                    <option value="Other">Other</option>
                  </select>
                </div>
                <div className="settings-form-field">
                  <label className="settings-form-label">Email</label>
                  <input
                    type="email"
                    className="settings-form-input"
                    value={settings.email}
                    onChange={(e) => handleInputChange('email', e.target.value)}
                  />
                </div>
                <div className="settings-form-field">
                  <label className="settings-form-label">Website</label>
                  <input
                    type="url"
                    className="settings-form-input"
                    value={settings.website}
                    onChange={(e) => handleInputChange('website', e.target.value)}
                  />
                </div>
                <div className="settings-form-field settings-form-full-width">
                  <label className="settings-form-label">Company Description</label>
                  <textarea
                    className="settings-form-textarea"
                    value={settings.description}
                    onChange={(e) => handleInputChange('description', e.target.value)}
                    rows={4}
                  />
                </div>
              </div>
            </div>
          )}

          {activeTab === 'notifications' && (
            <div className="settings-section">
              <div className="settings-notifications-section">
                <h3 className="settings-notifications-title">Notification Preferences</h3>
                
                <div className="settings-notification-item">
                  <div className="settings-notification-info">
                    <div className="settings-notification-title">New Applications</div>
                    <div className="settings-notification-description">
                      Get notified when someone applies to your projects
                    </div>
                  </div>
                  <div 
                    className={`settings-toggle ${settings.notifications.newApplications ? 'active' : ''}`}
                    onClick={() => handleNotificationChange('newApplications', !settings.notifications.newApplications)}
                  >
                    <div className="settings-toggle-thumb"></div>
                  </div>
                </div>

                <div className="settings-notification-item">
                  <div className="settings-notification-info">
                    <div className="settings-notification-title">Project Updates</div>
                    <div className="settings-notification-description">
                      Receive updates about your posted projects
                    </div>
                  </div>
                  <div 
                    className={`settings-toggle ${settings.notifications.projectUpdates ? 'active' : ''}`}
                    onClick={() => handleNotificationChange('projectUpdates', !settings.notifications.projectUpdates)}
                  >
                    <div className="settings-toggle-thumb"></div>
                  </div>
                </div>

                <div className="settings-notification-item">
                  <div className="settings-notification-info">
                    <div className="settings-notification-title">Weekly Reports</div>
                    <div className="settings-notification-description">
                      Get weekly analytics and performance reports
                    </div>
                  </div>
                  <div 
                    className={`settings-toggle ${settings.notifications.weeklyReports ? 'active' : ''}`}
                    onClick={() => handleNotificationChange('weeklyReports', !settings.notifications.weeklyReports)}
                  >
                    <div className="settings-toggle-thumb"></div>
                  </div>
                </div>

                <div className="settings-notification-item">
                  <div className="settings-notification-info">
                    <div className="settings-notification-title">Email Digest</div>
                    <div className="settings-notification-description">
                      Receive daily email summaries
                    </div>
                  </div>
                  <div 
                    className={`settings-toggle ${settings.notifications.emailDigest ? 'active' : ''}`}
                    onClick={() => handleNotificationChange('emailDigest', !settings.notifications.emailDigest)}
                  >
                    <div className="settings-toggle-thumb"></div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {activeTab === 'security' && (
            <div className="settings-section">
              <div className="settings-password-section">
                <h3 className="settings-password-title">Change Password</h3>
                <div className="settings-password-fields">
                  <div className="settings-form-field">
                    <label className="settings-form-label">Current Password</label>
                    <input
                      type="password"
                      className="settings-form-input"
                      placeholder="Enter current password"
                    />
                  </div>
                  <div className="settings-form-field">
                    <label className="settings-form-label">New Password</label>
                    <input
                      type="password"
                      className="settings-form-input"
                      placeholder="Enter new password"
                    />
                  </div>
                  <div className="settings-form-field">
                    <label className="settings-form-label">Confirm New Password</label>
                    <input
                      type="password"
                      className="settings-form-input"
                      placeholder="Confirm new password"
                    />
                  </div>
                </div>
              </div>

              {/* Danger Zone */}
              <div className="settings-danger-section">
                <h3 className="settings-danger-title">Danger Zone</h3>
                <p className="settings-danger-description">
                  Once you delete your account, there is no going back. Please be certain.
                </p>
                <button
                  onClick={() => setShowDeleteModal(true)}
                  className="settings-danger-button"
                >
                  Delete Account
                </button>
              </div>
            </div>
          )}

          {/* Save Button */}
          <div className="settings-save-button">
            <motion.button
              whileHover={{ scale: 1.02 }}
              whileTap={{ scale: 0.98 }}
              onClick={handleSave}
              className="settings-save-btn"
            >
              Save Changes
            </motion.button>
          </div>
        </div>
      </div>

      {/* Delete Account Modal */}
      {showDeleteModal && <DeleteModal />}
    </div>
  );
};

export default Settings;
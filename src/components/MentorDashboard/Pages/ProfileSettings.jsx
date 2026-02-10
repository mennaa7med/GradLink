import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { 
  FiUser, 
  FiMail, 
  FiPhone, 
  FiMapPin,
  FiBriefcase,
  FiLinkedin,
  FiGithub,
  FiGlobe,
  FiClock,
  FiBell,
  FiLock,
  FiCalendar,
  FiSave,
  FiCamera,
  FiEdit2,
  FiAward,
  FiBook
} from 'react-icons/fi';
import './ProfileSettings.css';

// Helper function to get user data from localStorage
const getUserData = () => {
  try {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      return JSON.parse(userStr);
    }
  } catch (e) {
    console.error('Error parsing user data:', e);
  }
  return null;
};

const ProfileSettings = () => {
  const [activeTab, setActiveTab] = useState('profile');
  const [isSaving, setIsSaving] = useState(false);
  
  const userData = getUserData();

  // Profile data
  const [profile, setProfile] = useState({
    fullName: userData?.fullName || 'Mentor',
    email: userData?.email || 'mentor@example.com',
    phone: '+20 100 123 4567',
    bio: 'Experienced software engineer and educator with 10+ years in web development, AI, and cloud computing. Passionate about mentoring the next generation of developers.',
    jobTitle: 'Senior Software Engineer',
    company: 'Tech Solutions Inc.',
    location: 'Cairo, Egypt',
    specializations: ['Web Development', 'AI/ML', 'Cloud Computing', 'Mobile Development'],
    experienceYears: 10,
    languages: ['English', 'Arabic'],
    linkedin: 'linkedin.com/in/ahmedmahmoud',
    github: 'github.com/ahmedmahmoud',
    website: 'ahmedmahmoud.dev',
    education: 'PhD in Computer Science - Cairo University',
    certifications: ['AWS Solutions Architect', 'Google Cloud Professional', 'Certified Scrum Master']
  });

  // Availability settings
  const [availability, setAvailability] = useState({
    acceptingMentees: true,
    maxMentees: 15,
    sessionDuration: 60,
    isFree: true,
    hourlyRate: 50,
    timezone: 'Africa/Cairo',
    availableSlots: {
      sunday: { enabled: false, start: '09:00', end: '17:00' },
      monday: { enabled: true, start: '09:00', end: '17:00' },
      tuesday: { enabled: true, start: '09:00', end: '17:00' },
      wednesday: { enabled: true, start: '09:00', end: '17:00' },
      thursday: { enabled: true, start: '09:00', end: '17:00' },
      friday: { enabled: false, start: '09:00', end: '17:00' },
      saturday: { enabled: false, start: '09:00', end: '17:00' }
    }
  });

  // Notification settings
  const [notifications, setNotifications] = useState({
    emailNewMentee: true,
    emailSessionReminder: true,
    emailMessages: true,
    pushNewMentee: true,
    pushSessionReminder: true,
    pushMessages: false,
    reminderTime: 30 // minutes before session
  });

  const handleSave = async () => {
    setIsSaving(true);
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1000));
    setIsSaving(false);
    alert('Settings saved successfully!');
  };

  const tabs = [
    { id: 'profile', label: 'Profile', icon: <FiUser /> },
    { id: 'availability', label: 'Availability', icon: <FiClock /> },
    { id: 'notifications', label: 'Notifications', icon: <FiBell /> },
    { id: 'security', label: 'Security', icon: <FiLock /> }
  ];

  const days = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'];

  return (
    <div className="profile-settings-page">
      {/* Header */}
      <div className="settings-header">
        <div>
          <h1 className="page-title">Profile & Settings</h1>
          <p className="page-subtitle">Manage your mentor profile and preferences</p>
        </div>
        <button 
          className="mentor-btn mentor-btn-primary"
          onClick={handleSave}
          disabled={isSaving}
        >
          <FiSave /> {isSaving ? 'Saving...' : 'Save Changes'}
        </button>
      </div>

      {/* Tabs */}
      <div className="settings-tabs">
        {tabs.map(tab => (
          <button
            key={tab.id}
            className={`settings-tab ${activeTab === tab.id ? 'active' : ''}`}
            onClick={() => setActiveTab(tab.id)}
          >
            {tab.icon}
            <span>{tab.label}</span>
          </button>
        ))}
      </div>

      {/* Tab Content */}
      <motion.div 
        className="settings-content"
        key={activeTab}
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
      >
        {/* Profile Tab */}
        {activeTab === 'profile' && (
          <div className="profile-tab">
            {/* Avatar Section */}
            <div className="avatar-section">
              <div className="avatar-large">
                {profile.fullName.charAt(0)}
                <button className="avatar-edit">
                  <FiCamera />
                </button>
              </div>
              <div className="avatar-info">
                <h3>{profile.fullName}</h3>
                <p>{profile.jobTitle}</p>
                <span className="mentor-badge-large">
                  <FiAward /> Verified Mentor
                </span>
              </div>
            </div>

            {/* Basic Info */}
            <div className="settings-section">
              <h3 className="section-title">
                <FiUser /> Basic Information
              </h3>
              <div className="form-grid">
                <div className="form-group">
                  <label>Full Name</label>
                  <input
                    type="text"
                    value={profile.fullName}
                    onChange={(e) => setProfile({...profile, fullName: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>Email</label>
                  <input
                    type="email"
                    value={profile.email}
                    onChange={(e) => setProfile({...profile, email: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>Phone</label>
                  <input
                    type="tel"
                    value={profile.phone}
                    onChange={(e) => setProfile({...profile, phone: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>Location</label>
                  <input
                    type="text"
                    value={profile.location}
                    onChange={(e) => setProfile({...profile, location: e.target.value})}
                  />
                </div>
              </div>
              <div className="form-group full-width">
                <label>Bio</label>
                <textarea
                  value={profile.bio}
                  onChange={(e) => setProfile({...profile, bio: e.target.value})}
                  rows={4}
                />
              </div>
            </div>

            {/* Professional Info */}
            <div className="settings-section">
              <h3 className="section-title">
                <FiBriefcase /> Professional Information
              </h3>
              <div className="form-grid">
                <div className="form-group">
                  <label>Job Title</label>
                  <input
                    type="text"
                    value={profile.jobTitle}
                    onChange={(e) => setProfile({...profile, jobTitle: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>Company</label>
                  <input
                    type="text"
                    value={profile.company}
                    onChange={(e) => setProfile({...profile, company: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>Years of Experience</label>
                  <input
                    type="number"
                    value={profile.experienceYears}
                    onChange={(e) => setProfile({...profile, experienceYears: parseInt(e.target.value)})}
                    min="0"
                  />
                </div>
                <div className="form-group">
                  <label>Education</label>
                  <input
                    type="text"
                    value={profile.education}
                    onChange={(e) => setProfile({...profile, education: e.target.value})}
                  />
                </div>
              </div>

              <div className="form-group full-width">
                <label>Specializations</label>
                <div className="tags-input">
                  {profile.specializations.map((spec, index) => (
                    <span key={index} className="tag">
                      {spec}
                      <button onClick={() => setProfile({
                        ...profile,
                        specializations: profile.specializations.filter((_, i) => i !== index)
                      })}>×</button>
                    </span>
                  ))}
                </div>
              </div>

              <div className="form-group full-width">
                <label>Certifications</label>
                <div className="tags-input">
                  {profile.certifications.map((cert, index) => (
                    <span key={index} className="tag cert">
                      <FiAward /> {cert}
                    </span>
                  ))}
                </div>
              </div>
            </div>

            {/* Social Links */}
            <div className="settings-section">
              <h3 className="section-title">
                <FiGlobe /> Social Links
              </h3>
              <div className="form-grid">
                <div className="form-group">
                  <label><FiLinkedin /> LinkedIn</label>
                  <input
                    type="url"
                    value={profile.linkedin}
                    onChange={(e) => setProfile({...profile, linkedin: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label><FiGithub /> GitHub</label>
                  <input
                    type="url"
                    value={profile.github}
                    onChange={(e) => setProfile({...profile, github: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label><FiGlobe /> Website</label>
                  <input
                    type="url"
                    value={profile.website}
                    onChange={(e) => setProfile({...profile, website: e.target.value})}
                  />
                </div>
              </div>
            </div>
          </div>
        )}

        {/* Availability Tab */}
        {activeTab === 'availability' && (
          <div className="availability-tab">
            <div className="settings-section">
              <h3 className="section-title">
                <FiClock /> Mentoring Preferences
              </h3>
              
              <div className="toggle-group">
                <div className="toggle-item">
                  <div className="toggle-info">
                    <span className="toggle-label">Accept New Mentees</span>
                    <span className="toggle-desc">Allow new students to request mentorship</span>
                  </div>
                  <label className="toggle-switch">
                    <input
                      type="checkbox"
                      checked={availability.acceptingMentees}
                      onChange={(e) => setAvailability({...availability, acceptingMentees: e.target.checked})}
                    />
                    <span className="slider"></span>
                  </label>
                </div>
              </div>

              <div className="form-grid">
                <div className="form-group">
                  <label>Maximum Mentees</label>
                  <input
                    type="number"
                    value={availability.maxMentees}
                    onChange={(e) => setAvailability({...availability, maxMentees: parseInt(e.target.value)})}
                    min="1"
                    max="50"
                  />
                </div>
                <div className="form-group">
                  <label>Session Duration (minutes)</label>
                  <select
                    value={availability.sessionDuration}
                    onChange={(e) => setAvailability({...availability, sessionDuration: parseInt(e.target.value)})}
                  >
                    <option value={30}>30 minutes</option>
                    <option value={45}>45 minutes</option>
                    <option value={60}>1 hour</option>
                    <option value={90}>1.5 hours</option>
                    <option value={120}>2 hours</option>
                  </select>
                </div>
                <div className="form-group">
                  <label>Timezone</label>
                  <select
                    value={availability.timezone}
                    onChange={(e) => setAvailability({...availability, timezone: e.target.value})}
                  >
                    <option value="Africa/Cairo">Cairo (GMT+2)</option>
                    <option value="Asia/Riyadh">Riyadh (GMT+3)</option>
                    <option value="Europe/London">London (GMT+0)</option>
                    <option value="America/New_York">New York (GMT-5)</option>
                  </select>
                </div>
              </div>

              <div className="toggle-group">
                <div className="toggle-item">
                  <div className="toggle-info">
                    <span className="toggle-label">Free Mentoring</span>
                    <span className="toggle-desc">Offer free mentorship sessions</span>
                  </div>
                  <label className="toggle-switch">
                    <input
                      type="checkbox"
                      checked={availability.isFree}
                      onChange={(e) => setAvailability({...availability, isFree: e.target.checked})}
                    />
                    <span className="slider"></span>
                  </label>
                </div>
              </div>

              {!availability.isFree && (
                <div className="form-group">
                  <label>Hourly Rate ($)</label>
                  <input
                    type="number"
                    value={availability.hourlyRate}
                    onChange={(e) => setAvailability({...availability, hourlyRate: parseInt(e.target.value)})}
                    min="0"
                  />
                </div>
              )}
            </div>

            <div className="settings-section">
              <h3 className="section-title">
                <FiCalendar /> Available Hours
              </h3>
              <div className="schedule-grid">
                {days.map(day => (
                  <div key={day} className="schedule-row">
                    <label className="day-toggle">
                      <input
                        type="checkbox"
                        checked={availability.availableSlots[day].enabled}
                        onChange={(e) => setAvailability({
                          ...availability,
                          availableSlots: {
                            ...availability.availableSlots,
                            [day]: { ...availability.availableSlots[day], enabled: e.target.checked }
                          }
                        })}
                      />
                      <span className="day-name">{day.charAt(0).toUpperCase() + day.slice(1)}</span>
                    </label>
                    {availability.availableSlots[day].enabled && (
                      <div className="time-inputs">
                        <input
                          type="time"
                          value={availability.availableSlots[day].start}
                          onChange={(e) => setAvailability({
                            ...availability,
                            availableSlots: {
                              ...availability.availableSlots,
                              [day]: { ...availability.availableSlots[day], start: e.target.value }
                            }
                          })}
                        />
                        <span>to</span>
                        <input
                          type="time"
                          value={availability.availableSlots[day].end}
                          onChange={(e) => setAvailability({
                            ...availability,
                            availableSlots: {
                              ...availability.availableSlots,
                              [day]: { ...availability.availableSlots[day], end: e.target.value }
                            }
                          })}
                        />
                      </div>
                    )}
                  </div>
                ))}
              </div>
            </div>
          </div>
        )}

        {/* Notifications Tab */}
        {activeTab === 'notifications' && (
          <div className="notifications-tab">
            <div className="settings-section">
              <h3 className="section-title">
                <FiMail /> Email Notifications
              </h3>
              <div className="toggle-group">
                <div className="toggle-item">
                  <div className="toggle-info">
                    <span className="toggle-label">New Mentee Requests</span>
                    <span className="toggle-desc">Receive email when a new student requests mentorship</span>
                  </div>
                  <label className="toggle-switch">
                    <input
                      type="checkbox"
                      checked={notifications.emailNewMentee}
                      onChange={(e) => setNotifications({...notifications, emailNewMentee: e.target.checked})}
                    />
                    <span className="slider"></span>
                  </label>
                </div>
                <div className="toggle-item">
                  <div className="toggle-info">
                    <span className="toggle-label">Session Reminders</span>
                    <span className="toggle-desc">Receive email reminders before scheduled sessions</span>
                  </div>
                  <label className="toggle-switch">
                    <input
                      type="checkbox"
                      checked={notifications.emailSessionReminder}
                      onChange={(e) => setNotifications({...notifications, emailSessionReminder: e.target.checked})}
                    />
                    <span className="slider"></span>
                  </label>
                </div>
                <div className="toggle-item">
                  <div className="toggle-info">
                    <span className="toggle-label">New Messages</span>
                    <span className="toggle-desc">Receive email when you get new messages</span>
                  </div>
                  <label className="toggle-switch">
                    <input
                      type="checkbox"
                      checked={notifications.emailMessages}
                      onChange={(e) => setNotifications({...notifications, emailMessages: e.target.checked})}
                    />
                    <span className="slider"></span>
                  </label>
                </div>
              </div>
            </div>

            <div className="settings-section">
              <h3 className="section-title">
                <FiBell /> Push Notifications
              </h3>
              <div className="toggle-group">
                <div className="toggle-item">
                  <div className="toggle-info">
                    <span className="toggle-label">New Mentee Requests</span>
                    <span className="toggle-desc">Get push notifications for new mentee requests</span>
                  </div>
                  <label className="toggle-switch">
                    <input
                      type="checkbox"
                      checked={notifications.pushNewMentee}
                      onChange={(e) => setNotifications({...notifications, pushNewMentee: e.target.checked})}
                    />
                    <span className="slider"></span>
                  </label>
                </div>
                <div className="toggle-item">
                  <div className="toggle-info">
                    <span className="toggle-label">Session Reminders</span>
                    <span className="toggle-desc">Get push notifications before sessions</span>
                  </div>
                  <label className="toggle-switch">
                    <input
                      type="checkbox"
                      checked={notifications.pushSessionReminder}
                      onChange={(e) => setNotifications({...notifications, pushSessionReminder: e.target.checked})}
                    />
                    <span className="slider"></span>
                  </label>
                </div>
              </div>

              <div className="form-group">
                <label>Reminder Time (minutes before session)</label>
                <select
                  value={notifications.reminderTime}
                  onChange={(e) => setNotifications({...notifications, reminderTime: parseInt(e.target.value)})}
                >
                  <option value={15}>15 minutes</option>
                  <option value={30}>30 minutes</option>
                  <option value={60}>1 hour</option>
                  <option value={120}>2 hours</option>
                  <option value={1440}>1 day</option>
                </select>
              </div>
            </div>
          </div>
        )}

        {/* Security Tab */}
        {activeTab === 'security' && (
          <div className="security-tab">
            <div className="settings-section">
              <h3 className="section-title">
                <FiLock /> Change Password
              </h3>
              <div className="form-grid">
                <div className="form-group full-width">
                  <label>Current Password</label>
                  <input type="password" placeholder="Enter current password" />
                </div>
                <div className="form-group">
                  <label>New Password</label>
                  <input type="password" placeholder="Enter new password" />
                </div>
                <div className="form-group">
                  <label>Confirm New Password</label>
                  <input type="password" placeholder="Confirm new password" />
                </div>
              </div>
              <button className="mentor-btn mentor-btn-secondary">
                Update Password
              </button>
            </div>

            <div className="settings-section">
              <h3 className="section-title danger">
                <FiLock /> Danger Zone
              </h3>
              <div className="danger-zone">
                <div className="danger-item">
                  <div className="danger-info">
                    <span className="danger-label">Deactivate Account</span>
                    <span className="danger-desc">Temporarily disable your mentor account</span>
                  </div>
                  <button className="mentor-btn danger-btn">Deactivate</button>
                </div>
                <div className="danger-item">
                  <div className="danger-info">
                    <span className="danger-label">Delete Account</span>
                    <span className="danger-desc">Permanently delete your account and all data</span>
                  </div>
                  <button className="mentor-btn danger-btn delete">Delete Account</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </motion.div>
    </div>
  );
};

export default ProfileSettings;







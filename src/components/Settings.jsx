import React, { useState } from 'react';
import './Settings.css';

const Settings = () => {
  const [profileImage, setProfileImage] = useState(null);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setProfileImage(URL.createObjectURL(file));
    }
  };

  return (
    <div className="settings-container">
      <h1 className="settings-title">Account Settings</h1>

      {/* Profile Picture Upload */}
      <section className="settings-section">
        <h2>Profile Picture</h2>
        <div className="profile-image-preview">
          <img
            src={profileImage || '/assets/images/default-profile.png'}
            alt="Profile"
          />
        </div>
        <input type="file" accept="image/*" onChange={handleImageChange} />
      </section>

      {/* Personal Info */}
      <section className="settings-section">
        <h2>Personal Information</h2>
        <input type="text" placeholder="Full Name" />
        <input type="email" placeholder="Email Address" />
        <input type="text" placeholder="Field of Expertise" />
        <button className="btn save-btn">Save Changes</button>
      </section>

      {/* Password */}
      <section className="settings-section">
        <h2>Change Password</h2>
        <input type="password" placeholder="Current Password" />
        <input type="password" placeholder="New Password" />
        <input type="password" placeholder="Confirm New Password" />
        <button className="btn save-btn">Update Password</button>
      </section>

      {/* Notifications */}
      <section className="settings-section">
        <h2>Notifications</h2>
        <label><input type="checkbox" /> Project Updates</label>
        <label><input type="checkbox" /> Mentor Messages</label>
        <label><input type="checkbox" /> Sponsor Offers</label>
      </section>

      {/* UI Preferences */}
      <section className="settings-section">
        <h2>UI Preferences</h2>
        <label>Theme:</label>
        <select>
          <option>Light</option>
          <option>Dark</option>
        </select>

        <label>Language:</label>
        <select>
          <option>English</option>
          <option>العربية</option>
        </select>
      </section>

      {/* Danger Zone */}
      <section className="settings-section danger-zone">
        <h2>Danger Zone</h2>
        <button className="btn delete-btn">Delete Account</button>
      </section>
    </div>
  );
};

export default Settings;

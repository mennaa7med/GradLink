import React, { useState, useEffect } from 'react';
import './Settings.css';
import * as usersApi from '../api/users';
import { useAuth } from '../contexts/AuthContext';

const Settings = () => {
  const { user, logout } = useAuth();
  const [profileImage, setProfileImage] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [profile, setProfile] = useState({
    fullName: '',
    bio: '',
    major: '',
    graduationYear: '',
    company: '',
    location: '',
    phoneNumber: ''
  });

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    try {
      setLoading(true);
      const data = await usersApi.getCurrentUser();
      setProfile({
        fullName: data.fullName || '',
        bio: data.bio || '',
        major: data.major || '',
        graduationYear: data.graduationYear || '',
        company: data.company || '',
        location: data.location || '',
        phoneNumber: data.phoneNumber || ''
      });
      if (data.profilePicture) {
        setProfileImage(data.profilePicture);
      }
    } catch (err) {
      console.error('Failed to load profile:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setProfile(prev => ({ ...prev, [name]: value }));
  };

  const handleImageChange = async (e) => {
    const file = e.target.files[0];
    if (file) {
      // Preview immediately
      setProfileImage(URL.createObjectURL(file));
      
      // Upload to server
      try {
        const result = await usersApi.uploadAvatar(file);
        if (result.profilePicture) {
          setProfileImage(result.profilePicture);
        }
        alert('Profile picture updated successfully!');
      } catch (err) {
        console.error('Failed to upload avatar:', err);
        alert('Failed to upload profile picture. Please try again.');
      }
    }
  };

  const handleSaveProfile = async (e) => {
    e.preventDefault();
    setSaving(true);
    
    try {
      await usersApi.updateProfile({
        ...profile,
        graduationYear: profile.graduationYear ? parseInt(profile.graduationYear) : null
      });
      alert('Profile updated successfully!');
    } catch (err) {
      console.error('Failed to update profile:', err);
      alert('Failed to update profile. Please try again.');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <div className="settings-container"><p>Loading...</p></div>;
  }

  return (
    <div className="settings-container">
      <h1 className="settings-title">Account Settings</h1>

      {/* Profile Picture Upload */}
      <section className="settings-section">
        <h2>Profile Picture</h2>
        <div className="profile-image-preview">
          <img
            src={profileImage || `https://ui-avatars.com/api/?name=${encodeURIComponent(profile.fullName || 'User')}&background=random&size=150`}
            alt="Profile"
          />
        </div>
        <input type="file" accept="image/*" onChange={handleImageChange} />
      </section>

      {/* Personal Info */}
      <section className="settings-section">
        <h2>Personal Information</h2>
        <form onSubmit={handleSaveProfile}>
          <input 
            type="text" 
            name="fullName"
            placeholder="Full Name" 
            value={profile.fullName}
            onChange={handleInputChange}
          />
          <input 
            type="text" 
            name="bio"
            placeholder="Bio" 
            value={profile.bio}
            onChange={handleInputChange}
          />
          <input 
            type="text" 
            name="major"
            placeholder="Major / Field of Expertise" 
            value={profile.major}
            onChange={handleInputChange}
          />
          <input 
            type="number" 
            name="graduationYear"
            placeholder="Graduation Year" 
            value={profile.graduationYear}
            onChange={handleInputChange}
          />
          <input 
            type="text" 
            name="company"
            placeholder="Company" 
            value={profile.company}
            onChange={handleInputChange}
          />
          <input 
            type="text" 
            name="location"
            placeholder="Location" 
            value={profile.location}
            onChange={handleInputChange}
          />
          <input 
            type="tel" 
            name="phoneNumber"
            placeholder="Phone Number" 
            value={profile.phoneNumber}
            onChange={handleInputChange}
          />
          <button type="submit" className="btn save-btn" disabled={saving}>
            {saving ? 'Saving...' : 'Save Changes'}
          </button>
        </form>
      </section>

      {/* Notifications */}
      <section className="settings-section">
        <h2>Notifications</h2>
        <label><input type="checkbox" defaultChecked /> Project Updates</label>
        <label><input type="checkbox" defaultChecked /> Mentor Messages</label>
        <label><input type="checkbox" defaultChecked /> Sponsor Offers</label>
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
        <button className="btn delete-btn" onClick={logout}>Logout</button>
      </section>
    </div>
  );
};

export default Settings;

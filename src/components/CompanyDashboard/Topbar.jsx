import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import './Topbar.css';

const Topbar = ({ toggleSidebar, sidebarOpen }) => {
  const [companyName, setCompanyName] = useState('');
  const [currentTime, setCurrentTime] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    // Get company name from localStorage (set during registration/login)
    const storedCompanyName = localStorage.getItem('companyName') || 'My Company';
    setCompanyName(storedCompanyName);
    
    // Set greeting based on time
    updateGreeting();
    
    // Update time every minute
    const interval = setInterval(updateGreeting, 60000);
    return () => clearInterval(interval);
  }, []);
  
  const updateGreeting = () => {
    const hour = new Date().getHours();
    let greeting = 'Good Evening';
    if (hour < 12) greeting = 'Good Morning';
    else if (hour < 18) greeting = 'Good Afternoon';
    setCurrentTime(greeting);
  };

  const handleCompanyNameChange = (e) => {
    const newName = e.target.value;
    setCompanyName(newName);
    localStorage.setItem('companyName', newName);
    // Trigger event to update other components
    window.dispatchEvent(new Event('companyNameChanged'));
  };

  const handleLogout = () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userId');
    localStorage.removeItem('userName');
    localStorage.removeItem('userRole');
    navigate('/signin');
  };

  return (
    <div className="topbar">
      <div className="topbar-left">
        <motion.button
          whileHover={{ scale: 1.1 }}
          whileTap={{ scale: 0.9 }}
          onClick={toggleSidebar}
          className="topbar-menu-button"
        >
          {sidebarOpen ? '✕' : '☰'}
        </motion.button>
        
        <div className="topbar-company-branding">
          <div className="topbar-company-logo">
            <span className="company-logo-icon">🏢</span>
          </div>
          <div className="topbar-company-info">
            <h2 className="topbar-company-title">{companyName}</h2>
            <p className="topbar-company-welcome">{currentTime}, Welcome back!</p>
          </div>
        </div>
      </div>

      <div className="topbar-right">
        <div className="topbar-user-section">
          <div className="topbar-user-avatar">
            <span>{companyName.charAt(0).toUpperCase()}</span>
          </div>
          <div className="topbar-user-info">
            <span className="topbar-user-name">{companyName}</span>
            <span className="topbar-user-role">Company Portal</span>
          </div>
        </div>
        
        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          onClick={handleLogout}
          className="topbar-logout-button"
        >
          <span className="logout-icon">🚪</span>
          <span>Logout</span>
        </motion.button>
      </div>
    </div>
  );
};

export default Topbar;

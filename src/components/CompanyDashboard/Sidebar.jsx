import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import './Sidebar.css';

const Sidebar = ({ activePage, setActivePage, isOpen }) => {
  const navigate = useNavigate();
  const [companyLogo, setCompanyLogo] = useState(() => {
    return localStorage.getItem('companyLogo') || null;
  });
  const [companyName, setCompanyName] = useState(() => {
    return localStorage.getItem('companyName') || 'My Company';
  });

  useEffect(() => {
    // Listen for logo changes
    const handleLogoChange = () => {
      setCompanyLogo(localStorage.getItem('companyLogo') || null);
    };

    // Listen for company name changes
    const handleCompanyNameChange = () => {
      setCompanyName(localStorage.getItem('companyName') || 'My Company');
    };

    window.addEventListener('logoChanged', handleLogoChange);
    window.addEventListener('companyNameChanged', handleCompanyNameChange);
    
    return () => {
      window.removeEventListener('logoChanged', handleLogoChange);
      window.removeEventListener('companyNameChanged', handleCompanyNameChange);
    };
  }, []);

  // Get company initials for avatar
  const getCompanyInitials = (name) => {
    if (!name) return 'GL';
    const words = name.trim().split(' ');
    if (words.length >= 2) {
      return (words[0].charAt(0) + words[1].charAt(0)).toUpperCase();
    }
    return name.substring(0, 2).toUpperCase();
  };

  const menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: '🏠' },
    { id: 'projects', label: 'Projects', icon: '📁' },
    { id: 'jobs', label: 'Jobs', icon: '💼' },
    { id: 'internships', label: 'Internships', icon: '🎓' },
    { id: 'applicants', label: 'Applicants', icon: '👥' },
    { id: 'analytics', label: 'Analytics', icon: '📊' },
    { id: 'settings', label: 'Settings', icon: '⚙️' },
  ];

  return (
    <div className="sidebar-content">
      {/* Logo Section */}
      <div className="sidebar-logo" onClick={() => navigate('/')} style={{ cursor: 'pointer' }}>
        <motion.div
          initial={false}
          animate={{ opacity: isOpen ? 1 : 0 }}
          transition={{ duration: 0.2 }}
          className="sidebar-logo-content"
        >
          <div className="sidebar-logo-icon">
            {companyLogo ? (
              <img 
                src={companyLogo} 
                alt={`${companyName} Logo`}
                style={{ 
                  width: '100%', 
                  height: '100%', 
                  objectFit: 'cover', 
                  borderRadius: '12px' 
                }}
              />
            ) : (
              getCompanyInitials(companyName)
            )}
          </div>
          {isOpen && (
            <motion.div
              initial={{ opacity: 0, x: -10 }}
              animate={{ opacity: 1, x: 0 }}
              transition={{ delay: 0.1 }}
              className="sidebar-logo-text"
            >
              <h1 className="sidebar-brand-name">{companyName}</h1>
              <p className="sidebar-brand-subtitle">Company Portal</p>
            </motion.div>
          )}
        </motion.div>
      </div>

      {/* Navigation Menu */}
      <nav className="sidebar-nav">
        <ul className="sidebar-menu">
          {menuItems.map((item) => {
            const isActive = activePage === item.id;
            
            return (
              <li key={item.id} className="sidebar-menu-item">
                <motion.button
                  whileHover={{ scale: 1.02 }}
                  whileTap={{ scale: 0.98 }}
                  onClick={() => setActivePage(item.id)}
                  className={`sidebar-menu-button ${isActive ? 'active' : ''}`}
                >
                  <span className="sidebar-menu-icon">{item.icon}</span>
                  {isOpen && (
                    <motion.span
                      initial={{ opacity: 0, x: -10 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ delay: 0.1 }}
                      className="sidebar-menu-text"
                    >
                      {item.label}
                    </motion.span>
                  )}
                </motion.button>
              </li>
            );
          })}
        </ul>
      </nav>

      {/* Company Info */}
      {isOpen && (
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
          className="sidebar-company-info"
        >
          <div className="sidebar-company-card">
            <div className="sidebar-company-card-content">
              <div className="sidebar-company-avatar">
                {companyLogo ? (
                  <img 
                    src={companyLogo} 
                    alt={`${companyName} Avatar`}
                    style={{ 
                      width: '100%', 
                      height: '100%', 
                      objectFit: 'cover', 
                      borderRadius: '8px' 
                    }}
                  />
                ) : (
                  getCompanyInitials(companyName)
                )}
              </div>
              <div className="sidebar-company-details">
                <p className="sidebar-company-name">{companyName}</p>
                <p className="sidebar-company-plan">Premium Plan</p>
              </div>
            </div>
          </div>
        </motion.div>
      )}
    </div>
  );
};

export default Sidebar;

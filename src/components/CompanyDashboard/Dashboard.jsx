import React, { useState } from 'react';
import { motion } from 'framer-motion';
import Sidebar from './Sidebar';
import Topbar from './Topbar';
import Content from './Content';
import './Dashboard.css';

const Dashboard = () => {
  const [activePage, setActivePage] = useState('dashboard');
  const [sidebarOpen, setSidebarOpen] = useState(true);

  const toggleSidebar = () => {
    setSidebarOpen(!sidebarOpen);
  };

  return (
    <div className="dashboard-main">
      <div className="dashboard-main-flex">
        {/* Sidebar */}
        <motion.div
          initial={false}
          animate={{ width: sidebarOpen ? '280px' : '80px' }}
          transition={{ duration: 0.3, ease: 'easeInOut' }}
          className="sidebar-container"
        >
          <Sidebar 
            activePage={activePage} 
            setActivePage={setActivePage}
            isOpen={sidebarOpen}
          />
        </motion.div>

        {/* Main Content Area */}
        <motion.div
          initial={false}
          animate={{ marginLeft: sidebarOpen ? '280px' : '80px' }}
          transition={{ duration: 0.3, ease: 'easeInOut' }}
          className="dashboard-content"
        >
          {/* Topbar */}
          <Topbar 
            toggleSidebar={toggleSidebar}
            sidebarOpen={sidebarOpen}
            setActivePage={setActivePage}
          />

          {/* Content */}
          <Content activePage={activePage} setActivePage={setActivePage} />
        </motion.div>
      </div>
    </div>
  );
};

export default Dashboard;

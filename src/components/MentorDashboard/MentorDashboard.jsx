import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import Sidebar from './Sidebar';
import Topbar from './Topbar';
import Overview from './Pages/Overview';
import MyMentees from './Pages/MyMentees';
import Sessions from './Pages/Sessions';
import Messages from './Pages/Messages';
import ProfileSettings from './Pages/ProfileSettings';
import './MentorDashboard.css';

const MentorDashboard = () => {
  const [activePage, setActivePage] = useState('overview');
  const [sidebarOpen, setSidebarOpen] = useState(true);

  const toggleSidebar = () => {
    setSidebarOpen(!sidebarOpen);
  };

  const pageVariants = {
    initial: { opacity: 0, y: 20 },
    animate: { opacity: 1, y: 0 },
    exit: { opacity: 0, y: -20 }
  };

  const renderPage = () => {
    const pages = {
      overview: <Overview />,
      mentees: <MyMentees />,
      sessions: <Sessions />,
      messages: <Messages />,
      profile: <ProfileSettings />
    };
    return pages[activePage] || <Overview />;
  };

  return (
    <div className="mentor-dashboard">
      <div className="mentor-dashboard-flex">
        {/* Sidebar */}
        <motion.div
          initial={false}
          animate={{ width: sidebarOpen ? '280px' : '80px' }}
          transition={{ duration: 0.3, ease: 'easeInOut' }}
          className="mentor-sidebar-container"
        >
          <Sidebar
            activePage={activePage}
            setActivePage={setActivePage}
            isOpen={sidebarOpen}
          />
        </motion.div>

        {/* Main Content */}
        <motion.div
          initial={false}
          animate={{ marginLeft: sidebarOpen ? '280px' : '80px' }}
          transition={{ duration: 0.3, ease: 'easeInOut' }}
          className="mentor-main-content"
        >
          {/* Topbar */}
          <Topbar
            toggleSidebar={toggleSidebar}
            sidebarOpen={sidebarOpen}
          />

          {/* Page Content */}
          <div className="mentor-page-content">
            <AnimatePresence mode="wait">
              <motion.div
                key={activePage}
                variants={pageVariants}
                initial="initial"
                animate="animate"
                exit="exit"
                transition={{ duration: 0.3 }}
              >
                {renderPage()}
              </motion.div>
            </AnimatePresence>
          </div>
        </motion.div>
      </div>
    </div>
  );
};

export default MentorDashboard;







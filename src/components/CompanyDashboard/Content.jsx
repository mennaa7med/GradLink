import React from 'react';
import { motion } from 'framer-motion';
import DashboardOverview from './Pages/DashboardOverview';
import Projects from './Pages/Projects';
import Jobs from './Pages/Jobs';
import Internships from './Pages/Internships';
import Applicants from './Pages/Applicants';
import Analytics from './Pages/Analytics';
import Settings from './Pages/Settings';

const Content = ({ activePage }) => {
  const pageVariants = {
    initial: { opacity: 0, x: 20 },
    in: { opacity: 1, x: 0 },
    out: { opacity: 0, x: -20 }
  };

  const pageTransition = {
    type: 'tween',
    ease: 'anticipate',
    duration: 0.3
  };

  const renderPage = () => {
    switch (activePage) {
      case 'dashboard':
        return <DashboardOverview />;
      case 'projects':
        return <Projects />;
      case 'jobs':
        return <Jobs />;
      case 'internships':
        return <Internships />;
      case 'applicants':
        return <Applicants />;
      case 'analytics':
        return <Analytics />;
      case 'settings':
        return <Settings />;
      default:
        return <DashboardOverview />;
    }
  };

  return (
    <motion.div
      initial="initial"
      animate="in"
      exit="out"
      variants={pageVariants}
      transition={pageTransition}
      className="dashboard-page-content"
    >
      {renderPage()}
    </motion.div>
  );
};

export default Content;

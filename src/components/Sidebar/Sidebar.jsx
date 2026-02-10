import React, { useState, useRef, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { motion } from "framer-motion";
import "./Sidebar.css";
import img11 from '../../assets/images/site.png';
import {
  FiGrid, FiCheckSquare, FiCalendar, FiBarChart2,
  FiUsers, FiUserCheck, FiBriefcase, FiSettings,
  FiHelpCircle, FiLogOut, FiFileText, FiAward, FiFile, FiFolder
} from 'react-icons/fi';
import { listTasks } from '../../api/tasks';
import { useLanguage } from '../../contexts/LanguageContext';

const Sidebar = () => {
  const [activeIndex, setActiveIndex] = useState(0);
  const itemRefs = useRef([]);
  const [indicatorTop, setIndicatorTop] = useState(0);
  const [taskCount, setTaskCount] = useState(0);
  const navigate = useNavigate();
  const location = useLocation();
  const { t, isRTL } = useLanguage();

  // Fetch task count
  useEffect(() => {
    const fetchTaskCount = async () => {
      try {
        const tasks = await listTasks();
        // Count pending/in-progress tasks
        const pendingTasks = tasks.filter(t => t.status !== 'Completed' && t.status !== 'Cancelled');
        setTaskCount(pendingTasks.length);
      } catch (err) {
        console.error('Failed to fetch tasks:', err);
        setTaskCount(0);
      }
    };
    
    fetchTaskCount();
    
    // Listen for task updates
    const handleTaskUpdate = () => fetchTaskCount();
    window.addEventListener('taskUpdated', handleTaskUpdate);
    
    // Refresh every 30 seconds
    const interval = setInterval(fetchTaskCount, 30000);
    
    return () => {
      window.removeEventListener('taskUpdated', handleTaskUpdate);
      clearInterval(interval);
    };
  }, []);

  useEffect(() => {
    const current = itemRefs.current[activeIndex];
    if (current) {
      setIndicatorTop(current.offsetTop);
    }
  }, [activeIndex]);

  // Update active index based on current route
  useEffect(() => {
    const path = location.pathname;
    const normalizedPath = path.endsWith('/') && path.length > 1 ? path.slice(0, -1) : path;

    let bestMatchIndex = -1;
    let maxMatchLength = -1;

    menuItems.forEach((item, index) => {
      if (item.path && normalizedPath.startsWith(item.path)) {
        if (item.path.length > maxMatchLength) {
          maxMatchLength = item.path.length;
          bestMatchIndex = index;
        }
      }
    });

    if (bestMatchIndex !== -1) {
      setActiveIndex(bestMatchIndex);
    }
  }, [location.pathname]);

  // Format badge text
  const getTaskBadge = () => {
    if (taskCount === 0) return null;
    if (taskCount > 99) return "99+";
    return taskCount.toString();
  };

  const menuItems = [
    { type: "section", label: t('sidebar.menu') },
    { icon: <FiGrid />, label: t('nav.dashboard'), path: "/dashboard" },
    { icon: <FiCheckSquare />, label: t('nav.tasks'), badge: getTaskBadge(), path: "/dashboard/tasks" },
    { icon: <FiFileText />, label: t('nav.materials'), path: "/dashboard/materials" }, 
    { icon: <FiUsers />, label: t('nav.team'), path: "/dashboard/teams" },
    { icon: <FiFolder />, label: t('nav.projects'), path: "/dashboard/projects" },
    { icon: <FiCalendar />, label: t('nav.calendar'), path: "/dashboard/calendar" },
    { icon: <FiUserCheck />, label: t('nav.mentors'), path: "/dashboard/mentors" },
    { icon: <FiBriefcase />, label: t('nav.sponsors'), path: "/dashboard/sponsors" },
    { type: "section", label: t('sidebar.tools') },
    { icon: <FiFile />, label: t('nav.resumeBuilder'), path: "/dashboard/resume-builder" },
    { icon: <FiAward />, label: t('nav.achievements'), path: "/dashboard/achievements" },
    { type: "section", label: t('sidebar.general') },
    { icon: <FiSettings />, label: t('nav.settings'), path: "/dashboard/settings" },
    { icon: <FiHelpCircle />, label: t('nav.help'), path: "/dashboard/help" },
    { icon: <FiLogOut />, label: t('nav.logout'), path: "/dashboard/logout" }
  ];

  const handleMenuClick = (index, item) => {
    setActiveIndex(index);
    if (item.path) {
      navigate(item.path);
    }
  };

  return (
    <motion.div 
      className="sidebar"
      initial={{ x: -250, opacity: 0 }}
      animate={{ x: 0, opacity: 1 }}
      transition={{ duration: 0.5, ease: "easeOut" }}
    >
      <motion.div 
        className="sidebar-indicator" 
        style={{ top: `${indicatorTop}px` }}
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ delay: 0.3 }}
      />

      <motion.div 
        className="logo-section"
        initial={{ scale: 0.8, opacity: 0 }}
        animate={{ scale: 1, opacity: 1 }}
        transition={{ delay: 0.2, duration: 0.5 }}
        onClick={() => navigate('/')}
        style={{ cursor: 'pointer' }}
      >
        <motion.img 
          src={img11} 
          alt="Logo" 
          className="logo-dashboard"
          whileHover={{ rotate: 360, scale: 1.1 }}
          transition={{ duration: 0.6 }}
        />
        <span className="logo-text">GradLink</span>
      </motion.div>

      <motion.div 
        className="menu-section"
        initial="hidden"
        animate="visible"
        variants={{
          hidden: { opacity: 0 },
          visible: {
            opacity: 1,
            transition: {
              staggerChildren: 0.05,
              delayChildren: 0.3
            }
          }
        }}
      >
        {menuItems.map((item, index) => {
          if (item.type === "section") {
            return (
              <motion.p 
                key={index} 
                className="menu-title"
                variants={{
                  hidden: { opacity: 0, x: -20 },
                  visible: { opacity: 1, x: 0 }
                }}
              >
                {item.label}
              </motion.p>
            );
          }

          return (
            <motion.div
              key={index}
              ref={(el) => (itemRefs.current[index] = el)}
              className={`menu-item ${activeIndex === index ? "active" : ""}`}
              onClick={() => handleMenuClick(index, item)}
              variants={{
                hidden: { opacity: 0, x: -20 },
                visible: { opacity: 1, x: 0 }
              }}
              whileHover={{ 
                x: 5, 
                backgroundColor: activeIndex === index ? "rgba(12, 39, 55, 0.08)" : "rgba(12, 39, 55, 0.05)",
                transition: { duration: 0.2 }
              }}
              whileTap={{ scale: 0.98 }}
            >
              <motion.div
                whileHover={{ scale: 1.2, rotate: 5 }}
                transition={{ type: "spring", stiffness: 300 }}
              >
                {item.icon}
              </motion.div>
              <span>{item.label}</span>
              {item.badge && (
                <motion.span 
                  className="badge"
                  initial={{ scale: 0 }}
                  animate={{ scale: 1 }}
                  transition={{ delay: 0.5, type: "spring" }}
                  whileHover={{ scale: 1.1 }}
                >
                  {item.badge}
                </motion.span>
              )}
            </motion.div>
          );
        })}
      </motion.div>
    </motion.div>
  );
};

export default Sidebar; 
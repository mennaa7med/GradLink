import React, { useState, useRef, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "./Sidebar.css";
import img11 from '../../assets/images/site.png';
import {
  FiGrid, FiCheckSquare, FiCalendar, FiBarChart2,
  FiUsers, FiUserCheck, FiBriefcase, FiSettings,
  FiHelpCircle, FiLogOut, FiFileText
} from 'react-icons/fi';

const Sidebar = () => {
  const [activeIndex, setActiveIndex] = useState(0);
  const itemRefs = useRef([]);
  const [indicatorTop, setIndicatorTop] = useState(0);
  const navigate = useNavigate();
  const location = useLocation();

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

  const menuItems = [
    { type: "section", label: "MENU" },
    { icon: <FiGrid />, label: "Dashboard", path: "/dashboard" },
    { icon: <FiCheckSquare />, label: "Tasks", badge: "12+", path: "/dashboard/tasks" },
    { icon: <FiFileText />, label: "Materials", path: "/dashboard/materials" }, 
    { icon: <FiUsers />, label: "Team", path: "/dashboard/teams" },
    { icon: <FiUserCheck />, label: "Mentors", path: "/dashboard/mentors" },
    { icon: <FiBriefcase />, label: "Sponsors", path: "/dashboard/sponsors" },
    { type: "section", label: "GENERAL" },
    { icon: <FiSettings />, label: "Settings", path: "/dashboard/settings" },
    { icon: <FiHelpCircle />, label: "Help", path: "/dashboard/help" },
    { icon: <FiLogOut />, label: "Logout", path: "/" }
  ];

  const handleMenuClick = (index, item) => {
    setActiveIndex(index);
    if (item.path) {
      navigate(item.path);
    }
  };

  return (
    <div className="sidebar">
      <div className="sidebar-indicator" style={{ top: `${indicatorTop}px` }} />

      <div className="logo-section">
        <img src={img11} alt="Logo" className="logo-dashboard" />
        <span className="logo-text">GradLink</span>
      </div>

      <div className="menu-section">
        {menuItems.map((item, index) => {
          if (item.type === "section") {
            return <p key={index} className="menu-title">{item.label}</p>;
          }

          return (
            <div
              key={index}
              ref={(el) => (itemRefs.current[index] = el)}
              className={`menu-item ${activeIndex === index ? "active" : ""}`}
              onClick={() => handleMenuClick(index, item)}
            >
              {item.icon}
              <span>{item.label}</span>
              {item.badge && <span className="badge">{item.badge}</span>}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default Sidebar; 
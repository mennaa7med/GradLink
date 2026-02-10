import './menu.css';
import React, { useState, useRef } from 'react';
import { Link } from 'react-router-dom';
import {
  FiGrid, FiCheckSquare, FiCalendar, FiBarChart2,
  FiUsers, FiUserCheck, FiBriefcase, FiSettings,
  FiHelpCircle, FiLogOut, FiFileText
} from 'react-icons/fi';
import img11 from '../../assets/images/site.png'; // تأكدي من مسار الصورة الصحيح

const Menu = () => {
  const [activeIndex, setActiveIndex] = useState(0);
  const itemRefs = useRef([]);
  const indicatorTop = activeIndex * 50; // أو حسب ارتفاع العنصر

  const menuItems = [
    { type: "section", label: "MENU" },
    { icon: <FiGrid />, label: "Dashboard" },
    { icon: <FiCheckSquare />, label: "Tasks", badge: "12+" },
    { icon: <FiFileText />, label: "Materials" },
    { icon: <FiUsers />, label: "Team" },
    { icon: <FiUserCheck />, label: "Mentors" },
    { icon: <FiBriefcase />, label: "Sponsors" },
    { type: "section", label: "GENERAL" },
    { icon: <FiSettings />, label: "Settings" },
    { icon: <FiHelpCircle />, label: "Help" },
    { icon: <FiLogOut />, label: "Logout" }
  ];
  return (
    <div className="dashboard-containerr">
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
                onClick={() => setActiveIndex(index)}
              >
                {item.icon}
                <span>{item.label}</span>
                {item.badge && <span className="badge">{item.badge}</span>}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default Menu;

import React from 'react';
import './Topbar.css';
import { FiSearch, FiMail, FiBell } from 'react-icons/fi';
import img12 from '../assets/images/mask.png';

const Topbar = () => {
  return (
    <div className="top-bar">
      <div className="search-container">
        <FiSearch className="seearch-icon" />
        <input
          type="text"
          placeholder="select task"
          className="search-input"
        />
      </div>
      <div className="right-section">
        <div className="icon-container">
          <div className="icon-wrapper">
            <FiMail className="icon" />
          </div>
          <div className="icon-wrapper">
            <FiBell className="icon" />
          </div>
        </div>
        <div className="user-section">
          <img
            src={img12}
            alt="User"
            className="user-photo"
          />
          <div className="user-info">
            <div className="username">Menna Ahmed</div>
            <div className="email">mennaahmed10@gmail.com</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Topbar;

import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import './Projectsbank.css';
import { FiSearch } from 'react-icons/fi'; // أيقونة البحث

const Projectsbank = () => {
  const navigate = useNavigate();

  const [scrolled, setScrolled] = useState(false);
  const [showModal, setShowModal] = useState(false); // حالة ظهور النافذة

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 50); // يتحرك الزر بعد 50px scroll
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  return (
    <>
      <div className="idea-project-page">
        {/* Content */}
        <div className="content">
          {/* Title and Search */}
          <div className="header-section">
            <h1 className="page-title">Idea Project</h1>
            <div className="search-bar">
              <FiSearch className="search-iicon" />
              <input type="text" placeholder="Search about project" />
            </div>
          </div>

          {/* Projects Section */}
          <div className="project-box">
            <h2 className="project-title">My Graduation Project</h2>
            <button className="details-btn" onClick={() => navigate('/Detailsproject')}>
              See Details
            </button>          </div>
          <div className="project-box">
            <h2 className="project-title">My Graduation Project</h2>
            <button className="details-btn" onClick={() => navigate('/Detailsproject')}>
              See Details
            </button>          </div>
          <div className="project-box">
            <h2 className="project-title">My Graduation Project</h2>
            <button className="details-btn" onClick={() => navigate('/Detailsproject')}>
              See Details
            </button>          </div>
          <div className="project-box">
            <h2 className="project-title">My Graduation Project</h2>
            <button className="details-btn" onClick={() => navigate('/Detailsproject')}>
              See Details
            </button>          </div>
          <div className="project-box">
            <h2 className="project-title">My Graduation Project</h2>
            <button className="details-btn" onClick={() => navigate('/Detailsproject')}>
              See Details
            </button>          </div>
          <div className="project-box">
            <h2 className="project-title">My Graduation Project</h2>
            <button className="details-btn" onClick={() => navigate('/Detailsproject')}>
              See Details
            </button>          </div>
          <div className="project-box">
            <h2 className="project-title">My Graduation Project</h2>
            <button className="details-btn" onClick={() => navigate('/Detailsproject')}>
              See Details
            </button>          </div>
          {/* ... نفس باقي الـ project-boxes كما في الكود الأصلي */}

        </div>

        {/* Add Project Button */}
        <button
          className={`add-project-btn ${scrolled ? 'scrolled' : ''}`}
          onClick={() => setShowModal(true)} // فتح النافذة عند الضغط
        >
          <span className="plus-icon">+</span> Add Project
        </button>

        {/* Modal Popup */}
        {showModal && (
          <div className="modal-overlay" onClick={() => setShowModal(false)}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
              <div className="modal-header">
                <input type="text" placeholder="Project Name" />
              </div>
              <div className="modal-body">
                <p className="upload-text">To Upload your Project</p>
                <button className="upload-file-btn">Upload File</button>
              </div>
              <div className="modal-footer">
                <button className="done-btn" onClick={() => setShowModal(false)}>
                  Done
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </>
  );
};

export default Projectsbank;

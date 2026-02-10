import React from 'react';
import { Link } from 'react-router-dom';
import './Features.css';
import img3 from '../assets/images/students.png';
import img4 from '../assets/images/graduates.png';

const Features = () => {
  return (
    <div className="features-section">
      <div className="features-header">
        <h1 className="features-title">Our Features</h1>
        <p className="features-subtitle">
          Empowering students and graduates with the tools they need to succeed in their academic and professional journey
        </p>
      </div>

      {/* Students Section */}
      <div className="feature-block student-block">
        <div className="feature-text">
          <div className="feature-text-header">
            <div className="feature-icon">🎓</div>
            <h2>For Students</h2>
          </div>
          <ul>
            <li>Ready-made project ideas to inspire your graduation project</li>
            <li>Access to helpful resources, templates, and guides</li>
            <li>Step-by-step project tracking tools</li>
            <li>Peer collaboration features</li>
            <li>Mentor support from experienced developers</li>
          </ul>
        </div>
        <div className="feature-image">
          <img src={img3} alt="For Students" />
        </div>
      </div>

      {/* Graduates Section */}
      <div className="feature-block graduate-block">
        <div className="feature-image">
          <img src={img4} alt="For Graduates" />
        </div>
        <div className="feature-text">
          <div className="feature-text-header">
            <div className="feature-icon">💼</div>
            <h2>For Graduates</h2>
          </div>
          <ul>
            <li>Job opportunities tailored to your skills</li>
            <li>Project showcase page to impress recruiters</li>
            <li>Sponsor connections to fund your project ideas</li>
            <li>Professional networking with industry experts</li>
            <li>Career guidance and interview preparation</li>
          </ul>
        </div>
      </div>

      <div className="view-more-wrapper">
        <Link to="/AllFeatures" className="view-more-btn">
          Explore All Features
        </Link>
      </div>
    </div>
  );
};

export default Features;

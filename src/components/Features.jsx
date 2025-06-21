import React from 'react';
import './Features.css';
import img3 from '../assets/images/students.png';
import img4 from '../assets/images/graduates.png';

const Features = () => {
  return (
    <div className="features-section">
      <h1 className="features-title">Our Features</h1>

      {/* Students Section */}
      <div className="feature-block student-block">
        <div className="feature-text">
          <h2>For Students</h2>
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
          <h2>For Graduates</h2>
          <ul>
            <li>Job opportunities tailored to your skills</li>
            <li>Project showcase page to impress recruiters</li>
            <li>Sponsor connections to fund your project ideas</li>
          </ul>
        </div>
      </div>
      <div className="view-more-wrapper">
  <button className="view-more-btn">View More</button>
</div>

    </div>
  );
};

export default Features;

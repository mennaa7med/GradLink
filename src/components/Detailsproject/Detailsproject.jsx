import React from 'react';
import './Detailsproject.css';
import Navbar2 from '../Navbar2/Navbar2';

const Detailsproject = () => {
  return (
    <>
      <Navbar2 />
      <div className="project-page">
        <div className="project-ccard">
          <h4 className="label">Project name</h4>
          <h2 className="project-title">AI-Based Traffic Monitoring System</h2>

          <div className="row">
            <p><strong>Project owner :</strong> <span className="link-text">Menna ahmed</span></p>
            <p><strong>Category :</strong> <span className="link-text">Technology / AI</span></p>
          </div>

          <div className="row">
            <p><strong>Submission Date :</strong> <span className="link-text">April 25, 2025</span></p>
            <p><strong>Status :</strong> <span className="link-text">Completed</span></p>

          </div>

          <h4 className="label">Project Description</h4>
          <p className="descriptionn">
            This project is designed to help students easily share and explore innovative ideas by
            providing a simple platform to upload, organize, and showcase their projects. The goal is to
            encourage collaboration, creativity, and knowledge sharing within educational communities.
          </p>

          <button className="download-btnn">Download</button>
        </div>
      </div>
    </>
  );
};

export default Detailsproject;

import React, { useState } from 'react';
import './Matchmaking.css';
import { FiPlus } from 'react-icons/fi';

const Matchmaking = () => {
  const [showJoinModal, setShowJoinModal] = useState(false);
  
  return (
    <div className="matchmaking-container">
      <h1 className="matchmaking-title">Matchmaking</h1>
      
      <div className="projectcard">
        <div className="project-info">
          <div className="project-meta">
            <h3 className="meta-label">Project name</h3>
            <h2 className="project-name">AI-Based Traffic Monitoring System</h2>
          </div>
          
          <div className="project-meta">
            <h3 className="meta-label">Project Description</h3>
            <p className="text">
              This project is designed to help students easily share and 
              explore innovative ideas by providing a simple platform to 
              upload,organize, and showcase their projects. 
              The goal 
              is to encourage collaboration, creativity, and knowledge 
              sharing within educational communities.
            </p>
          </div>
        </div>
        
        <div className="project-actions">
          <button className="joinbtn" onClick={() => setShowJoinModal(true)}>Request to join</button>
        </div>
      </div>
      
      <div className="create-team-container">
        <button className="create-team-btn">
          <FiPlus /> Create New team
        </button>
      </div>

      {/* Join Project Modal */}
      {showJoinModal && (
        <div className="modal-overlay" onClick={() => setShowJoinModal(false)}>
          <div className="join-modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="join-modal-header">
              <h4 className="modal-label">Project name</h4>
              <h2 className="modal-project-name">NovaTrack</h2>
            </div>
            <div className="join-modal-body">
              <h4 className="modal-label">Description of project</h4>
              <p className="modal-description">
                This project aims to monitor traffic patterns using AI-powered image recognition...
              </p>
            </div>
            <div className="join-modal-footer">
              <button className="add-btn" onClick={() => setShowJoinModal(false)}>
                Add
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
export default Matchmaking;



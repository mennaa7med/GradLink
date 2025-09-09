import React, { useState } from 'react';
import './Matchmaking.css';
import { FiPlus } from 'react-icons/fi';

const Matchmaking = () => {
  const [showJoinModal, setShowJoinModal] = useState(false);
  const [showCreateTeamModal, setShowCreateTeamModal] = useState(false);

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
              upload, organize, and showcase their projects. The goal
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
        <button className="create-team-btn" onClick={() => setShowCreateTeamModal(true)}>
          <FiPlus /> Create New Team
        </button>
      </div>

      {/* Join Project Modal */}
      {showJoinModal && (
        <div className="modal-overlay" onClick={() => setShowJoinModal(false)}>
          <div className="join-modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="join-modal-header">
              <h2>Join Project</h2>
            </div>
            <form className="join-form">
              <input type="text" placeholder="Full Name" required />
              <input type="email" placeholder="Email Address" required />
              <input type="text" placeholder="Field of Study" required />
              <input type="text" placeholder="University" required />
              <input type="text" placeholder="Faculty" required />
              <button type="submit" className="add-btn">Submit Request</button>
            </form>
          </div>
        </div>
      )}

      {/* Create Team Modal */}
      {showCreateTeamModal && (
        <div className="modal-overlay" onClick={() => setShowCreateTeamModal(false)}>
          <div className="create-modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="create-modal-header">
              <h2>Create New Team</h2>
            </div>
            <form className="create-team-form">
              <input type="text" placeholder="Team Name" required />
              <input type="number" placeholder="Number of Members" required />
              <textarea placeholder="List of Members, Fields, Universities, Faculties..." rows="4" required></textarea>
              <button type="submit" className="add-btn">Create Team</button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Matchmaking;

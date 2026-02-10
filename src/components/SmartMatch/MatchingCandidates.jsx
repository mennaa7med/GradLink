import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { FaUser, FaStar, FaDownload, FaEye, FaArrowLeft, FaSearch, FaFilter } from 'react-icons/fa';
import './MatchingCandidates.css';

const MatchingCandidates = () => {
  const location = useLocation();
  const [candidates, setCandidates] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('score');
  const [isLoading, setIsLoading] = useState(true);

  // Mock data for demonstration
  const mockCandidates = [
    {
      id: 1,
      name: 'Ahmed Hassan',
      email: 'ahmed.hassan@email.com',
      score: 95,
      experience: '5 years',
      skills: ['React', 'JavaScript', 'Node.js', 'Python', 'MongoDB'],
      resume: 'ahmed_hassan_resume.pdf',
      avatar: 'https://ui-avatars.com/api/?name=Ahmed+Hassan&background=667eea&color=fff'
    },
    {
      id: 2,
      name: 'Sara Mohamed',
      email: 'sara.mohamed@email.com',
      score: 88,
      experience: '3 years',
      skills: ['Python', 'Machine Learning', 'Data Science', 'SQL'],
      resume: 'sara_mohamed_resume.pdf',
      avatar: 'https://ui-avatars.com/api/?name=Sara+Mohamed&background=10b981&color=fff'
    },
    {
      id: 3,
      name: 'Mahmoud Khalil',
      email: 'mahmoud.khalil@email.com',
      score: 92,
      experience: '4 years',
      skills: ['Full Stack', 'React', 'Node.js', 'PostgreSQL', 'AWS'],
      resume: 'mahmoud_khalil_resume.pdf',
      avatar: 'https://ui-avatars.com/api/?name=Mahmoud+Khalil&background=8b5cf6&color=fff'
    },
    {
      id: 4,
      name: 'Fatma Ibrahim',
      email: 'fatma.ibrahim@email.com',
      score: 85,
      experience: '2 years',
      skills: ['UI/UX', 'Figma', 'Adobe XD', 'JavaScript', 'CSS'],
      resume: 'fatma_ibrahim_resume.pdf',
      avatar: 'https://ui-avatars.com/api/?name=Fatma+Ibrahim&background=f59e0b&color=fff'
    }
  ];

  useEffect(() => {
    // Simulate API call
    const loadCandidates = async () => {
      setIsLoading(true);
      await new Promise(resolve => setTimeout(resolve, 1500));
      setCandidates(mockCandidates);
      setIsLoading(false);
    };

    loadCandidates();
  }, []);

  const filteredCandidates = candidates.filter(candidate =>
    candidate.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    candidate.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
    candidate.skills.some(skill => skill.toLowerCase().includes(searchTerm.toLowerCase()))
  );

  const sortedCandidates = [...filteredCandidates].sort((a, b) => {
    switch (sortBy) {
      case 'score':
        return b.score - a.score;
      case 'name':
        return a.name.localeCompare(b.name);
      case 'experience':
        return b.experience.localeCompare(a.experience);
      default:
        return 0;
    }
  });

  const getScoreColor = (score) => {
    if (score >= 90) return '#27ae60';
    if (score >= 80) return '#f39c12';
    if (score >= 70) return '#e67e22';
    return '#e74c3c';
  };

  const handleGoBack = () => {
    window.history.back();
  };

  if (isLoading) {
    return (
      <div className="matching-candidates-page">
        <div className="loading-container">
          <div className="loading-spinner"></div>
          <p>Analyzing candidates...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="matching-candidates-page">
      <div className="container">
        {/* Header */}
        <header className="page-header">
          <button className="back-button" onClick={handleGoBack}>
            <FaArrowLeft />
            Back
          </button>
          <h1 className="page-title">Matching Candidates</h1>
        </header>

        {/* Results Summary */}
        <div className="results-summary">
          <h2>AI-Powered Candidate Matching Results</h2>
          <p>Found {candidates.length} candidates matching your job requirements</p>
        </div>

        {/* Search and Filter */}
        <div className="search-filter-section">
          <div className="search-container">
            <FaSearch className="search-icon" />
            <input
              type="text"
              placeholder="Search candidates by name, email, or skills..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
            />
          </div>
          
          <div className="filter-container">
            <FaFilter className="filter-icon" />
            <select
              value={sortBy}
              onChange={(e) => setSortBy(e.target.value)}
              className="filter-select"
            >
              <option value="score">Sort by Match Score</option>
              <option value="name">Sort by Name</option>
              <option value="experience">Sort by Experience</option>
            </select>
          </div>
        </div>

        {/* Candidates Grid */}
        <div className="candidates-grid">
          {sortedCandidates.map((candidate) => (
            <div key={candidate.id} className="candidate-card">
              <div className="candidate-header">
                <div className="candidate-avatar">
                  <img 
                    src={candidate.avatar} 
                    alt={candidate.name}
                    onError={(e) => {
                      e.target.src = 'https://ui-avatars.com/api/?name=' + encodeURIComponent(candidate.name) + '&background=667eea&color=fff';
                    }}
                  />
                </div>
                <div className="candidate-info">
                  <h3 className="candidate-name">{candidate.name}</h3>
                  <p className="candidate-email">{candidate.email}</p>
                  <p className="candidate-experience">{candidate.experience} experience</p>
                </div>
                <div className="match-score">
                  <div 
                    className="score-circle"
                    style={{ borderColor: getScoreColor(candidate.score) }}
                  >
                    <span 
                      className="score-number"
                      style={{ color: getScoreColor(candidate.score) }}
                    >
                      {candidate.score}%
                    </span>
                  </div>
                  <span className="score-label">Match</span>
                </div>
              </div>

              <div className="candidate-skills">
                <h4>Key Skills:</h4>
                <div className="skills-list">
                  {candidate.skills.map((skill, index) => (
                    <span key={index} className="skill-tag">
                      {skill}
                    </span>
                  ))}
                </div>
              </div>

              <div className="candidate-actions">
                <button className="action-btn view-btn">
                  <FaEye />
                  View Profile
                </button>
                <button className="action-btn download-btn">
                  <FaDownload />
                  Download CV
                </button>
              </div>
            </div>
          ))}
        </div>

        {sortedCandidates.length === 0 && (
          <div className="no-results">
            <FaUser className="no-results-icon" />
            <h3>No candidates found</h3>
            <p>Try adjusting your search criteria</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default MatchingCandidates;

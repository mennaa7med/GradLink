import React, { useEffect, useState, useCallback } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import './Career.css';
import Footer from '../components/Footer';
import { getAllOpportunities, getCareerStats } from '../api/career';
// Fallback imports for when new API is not available
import { listJobs } from '../api/jobs';
import { listInternships } from '../api/internships';
import { listProjects } from '../api/projects';

const Career = () => {
  const [companies, setCompanies] = useState([]);
  const [stats, setStats] = useState(null);
  const [expandedCompany, setExpandedCompany] = useState(null);
  const [expandedSection, setExpandedSection] = useState(null);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [typeFilter, setTypeFilter] = useState('');

  // Try new API first, fallback to old method
  const loadData = useCallback(async () => {
    setLoading(true);
    try {
      // Try to use new Career API endpoint (server-side grouping)
      const [careerData, statsData] = await Promise.all([
        getAllOpportunities({ search, type: typeFilter }),
        getCareerStats().catch(() => null)
      ]);
      
      // Map API response to component format
      const companiesList = careerData.companies.map(c => ({
        id: c.companyId,
        name: c.companyName,
        logo: c.companyLogo,
        industry: c.industry,
        location: c.location,
        website: c.website,
        description: c.description,
        isVerified: c.isVerified,
        jobs: c.jobs || [],
        internships: c.internships || [],
        projects: c.projects || []
      }));
      
      setCompanies(companiesList);
      setStats(statsData);
    } catch (apiError) {
      console.log('New Career API not available, using fallback...', apiError);
      
      // Fallback: Use old method (client-side grouping)
      try {
        const [jobsData, projectsData] = await Promise.all([
          listJobs(),
          listProjects(),
        ]);

        let internshipsData = [];
        try {
          internshipsData = await listInternships();
        } catch (e) {
          console.log('No internships data yet');
        }

        // Group by company (client-side)
        const companyMap = {};

        jobsData.forEach((job) => {
          const companyName = job.companyName || job.postedByName || 'Unknown Company';
          if (!companyMap[companyName]) {
            companyMap[companyName] = { name: companyName, jobs: [], internships: [], projects: [] };
          }
          companyMap[companyName].jobs.push(job);
        });

        internshipsData.forEach((internship) => {
          const companyName = internship.companyName || internship.postedByName || 'Unknown Company';
          if (!companyMap[companyName]) {
            companyMap[companyName] = { name: companyName, jobs: [], internships: [], projects: [] };
          }
          companyMap[companyName].internships.push(internship);
        });

        projectsData.forEach((project) => {
          const companyName = project.companyName || project.owner?.fullName || 'Unknown Company';
          if (!companyMap[companyName]) {
            companyMap[companyName] = { name: companyName, jobs: [], internships: [], projects: [] };
          }
          companyMap[companyName].projects.push(project);
        });

        setCompanies(Object.values(companyMap));
      } catch (fallbackError) {
        console.error('Failed to load career data:', fallbackError);
      }
    } finally {
      setLoading(false);
    }
  }, [search, typeFilter]);

  useEffect(() => {
    loadData();
  }, [loadData]);

  const toggleCompany = (companyName) => {
    if (expandedCompany === companyName) {
      setExpandedCompany(null);
      setExpandedSection(null);
    } else {
      setExpandedCompany(companyName);
      setExpandedSection(null);
    }
  };

  const toggleSection = (section) => {
    if (expandedSection === section) {
      setExpandedSection(null);
    } else {
      setExpandedSection(section);
    }
  };

  const renderItems = (items, type) => {
    if (!items || items.length === 0) {
      return (
        <div className="career-no-items">
          <p>No {type} available</p>
        </div>
      );
    }

    return (
      <div className="career-items-grid">
        {items.map((item, index) => (
          <motion.div
            key={index}
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ delay: index * 0.05 }}
            className="career-item-card"
          >
            <h4 className="career-item-title">{item.title}</h4>
            <p className="career-item-description">{item.description}</p>

            {type === 'jobs' && (
              <div className="career-item-details">
                {item.location && <span className="career-badge">📍 {item.location}</span>}
                {item.employmentType && <span className="career-badge">💼 {item.employmentType}</span>}
                {item.salaryMin && item.salaryMax && (
                  <span className="career-badge">💰 ${item.salaryMin} - ${item.salaryMax}</span>
                )}
              </div>
            )}

            {type === 'internships' && (
              <div className="career-item-details">
                {item.location && <span className="career-badge">📍 {item.location}</span>}
                {item.duration && <span className="career-badge">⏱️ {item.duration}</span>}
                {item.isPaid && <span className="career-badge paid">💰 Paid</span>}
              </div>
            )}

            {type === 'projects' && (
              <div className="career-item-details">
                {item.status && <span className="career-badge">📊 {item.status}</span>}
              </div>
            )}

            {item.skills && item.skills.length > 0 && (
              <div className="career-skills-list">
                {item.skills.map((skill, idx) => (
                  <span key={idx} className="career-skill-badge">
                    {skill}
                  </span>
                ))}
              </div>
            )}

            {item.technologies && (
              <div className="career-skills-list">
                {item.technologies.split(',').map((tech, idx) => (
                  <span key={idx} className="career-skill-badge">
                    {tech.trim()}
                  </span>
                ))}
              </div>
            )}
          </motion.div>
        ))}
      </div>
    );
  };

  return (
    <div className="career-page">

      <div className="career-container">
        {/* Header */}
        <motion.div
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          className="career-header"
        >
          <h1 className="career-main-title">Career Opportunities</h1>
          <p className="career-main-subtitle">Explore jobs, internships, and projects from top companies</p>
          
          {/* Stats Display */}
          {stats && (
            <div className="career-stats-bar">
              <div className="career-stat-item">
                <span className="stat-number">{stats.totalJobs || 0}</span>
                <span className="stat-label">Jobs</span>
              </div>
              <div className="career-stat-item">
                <span className="stat-number">{stats.totalInternships || 0}</span>
                <span className="stat-label">Internships</span>
              </div>
              <div className="career-stat-item">
                <span className="stat-number">{stats.totalProjects || 0}</span>
                <span className="stat-label">Projects</span>
              </div>
              <div className="career-stat-item">
                <span className="stat-number">{stats.totalCompanies || companies.length}</span>
                <span className="stat-label">Companies</span>
              </div>
            </div>
          )}
          
          {/* Search & Filter */}
          <div className="career-search-container">
            <input
              type="text"
              placeholder="🔍 Search jobs, skills, or companies..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="career-search-input"
              onKeyDown={(e) => e.key === 'Enter' && loadData()}
            />
            <select
              value={typeFilter}
              onChange={(e) => setTypeFilter(e.target.value)}
              className="career-filter-dropdown"
            >
              <option value="">All Types</option>
              <option value="Job">💼 Jobs</option>
              <option value="Internship">🎓 Internships</option>
              <option value="Project">📁 Projects</option>
            </select>
            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              onClick={loadData}
              className="career-search-button"
              disabled={loading}
            >
              {loading ? '⏳' : '🔍'} Search
            </motion.button>
          </div>
          
          <motion.button
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
            onClick={() => {
              setSearch('');
              setTypeFilter('');
              loadData();
            }}
            className="career-refresh-button"
            disabled={loading}
          >
            <span className="refresh-icon">{loading ? '⏳' : '🔄'}</span>
            {loading ? 'Refreshing...' : 'Refresh Opportunities'}
          </motion.button>
        </motion.div>

        {/* Content */}
        <div className="career-wrapper">
          {loading ? (
            <div className="career-loading">
              <div className="loader"></div>
              <p>Loading opportunities...</p>
            </div>
          ) : companies.length === 0 ? (
            <div className="career-empty">
              <span className="empty-icon">📭</span>
              <h2>No Companies Found</h2>
              <p>There are no career opportunities available at the moment.</p>
            </div>
          ) : (
            <div className="career-companies-container">
              {companies.map((company, companyIndex) => (
                <motion.div
                  key={companyIndex}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: companyIndex * 0.1 }}
                  className="company-accordion"
                >
                  {/* Company Header */}
                  <button
                    className={`company-header ${expandedCompany === company.name ? 'expanded' : ''}`}
                    onClick={() => toggleCompany(company.name)}
                  >
                    <div className="company-info">
                      {company.logo ? (
                        <img 
                          src={company.logo} 
                          alt={company.name}
                          className="company-logo-img"
                          onError={(e) => {
                            e.target.style.display = 'none';
                            e.target.nextElementSibling.style.display = 'flex';
                          }}
                        />
                      ) : (
                        <div className="company-icon">🏢</div>
                      )}
                      <div className="company-icon" style={{ display: 'none' }}>🏢</div>
                      <div className="company-details">
                        <h2 className="company-name">{company.name}</h2>
                        <div className="company-stats">
                          <span>💼 {company.jobs.length} Jobs</span>
                          <span>🎓 {company.internships.length} Internships</span>
                          <span>📁 {company.projects.length} Projects</span>
                        </div>
                      </div>
                    </div>
                    <span className="expand-icon">{expandedCompany === company.name ? '▼' : '▶'}</span>
                  </button>

                  {/* Company Sections */}
                  <AnimatePresence>
                    {expandedCompany === company.name && (
                      <motion.div
                        initial={{ height: 0, opacity: 0 }}
                        animate={{ height: 'auto', opacity: 1 }}
                        exit={{ height: 0, opacity: 0 }}
                        transition={{ duration: 0.3 }}
                        className="company-sections"
                      >
                        {/* Jobs Section */}
                        {company.jobs.length > 0 && (
                          <div className="section-wrapper">
                            <button
                              className={`section-header ${expandedSection === 'jobs' ? 'expanded' : ''}`}
                              onClick={() => toggleSection('jobs')}
                            >
                              <span className="section-title">💼 Jobs ({company.jobs.length})</span>
                              <span className="section-arrow">{expandedSection === 'jobs' ? '▼' : '▶'}</span>
                            </button>
                            <AnimatePresence>
                              {expandedSection === 'jobs' && (
                                <motion.div
                                  initial={{ height: 0, opacity: 0 }}
                                  animate={{ height: 'auto', opacity: 1 }}
                                  exit={{ height: 0, opacity: 0 }}
                                  className="section-content"
                                >
                                  {renderItems(company.jobs, 'jobs')}
                                </motion.div>
                              )}
                            </AnimatePresence>
                          </div>
                        )}

                        {/* Internships Section */}
                        {company.internships.length > 0 && (
                          <div className="section-wrapper">
                            <button
                              className={`section-header ${expandedSection === 'internships' ? 'expanded' : ''}`}
                              onClick={() => toggleSection('internships')}
                            >
                              <span className="section-title">🎓 Internships ({company.internships.length})</span>
                              <span className="section-arrow">{expandedSection === 'internships' ? '▼' : '▶'}</span>
                            </button>
                            <AnimatePresence>
                              {expandedSection === 'internships' && (
                                <motion.div
                                  initial={{ height: 0, opacity: 0 }}
                                  animate={{ height: 'auto', opacity: 1 }}
                                  exit={{ height: 0, opacity: 0 }}
                                  className="section-content"
                                >
                                  {renderItems(company.internships, 'internships')}
                                </motion.div>
                              )}
                            </AnimatePresence>
                          </div>
                        )}

                        {/* Projects Section */}
                        {company.projects.length > 0 && (
                          <div className="section-wrapper">
                            <button
                              className={`section-header ${expandedSection === 'projects' ? 'expanded' : ''}`}
                              onClick={() => toggleSection('projects')}
                            >
                              <span className="section-title">📁 Projects ({company.projects.length})</span>
                              <span className="section-arrow">{expandedSection === 'projects' ? '▼' : '▶'}</span>
                            </button>
                            <AnimatePresence>
                              {expandedSection === 'projects' && (
                                <motion.div
                                  initial={{ height: 0, opacity: 0 }}
                                  animate={{ height: 'auto', opacity: 1 }}
                                  exit={{ height: 0, opacity: 0 }}
                                  className="section-content"
                                >
                                  {renderItems(company.projects, 'projects')}
                                </motion.div>
                              )}
                            </AnimatePresence>
                          </div>
                        )}
                      </motion.div>
                    )}
                  </AnimatePresence>
                </motion.div>
              ))}
            </div>
          )}
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default Career;


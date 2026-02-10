import React, { useState, useEffect, useCallback } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { getAllOpportunities, getCareerStats } from '../../api/career';
import './CareerPage.css';

const CareerPage = () => {
  // State
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [data, setData] = useState(null);
  const [stats, setStats] = useState(null);
  
  // Filters
  const [search, setSearch] = useState('');
  const [location, setLocation] = useState('');
  const [typeFilter, setTypeFilter] = useState('');
  const [employmentType, setEmploymentType] = useState('');
  const [page, setPage] = useState(1);
  
  // Load data
  const loadData = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const [opportunitiesData, statsData] = await Promise.all([
        getAllOpportunities({
          search,
          location,
          type: typeFilter,
          employmentType,
          page,
          pageSize: 10
        }),
        getCareerStats()
      ]);
      
      setData(opportunitiesData);
      setStats(statsData);
    } catch (err) {
      console.error('Failed to load career data:', err);
      setError('Failed to load opportunities. Please try again later.');
    } finally {
      setLoading(false);
    }
  }, [search, location, typeFilter, employmentType, page]);
  
  useEffect(() => {
    loadData();
  }, [loadData]);
  
  // Handle search
  const handleSearch = (e) => {
    e.preventDefault();
    setPage(1);
    loadData();
  };
  
  // Handle filter change
  const handleTypeFilter = (type) => {
    setTypeFilter(type === typeFilter ? '' : type);
    setPage(1);
  };
  
  // Format date
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const now = new Date();
    const diffTime = Math.abs(now - date);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    
    if (diffDays === 0) return 'Today';
    if (diffDays === 1) return 'Yesterday';
    if (diffDays < 7) return `${diffDays} days ago`;
    if (diffDays < 30) return `${Math.floor(diffDays / 7)} weeks ago`;
    return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
  };
  
  // Format salary
  const formatSalary = (min, max) => {
    const format = (num) => {
      if (num >= 1000) return `$${(num / 1000).toFixed(0)}k`;
      return `$${num}`;
    };
    
    if (min && max) return `${format(min)} - ${format(max)}`;
    if (min) return `From ${format(min)}`;
    if (max) return `Up to ${format(max)}`;
    return null;
  };
  
  // Get company initials
  const getInitials = (name) => {
    if (!name) return '?';
    return name.split(' ').map(w => w[0]).join('').toUpperCase().slice(0, 2);
  };
  
  // Render opportunity card
  const OpportunityCard = ({ opportunity, type }) => (
    <motion.div
      className={`career-opportunity-card ${type.toLowerCase()}`}
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      whileHover={{ y: -4 }}
      transition={{ duration: 0.2 }}
    >
      <h4 className="career-opportunity-title">{opportunity.title}</h4>
      
      {opportunity.description && (
        <p className="career-opportunity-description">{opportunity.description}</p>
      )}
      
      <div className="career-opportunity-meta">
        {opportunity.location && (
          <span className="career-opportunity-tag">📍 {opportunity.location}</span>
        )}
        {opportunity.employmentType && (
          <span className="career-opportunity-tag">💼 {opportunity.employmentType}</span>
        )}
        {opportunity.duration && (
          <span className="career-opportunity-tag">⏱️ {opportunity.duration}</span>
        )}
        {opportunity.isPaid !== undefined && (
          <span className="career-opportunity-tag">
            {opportunity.isPaid ? '💰 Paid' : '📚 Unpaid'}
          </span>
        )}
      </div>
      
      {opportunity.skills && opportunity.skills.length > 0 && (
        <div className="career-opportunity-skills">
          {opportunity.skills.slice(0, 4).map((skill, idx) => (
            <span key={idx} className="career-skill-tag">{skill}</span>
          ))}
          {opportunity.skills.length > 4 && (
            <span className="career-skill-tag">+{opportunity.skills.length - 4}</span>
          )}
        </div>
      )}
      
      <div className="career-opportunity-footer">
        <div>
          <span className="career-opportunity-date">
            Posted {formatDate(opportunity.createdAt)}
          </span>
          {(opportunity.salaryMin || opportunity.salaryMax) && (
            <div className="career-salary">
              {formatSalary(opportunity.salaryMin, opportunity.salaryMax)}
            </div>
          )}
        </div>
        <button className="career-apply-btn">
          View Details →
        </button>
      </div>
    </motion.div>
  );
  
  // Render company card
  const CompanyCard = ({ company, index }) => (
    <motion.div
      className="career-company-card"
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ delay: index * 0.1 }}
    >
      {/* Company Header */}
      <div className="career-company-header">
        <div className="career-company-logo">
          {company.companyLogo ? (
            <img src={company.companyLogo} alt={company.companyName} />
          ) : (
            getInitials(company.companyName)
          )}
        </div>
        
        <div className="career-company-info">
          <h3 className="career-company-name">
            {company.companyName}
            {company.isVerified && (
              <span className="career-verified-badge">✓ Verified</span>
            )}
          </h3>
          <div className="career-company-meta">
            {company.industry && <span>🏢 {company.industry}</span>}
            {company.location && <span>📍 {company.location}</span>}
            {company.website && (
              <a href={company.website} target="_blank" rel="noopener noreferrer">
                🔗 Website
              </a>
            )}
          </div>
        </div>
        
        <div className="career-company-counts">
          {company.totalJobs > 0 && (
            <div className="career-count-badge jobs">
              <span className="career-count-value">{company.totalJobs}</span>
              <span className="career-count-label">Jobs</span>
            </div>
          )}
          {company.totalInternships > 0 && (
            <div className="career-count-badge internships">
              <span className="career-count-value">{company.totalInternships}</span>
              <span className="career-count-label">Internships</span>
            </div>
          )}
          {company.totalProjects > 0 && (
            <div className="career-count-badge projects">
              <span className="career-count-value">{company.totalProjects}</span>
              <span className="career-count-label">Projects</span>
            </div>
          )}
        </div>
      </div>
      
      {/* Opportunities Section */}
      <div className="career-opportunities-section">
        {/* Jobs */}
        {company.jobs && company.jobs.length > 0 && (
          <div>
            <h4 className="career-section-title jobs">
              💼 Jobs ({company.jobs.length})
            </h4>
            <div className="career-opportunities-grid">
              {company.jobs.map((job) => (
                <OpportunityCard key={`job-${job.id}`} opportunity={job} type="Job" />
              ))}
            </div>
          </div>
        )}
        
        {/* Internships */}
        {company.internships && company.internships.length > 0 && (
          <div style={{ marginTop: company.jobs?.length > 0 ? '24px' : 0 }}>
            <h4 className="career-section-title internships">
              🎓 Internships ({company.internships.length})
            </h4>
            <div className="career-opportunities-grid">
              {company.internships.map((internship) => (
                <OpportunityCard 
                  key={`internship-${internship.id}`} 
                  opportunity={internship} 
                  type="Internship" 
                />
              ))}
            </div>
          </div>
        )}
        
        {/* Projects */}
        {company.projects && company.projects.length > 0 && (
          <div style={{ marginTop: (company.jobs?.length > 0 || company.internships?.length > 0) ? '24px' : 0 }}>
            <h4 className="career-section-title projects">
              🚀 Project Opportunities ({company.projects.length})
            </h4>
            <div className="career-opportunities-grid">
              {company.projects.map((project) => (
                <OpportunityCard 
                  key={`project-${project.id}`} 
                  opportunity={project} 
                  type="Project" 
                />
              ))}
            </div>
          </div>
        )}
      </div>
    </motion.div>
  );

  return (
    <div className="career-page">
      {/* Hero Section */}
      <section className="career-hero">
        <motion.div 
          className="career-hero-content"
          initial={{ opacity: 0, y: 30 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
        >
          <h1>Find Your Next Opportunity</h1>
          <p>
            Discover jobs, internships, and project opportunities from top companies
          </p>
          
          {/* Stats */}
          {stats && (
            <motion.div 
              className="career-stats"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 0.3 }}
            >
              <div className="career-stat">
                <span className="career-stat-value">{stats.totalJobs || 0}</span>
                <span className="career-stat-label">Jobs</span>
              </div>
              <div className="career-stat">
                <span className="career-stat-value">{stats.totalInternships || 0}</span>
                <span className="career-stat-label">Internships</span>
              </div>
              <div className="career-stat">
                <span className="career-stat-value">{stats.totalProjects || 0}</span>
                <span className="career-stat-label">Projects</span>
              </div>
              <div className="career-stat">
                <span className="career-stat-value">{stats.totalCompanies || 0}</span>
                <span className="career-stat-label">Companies</span>
              </div>
            </motion.div>
          )}
        </motion.div>
      </section>
      
      {/* Search Section */}
      <section className="career-search-section">
        <motion.div 
          className="career-search-card"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.2 }}
        >
          <form onSubmit={handleSearch}>
            <div className="career-search-row">
              <div className="career-search-input-wrapper">
                <input
                  type="text"
                  className="career-search-input"
                  placeholder="Search jobs, skills, or companies..."
                  value={search}
                  onChange={(e) => setSearch(e.target.value)}
                />
              </div>
              
              <input
                type="text"
                className="career-filter-select"
                placeholder="📍 Location"
                value={location}
                onChange={(e) => setLocation(e.target.value)}
              />
              
              <select
                className="career-filter-select"
                value={employmentType}
                onChange={(e) => setEmploymentType(e.target.value)}
              >
                <option value="">All Types</option>
                <option value="Full-time">Full-time</option>
                <option value="Part-time">Part-time</option>
                <option value="Contract">Contract</option>
                <option value="Remote">Remote</option>
              </select>
              
              <button type="submit" className="career-search-btn">
                🔍 Search
              </button>
            </div>
          </form>
          
          {/* Filter Pills */}
          <div className="career-filter-pills">
            <button 
              className={`career-filter-pill ${typeFilter === '' ? 'active' : ''}`}
              onClick={() => handleTypeFilter('')}
            >
              All Opportunities
            </button>
            <button 
              className={`career-filter-pill ${typeFilter === 'Job' ? 'active' : ''}`}
              onClick={() => handleTypeFilter('Job')}
            >
              💼 Jobs
            </button>
            <button 
              className={`career-filter-pill ${typeFilter === 'Internship' ? 'active' : ''}`}
              onClick={() => handleTypeFilter('Internship')}
            >
              🎓 Internships
            </button>
            <button 
              className={`career-filter-pill ${typeFilter === 'Project' ? 'active' : ''}`}
              onClick={() => handleTypeFilter('Project')}
            >
              🚀 Projects
            </button>
          </div>
        </motion.div>
      </section>
      
      {/* Main Content */}
      <main className="career-content">
        {/* Error State */}
        {error && (
          <div className="career-error">
            <span className="career-error-icon">⚠️</span>
            {error}
          </div>
        )}
        
        {/* Loading State */}
        {loading && (
          <div className="career-loading">
            <div className="career-loading-spinner"></div>
            <p>Loading opportunities...</p>
          </div>
        )}
        
        {/* Empty State */}
        {!loading && !error && (!data?.companies || data.companies.length === 0) && (
          <motion.div 
            className="career-empty"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
          >
            <div className="career-empty-icon">🔍</div>
            <h3>No opportunities found</h3>
            <p>Try adjusting your search filters or check back later for new postings.</p>
          </motion.div>
        )}
        
        {/* Company Cards */}
        {!loading && !error && data?.companies && data.companies.length > 0 && (
          <AnimatePresence>
            {data.companies.map((company, index) => (
              <CompanyCard 
                key={company.companyId} 
                company={company} 
                index={index}
              />
            ))}
          </AnimatePresence>
        )}
        
        {/* Pagination */}
        {!loading && data?.companies && data.companies.length > 0 && (
          <div className="career-pagination">
            <button 
              className="career-pagination-btn"
              onClick={() => setPage(p => Math.max(1, p - 1))}
              disabled={page === 1}
            >
              ← Previous
            </button>
            <button className="career-pagination-btn active">
              {page}
            </button>
            <button 
              className="career-pagination-btn"
              onClick={() => setPage(p => p + 1)}
              disabled={data.companies.length < 10}
            >
              Next →
            </button>
          </div>
        )}
      </main>
    </div>
  );
};

export default CareerPage;



















import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Opportunities.css';
import { FiSearch, FiBriefcase, FiMapPin, FiClock, FiDollarSign } from 'react-icons/fi';
import { listInternships } from '../api/internships';
import { listJobs } from '../api/jobs';

const Opportunities = () => {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState('internships');
  const [searchQuery, setSearchQuery] = useState('');
  const [internships, setInternships] = useState([]);
  const [jobs, setJobs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Fallback data for when API returns empty
  const fallbackInternships = [
    {
      id: 1,
      title: 'Frontend Development Intern',
      companyName: 'CodeWave Technologies',
      description: 'Work on building responsive user interfaces using HTML, CSS, JavaScript, and React.js.',
      location: 'Remote',
      duration: '3 months',
      isPaid: true,
      stipend: 500
    },
    {
      id: 2,
      title: 'Machine Learning Intern',
      companyName: 'DeepVision Labs',
      description: 'Assist in building predictive models, training datasets, and applying ML algorithms.',
      location: 'Cairo, Egypt',
      duration: '6 months',
      isPaid: true,
      stipend: 800
    },
    {
      id: 3,
      title: 'UI/UX Design Intern',
      companyName: 'PixelFrame Studio',
      description: 'Collaborate on wireframing, prototyping, and user testing to create seamless experiences.',
      location: 'Remote',
      duration: '4 months',
      isPaid: false
    }
  ];

  const fallbackJobs = [
    {
      id: 1,
      title: 'Frontend Developer',
      companyName: 'NovaTech Solutions',
      description: 'Build modern and responsive web applications using React.js, Vue.js, and TailwindCSS.',
      location: 'Cairo, Egypt',
      salaryRange: '$40k - $60k'
    },
    {
      id: 2,
      title: 'Data Scientist',
      companyName: 'InsightSphere',
      description: 'Analyze large datasets, build machine learning models, and deliver actionable insights.',
      location: 'Remote',
      salaryRange: '$60k - $90k'
    },
    {
      id: 3,
      title: 'Backend Developer',
      companyName: 'CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs and databases.',
      location: 'Alexandria, Egypt',
      salaryRange: '$45k - $70k'
    }
  ];

  useEffect(() => {
    const loadData = async () => {
      setLoading(true);
      try {
        const [internshipsData, jobsData] = await Promise.all([
          listInternships().catch(() => []),
          listJobs().catch(() => [])
        ]);
        
        setInternships(internshipsData?.length > 0 ? internshipsData : fallbackInternships);
        setJobs(jobsData?.length > 0 ? jobsData : fallbackJobs);
      } catch (err) {
        console.error('Failed to load opportunities:', err);
        setInternships(fallbackInternships);
        setJobs(fallbackJobs);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, []);

  // Filter opportunities based on search query
  const filteredInternships = internships.filter(
    (internship) =>
      internship.title?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      internship.description?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      internship.companyName?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const filteredJobs = jobs.filter(
    (job) =>
      job.title?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      job.description?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      job.companyName?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const handleApply = (type, id) => {
    // Navigate to career page or show apply modal
    navigate('/career');
  };

  return (
    <div className="opportunities-container">
      <h1 className="opportunities-title">Opportunities</h1>

      <div className="opportunities-search-container">
        <div className="search-boxx">
          <FiSearch className="searchicon" />
          <input
            type="text"
            placeholder={activeTab === 'internships' ? 'Search internships...' : 'Search jobs...'}
            className="search-input"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>

        <div className="tabs-container">
          <button
            className={`tab ${activeTab === 'internships' ? 'active' : ''}`}
            onClick={() => setActiveTab('internships')}
          >
            Internships ({internships.length})
          </button>
          <button
            className={`tab ${activeTab === 'jobs' ? 'active' : ''}`}
            onClick={() => setActiveTab('jobs')}
          >
            Jobs ({jobs.length})
          </button>
        </div>
      </div>

      {loading ? (
        <div className="opportunities-loading">
          <div className="loading-spinner"></div>
          <p>Loading opportunities...</p>
        </div>
      ) : (
        <div className="opportunities-grid">
          {activeTab === 'internships' ? (
            filteredInternships.length > 0 ? (
              filteredInternships.map((internship) => (
                <div className="opportunity-card" key={internship.id}>
                  <div className="opportunity-header">
                    <div className="company-logo">
                      {internship.companyName?.charAt(0) || 'C'}
                    </div>
                    <div className="opportunity-info">
                      <h3 className="opportunity-title">{internship.title}</h3>
                      <p className="company-name">{internship.companyName}</p>
                    </div>
                  </div>
                  <p className="opportunity-description">{internship.description}</p>
                  <div className="opportunity-meta">
                    {internship.location && (
                      <span className="meta-item">
                        <FiMapPin /> {internship.location}
                      </span>
                    )}
                    {internship.duration && (
                      <span className="meta-item">
                        <FiClock /> {internship.duration}
                      </span>
                    )}
                    {internship.isPaid && internship.stipend && (
                      <span className="meta-item paid">
                        <FiDollarSign /> ${internship.stipend}/month
                      </span>
                    )}
                  </div>
                  <button 
                    className="apply-btn"
                    onClick={() => handleApply('internship', internship.id)}
                  >
                    Apply Now
                  </button>
                </div>
              ))
            ) : (
              <div className="no-results">
                <FiBriefcase className="no-results-icon" />
                <p>No internships match your search.</p>
              </div>
            )
          ) : filteredJobs.length > 0 ? (
            filteredJobs.map((job) => (
              <div className="opportunity-card" key={job.id}>
                <div className="opportunity-header">
                  <div className="company-logo">
                    {job.companyName?.charAt(0) || 'C'}
                  </div>
                  <div className="opportunity-info">
                    <h3 className="opportunity-title">{job.title}</h3>
                    <p className="company-name">{job.companyName}</p>
                  </div>
                </div>
                <p className="opportunity-description">{job.description}</p>
                <div className="opportunity-meta">
                  {job.location && (
                    <span className="meta-item">
                      <FiMapPin /> {job.location}
                    </span>
                  )}
                  {job.salaryRange && (
                    <span className="meta-item paid">
                      <FiDollarSign /> {job.salaryRange}
                    </span>
                  )}
                </div>
                <button 
                  className="apply-btn"
                  onClick={() => handleApply('job', job.id)}
                >
                  Apply Now
                </button>
              </div>
            ))
          ) : (
            <div className="no-results">
              <FiBriefcase className="no-results-icon" />
              <p>No jobs match your search.</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default Opportunities;
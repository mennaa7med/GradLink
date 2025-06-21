import React, { useState } from 'react';
import './Opportunities.css';
import { FiSearch } from 'react-icons/fi';

const Opportunities = () => {
  const [activeTab, setActiveTab] = useState('internships');
  const [searchQuery, setSearchQuery] = useState(''); // State for search input

  const internships = [
    {
      title: 'Frontend Development Intern – CodeWave Technologies',
      description: 'Work on building responsive user interfaces using HTML, CSS, JavaScript, and React.js.',
    },
    {
      title: 'Machine Learning Intern – DeepVision Labs',
      description: 'Assist in building predictive models, training datasets, and applying machine learning algorithms on real-world data.',
    },
    {
      title: 'Cybersecurity Intern – SafeNet Systems',
      description: 'Help in vulnerability assessment, penetration testing, and building secure architectures for cloud applications.',
    },
    {
      title: 'Cloud Computing Intern – SkyCloud Solutions',
      description: 'Support cloud migration projects, learn about AWS and Azure infrastructure, and help optimize cloud resources.',
    },
    {
      title: 'UI/UX Design Intern – PixelFrame Studio',
      description: 'Collaborate on wireframing, prototyping, and user testing to create seamless digital experiences.',
    },
    {
      title: 'Backend Development Intern – ByteForge',
      description: 'Develop server-side logic, work with databases (MySQL, MongoDB), and build APIs using Node.js or Python.',
    },
     {
      title: 'Backend Development Intern – ByteForge',
      description: 'Develop server-side logic, work with databases (MySQL, MongoDB), and build APIs using Node.js or Python.',
    },
     {
      title: 'Backend Development Intern – ByteForge',
      description: 'Develop server-side logic, work with databases (MySQL, MongoDB), and build APIs using Node.js or Python.',
    },
     {
      title: 'Backend Development Intern – ByteForge',
      description: 'Develop server-side logic, work with databases (MySQL, MongoDB), and build APIs using Node.js or Python.',
    },
     {
      title: 'Backend Development Intern – ByteForge',
      description: 'Develop server-side logic, work with databases (MySQL, MongoDB), and build APIs using Node.js or Python.',
    },
     {
      title: 'Backend Development Intern – ByteForge',
      description: 'Develop server-side logic, work with databases (MySQL, MongoDB), and build APIs using Node.js or Python.',
    },
     {
      title: 'Backend Development Intern – ByteForge',
      description: 'Develop server-side logic, work with databases (MySQL, MongoDB), and build APIs using Node.js or Python.',
    },
    // Removed duplicate entries for brevity
  ];

  const jobs = [
    {
      title: 'Frontend Developer – NovaTech Solutions',
      description: 'Build modern and responsive web applications using React.js, Vue.js, and TailwindCSS.',
    },
    {
      title: 'Data Scientist – InsightSphere',
      description: 'Analyze large datasets, build machine learning models, and deliver actionable business insights.',
    },
    {
      title: 'Cybersecurity Analyst – GuardNet Systems',
      description: 'Monitor network activities, identify potential threats, and help maintain secure IT infrastructures.',
    },
    {
      title: 'Cloud Engineer – AeroCloud Technologies',
      description: 'Manage and optimize AWS cloud services, automate deployments, and ensure scalability of cloud systems.',
    },
    {
      title: 'UI/UX Designer – PixelFusion Studio',
      description: 'Design intuitive user interfaces, create user flows, and conduct usability testing to enhance user experience.',
    },
    {
      title: 'Backend Developer – CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs, and manage databases like PostgreSQL and MongoDB.',
    },
    {
      title: 'Backend Developer – CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs, and manage databases like PostgreSQL and MongoDB.',
    },
    {
      title: 'Backend Developer – CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs, and manage databases like PostgreSQL and MongoDB.',
    },
    {
      title: 'Backend Developer – CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs, and manage databases like PostgreSQL and MongoDB.',
    },
    {
      title: 'Backend Developer – CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs, and manage databases like PostgreSQL and MongoDB.',
    },
    {
      title: 'Backend Developer – CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs, and manage databases like PostgreSQL and MongoDB.',
    },
    {
      title: 'Backend Developer – CoreLogic Tech',
      description: 'Build and maintain server-side applications, work with REST APIs, and manage databases like PostgreSQL and MongoDB.',
    },
    // Removed duplicate entries for brevity
  ];

  // Filter opportunities based on search query
  const filteredInternships = internships.filter(
    (internship) =>
      internship.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      internship.description.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const filteredJobs = jobs.filter(
    (job) =>
      job.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      job.description.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div className="opportunities-container">
      <h1 className="opportunities-title">Opportunities</h1>

      <div className="opportunities-search-container">
        <div className="search-box">
          <FiSearch className="searchicon" />
          <input
            type="text"
            placeholder={activeTab === 'internships' ? 'Search about internships' : 'Search about jobs'}
            className="search-input"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)} // Update search query state
          />
        </div>

        <div className="tabs-container">
          <button
            className={`tab ${activeTab === 'internships' ? 'active' : ''}`}
            onClick={() => setActiveTab('internships')}
          >
            Internships
          </button>
          <button
            className={`tab ${activeTab === 'jobs' ? 'active' : ''}`}
            onClick={() => setActiveTab('jobs')}
          >
            Jobs
          </button>
        </div>
      </div>

      <div className="opportunities-grid">
        {activeTab === 'internships' ? (
          filteredInternships.length > 0 ? (
            filteredInternships.map((internship, index) => (
              <div className="opportunity-card" key={index}>
                <h3 className="opportunity-title">{internship.title}</h3>
                <p className="opportunity-description">{internship.description}</p>
                <button className="apply-btn">Apply</button>
              </div>
            ))
          ) : (
            <p>No internships match your search.</p>
          )
        ) : filteredJobs.length > 0 ? (
          filteredJobs.map((job, index) => (
            <div className="opportunity-card" key={index}>
              <h3 className="opportunity-title">{job.title}</h3>
              <p className="opportunity-description">{job.description}</p>
              <button className="apply-btn">Apply</button>
            </div>
          ))
        ) : (
          <p>No jobs match your search.</p>
        )}
      </div>
    </div>
  );
};

export default Opportunities;
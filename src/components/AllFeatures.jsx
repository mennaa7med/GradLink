import React from 'react';
import { Link } from 'react-router-dom';
import Footer from './Footer';
import './AllFeatures.css';

const featuresList = [
  {
    id: 1,
    icon: '💡',
    title: 'Project Idea Selection',
    description: 'Helps students explore and choose project ideas based on categories and market needs.',
  },
  {
    id: 2,
    icon: '📊',
    title: 'Task Management & Progress Tracking',
    description: 'Tools for adding tasks, deadlines, and visualizing progress through charts and dashboards.',
  },
  {
    id: 3,
    icon: '👨‍🏫',
    title: 'Mentor Communication System',
    description: 'Directory of mentors with profile previews and integrated chat for support and guidance.',
  },
  {
    id: 4,
    icon: '💰',
    title: 'Sponsor & Funding Opportunities',
    description: 'Connects students with sponsors who provide financial or technical support.',
  },
  {
    id: 5,
    icon: '💼',
    title: 'Project-Based Jobs System',
    description: 'Companies post short-term paid tasks for students to complete and gain real experience.',
  },
  {
    id: 6,
    icon: '🤖',
    title: 'Smart Resume Screening',
    description: 'AI-powered CV evaluation to help companies find the most suitable candidates.',
  },
  {
    id: 7,
    icon: '🎓',
    title: 'Graduate Profile Hub',
    description: 'Graduates can showcase projects, build profiles, and connect with companies for internships or jobs.',
  },
  {
    id: 8,
    icon: '📁',
    title: 'File Upload & Project Workspace',
    description: 'Upload, store, and manage all project files and final submissions.',
  },
  {
    id: 9,
    icon: '🏢',
    title: 'Company Dashboard',
    description: 'Companies can post tasks, review applicants, evaluate submissions, and track talents.',
  },
  {
    id: 10,
    icon: '📚',
    title: 'Learning Resources & Support',
    description: 'Technical guides, tips, and resources to help students improve skills and build better projects.',
  },
];

const AllFeatures = () => {
  return (
    <div className="all-features-page">
      {/* Hero Header Section */}
      <header className="features-hero">
        <div className="features-hero-content">
          <h1 className="features-hero-title">
            Explore GradLink Features
          </h1>
          <p className="features-hero-subtitle">
            Your comprehensive platform that supports students throughout their graduation project journey 
            and connects graduates with top companies for career opportunities.
          </p>
        </div>
        <div className="features-hero-decoration">
          <div className="hero-circle hero-circle-1"></div>
          <div className="hero-circle hero-circle-2"></div>
          <div className="hero-circle hero-circle-3"></div>
        </div>
      </header>

      {/* Features Grid Section */}
      <section className="features-main-section">
        <div className="features-container">
          <div className="features-grid">
            {featuresList.map((feature, index) => (
              <article 
                key={feature.id} 
                className="feature-card"
                style={{ animationDelay: `${index * 0.1}s` }}
              >
                <div className="feature-icon-wrapper">
                  <span className="feature-icon">{feature.icon}</span>
                </div>
                <h3 className="feature-title">{feature.title}</h3>
                <p className="feature-description">{feature.description}</p>
                <Link to={`/feature/${feature.id}`} className="feature-learn-more">
                  Learn More
                  <svg 
                    className="learn-more-arrow" 
                    width="16" 
                    height="16" 
                    viewBox="0 0 24 24" 
                    fill="none" 
                    stroke="currentColor" 
                    strokeWidth="2"
                  >
                    <path d="M5 12h14M12 5l7 7-7 7"/>
                  </svg>
                </Link>
              </article>
            ))}
          </div>
        </div>
      </section>

      {/* Call to Action Section */}
      <section className="features-cta">
        <div className="cta-content">
          <h2 className="cta-title">Ready to Get Started?</h2>
          <p className="cta-description">
            Join thousands of students and graduates who are building their future with GradLink.
          </p>
          <div className="cta-buttons">
            <Link to="/signup" className="cta-btn cta-btn-primary">
              Create Account
            </Link>
            <Link to="/" className="cta-btn cta-btn-secondary">
              Back to Home
            </Link>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  );
};

export default AllFeatures;

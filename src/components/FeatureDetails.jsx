import React from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import Footer from './Footer';
import './FeatureDetails.css';

const featuresData = {
  1: {
    icon: '💡',
    title: 'Project Idea Selection',
    shortDescription: 'Helps students explore and choose project ideas based on categories and market needs.',
    fullDescription: `Finding the right graduation project idea can be overwhelming. Our Project Idea Selection feature provides a curated library of innovative project concepts tailored to various fields and industry demands.`,
    benefits: [
      'Browse through categorized project ideas (Web, Mobile, AI, IoT, etc.)',
      'Filter projects by difficulty level and required technologies',
      'See market trends and in-demand project types',
      'Get AI-powered suggestions based on your skills and interests',
      'View similar completed projects for inspiration',
      'Save favorite ideas to your personal list',
    ],
    howItWorks: [
      'Sign up and complete your profile with skills and interests',
      'Browse the project ideas bank or use smart filters',
      'Preview project details, requirements, and expected outcomes',
      'Select and start working on your chosen project',
    ],
  },
  2: {
    icon: '📊',
    title: 'Task Management & Progress Tracking',
    shortDescription: 'Tools for adding tasks, deadlines, and visualizing progress through charts and dashboards.',
    fullDescription: `Stay organized and on track throughout your graduation project journey. Our comprehensive task management system helps you break down your project into manageable tasks and monitor your progress in real-time.`,
    benefits: [
      'Create and organize tasks with deadlines and priorities',
      'Visual Kanban boards for easy task management',
      'Progress charts and analytics dashboard',
      'Milestone tracking to celebrate achievements',
      'Deadline reminders and notifications',
      'Team collaboration with task assignments',
      'Time tracking for better project estimation',
    ],
    howItWorks: [
      'Create your project and define major milestones',
      'Break down milestones into smaller, actionable tasks',
      'Assign tasks to team members with deadlines',
      'Track progress through visual dashboards and charts',
      'Receive notifications for upcoming deadlines',
    ],
  },
  3: {
    icon: '👨‍🏫',
    title: 'Mentor Communication System',
    shortDescription: 'Directory of mentors with profile previews and integrated chat for support and guidance.',
    fullDescription: `Connect with experienced mentors from academia and industry who can guide you through your graduation project. Our mentorship platform facilitates meaningful connections between students and experts.`,
    benefits: [
      'Browse mentor profiles with expertise areas and ratings',
      'Real-time chat with mentors for quick guidance',
      'Schedule virtual meetings and consultations',
      'Request project reviews and feedback',
      'Access mentor recommendations and resources',
      'Get career advice alongside project guidance',
    ],
    howItWorks: [
      'Search mentors by expertise, availability, or rating',
      'View detailed mentor profiles and specializations',
      'Send connection requests to potential mentors',
      'Chat, schedule meetings, or request document reviews',
      'Rate and review your mentoring experience',
    ],
  },
  4: {
    icon: '💰',
    title: 'Sponsor & Funding Opportunities',
    shortDescription: 'Connects students with sponsors who provide financial or technical support.',
    fullDescription: `Turn your innovative ideas into reality with sponsorship support. Our platform connects promising student projects with sponsors willing to provide financial backing, resources, or technical expertise.`,
    benefits: [
      'Access to verified sponsors and funding opportunities',
      'Create compelling project pitches',
      'Receive financial support for project materials',
      'Get technical resources and equipment sponsorship',
      'Connect with industry partners for real-world applications',
      'Build relationships for future career opportunities',
    ],
    howItWorks: [
      'Create a detailed project pitch with goals and budget',
      'Browse available sponsorship opportunities',
      'Apply for relevant funding programs',
      'Present your project to interested sponsors',
      'Receive and manage sponsorship support',
    ],
  },
  5: {
    icon: '💼',
    title: 'Project-Based Jobs System',
    shortDescription: 'Companies post short-term paid tasks for students to complete and gain real experience.',
    fullDescription: `Gain valuable work experience while still studying. Our project-based jobs system connects students with companies offering short-term, paid tasks that help you build your portfolio and earn money.`,
    benefits: [
      'Access real-world paid project opportunities',
      'Build professional portfolio while studying',
      'Flexible work that fits your academic schedule',
      'Direct interaction with industry professionals',
      'Earn income while developing practical skills',
      'Receive ratings and reviews for future opportunities',
    ],
    howItWorks: [
      'Browse available project-based jobs',
      'Apply with your profile and relevant experience',
      'Get selected and receive project requirements',
      'Complete tasks and submit your work',
      'Get paid and receive performance reviews',
    ],
  },
  6: {
    icon: '🤖',
    title: 'Smart Resume Screening',
    shortDescription: 'AI-powered CV evaluation to help companies find the most suitable candidates.',
    fullDescription: `Leverage artificial intelligence to optimize your resume and stand out to employers. Our smart screening system analyzes CVs against job requirements, providing insights for both candidates and recruiters.`,
    benefits: [
      'AI-powered resume analysis and scoring',
      'Keyword optimization suggestions',
      'Match percentage with job descriptions',
      'Personalized improvement recommendations',
      'Industry-standard formatting guidance',
      'Skills gap identification and learning suggestions',
    ],
    howItWorks: [
      'Upload your resume to the platform',
      'AI analyzes your CV against best practices',
      'Receive detailed feedback and improvement tips',
      'Compare your resume with job requirements',
      'Apply to positions with confidence',
    ],
  },
  7: {
    icon: '🎓',
    title: 'Graduate Profile Hub',
    shortDescription: 'Graduates can showcase projects, build profiles, and connect with companies for internships or jobs.',
    fullDescription: `Create a professional online presence that showcases your skills, projects, and achievements. The Graduate Profile Hub is your digital portfolio that connects you with potential employers and opportunities.`,
    benefits: [
      'Professional portfolio website with custom URL',
      'Showcase graduation projects with media and demos',
      'Highlight skills, certifications, and achievements',
      'Get discovered by companies and recruiters',
      'Receive connection requests and job offers',
      'Track profile views and engagement analytics',
    ],
    howItWorks: [
      'Complete your graduate profile with details',
      'Add projects, skills, and achievements',
      'Upload project demos, videos, and documentation',
      'Make your profile public for recruiters',
      'Connect with companies and explore opportunities',
    ],
  },
  8: {
    icon: '📁',
    title: 'File Upload & Project Workspace',
    shortDescription: 'Upload, store, and manage all project files and final submissions.',
    fullDescription: `Keep all your project files organized and accessible in one secure location. Our workspace provides comprehensive file management with version control and collaboration features.`,
    benefits: [
      'Secure cloud storage for all project files',
      'Version control to track document changes',
      'Share files with team members and mentors',
      'Support for various file formats',
      'Easy final project submission',
      'Download project archives anytime',
    ],
    howItWorks: [
      'Create project folders and organize structure',
      'Upload files by drag-and-drop or selection',
      'Share access with team members',
      'Track versions and restore previous files',
      'Submit final deliverables through the platform',
    ],
  },
  9: {
    icon: '🏢',
    title: 'Company Dashboard',
    shortDescription: 'Companies can post tasks, review applicants, evaluate submissions, and track talents.',
    fullDescription: `A comprehensive dashboard for companies to manage their interaction with students and graduates. Post opportunities, evaluate candidates, and build your talent pipeline all in one place.`,
    benefits: [
      'Post job opportunities and project-based tasks',
      'Access pool of talented students and graduates',
      'AI-assisted candidate screening and ranking',
      'Review applications and portfolios easily',
      'Track and manage multiple opportunities',
      'Build relationships with emerging talent',
    ],
    howItWorks: [
      'Create company profile and verify account',
      'Post jobs, internships, or project-based tasks',
      'Review applications with AI-powered screening',
      'Shortlist and contact promising candidates',
      'Evaluate submissions and provide feedback',
    ],
  },
  10: {
    icon: '📚',
    title: 'Learning Resources & Support',
    shortDescription: 'Technical guides, tips, and resources to help students improve skills and build better projects.',
    fullDescription: `Access a comprehensive library of learning materials designed to help you succeed in your graduation project. From technical tutorials to project management tips, find everything you need to excel.`,
    benefits: [
      'Curated tutorials and technical guides',
      'Best practices for project documentation',
      'Video courses on popular technologies',
      'Templates for reports and presentations',
      'Community forums for peer support',
      'Expert articles and industry insights',
    ],
    howItWorks: [
      'Browse resources by category or technology',
      'Save useful materials to your library',
      'Follow learning paths for skill development',
      'Participate in community discussions',
      'Access downloadable templates and tools',
    ],
  },
};

const FeatureDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const feature = featuresData[id];

  if (!feature) {
    return (
      <div className="feature-details-page">
        <div className="feature-not-found">
          <h1>Feature Not Found</h1>
          <p>The feature you're looking for doesn't exist.</p>
          <Link to="/AllFeatures" className="back-to-features-btn">
            Back to All Features
          </Link>
        </div>
        <Footer />
      </div>
    );
  }

  return (
    <div className="feature-details-page">
      {/* Hero Section */}
      <header className="feature-details-hero">
        <button className="back-button" onClick={() => navigate('/AllFeatures')}>
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <path d="M19 12H5M12 19l-7-7 7-7" />
          </svg>
          Back to Features
        </button>
        <div className="feature-details-hero-content">
          <div className="feature-details-icon-wrapper">
            <span className="feature-details-icon">{feature.icon}</span>
          </div>
          <h1 className="feature-details-title">{feature.title}</h1>
          <p className="feature-details-subtitle">{feature.shortDescription}</p>
        </div>
      </header>

      {/* Main Content */}
      <main className="feature-details-main">
        <div className="feature-details-container">
          
          {/* Full Description */}
          <section className="feature-section">
            <h2 className="section-title">Overview</h2>
            <p className="section-text">{feature.fullDescription}</p>
          </section>

          {/* Benefits */}
          <section className="feature-section">
            <h2 className="section-title">Key Benefits</h2>
            <ul className="benefits-list">
              {feature.benefits.map((benefit, index) => (
                <li key={index} className="benefit-item">
                  <span className="benefit-check">✓</span>
                  {benefit}
                </li>
              ))}
            </ul>
          </section>

          {/* How It Works */}
          <section className="feature-section">
            <h2 className="section-title">How It Works</h2>
            <ol className="steps-list">
              {feature.howItWorks.map((step, index) => (
                <li key={index} className="step-item">
                  <span className="step-number">{index + 1}</span>
                  <span className="step-text">{step}</span>
                </li>
              ))}
            </ol>
          </section>

          {/* CTA Section */}
          <section className="feature-cta-section">
            <h3 className="feature-cta-title">Ready to get started?</h3>
            <p className="feature-cta-text">
              Create your account and start using {feature.title} today.
            </p>
            <div className="feature-cta-buttons">
              <Link to="/signup" className="feature-cta-btn primary">
                Get Started
              </Link>
              <Link to="/AllFeatures" className="feature-cta-btn secondary">
                Explore More Features
              </Link>
            </div>
          </section>

        </div>
      </main>

      <Footer />
    </div>
  );
};

export default FeatureDetails;







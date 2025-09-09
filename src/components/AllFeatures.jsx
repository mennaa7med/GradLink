import React from 'react';
import './AllFeatures.css';

const featuresList = [
  {
    title: '🎯 Ready-made Project Ideas',
    description:
      'Discover curated project ideas designed to match your interests and industry trends, making it easier to start your graduation journey.',
  },
  {
    title: '📚 Helpful Resources & Templates',
    description:
      'Access a library of documents, templates, tutorials, and best practices to guide you throughout your project.',
  },
  {
    title: '📈 Project Tracking System',
    description:
      'Manage your project tasks, milestones, and files through an interactive dashboard to track your progress.',
  },
  {
    title: '🤝 Peer Collaboration',
    description:
      'Work with teammates using real-time tools that help you assign roles, share updates, and stay in sync.',
  },
  {
    title: '👨‍🏫 Mentor Support',
    description:
      'Connect with expert mentors from academia and industry for personalized guidance and reviews.',
  },
  {
    title: '💼 Job Opportunities',
    description:
      'Discover job posts that match your skills and completed projects through our graduate portal.',
  },
  {
    title: '🚀 Project Showcase',
    description:
      'Build a professional profile to showcase your projects and share them with recruiters and investors.',
  },
  {
    title: '🤖 AI Assistant',
    description:
      'Use our built-in AI assistant to generate ideas, summarize documents, and get coding help in real time.',
  },
  {
    title: '💸 Sponsor Matching',
    description:
      'Pitch your ideas to potential sponsors and get financial or technical support for your innovation.',
  },
];

const AllFeatures = () => {
  return (
    <div className="all-features-container">
      <h1 className="all-features-title">All Features</h1>
      <div className="features-grid">
        {featuresList.map((feature, index) => (
          <div key={index} className="feature-card">
            <h2>{feature.title}</h2>
            <p>{feature.description}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default AllFeatures;

import React, { useState } from 'react';
import './Help.css';

const Help = () => {
  const [showAllFaq, setShowAllFaq] = useState(false);

  const faqList = [
    {
      question: "How can I create a new task?",
      answer: 'Click the "Add Task" button in the dashboard, fill in the form, and click "Submit".'
    },
    {
      question: "Can I assign tasks to team members?",
      answer: "Yes, you can select a team member from the 'Assignee' dropdown when adding a task."
    },
    {
      question: "How do I change a task’s status?",
      answer: "Drag the task to the appropriate column: Pending, In Progress, or Completed."
    },
    {
      question: "Is there a way to filter tasks?",
      answer: "Yes, you can filter tasks by status, assignee, or tags using the filter controls."
    },
    {
      question: "Can I add subtasks and notes?",
      answer: "Absolutely. Click on a task to open the modal, then add subtasks or notes as needed."
    },
    {
      question: "How to export my tasks?",
      answer: "Use the 'Export' button to download your task data in CSV format."
    },
    {
      question: "What happens if I delete a task?",
      answer: "Once deleted, a task cannot be recovered. Please confirm before deleting."
    },
    {
      question: "Is there dark mode support?",
      answer: "Yes, the interface automatically adapts to your system's dark mode settings."
    },
    {
      question: "Can I track my progress visually?",
      answer: "Yes, progress bars and charts on the dashboard help you track overall completion."
    },
    {
      question: "Is my data secure?",
      answer: "All data is encrypted and securely stored following industry-standard practices."
    },
    {
      question: "How to contact support?",
      answer: "Scroll to the support section of this page and fill in the contact form."
    }
  ];

  const displayedFaqs = showAllFaq ? faqList : faqList.slice(0, 2);

  return (
    
    <div className="help-page">
      <h1 className="help-title">Need Help? We've Got You Covered!</h1>
<section className="help-section">
        <h2>Getting Started</h2>
        <ul className="step-list">
          <li>Sign up and create your account.</li>
          <li>Explore the dashboard and add your first task.</li>
          <li>Assign tasks to specific columns: To Do, In Progress, or Done.</li>
          <li>Click on any task to edit details, add notes, or manage subtasks.</li>
        </ul>
      </section>
      <section className="help-section">
        <h2>
          Frequently Asked Questions{" "}
          <span 
            className="faq-toggle" 
            onClick={() => setShowAllFaq(!showAllFaq)}
          >
            {showAllFaq ? "▲" : "▼"}
          </span>
        </h2>

        <ul className="faq-list">
          {displayedFaqs.map((faq, index) => (
            <li key={index}>
              <strong>{faq.question}</strong>
              <br />
              {faq.answer}
            </li>
          ))}
        </ul>

        <div className="faq-button-container">
          <button className="faq-toggle-button" onClick={() => setShowAllFaq(!showAllFaq)}>
            {showAllFaq ? "View Less" : "View More"}
          </button>
        </div>
      </section>

      

      <section className="help-section">
        <h2>Video Tutorials</h2>
        <div className="video-grid">
          <iframe
            title="How to Use the Task Manager"
            src="https://www.youtube.com/embed/dQw4w9WgXcQ"
            allowFullScreen
          ></iframe>
          <iframe
            title="Dashboard Walkthrough"
            src="https://www.youtube.com/embed/dQw4w9WgXcQ"
            allowFullScreen
          ></iframe>
        </div>
      </section>

      <section className="help-section">
        <h2>Contact Support</h2>
        <form className="help-contact-form">
          <input type="text" placeholder="Your Name" required />
          <input type="email" placeholder="Your Email" required />
          <textarea rows="4" placeholder="How can we help you?" required></textarea>
          <button type="submit">Send Message</button>
        </form>
      </section>
    </div>
  );
};

export default Help;

import React from 'react';
import { Link } from 'react-router-dom';
import { FiArrowLeft } from 'react-icons/fi';
import './Legal.css';

const TermsOfService = () => {
  return (
    <div className="legal-page">
      <div className="legal-container">
        <Link to="/" className="back-link">
          <FiArrowLeft /> Back to Home
        </Link>

        <div className="legal-header">
          <h1>Terms of Service</h1>
          <p className="last-updated">Last updated: January 13, 2026</p>
        </div>

        <div className="legal-content">
          <section>
            <h2>1. Acceptance of Terms</h2>
            <p>
              By accessing and using GradLink ("the Platform"), you accept and agree to be bound by 
              these Terms of Service. If you do not agree to these terms, please do not use our services.
            </p>
          </section>

          <section>
            <h2>2. Description of Service</h2>
            <p>
              GradLink is a comprehensive platform designed to support students in their graduation 
              projects by providing:
            </p>
            <ul>
              <li>Job and internship opportunities</li>
              <li>Project collaboration and team building</li>
              <li>Mentorship connections</li>
              <li>Sponsorship opportunities</li>
              <li>Learning resources and materials</li>
              <li>AI-powered assistance</li>
            </ul>
          </section>

          <section>
            <h2>3. User Accounts</h2>
            <p>
              To access certain features, you must create an account. You agree to:
            </p>
            <ul>
              <li>Provide accurate and complete information</li>
              <li>Maintain the security of your account credentials</li>
              <li>Accept responsibility for all activities under your account</li>
              <li>Notify us immediately of any unauthorized use</li>
            </ul>
          </section>

          <section>
            <h2>4. User Types and Responsibilities</h2>
            
            <h3>4.1 Students</h3>
            <p>Students agree to provide accurate academic information and use the platform for legitimate educational and career purposes.</p>
            
            <h3>4.2 Companies</h3>
            <p>Companies agree to post genuine job opportunities and treat all applicants fairly and professionally.</p>
            
            <h3>4.3 Mentors</h3>
            <p>Mentors agree to provide guidance in good faith and maintain professional conduct with mentees.</p>
            
            <h3>4.4 Sponsors</h3>
            <p>Sponsors agree to fulfill funding commitments and maintain transparent communication with sponsored projects.</p>
          </section>

          <section>
            <h2>5. Prohibited Activities</h2>
            <p>Users are prohibited from:</p>
            <ul>
              <li>Posting false, misleading, or fraudulent content</li>
              <li>Harassing, threatening, or discriminating against other users</li>
              <li>Attempting to gain unauthorized access to the platform</li>
              <li>Using the platform for any illegal activities</li>
              <li>Scraping or collecting user data without consent</li>
              <li>Impersonating other users or entities</li>
            </ul>
          </section>

          <section>
            <h2>6. Intellectual Property</h2>
            <p>
              Users retain ownership of content they create. By posting content, you grant GradLink 
              a non-exclusive license to display and distribute your content on the platform. 
              The GradLink name, logo, and branding are our intellectual property.
            </p>
          </section>

          <section>
            <h2>7. Privacy</h2>
            <p>
              Your privacy is important to us. Please review our{' '}
              <Link to="/privacy-policy">Privacy Policy</Link> to understand how we collect, 
              use, and protect your information.
            </p>
          </section>

          <section>
            <h2>8. Limitation of Liability</h2>
            <p>
              GradLink is provided "as is" without warranties of any kind. We are not liable for 
              any damages arising from your use of the platform, including but not limited to 
              direct, indirect, incidental, or consequential damages.
            </p>
          </section>

          <section>
            <h2>9. Termination</h2>
            <p>
              We reserve the right to suspend or terminate accounts that violate these terms. 
              You may also delete your account at any time through your account settings.
            </p>
          </section>

          <section>
            <h2>10. Changes to Terms</h2>
            <p>
              We may update these terms from time to time. Continued use of the platform after 
              changes constitutes acceptance of the new terms. We will notify users of significant 
              changes via email or platform notification.
            </p>
          </section>

          <section>
            <h2>11. Contact Information</h2>
            <p>
              For questions about these Terms of Service, please contact us at:
            </p>
            <ul>
              <li>Email: support@gradlink.com</li>
              <li>Website: <Link to="/">www.gradlink.com</Link></li>
            </ul>
          </section>
        </div>

        <div className="legal-footer">
          <p>By using GradLink, you acknowledge that you have read and understood these Terms of Service.</p>
        </div>
      </div>
    </div>
  );
};

export default TermsOfService;
















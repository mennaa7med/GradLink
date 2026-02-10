import React from 'react';
import { Link } from 'react-router-dom';
import { FiArrowLeft } from 'react-icons/fi';
import './Legal.css';

const PrivacyPolicy = () => {
  return (
    <div className="legal-page">
      <div className="legal-container">
        <Link to="/" className="back-link">
          <FiArrowLeft /> Back to Home
        </Link>

        <div className="legal-header">
          <h1>Privacy Policy</h1>
          <p className="last-updated">Last updated: January 13, 2026</p>
        </div>

        <div className="legal-content">
          <section>
            <h2>1. Introduction</h2>
            <p>
              At GradLink, we take your privacy seriously. This Privacy Policy explains how we 
              collect, use, disclose, and safeguard your information when you use our platform.
            </p>
          </section>

          <section>
            <h2>2. Information We Collect</h2>
            
            <h3>2.1 Personal Information</h3>
            <p>We may collect personally identifiable information, including:</p>
            <ul>
              <li>Name and email address</li>
              <li>Phone number</li>
              <li>Educational information (university, major, graduation year)</li>
              <li>Professional information (company, job title, experience)</li>
              <li>Profile picture</li>
              <li>Resume and portfolio</li>
            </ul>

            <h3>2.2 Usage Information</h3>
            <p>We automatically collect certain information when you use the platform:</p>
            <ul>
              <li>Device and browser information</li>
              <li>IP address and location data</li>
              <li>Pages visited and features used</li>
              <li>Time spent on the platform</li>
              <li>Referring URLs</li>
            </ul>
          </section>

          <section>
            <h2>3. How We Use Your Information</h2>
            <p>We use collected information to:</p>
            <ul>
              <li>Provide and maintain our services</li>
              <li>Match students with opportunities and mentors</li>
              <li>Process applications and communications</li>
              <li>Send notifications and updates</li>
              <li>Improve our platform and user experience</li>
              <li>Ensure security and prevent fraud</li>
              <li>Comply with legal obligations</li>
            </ul>
          </section>

          <section>
            <h2>4. Information Sharing</h2>
            <p>We may share your information with:</p>
            <ul>
              <li><strong>Other Users:</strong> Profile information visible based on your privacy settings</li>
              <li><strong>Companies:</strong> When you apply for jobs or internships</li>
              <li><strong>Mentors/Sponsors:</strong> When you engage in mentorship or seek sponsorship</li>
              <li><strong>Service Providers:</strong> Third parties that help us operate the platform</li>
              <li><strong>Legal Requirements:</strong> When required by law or to protect rights</li>
            </ul>
            <p>We never sell your personal information to third parties.</p>
          </section>

          <section>
            <h2>5. Data Security</h2>
            <p>
              We implement industry-standard security measures to protect your data:
            </p>
            <ul>
              <li>Encryption of data in transit and at rest</li>
              <li>Secure authentication with JWT tokens</li>
              <li>Regular security audits</li>
              <li>Access controls and monitoring</li>
            </ul>
            <p>
              However, no method of transmission over the Internet is 100% secure, and we cannot 
              guarantee absolute security.
            </p>
          </section>

          <section>
            <h2>6. Your Rights</h2>
            <p>You have the right to:</p>
            <ul>
              <li><strong>Access:</strong> Request a copy of your personal data</li>
              <li><strong>Correction:</strong> Update or correct inaccurate information</li>
              <li><strong>Deletion:</strong> Request deletion of your account and data</li>
              <li><strong>Portability:</strong> Export your data in a portable format</li>
              <li><strong>Opt-out:</strong> Unsubscribe from marketing communications</li>
              <li><strong>Restriction:</strong> Limit how we use your data</li>
            </ul>
            <p>
              To exercise these rights, contact us at privacy@gradlink.com or through your account settings.
            </p>
          </section>

          <section>
            <h2>7. Cookies and Tracking</h2>
            <p>We use cookies and similar technologies to:</p>
            <ul>
              <li>Remember your login session</li>
              <li>Store your preferences</li>
              <li>Analyze platform usage</li>
              <li>Improve performance</li>
            </ul>
            <p>
              You can control cookies through your browser settings. Note that disabling cookies 
              may affect platform functionality.
            </p>
          </section>

          <section>
            <h2>8. Third-Party Services</h2>
            <p>
              Our platform may integrate with third-party services (e.g., Google for authentication, 
              analytics services). These services have their own privacy policies, and we encourage 
              you to review them.
            </p>
          </section>

          <section>
            <h2>9. Children's Privacy</h2>
            <p>
              GradLink is intended for users 16 years and older. We do not knowingly collect 
              information from children under 16. If you believe we have collected such information, 
              please contact us immediately.
            </p>
          </section>

          <section>
            <h2>10. Data Retention</h2>
            <p>
              We retain your data for as long as your account is active or as needed to provide 
              services. After account deletion, we may retain certain data for legal compliance 
              or legitimate business purposes for up to 30 days.
            </p>
          </section>

          <section>
            <h2>11. International Data Transfers</h2>
            <p>
              Your data may be transferred to and processed in countries outside your residence. 
              We ensure appropriate safeguards are in place for such transfers.
            </p>
          </section>

          <section>
            <h2>12. Changes to This Policy</h2>
            <p>
              We may update this Privacy Policy periodically. We will notify you of significant 
              changes through email or platform notification. Continued use after changes 
              constitutes acceptance.
            </p>
          </section>

          <section>
            <h2>13. Contact Us</h2>
            <p>For privacy-related questions or concerns, contact us at:</p>
            <ul>
              <li>Email: privacy@gradlink.com</li>
              <li>Support: support@gradlink.com</li>
            </ul>
          </section>
        </div>

        <div className="legal-footer">
          <p>By using GradLink, you acknowledge that you have read and understood this Privacy Policy.</p>
        </div>
      </div>
    </div>
  );
};

export default PrivacyPolicy;
















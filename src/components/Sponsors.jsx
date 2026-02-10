import React, { useState, useEffect } from 'react';
import './Sponsors.css';
import * as sponsorsApi from '../api/sponsors';

const Sponsors = () => {
  const [sponsors, setSponsors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    companyName: '',
    sponsorshipType: 'Funding',
    message: ''
  });
  const [submitting, setSubmitting] = useState(false);
  const [submitSuccess, setSubmitSuccess] = useState(false);

  useEffect(() => {
    loadSponsors();
  }, []);

  const loadSponsors = async () => {
    try {
      setLoading(true);
      const data = await sponsorsApi.listSponsors();
      setSponsors(data);
      setError(null);
    } catch (err) {
      console.error('Failed to load sponsors:', err);
      setError('Failed to load sponsors. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    setSubmitSuccess(false);
    
    try {
      await sponsorsApi.submitApplication(formData);
      setSubmitSuccess(true);
      setFormData({
        fullName: '',
        email: '',
        companyName: '',
        sponsorshipType: 'Funding',
        message: ''
      });
    } catch (err) {
      console.error('Failed to submit application:', err);
      alert(err.response?.data?.error || 'Failed to submit application. Please try again.');
    } finally {
      setSubmitting(false);
    }
  };

  const getLogoPlaceholder = (name) => {
    const colors = ['🟢', '🟣', '✔️', '🟠', '🔵', '🔴', '⭐', '💎'];
    const index = name.charCodeAt(0) % colors.length;
    return colors[index];
  };

  return (
    <div className="sponsors-page">
      <section className="hero">
        <h1>Empower the future.<br />Become a sponsor.</h1>
        <p>Help talented students bring their graduation projects to life. <br />From funding to mentorship, your support can make a real impact.</p>
        <button className="apply-btn" onClick={() => document.querySelector('.form-container').scrollIntoView({ behavior: 'smooth' })}>
          Apply as Sponsor
        </button>
      </section>

      <section className="our-sponsors">
        <h2>Our Sponsors</h2>
        {loading ? (
          <div className="loading">Loading sponsors...</div>
        ) : error ? (
          <div className="error">{error}</div>
        ) : sponsors.length === 0 ? (
          <div className="no-sponsors">
            <p>No sponsors yet. Be the first to support our students!</p>
          </div>
        ) : (
          <div className="sponsor-grid">
            {sponsors.map((sponsor) => (
              <div className="sponsor-card" key={sponsor.id}>
                <div className="logo-placeholder">
                  {sponsor.logo || getLogoPlaceholder(sponsor.name)}
                </div>
                <h3>{sponsor.name}</h3>
                <p>{sponsor.field || 'General'}</p>
                {sponsor.supportedProject && (
                  <p className="project-title">Supported project:<br />{sponsor.supportedProject}</p>
                )}
                {sponsor.link && (
                  <a href={sponsor.link} target="_blank" rel="noreferrer">Website ↗</a>
                )}
              </div>
            ))}
          </div>
        )}
      </section>

      <section className="become-sponsor-section">
        <div className="form-container">
          <h3>Become a Sponsor</h3>
          {submitSuccess ? (
            <div className="success-message">
              <p>✅ Your application has been submitted successfully!</p>
              <p>We'll review your application and get back to you soon.</p>
              <button onClick={() => setSubmitSuccess(false)}>Submit Another</button>
            </div>
          ) : (
            <form onSubmit={handleSubmit}>
              <input 
                type="text" 
                name="fullName"
                placeholder="Full Name" 
                value={formData.fullName}
                onChange={handleInputChange}
                required 
              />
              <input 
                type="email" 
                name="email"
                placeholder="Email Address" 
                value={formData.email}
                onChange={handleInputChange}
                required 
              />
              <input 
                type="text" 
                name="companyName"
                placeholder="Company Name" 
                value={formData.companyName}
                onChange={handleInputChange}
                required 
              />
              <select 
                name="sponsorshipType"
                value={formData.sponsorshipType}
                onChange={handleInputChange}
              >
                <option value="Funding">Funding</option>
                <option value="Mentorship">Mentorship</option>
                <option value="Resources">Resources</option>
              </select>
              <textarea 
                name="message"
                placeholder="Message (optional)" 
                rows="4"
                value={formData.message}
                onChange={handleInputChange}
              ></textarea>
              <button type="submit" disabled={submitting}>
                {submitting ? 'Submitting...' : 'Submit'}
              </button>
            </form>
          )}
        </div>

        <div className="why-sponsor">
          <h3>Why Sponsor?</h3>
          <ul>
            <li>Connect with talented students</li>
            <li>Scout potential future collaborations</li>
            <li>Get your logo displayed on our platform</li>
            <li>Early access to emerging talent</li>
            <li>Make a positive social impact</li>
          </ul>
        </div>
      </section>
    </div>
  );
};

export default Sponsors;

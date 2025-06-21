import React from 'react';
import './Sponsors.css';

const sponsors = [
  {
    name: 'TechForFuture',
    field: 'Software Development',
    project: 'At Assistant, for Janitor',
    link: '#',
    logo: '🟢'
  },
  {
    name: 'InnovateNow',
    field: 'Education',
    project: 'Wearable Health *Tacker',
    link: '#',
    logo: '🟣'
  },
  {
    name: 'BuildBright',
    field: 'Manufacturing',
    project: 'Solar Car, Rebotics Arm',
    link: '#',
    logo: '✔️'
  },
  {
    name: 'EduMentor',
    field: 'Education',
    project: 'Language Learning Platform',
    link: '#',
    logo: '🟠'
  }
];

const Sponsors = () => {
  return (
    <div className="sponsors-page">
      <section className="hero">
        <h1>Empower the future.<br />Become a sponsor.</h1>
        <p>Help talented students bring their graduation projects to life. <br />From funding to mentorship, your support can make a real impact.</p>
        <button className="apply-btn">Apply as Sponsor</button>
      </section>

      <section className="our-sponsors">
        <h2>Our Sponsors</h2>
        <div className="sponsor-grid">
          {sponsors.map((sponsor, idx) => (
            <div className="sponsor-card" key={idx}>
              <div className="logo-placeholder">{sponsor.logo}</div>
              <h3>{sponsor.name}</h3>
              <p>{sponsor.field}</p>
              <p className="project-title">Supported project:<br />{sponsor.project}</p>
              <a href={sponsor.link}>Website ↗</a>
            </div>
          ))}
        </div>
      </section>

      <section className="become-sponsor-section">
        <div className="form-container">
          <h3>Become a Sponsor</h3>
          <form>
            <input type="text" placeholder="Full Name" />
            <input type="email" placeholder="Email Address" />
            <input type="text" placeholder="Company Name" />
            <select>
              <option>Funding</option>
              <option>Mentorship</option>
              <option>Resources</option>
            </select>
            <textarea placeholder="Message" rows="4"></textarea>
            <button type="submit">Submit</button>
          </form>
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

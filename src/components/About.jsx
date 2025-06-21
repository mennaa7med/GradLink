import React from 'react';
import './About.css';
import img5 from '../assets/images/about.png';

const About = () => {
  return (
    <section className="about-section">
      <h2 className="about-title">About Us</h2>
      <div className="about-content">
        <div className="about-image">
          <img src={img5} alt="About Us" />
        </div>
        <div className="about-text">
          <p>
            We’re a student-driven platform built to support university students and fresh graduates
            in their academic and professional journey. Our mission is to make graduation projects
            easier, more impactful, and more connected to real-life opportunities — from idea to job.
          </p>
        </div>
      </div>
    </section>
  );
};

export default About;


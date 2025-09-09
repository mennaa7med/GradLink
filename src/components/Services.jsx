import React from 'react';
import './Services.css';

const Services = () => {
  return (
    <div className="services-page">
      <div className="main-section-title">
        <h1>Our Services</h1>
        <div className="header-decoration"></div>
      </div>

      <div className="services-container">
        <div className="service-column">
          <h3 className="service-title"> Students</h3>
          <div className="service-card student-service">
            <p>
              We help students through their graduation project journey by providing useful resources,
              project ideas, and guidance, all designed to make the process easier and more organized.
            </p>
          </div>
        </div>

        <div className="service-column">
          <h3 className="service-title right"> Graduates</h3>
          <div className="service-card graduate-service">
            <p>
              We support graduates by offering job opportunities, networking chances, and sponsor connections
              to help turn their projects into real-life success stories.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Services;
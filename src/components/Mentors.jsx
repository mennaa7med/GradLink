import React, { useState } from 'react';
import './Mentors.css';
import { useNavigate } from 'react-router-dom';

const mentorsData = [
  {
    id: 1,
    name: 'Dr. Amina Khaled',
    field: 'Machine Learning',
    description: 'Experienced ML engineer helping students build intelligent systems.',
    image: 'https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    email: 'amina.khaled@example.com',
    facebook: 'https://facebook.com/amina.khaled',
    whatsapp: 'https://wa.me/201000000001'
  },
  {
    id: 2,
    name: 'Eng. Samy Tarek',
    field: 'UI/UX Design',
    description: 'Specialist in user experience design with 5+ years mentoring students.',
    image: 'https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    email: 'omar.tarek@example.com',
    facebook: 'https://facebook.com/omar.tarek',
    whatsapp: 'https://wa.me/201000000002'
  },
  {
    id: 3,
    name: 'Prof. Laila Mahmoud',
    field: 'Cyber Security',
    description: 'Professor and researcher in cyber security guiding senior projects.',
    image: 'https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    email: 'laila.mahmoud@example.com',
    facebook: 'https://facebook.com/laila.mahmoud',
    whatsapp: 'https://wa.me/201000000003'
  }
];

const Mentors = () => {
  const [selectedMentor, setSelectedMentor] = useState(null);
  const [popupType, setPopupType] = useState('');
  const [showPopup, setShowPopup] = useState(false);

  const openPopup = (mentor, type) => {
    setSelectedMentor(mentor);
    setPopupType(type);
    setShowPopup(true);
  };

  const closePopup = () => {
    setShowPopup(false);
    setSelectedMentor(null);
    setPopupType('');
  };

  return (
    <div className="mentors-container">
      <div className="mentors-header">
        <h1>Meet Our Mentors</h1>
        <p>Connect with experienced professionals ready to support your project journey.</p>
      </div>

      <div className="mentor-cards">
        {mentorsData.map((mentor) => (
          <div className="mentor-card" key={mentor.id}>
            <img src={mentor.image} alt={mentor.name} className="mentor-img" />
            <h2>{mentor.name}</h2>
            <p className="mentor-field">{mentor.field}</p>
            <p className="mentor-desc">{mentor.description}</p>
            <div className="mentor-buttons">
              <button onClick={() => openPopup(mentor, 'profile')} className="btn profile-btn">View Profile</button>
              <button onClick={() => openPopup(mentor, 'chat')} className="btn chat-btn">Chat</button>
            </div>
          </div>
        ))}
      </div>

      <div className="become-mentor">
        <h2>Want to become a Mentor?</h2>
        <p>Share your knowledge and guide the next generation of innovators.</p>
        <button className="btn apply-btn" onClick={() => openPopup(null, 'apply')}>Apply Now</button>
      </div>

      {/* Pop-up Modal */}
      {showPopup && (
        <div className="popup-overlay" onClick={closePopup}>
          <div className="popup-content" onClick={(e) => e.stopPropagation()}>
            <button className="popup-close" onClick={closePopup}>✖</button>

            {popupType === 'profile' && selectedMentor && (
              <>
                <h2>{selectedMentor.name}</h2>
                <p><strong>Field:</strong> {selectedMentor.field}</p>
                <p>{selectedMentor.description}</p>
                <p><strong>Email:</strong> {selectedMentor.email}</p>
              </>
            )}

            {popupType === 'chat' && selectedMentor && (
              <>
                <h2>Contact {selectedMentor.name}</h2>
                <ul className="contact-list">
                  <li><a href={selectedMentor.facebook} target="_blank" rel="noreferrer">Facebook</a></li>
                  <li><a href={selectedMentor.whatsapp} target="_blank" rel="noreferrer">WhatsApp</a></li>
                  <li><a href={`mailto:${selectedMentor.email}`}>Email</a></li>
                </ul>
              </>
            )}

            {popupType === 'apply' && (
              <>
                <h2>Mentor Application Form</h2>
                <form className="apply-form">
                  <input type="text" placeholder="Full Name" required />
                  <input type="email" placeholder="Email Address" required />
                  <input type="text" placeholder="Field of Expertise" required />
                  <textarea placeholder="Brief Bio / Experience" required></textarea>
                  <button type="submit" className="btn submit-btn">Submit</button>
                </form>
              </>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default Mentors;

import React from 'react';
import './Mentors.css';
import { useNavigate } from 'react-router-dom';

const mentorsData = [
  {
    id: 1,
    name: 'Dr. Amina Khaled',
    field: 'Machine Learning',
    description: 'Experienced ML engineer helping students build intelligent systems.',
    image: '/assets/images/mentor1.png'
  },
  {
    id: 1,
    name: 'Dr. Amina Khaled',
    field: 'Machine Learning',
    description: 'Experienced ML engineer helping students build intelligent systems.',
    image: '/assets/images/mentor1.png'
  },
  {
    id: 1,
    name: 'Dr. Amina Khaled',
    field: 'Machine Learning',
    description: 'Experienced ML engineer helping students build intelligent systems.',
    image: '/assets/images/mentor1.png'
  },
  {
    id: 1,
    name: 'Dr. Amina Khaled',
    field: 'Machine Learning',
    description: 'Experienced ML engineer helping students build intelligent systems.',
    image: '/assets/images/mentor1.png'
  },
  {
    id: 1,
    name: 'Dr. Amina Khaled',
    field: 'Machine Learning',
    description: 'Experienced ML engineer helping students build intelligent systems.',
    image: '/assets/images/mentor1.png'
  },
  {
    id: 1,
    name: 'Dr. Amina Khaled',
    field: 'Machine Learning',
    description: 'Experienced ML engineer helping students build intelligent systems.',
    image: '/assets/images/mentor1.png'
  },
  {
    id: 2,
    name: 'Eng. Omar Tarek',
    field: 'UI/UX Design',
    description: 'Specialist in user experience design with 5+ years mentoring students.',
    image: '/assets/images/mentor2.png'
  },
  {
    id: 3,
    name: 'Prof. Laila Mahmoud',
    field: 'Cyber Security',
    description: 'Professor and researcher in cyber security guiding senior projects.',
    image: '/assets/images/mentor3.png'
  }
];

const Mentors = () => {
  const navigate = useNavigate();

  const handleViewProfile = (id) => {
    navigate(`/mentors/${id}`);
  };

  const handleChat = (id) => {
    navigate(`/chat/${id}`);
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
              <button onClick={() => handleViewProfile(mentor.id)} className="btn profile-btn">View Profile</button>
              <button onClick={() => handleChat(mentor.id)} className="btn chat-btn">Chat</button>
            </div>
          </div>
        ))}
      </div>

      <div className="become-mentor">
        <h2>Want to become a Mentor?</h2>
        <p>Share your knowledge and guide the next generation of innovators.</p>
        <button className="btn apply-btn">Apply Now</button>
      </div>
    </div>
  );
};

export default Mentors;

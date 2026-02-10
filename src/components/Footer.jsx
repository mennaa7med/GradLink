import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import './Footer.css';
import img6 from '../assets/images/footer.png';
import ContactForm from './ContactForm/ContactForm';
import { FiMail, FiMapPin, FiPhone, FiX } from 'react-icons/fi';

function Footer() {
    const [showContactModal, setShowContactModal] = useState(false);

    const scrollToSection = (id) => {
        const element = document.getElementById(id);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth' });
        }
    };

    return (
        <footer className="footer">
            <div className="footer-top">
                <div className="footer-column logo-column">
                    <Link to="/" className="footer-logo-link">
                        <img src={img6} alt="GradLink Logo" className="logo-face" />
                        <h3 className="gradlink-text">GradLink</h3>
                    </Link>
                    <p className="footer-tagline">From idea to Job, all in one place</p>
                </div>

                <div className="footer-column">
                    <h4>Quick Links</h4>
                    <ul>
                        <li><button onClick={() => scrollToSection('home')}>Home</button></li>
                        <li><button onClick={() => scrollToSection('services')}>Services</button></li>
                        <li><button onClick={() => scrollToSection('features')}>Features</button></li>
                        <li><button onClick={() => scrollToSection('about')}>About Us</button></li>
                        <li><Link to="/career">Career</Link></li>
                    </ul>
                </div>

                <div className="footer-column">
                    <h4>For Students</h4>
                    <ul>
                        <li><Link to="/projectsbank">Projects Bank</Link></li>
                        <li><Link to="/opportunities">Opportunities</Link></li>
                        <li><Link to="/matchmaking">Team Building</Link></li>
                        <li><Link to="/apply-mentor">Become a Mentor</Link></li>
                    </ul>
                </div>

                <div className="footer-column">
                    <h4>Contact Us</h4>
                    <div className="contact-info">
                        <p><FiMail /> gradlink@example.com</p>
                        <p><FiPhone /> +20 123 456 7890</p>
                        <p><FiMapPin /> Cairo, Egypt</p>
                    </div>
                    <button className="contact-btn" onClick={() => setShowContactModal(true)}>
                        Send Message
                    </button>
                </div>
            </div>

            <div className="footer-middle">
                <Link to="/signup" className="signup-button">Get Started Free</Link>
            </div>

            <div className="footer-bottom">
                <div className="copyright">
                    © {new Date().getFullYear()} GradLink. All rights reserved.
                </div>
                <div className="legal-links">
                    <Link to="/privacy-policy">Privacy Policy</Link>
                    <Link to="/terms-of-service">Terms of Service</Link>
                </div>
            </div>

            {/* Contact Form Modal */}
            {showContactModal && (
                <div className="modal-overlay" onClick={() => setShowContactModal(false)}>
                    <div className="modal-content contact-modal" onClick={e => e.stopPropagation()}>
                        <button className="modal-close" onClick={() => setShowContactModal(false)}>
                            <FiX />
                        </button>
                        <ContactForm onClose={() => setShowContactModal(false)} />
                    </div>
                </div>
            )}
        </footer>
    );
}

export default Footer;

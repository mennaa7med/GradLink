import React from 'react';
import './Footer.css';
import img6 from '../assets/images/footer.png';

function Footer() {
    return (
        <footer className="footer">
            <div className="footer-top">
                <div className="footer-column logo-column">
                    <img src={img6} alt="GradLink Logo" className="logo-face" />
                    <h3 className="gradlink-text">gradlink</h3>
                </div>

                <div className="footer-column">
                    <h4>Quick Links</h4>
                    <ul>
                        <li>Home</li>
                        <li>Services</li>
                        <li>Features</li>
                        <li>About Us</li>
                    </ul>
                </div>

                <div className="footer-column">
                    <h4>Contacts</h4>
                    <p>gradlink@example.com</p>
                </div>

                {/* جملة الحقوق هنا علشان تبقى تحت اللوجو والكولومنز */}
                <div className="copyright">
                    © 2025 GradConnect. All rights reserved.
                </div>
            </div>

            <div className="footer-middle">
                <button className="signup-button">Sign Up</button>
            </div>

            <div className="footer-bottom">
                <div className="legal-links">
                    <span>Privacy Notice</span>
                    <span>Accessibility</span>
                    <span>Terms of Use</span>
                </div>
            </div>
        </footer>
    );
}

export default Footer;

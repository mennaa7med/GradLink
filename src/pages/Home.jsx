import React from 'react';
import { Link } from 'react-router-dom';

import './Home.css';
import img1 from '../assets/images/image1.png';
import img2 from '../assets/images/circle.png';
import Services from '../components/Services';
import Features from '../components/Features';
import About from '../components/About';
import Navbar from '../components/Navbar';
import Footer from '../components/Footer';
import GeminiChatButton from '../components/GeminiChatButton'; // ✅ استدعاء الزر العائم

const Home = () => {
    return (
        <>
            <div id="navbar">
                <Navbar />
            </div>

            <div id="home">
                <div className="home-container">
                    <div className="text-section">
                        <h1 className="heading">
                            From idea to Job <br />
                            all in <span className="highlight-wrapper">
                                <span className="highlight">one</span>
                                <img src={img2} alt="circle" className="circle-image" />
                            </span> place
                        </h1>
                        <p className="description">
                            A comprehensive website tailored to support you in
                            your graduation project by providing job
                            opportunities, funding options, and resources for
                            project development, both during your studies and
                            beyond graduation.
                        </p>

                        <div className="buttons">
                            <Link to="/signin" className="start-btn">Start signing</Link>
                            <Link to="/Matchmaking" className="join-btn">Join Team</Link>
                        </div>
                    </div>
                    <div className="image-section">
                        <img src={img1} alt="Image Description" className="image" />
                    </div>
                </div>
            </div>

            <div id="services">
                <Services />
            </div>

            <div id="features">
                <Features />
            </div>

            <div id="about">
                <About />
            </div>

            {/* ✅ الزر العائم الخاص بالشات */}
            <GeminiChatButton />

            <div id="footer">
                <Footer />
            </div>
        </>
    );
}

export default Home;

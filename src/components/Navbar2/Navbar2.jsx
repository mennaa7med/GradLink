import React from 'react';
import './Navbar2.css';
import { Link } from 'react-router-dom';


const Navbar2 = () => {
    return (
        <header className="navbar2">
            <div className="navbar-left2">
               
            </div>

            <nav className="navbar-right2">
                <ul className="nav-links2">
                    <li><a href="/">Home</a></li>
                    <li><a href="/">Services</a></li>
                    <li><a href="/">Features</a></li>
                    <li><a href="/">About Us</a></li>
                    <li><Link to="/projectsbank">projects bank</Link></li>
                    <li><Link to="/career">Career</Link></li>


                </ul>
                
            </nav>
        </header>
    );
};

export default Navbar2;

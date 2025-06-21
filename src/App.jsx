import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar'
import Home from './pages/Home';
import Signup from './components/Signup/Signup';
import Signin from './components/Signin/Signin';
import Navbar2 from './components/Navbar2/Navbar2';
import Detailsproject from './components/Detailsproject/Detailsproject';
import Matchmaking from './components/Matchmaking';
import Ideaproject from './components/Projectsbank/ideaproject';
import Opportunities from './components/Opportunities';
import GeminiChatButton from './components/GeminiChatButton';
import DashboardLayout from './components/DashboardLayout/DashboardLayout';
import Dashboard from './pages/Dashboard';
import TaskManager from './components/TaskManager';
import Materials from './components/Matrials';
import Teams from './components/Teams';
import Sponsors from './components/Sponsors';
import Mentors from './components/Mentors';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/navbar" element={<Navbar />} />
        <Route path="/" element={<Home />} />
        <Route path="/signup" element={<Signup />} />
        <Route path="/signin" element={<Signin />} />
        <Route path="/projectsbank" element={<Ideaproject />} />
        <Route path="/navbar2" element={<Navbar2 />} />
        <Route path="/matchmaking" element={<Matchmaking />} />
        <Route path="/Detailsproject" element={<Detailsproject />} />
        <Route path="/Opportunities" element={<Opportunities />} />
        <Route path="/GeminiChatButton" element={<GeminiChatButton />} />
        
        <Route path="/dashboard" element={<DashboardLayout />}>
          <Route index element={<Dashboard />} />
          <Route path="tasks" element={<TaskManager />} />
          <Route path="materials" element={<Materials />} />
          <Route path="teams" element={<Teams />} />
          <Route path="mentors" element={<Mentors />} />
          <Route path="sponsors" element={<Sponsors />} />
        </Route>
        
        <Route path="/Dashboard" element={<DashboardLayout />}>
          <Route index element={<Dashboard />} />
        </Route>
        <Route path="/TaskManager" element={<TaskManager />} />
        <Route path="/Materials" element={<Materials />} />
        <Route path="/Teams" element={<Teams />} />
        <Route path="/Sponsors" element={<Sponsors />} />
        <Route path="/Mentors" element={<Mentors />} />
      </Routes>
    </Router>
  );
}

export default App;



import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from './contexts/ThemeContext';
import RequireAuth from './components/RequireAuth';
import ErrorBoundary from './components/ErrorBoundary';
import MainLayout from './components/MainLayout/MainLayout';
import Home from './pages/Home';
import Signup from './components/Signup/Signup';
import Signin from './components/Signin/Signin';
import ForgotPassword from './components/ForgotPassword/ForgotPassword';
import ResetPassword from './components/ResetPassword/ResetPassword';
import Navbar2 from './components/Navbar2/Navbar2';
import Detailsproject from './components/Detailsproject/Detailsproject';
import Matchmaking from './components/Matchmaking';
import Opportunities from './components/Opportunities';
import GeminiChatButton from './components/GeminiChatButton';
import Chat from './components/Chat';
import DashboardLayout from './components/DashboardLayout/DashboardLayout';
import Dashboard from './pages/Dashboard';
import TaskManager from './components/TaskManager';
import Materials from './components/Matrials'; 
import Teams from './components/Teams';
import Sponsors from './components/Sponsors';
import Mentors from './components/Mentors';
import SponsorDashboard from './components/SponsorDashboard';
import Settings from './components/Settings';
import Help from './components/Help';
import Logout from './components/Logout';
import AllFeatures from './components/AllFeatures';
import FeatureDetails from './components/FeatureDetails';
import CompanyDashboardNew from './components/CompanyDashboard/Dashboard';
import MentorDashboard from './components/MentorDashboard/MentorDashboard';
import AdminDashboard from './components/AdminDashboard/AdminDashboard';
import JobForm from './components/SmartMatch/JobForm';  
import MatchingCandidates from './components/SmartMatch/MatchingCandidates';
import ResumeScreener from './components/ResumeScreener';
import UploadResume from './components/UploadResume';
import JobDescription from './components/JobDescription';
import DeleteResumes from './components/DeleteResumes';
import JobDescriptionAnalysis from './components/JobDescriptionAnalysis';
import ScreeningResume from './pages/ScreeningResume';
import Career from './pages/Career';
import ApplyMentor from './pages/ApplyMentor/ApplyMentor';
import MentorTest from './pages/MentorTest/MentorTest';
import NotificationsPage from './components/NotificationsPage/NotificationsPage';
import MessagesPage from './components/MessagesPage/MessagesPage';
import NotFound from './pages/NotFound/NotFound';
import TermsOfService from './pages/Legal/TermsOfService';
import PrivacyPolicy from './pages/Legal/PrivacyPolicy';
import EmailVerification from './pages/EmailVerification/EmailVerification';
import VerifyOtp from './pages/VerifyOtp/VerifyOtp';
import CompleteProfile from './pages/CompleteProfile/CompleteProfile';
import PublicProfile from './pages/PublicProfile/PublicProfile';
import ResumeBuilder from './components/ResumeBuilder/ResumeBuilder';
import AchievementBadges from './components/AchievementBadges/AchievementBadges';
import StudentProjects from './components/StudentProjects/StudentProjects';
import OAuthCallback from './pages/OAuthCallback/OAuthCallback';
import Calendar from './components/Calendar/Calendar';
import Analytics from './components/Analytics/Analytics';

function App() {
  return (
    <ErrorBoundary>
      <ThemeProvider>
        <Router basename="/">
          <Routes>
            {/* Dashboard Routes - WITHOUT Navbar */}
            <Route path="/dashboard" element={<RequireAuth><DashboardLayout /></RequireAuth>}>
              <Route index element={<Dashboard />} />
              <Route path="tasks" element={<TaskManager />} />
              <Route path="materials" element={<Materials />} />
              <Route path="teams" element={<Teams />} />
              <Route path="projects" element={<StudentProjects />} />
              <Route path="calendar" element={<Calendar />} />
              <Route path="analytics" element={<Analytics />} />
              <Route path="mentors" element={<Mentors />} />
              <Route path="sponsors" element={<Sponsors />} />
              <Route path="notifications" element={<NotificationsPage />} />
              <Route path="messages" element={<MessagesPage />} />
              <Route path="resume-builder" element={<ResumeBuilder />} />
              <Route path="achievements" element={<AchievementBadges />} />
              <Route path="settings" element={<Settings />} />
              <Route path="help" element={<Help />} />
              <Route path="logout" element={<Logout />} />
            </Route>
            
            {/* Other Dashboard Pages - WITHOUT Navbar */}
            <Route path="/company-dashboard-new" element={<RequireAuth><CompanyDashboardNew /></RequireAuth>} />
            <Route path="/sponsor-dashboard" element={<RequireAuth><SponsorDashboard /></RequireAuth>} />
            <Route path="/mentor-dashboard" element={<RequireAuth><MentorDashboard /></RequireAuth>} />
            <Route path="/admin-dashboard" element={<RequireAuth><AdminDashboard /></RequireAuth>} />

            {/* Auth Pages - WITHOUT Navbar */}
            <Route path="/signup" element={<Signup />} />
            <Route path="/signin" element={<Signin />} />
            <Route path="/verify-otp" element={<VerifyOtp />} />
            <Route path="/forgot-password" element={<ForgotPassword />} />
            <Route path="/reset-password" element={<ResetPassword />} />
            <Route path="/oauth/callback/:provider" element={<OAuthCallback />} />

            {/* Pages WITH Navbar */}
            <Route element={<MainLayout />}>
              <Route path="/" element={<Home />} />
              <Route path="/navbar2" element={<Navbar2 />} />
              <Route path="/matchmaking" element={<Matchmaking />} />
              <Route path="/Detailsproject" element={<Detailsproject />} />
              <Route path="/Opportunities" element={<Opportunities />} />
              <Route path="/GeminiChatButton" element={<GeminiChatButton />} />
              <Route path="/chat" element={<RequireAuth><Chat /></RequireAuth>} />
              <Route path="/Settings" element={<Settings />} />
              <Route path="/Help" element={<Help />} />
              <Route path="/logout" element={<Logout />} />
              <Route path="/AllFeatures" element={<AllFeatures />} />
              <Route path="/feature/:id" element={<FeatureDetails />} />
              <Route path="/job-form" element={<JobForm />} />
              <Route path="/resume-screener" element={<ResumeScreener />} />
              <Route path="/upload-resume" element={<UploadResume />} />
              <Route path="/job-description" element={<JobDescription />} />
              <Route path="/delete-resumes" element={<DeleteResumes />} />
              <Route path="/job-description-analysis" element={<JobDescriptionAnalysis />} />
              <Route path="/matching-candidates" element={<MatchingCandidates />} />
              <Route path="/screening-resume" element={<ScreeningResume />} />
              <Route path="/career" element={<Career />} />
              <Route path="/apply-mentor" element={<ApplyMentor />} />
              <Route path="/mentor-test" element={<MentorTest />} />
              
              {/* Legal Pages */}
              <Route path="/terms-of-service" element={<TermsOfService />} />
              <Route path="/privacy-policy" element={<PrivacyPolicy />} />
              
              {/* Email Verification */}
              <Route path="/verify-email" element={<EmailVerification />} />
              
              {/* Complete Profile */}
              <Route path="/complete-profile" element={<RequireAuth><CompleteProfile /></RequireAuth>} />
              
              {/* Public Profile */}
              <Route path="/profile/:userId" element={<PublicProfile />} />
              
              {/* 404 Not Found */}
              <Route path="*" element={<NotFound />} />
            </Route>

          </Routes>
          
          {/* Gemini Chatbot - يظهر في كل الصفحات */}
          <GeminiChatButton />
        </Router>
      </ThemeProvider>
    </ErrorBoundary>
  );
}

export default App;

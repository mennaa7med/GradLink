import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './SponsorDashboard.css';

const SponsorDashboard = () => {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState('overview');
  const [loading, setLoading] = useState(true);
  const [selectedProject, setSelectedProject] = useState(null);
  const [selectedRequest, setSelectedRequest] = useState(null);
  const [showChat, setShowChat] = useState(false);
  const [chatTarget, setChatTarget] = useState(null);
  const [showProfileModal, setShowProfileModal] = useState(false);

  // Sponsor Profile
  const [profile, setProfile] = useState({
    companyName: localStorage.getItem('companyName') || 'TechVentures Inc.',
    contactName: JSON.parse(localStorage.getItem('user') || '{}').fullName || 'John Smith',
    email: JSON.parse(localStorage.getItem('user') || '{}').email || 'sponsor@techventures.com',
    phone: '+1 (555) 123-4567',
    industry: 'Technology & Innovation',
    website: 'https://techventures.com',
    logo: null,
    description: 'Leading technology company focused on supporting innovative student projects and fostering next-generation talent.',
    areasOfInterest: ['AI/ML', 'Web Development', 'Mobile Apps', 'IoT'],
    budgetRange: '$10,000 - $50,000',
    isVerified: true,
    memberSince: '2024',
    totalBudget: 150000,
    usedBudget: 47500,
    notificationPreferences: {
      email: true,
      milestones: true,
      messages: true,
      weeklyDigest: true
    }
  });

  // Dashboard Stats
  const [stats] = useState({
    totalSponsored: 8,
    activeProjects: 5,
    pendingRequests: 4,
    completedProjects: 3,
    totalInvested: 47500,
    avgProjectProgress: 68,
    studentsSupported: 24,
    mentorsConnected: 6
  });

  // Activity Feed
  const [activities] = useState([
    { id: 1, type: 'milestone', project: 'AI Healthcare Assistant', text: 'Milestone 3 completed: API Integration', time: '2 hours ago', icon: '🎯' },
    { id: 2, type: 'file', project: 'Smart Campus App', text: 'New file uploaded: Progress_Report_Dec.pdf', time: '5 hours ago', icon: '📄' },
    { id: 3, type: 'message', project: 'E-Learning Platform', text: 'New message from Omar Hassan', time: '1 day ago', icon: '💬' },
    { id: 4, type: 'request', project: 'Blockchain Voting', text: 'Sponsorship request approved', time: '2 days ago', icon: '✅' },
    { id: 5, type: 'alert', project: 'IoT Smart Farm', text: 'Project completed successfully!', time: '3 days ago', icon: '🎉' },
  ]);

  // Sponsored Projects
  const [projects] = useState([
    {
      id: 1,
      title: 'AI Healthcare Assistant',
      team: 'MedTech Innovators',
      teamSize: 4,
      category: 'AI/ML',
      phase: 'Development',
      progress: 75,
      funding: 8000,
      fundingUsed: 6000,
      startDate: '2024-09-15',
      endDate: '2025-05-30',
      description: 'An AI-powered healthcare assistant that helps patients manage their health records, schedule appointments, and get preliminary health assessments.',
      mentor: { name: 'Dr. Sarah Johnson', expertise: 'Machine Learning', avatar: 'SJ' },
      students: ['Ahmed Mohamed', 'Sara Ali', 'Omar Hassan', 'Fatma Ahmed'],
      milestones: [
        { name: 'Requirements Analysis', status: 'completed', date: '2024-10-01' },
        { name: 'UI/UX Design', status: 'completed', date: '2024-11-01' },
        { name: 'API Integration', status: 'completed', date: '2024-12-15' },
        { name: 'Testing & QA', status: 'in-progress', date: '2025-02-01' },
        { name: 'Final Deployment', status: 'pending', date: '2025-05-30' },
      ],
      files: [
        { name: 'Project_Proposal.pdf', type: 'pdf', size: '2.4 MB', date: '2024-09-20' },
        { name: 'UI_Mockups.fig', type: 'figma', size: '15 MB', date: '2024-10-25' },
        { name: 'Progress_Report_Nov.pdf', type: 'pdf', size: '1.8 MB', date: '2024-11-30' },
        { name: 'API_Documentation.pdf', type: 'pdf', size: '3.2 MB', date: '2024-12-10' },
      ],
      impact: { users: '10,000+', market: 'Healthcare', severity: 'High', hiringPotential: 3 },
      status: 'active'
    },
    {
      id: 2,
      title: 'Smart Campus Navigation',
      team: 'Campus Guides',
      teamSize: 3,
      category: 'Mobile',
      phase: 'Testing',
      progress: 85,
      funding: 5000,
      fundingUsed: 4500,
      startDate: '2024-08-01',
      endDate: '2025-03-15',
      description: 'Mobile app providing indoor navigation for university campuses with AR features and real-time updates.',
      mentor: { name: 'Prof. Michael Chen', expertise: 'Mobile Development', avatar: 'MC' },
      students: ['Layla Mahmoud', 'Karim Salah', 'Nour Ibrahim'],
      milestones: [
        { name: 'Research & Planning', status: 'completed', date: '2024-08-15' },
        { name: 'App Development', status: 'completed', date: '2024-11-01' },
        { name: 'AR Integration', status: 'completed', date: '2024-12-01' },
        { name: 'Beta Testing', status: 'in-progress', date: '2025-01-15' },
        { name: 'Launch', status: 'pending', date: '2025-03-15' },
      ],
      files: [
        { name: 'App_Architecture.pdf', type: 'pdf', size: '1.5 MB', date: '2024-08-20' },
        { name: 'Beta_Testing_Results.xlsx', type: 'excel', size: '800 KB', date: '2025-01-10' },
      ],
      impact: { users: '5,000+', market: 'Education', severity: 'Medium', hiringPotential: 2 },
      status: 'active'
    },
    {
      id: 3,
      title: 'E-Learning Platform',
      team: 'EduTech Solutions',
      teamSize: 5,
      category: 'Web',
      phase: 'Development',
      progress: 55,
      funding: 12000,
      fundingUsed: 6600,
      startDate: '2024-10-01',
      endDate: '2025-06-30',
      description: 'Comprehensive e-learning platform with video streaming, interactive quizzes, and progress tracking for students.',
      mentor: { name: 'Dr. Emily Watson', expertise: 'Web Development', avatar: 'EW' },
      students: ['Youssef Khaled', 'Mona Adel', 'Hassan Ali', 'Dina Mostafa', 'Khaled Ibrahim'],
      milestones: [
        { name: 'Platform Architecture', status: 'completed', date: '2024-10-20' },
        { name: 'Core Features', status: 'completed', date: '2024-12-01' },
        { name: 'Video Streaming', status: 'in-progress', date: '2025-02-15' },
        { name: 'Mobile Responsive', status: 'pending', date: '2025-04-01' },
        { name: 'Final Launch', status: 'pending', date: '2025-06-30' },
      ],
      files: [
        { name: 'Technical_Specs.pdf', type: 'pdf', size: '4.1 MB', date: '2024-10-15' },
      ],
      impact: { users: '20,000+', market: 'Education', severity: 'High', hiringPotential: 4 },
      status: 'active'
    },
    {
      id: 4,
      title: 'IoT Smart Farm',
      team: 'AgriTech Pioneers',
      teamSize: 4,
      category: 'IoT',
      phase: 'Completed',
      progress: 100,
      funding: 10000,
      fundingUsed: 10000,
      startDate: '2024-03-01',
      endDate: '2024-11-30',
      description: 'IoT-based smart farming solution for monitoring soil conditions, irrigation, and crop health.',
      mentor: { name: 'Dr. Ahmed Farouk', expertise: 'IoT & Embedded Systems', avatar: 'AF' },
      students: ['Mohamed Samir', 'Aya Hassan', 'Tarek Nabil', 'Salma Youssef'],
      milestones: [
        { name: 'Hardware Design', status: 'completed', date: '2024-04-15' },
        { name: 'Sensor Integration', status: 'completed', date: '2024-06-30' },
        { name: 'Cloud Platform', status: 'completed', date: '2024-09-01' },
        { name: 'Field Testing', status: 'completed', date: '2024-10-30' },
        { name: 'Final Presentation', status: 'completed', date: '2024-11-30' },
      ],
      files: [
        { name: 'Final_Report.pdf', type: 'pdf', size: '8.5 MB', date: '2024-11-28' },
        { name: 'Presentation.pptx', type: 'ppt', size: '25 MB', date: '2024-11-30' },
        { name: 'GitHub_Link.txt', type: 'link', size: '-', date: '2024-11-30' },
      ],
      impact: { users: '500+', market: 'Agriculture', severity: 'High', hiringPotential: 3 },
      status: 'completed'
    },
    {
      id: 5,
      title: 'Blockchain Voting System',
      team: 'SecureVote',
      teamSize: 3,
      category: 'Blockchain',
      phase: 'Idea',
      progress: 20,
      funding: 6000,
      fundingUsed: 1200,
      startDate: '2024-12-01',
      endDate: '2025-08-30',
      description: 'Decentralized voting system using blockchain technology for secure and transparent elections.',
      mentor: { name: 'Prof. James Wilson', expertise: 'Blockchain', avatar: 'JW' },
      students: ['Ali Mahmoud', 'Reem Ahmed', 'Amr Hassan'],
      milestones: [
        { name: 'Concept Validation', status: 'completed', date: '2024-12-20' },
        { name: 'Smart Contract Design', status: 'in-progress', date: '2025-02-28' },
        { name: 'Frontend Development', status: 'pending', date: '2025-05-01' },
        { name: 'Security Audit', status: 'pending', date: '2025-07-01' },
        { name: 'Deployment', status: 'pending', date: '2025-08-30' },
      ],
      files: [
        { name: 'Whitepaper.pdf', type: 'pdf', size: '2.1 MB', date: '2024-12-15' },
      ],
      impact: { users: '1,000+', market: 'Government', severity: 'Critical', hiringPotential: 2 },
      status: 'active'
    },
  ]);

  // Sponsorship Requests
  const [requests] = useState([
    {
      id: 1,
      title: 'VR Educational Platform',
      team: 'VR Innovators',
      teamSize: 4,
      pitch: 'Revolutionary VR platform for immersive learning experiences in STEM education.',
      problem: 'Traditional learning methods fail to engage students in complex scientific concepts.',
      solution: 'Using VR technology to create interactive 3D environments for hands-on learning.',
      supportType: ['Funding', 'Technical Mentorship'],
      requestedBudget: 15000,
      category: 'VR/AR',
      submittedDate: '2024-12-28',
      priority: 'high',
      students: ['Yasmine Nabil', 'Kareem Said', 'Mariam Fathy', 'Mostafa Ali']
    },
    {
      id: 2,
      title: 'Mental Health Companion App',
      team: 'MindCare',
      teamSize: 3,
      pitch: 'AI-powered mental wellness app providing personalized support and resources.',
      problem: 'University students face increasing mental health challenges with limited access to support.',
      solution: 'Mobile app with mood tracking, AI chat support, and connection to professional resources.',
      supportType: ['Funding', 'Tools'],
      requestedBudget: 8000,
      category: 'Mobile',
      submittedDate: '2024-12-26',
      priority: 'medium',
      students: ['Nada Ahmed', 'Hussein Omar', 'Sally Mohamed']
    },
    {
      id: 3,
      title: 'Green Energy Monitor',
      team: 'EcoTech',
      teamSize: 5,
      pitch: 'Real-time energy monitoring system for buildings to reduce carbon footprint.',
      problem: 'Buildings waste significant energy due to lack of visibility into consumption patterns.',
      solution: 'IoT sensors with ML analytics to optimize energy usage and predict maintenance needs.',
      supportType: ['Funding', 'Hardware', 'Technical Mentorship'],
      requestedBudget: 20000,
      category: 'IoT',
      submittedDate: '2024-12-24',
      priority: 'high',
      students: ['Amira Saleh', 'Mahmoud Ezzat', 'Heba Adel', 'Wael Hassan', 'Dalia Kamal']
    },
    {
      id: 4,
      title: 'Accessibility Toolkit',
      team: 'IncluTech',
      teamSize: 3,
      pitch: 'Suite of tools to make web applications more accessible for people with disabilities.',
      problem: 'Many websites lack proper accessibility features, excluding users with disabilities.',
      solution: 'Browser extension and developer toolkit for automatic accessibility improvements.',
      supportType: ['Funding'],
      requestedBudget: 5000,
      category: 'Web',
      submittedDate: '2024-12-22',
      priority: 'low',
      students: ['Rania Mostafa', 'Tamer Gamal', 'Noura Sayed']
    },
  ]);

  // Messages
  const [messages] = useState([
    { id: 1, from: 'Ahmed Mohamed', project: 'AI Healthcare Assistant', avatar: 'AM', text: 'Thank you for approving the milestone! We are now moving to the testing phase.', time: '2 hours ago', unread: true },
    { id: 2, from: 'Dr. Sarah Johnson', role: 'Mentor', project: 'AI Healthcare Assistant', avatar: 'SJ', text: 'The team is making excellent progress. I recommend continuing the support.', time: '1 day ago', unread: true },
    { id: 3, from: 'Omar Hassan', project: 'E-Learning Platform', avatar: 'OH', text: 'We have uploaded the latest progress report. Please review when you have time.', time: '2 days ago', unread: false },
    { id: 4, from: 'Layla Mahmoud', project: 'Smart Campus Navigation', avatar: 'LM', text: 'Beta testing is going well! We would love your feedback on the app.', time: '3 days ago', unread: false },
  ]);

  // Notifications
  const [notifications] = useState([
    { id: 1, type: 'milestone', text: 'AI Healthcare Assistant completed Milestone 3', time: '2h ago', read: false },
    { id: 2, type: 'file', text: 'New progress report uploaded by Smart Campus team', time: '5h ago', read: false },
    { id: 3, type: 'request', text: 'New sponsorship request: VR Educational Platform', time: '1d ago', read: true },
    { id: 4, type: 'message', text: '2 new messages from project teams', time: '1d ago', read: true },
  ]);

  // Sponsorship Records
  const [sponsorships] = useState([
    { id: 1, project: 'AI Healthcare Assistant', type: 'Financial', amount: 8000, status: 'Active', startDate: '2024-09-15', endDate: '2025-05-30' },
    { id: 2, project: 'Smart Campus Navigation', type: 'Financial', amount: 5000, status: 'Active', startDate: '2024-08-01', endDate: '2025-03-15' },
    { id: 3, project: 'E-Learning Platform', type: 'Financial + Tools', amount: 12000, status: 'Active', startDate: '2024-10-01', endDate: '2025-06-30' },
    { id: 4, project: 'IoT Smart Farm', type: 'Financial + Hardware', amount: 10000, status: 'Completed', startDate: '2024-03-01', endDate: '2024-11-30' },
    { id: 5, project: 'Blockchain Voting System', type: 'Financial', amount: 6000, status: 'Active', startDate: '2024-12-01', endDate: '2025-08-30' },
  ]);

  useEffect(() => {
    setTimeout(() => setLoading(false), 600);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('userRole');
    navigate('/signin');
  };

  const handleApproveRequest = (id) => {
    alert(`Request ${id} approved!`);
    setSelectedRequest(null);
  };

  const handleRejectRequest = (id) => {
    alert(`Request ${id} rejected.`);
    setSelectedRequest(null);
  };

  const openChat = (target) => {
    setChatTarget(target);
    setShowChat(true);
  };

  if (loading) {
    return (
      <div className="sd">
        <div className="sd-loading">
          <div className="sd-spinner"></div>
          <p>Loading Sponsor Dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="sd">
      {/* Sidebar */}
      <aside className="sd-sidebar">
        <div className="sd-brand">
          <div className="sd-logo">💎</div>
          <div className="sd-brand-text">
            <span>GradLink</span>
            <small>Sponsor Portal</small>
          </div>
        </div>

        <div className="sd-profile-mini">
          <div className="sd-avatar">{profile.companyName.charAt(0)}</div>
          <div className="sd-profile-info">
            <h4>{profile.companyName}</h4>
            <span className={`sd-badge ${profile.isVerified ? 'verified' : ''}`}>
              {profile.isVerified ? '✓ Verified Sponsor' : 'Pending Verification'}
            </span>
          </div>
        </div>

        <nav className="sd-nav">
          <div className="sd-nav-section">
            <span className="sd-nav-title">Dashboard</span>
            <button className={activeTab === 'overview' ? 'active' : ''} onClick={() => setActiveTab('overview')}>
              <span className="icon">📊</span> Overview
            </button>
            <button className={activeTab === 'projects' ? 'active' : ''} onClick={() => setActiveTab('projects')}>
              <span className="icon">📁</span> Sponsored Projects
              {stats.activeProjects > 0 && <span className="sd-count">{stats.activeProjects}</span>}
            </button>
            <button className={activeTab === 'requests' ? 'active' : ''} onClick={() => setActiveTab('requests')}>
              <span className="icon">📩</span> Requests
              {stats.pendingRequests > 0 && <span className="sd-count alert">{stats.pendingRequests}</span>}
            </button>
          </div>

          <div className="sd-nav-section">
            <span className="sd-nav-title">Communication</span>
            <button className={activeTab === 'messages' ? 'active' : ''} onClick={() => setActiveTab('messages')}>
              <span className="icon">💬</span> Messages
              {messages.filter(m => m.unread).length > 0 && <span className="sd-count">{messages.filter(m => m.unread).length}</span>}
            </button>
            <button className={activeTab === 'files' ? 'active' : ''} onClick={() => setActiveTab('files')}>
              <span className="icon">📂</span> Files & Reports
            </button>
          </div>

          <div className="sd-nav-section">
            <span className="sd-nav-title">Analytics</span>
            <button className={activeTab === 'impact' ? 'active' : ''} onClick={() => setActiveTab('impact')}>
              <span className="icon">🎯</span> Impact & ROI
            </button>
            <button className={activeTab === 'analytics' ? 'active' : ''} onClick={() => setActiveTab('analytics')}>
              <span className="icon">📈</span> Analytics
            </button>
          </div>

          <div className="sd-nav-section">
            <span className="sd-nav-title">Management</span>
            <button className={activeTab === 'sponsorships' ? 'active' : ''} onClick={() => setActiveTab('sponsorships')}>
              <span className="icon">💰</span> Sponsorships
            </button>
            <button className={activeTab === 'settings' ? 'active' : ''} onClick={() => setActiveTab('settings')}>
              <span className="icon">⚙️</span> Settings
            </button>
          </div>
        </nav>

        <div className="sd-sidebar-footer">
          <div className="sd-budget-mini">
            <div className="sd-budget-header">
              <span>Budget Used</span>
              <span>${profile.usedBudget.toLocaleString()} / ${profile.totalBudget.toLocaleString()}</span>
            </div>
            <div className="sd-budget-bar">
              <div className="sd-budget-fill" style={{ width: `${(profile.usedBudget / profile.totalBudget) * 100}%` }}></div>
            </div>
          </div>
          <button className="sd-logout" onClick={handleLogout}>
            <span className="icon">🚪</span> Logout
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <main className="sd-main">
        {/* Header */}
        <header className="sd-header">
          <div className="sd-header-left">
            <h1>
              {activeTab === 'overview' && '📊 Dashboard Overview'}
              {activeTab === 'projects' && '📁 Sponsored Projects'}
              {activeTab === 'requests' && '📩 Sponsorship Requests'}
              {activeTab === 'messages' && '💬 Messages & Communication'}
              {activeTab === 'files' && '📂 Files & Reports'}
              {activeTab === 'impact' && '🎯 Impact & ROI'}
              {activeTab === 'analytics' && '📈 Analytics & Insights'}
              {activeTab === 'sponsorships' && '💰 Sponsorship Management'}
              {activeTab === 'settings' && '⚙️ Profile & Settings'}
            </h1>
            <p className="sd-subtitle">{new Date().toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}</p>
          </div>
          <div className="sd-header-right">
            <div className="sd-notifications">
              <button className="sd-notif-btn">
                🔔
                {notifications.filter(n => !n.read).length > 0 && (
                  <span className="sd-notif-badge">{notifications.filter(n => !n.read).length}</span>
                )}
              </button>
            </div>
            <div className="sd-user-menu" onClick={() => setShowProfileModal(true)}>
              <div className="sd-avatar-sm">{profile.companyName.charAt(0)}</div>
              <span>{profile.contactName}</span>
            </div>
          </div>
        </header>

        {/* Content Area */}
        <div className="sd-content">
          
          {/* ==================== OVERVIEW TAB ==================== */}
          {activeTab === 'overview' && (
            <div className="sd-overview">
              {/* Stats Row */}
              <div className="sd-stats-row">
                <div className="sd-stat-card primary">
                  <div className="sd-stat-icon">📁</div>
                  <div className="sd-stat-content">
                    <h3>{stats.totalSponsored}</h3>
                    <p>Total Sponsored</p>
                  </div>
                </div>
                <div className="sd-stat-card success">
                  <div className="sd-stat-icon">🚀</div>
                  <div className="sd-stat-content">
                    <h3>{stats.activeProjects}</h3>
                    <p>Active Projects</p>
                  </div>
                </div>
                <div className="sd-stat-card warning">
                  <div className="sd-stat-icon">📩</div>
                  <div className="sd-stat-content">
                    <h3>{stats.pendingRequests}</h3>
                    <p>Pending Requests</p>
                  </div>
                </div>
                <div className="sd-stat-card info">
                  <div className="sd-stat-icon">✅</div>
                  <div className="sd-stat-content">
                    <h3>{stats.completedProjects}</h3>
                    <p>Completed</p>
                  </div>
                </div>
              </div>

              {/* Budget & Progress */}
              <div className="sd-row">
                <div className="sd-card sd-budget-card">
                  <h3>💰 Budget Overview</h3>
                  <div className="sd-budget-content">
                    <div className="sd-budget-circle">
                      <svg viewBox="0 0 36 36">
                        <path className="sd-circle-bg" d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" />
                        <path className="sd-circle-fill" strokeDasharray={`${(profile.usedBudget / profile.totalBudget) * 100}, 100`} d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831" />
                      </svg>
                      <div className="sd-circle-text">
                        <span className="sd-circle-value">{Math.round((profile.usedBudget / profile.totalBudget) * 100)}%</span>
                        <span className="sd-circle-label">Used</span>
                      </div>
                    </div>
                    <div className="sd-budget-details">
                      <div className="sd-budget-item">
                        <span className="label">Total Budget</span>
                        <span className="value">${profile.totalBudget.toLocaleString()}</span>
                      </div>
                      <div className="sd-budget-item">
                        <span className="label">Invested</span>
                        <span className="value invested">${profile.usedBudget.toLocaleString()}</span>
                      </div>
                      <div className="sd-budget-item">
                        <span className="label">Available</span>
                        <span className="value available">${(profile.totalBudget - profile.usedBudget).toLocaleString()}</span>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="sd-card sd-progress-card">
                  <h3>📊 Overall Progress</h3>
                  <div className="sd-progress-list">
                    {projects.filter(p => p.status === 'active').slice(0, 4).map(project => (
                      <div key={project.id} className="sd-progress-item">
                        <div className="sd-progress-header">
                          <span className="sd-progress-title">{project.title}</span>
                          <span className="sd-progress-percent">{project.progress}%</span>
                        </div>
                        <div className="sd-progress-bar">
                          <div className="sd-progress-fill" style={{ width: `${project.progress}%` }}></div>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              </div>

              {/* Activity Feed */}
              <div className="sd-card">
                <h3>📋 Recent Activity</h3>
                <div className="sd-activity-feed">
                  {activities.map(activity => (
                    <div key={activity.id} className="sd-activity-item">
                      <div className="sd-activity-icon">{activity.icon}</div>
                      <div className="sd-activity-content">
                        <p><strong>{activity.project}</strong> - {activity.text}</p>
                        <span className="sd-activity-time">{activity.time}</span>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          )}

          {/* ==================== PROJECTS TAB ==================== */}
          {activeTab === 'projects' && (
            <div className="sd-projects">
              <div className="sd-toolbar">
                <div className="sd-filters">
                  <select>
                    <option value="">All Status</option>
                    <option value="active">Active</option>
                    <option value="completed">Completed</option>
                  </select>
                  <select>
                    <option value="">All Categories</option>
                    <option value="AI/ML">AI/ML</option>
                    <option value="Web">Web</option>
                    <option value="Mobile">Mobile</option>
                    <option value="IoT">IoT</option>
                    <option value="Blockchain">Blockchain</option>
                  </select>
                </div>
                <div className="sd-search">
                  <input type="text" placeholder="Search projects..." />
                </div>
              </div>

              <div className="sd-projects-grid">
                {projects.map(project => (
                  <div key={project.id} className={`sd-project-card ${project.status}`}>
                    <div className="sd-project-header">
                      <span className={`sd-phase ${project.phase.toLowerCase()}`}>{project.phase}</span>
                      <span className={`sd-status-badge ${project.status}`}>{project.status}</span>
                    </div>
                    <h4>{project.title}</h4>
                    <p className="sd-team">👥 {project.team} • {project.teamSize} members</p>
                    <div className="sd-project-meta">
                      <span className="sd-category">{project.category}</span>
                      <span className="sd-funding">${project.funding.toLocaleString()}</span>
                    </div>
                    <div className="sd-project-progress">
                      <div className="sd-progress-header">
                        <span>Progress</span>
                        <span>{project.progress}%</span>
                      </div>
                      <div className="sd-progress-bar">
                        <div className="sd-progress-fill" style={{ width: `${project.progress}%` }}></div>
                      </div>
                    </div>
                    <div className="sd-project-actions">
                      <button className="sd-btn primary" onClick={() => setSelectedProject(project)}>View Details</button>
                      <button className="sd-btn secondary" onClick={() => openChat(project.team)}>Message</button>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* ==================== REQUESTS TAB ==================== */}
          {activeTab === 'requests' && (
            <div className="sd-requests">
              <div className="sd-requests-list">
                {requests.map(request => (
                  <div key={request.id} className={`sd-request-card priority-${request.priority}`}>
                    <div className="sd-request-header">
                      <div className="sd-request-title">
                        <h4>{request.title}</h4>
                        <span className="sd-category">{request.category}</span>
                      </div>
                      <span className={`sd-priority ${request.priority}`}>{request.priority} priority</span>
                    </div>
                    <p className="sd-pitch">{request.pitch}</p>
                    <div className="sd-request-details">
                      <div className="sd-detail-item">
                        <span className="label">Team</span>
                        <span className="value">{request.team} ({request.teamSize} members)</span>
                      </div>
                      <div className="sd-detail-item">
                        <span className="label">Requested Budget</span>
                        <span className="value highlight">${request.requestedBudget.toLocaleString()}</span>
                      </div>
                      <div className="sd-detail-item">
                        <span className="label">Support Type</span>
                        <span className="value">{request.supportType.join(', ')}</span>
                      </div>
                    </div>
                    <div className="sd-request-actions">
                      <button className="sd-btn success" onClick={() => handleApproveRequest(request.id)}>✓ Approve</button>
                      <button className="sd-btn danger" onClick={() => handleRejectRequest(request.id)}>✗ Reject</button>
                      <button className="sd-btn secondary" onClick={() => setSelectedRequest(request)}>View Details</button>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* ==================== MESSAGES TAB ==================== */}
          {activeTab === 'messages' && (
            <div className="sd-messages">
              <div className="sd-messages-header">
                <h3>All Conversations</h3>
              </div>
              <div className="sd-messages-list">
                {messages.map(msg => (
                  <div key={msg.id} className={`sd-message-item ${msg.unread ? 'unread' : ''}`} onClick={() => openChat(msg.from)}>
                    <div className="sd-msg-avatar">{msg.avatar}</div>
                    <div className="sd-msg-content">
                      <div className="sd-msg-header">
                        <h4>{msg.from} {msg.role && <span className="sd-role">{msg.role}</span>}</h4>
                        <span className="sd-msg-time">{msg.time}</span>
                      </div>
                      <p className="sd-msg-project">Re: {msg.project}</p>
                      <p className="sd-msg-text">{msg.text}</p>
                    </div>
                    {msg.unread && <span className="sd-unread-indicator"></span>}
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* ==================== FILES TAB ==================== */}
          {activeTab === 'files' && (
            <div className="sd-files">
              <div className="sd-files-toolbar">
                <select>
                  <option value="">All Projects</option>
                  {projects.map(p => <option key={p.id} value={p.id}>{p.title}</option>)}
                </select>
                <select>
                  <option value="">All Types</option>
                  <option value="pdf">PDF</option>
                  <option value="ppt">Presentations</option>
                  <option value="excel">Spreadsheets</option>
                  <option value="link">Links</option>
                </select>
              </div>

              <div className="sd-files-grid">
                {projects.map(project => (
                  <div key={project.id} className="sd-file-section">
                    <h4>{project.title}</h4>
                    <div className="sd-file-list">
                      {project.files.map((file, idx) => (
                        <div key={idx} className="sd-file-item">
                          <div className="sd-file-icon">
                            {file.type === 'pdf' && '📄'}
                            {file.type === 'ppt' && '📊'}
                            {file.type === 'excel' && '📗'}
                            {file.type === 'figma' && '🎨'}
                            {file.type === 'link' && '🔗'}
                          </div>
                          <div className="sd-file-info">
                            <span className="sd-file-name">{file.name}</span>
                            <span className="sd-file-meta">{file.size} • {file.date}</span>
                          </div>
                          <div className="sd-file-actions">
                            <button className="sd-btn-icon">👁️</button>
                            <button className="sd-btn-icon">⬇️</button>
                          </div>
                        </div>
                      ))}
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* ==================== IMPACT TAB ==================== */}
          {activeTab === 'impact' && (
            <div className="sd-impact">
              <div className="sd-impact-summary">
                <div className="sd-impact-card">
                  <div className="sd-impact-icon">👥</div>
                  <h3>{stats.studentsSupported}</h3>
                  <p>Students Supported</p>
                </div>
                <div className="sd-impact-card">
                  <div className="sd-impact-icon">🎓</div>
                  <h3>{stats.mentorsConnected}</h3>
                  <p>Mentors Connected</p>
                </div>
                <div className="sd-impact-card">
                  <div className="sd-impact-icon">💼</div>
                  <h3>12</h3>
                  <p>Hiring Potential</p>
                </div>
                <div className="sd-impact-card highlight">
                  <div className="sd-impact-icon">📈</div>
                  <h3>35K+</h3>
                  <p>Expected Users Reached</p>
                </div>
              </div>

              <div className="sd-card">
                <h3>🎯 Project Impact Analysis</h3>
                <div className="sd-impact-table">
                  <table>
                    <thead>
                      <tr>
                        <th>Project</th>
                        <th>Market</th>
                        <th>Expected Users</th>
                        <th>Severity</th>
                        <th>Hiring Potential</th>
                        <th>ROI Score</th>
                      </tr>
                    </thead>
                    <tbody>
                      {projects.map(project => (
                        <tr key={project.id}>
                          <td><strong>{project.title}</strong></td>
                          <td>{project.impact.market}</td>
                          <td>{project.impact.users}</td>
                          <td><span className={`sd-severity ${project.impact.severity.toLowerCase()}`}>{project.impact.severity}</span></td>
                          <td>{project.impact.hiringPotential} candidates</td>
                          <td><span className="sd-roi-score">{Math.floor(Math.random() * 30) + 70}%</span></td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}

          {/* ==================== ANALYTICS TAB ==================== */}
          {activeTab === 'analytics' && (
            <div className="sd-analytics">
              <div className="sd-charts-row">
                <div className="sd-chart-card">
                  <h3>📊 Investment by Category</h3>
                  <div className="sd-pie-chart">
                    <div className="sd-pie"></div>
                    <div className="sd-pie-legend">
                      <div className="sd-legend-item"><span className="dot ai"></span> AI/ML - 35%</div>
                      <div className="sd-legend-item"><span className="dot web"></span> Web - 25%</div>
                      <div className="sd-legend-item"><span className="dot mobile"></span> Mobile - 20%</div>
                      <div className="sd-legend-item"><span className="dot iot"></span> IoT - 15%</div>
                      <div className="sd-legend-item"><span className="dot other"></span> Other - 5%</div>
                    </div>
                  </div>
                </div>

                <div className="sd-chart-card">
                  <h3>📈 Monthly Investment</h3>
                  <div className="sd-bar-chart">
                    {['Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'].map((month, i) => (
                      <div key={month} className={`sd-bar-item ${month === 'Dec' ? 'current' : ''}`}>
                        <div className="sd-bar" style={{ height: `${[45, 60, 55, 80, 70, 90][i]}%` }}>
                          <span>${[6, 8, 7.5, 11, 9, 12][i]}K</span>
                        </div>
                        <p>{month}</p>
                      </div>
                    ))}
                  </div>
                </div>
              </div>

              <div className="sd-card">
                <h3>📋 Project Performance Comparison</h3>
                <div className="sd-comparison-table">
                  <table>
                    <thead>
                      <tr>
                        <th>Project</th>
                        <th>Progress</th>
                        <th>Team Engagement</th>
                        <th>On Schedule</th>
                        <th>Risk Level</th>
                      </tr>
                    </thead>
                    <tbody>
                      {projects.filter(p => p.status === 'active').map(project => (
                        <tr key={project.id}>
                          <td><strong>{project.title}</strong></td>
                          <td>
                            <div className="sd-mini-progress">
                              <div className="sd-mini-fill" style={{ width: `${project.progress}%` }}></div>
                              <span>{project.progress}%</span>
                            </div>
                          </td>
                          <td><span className="sd-engagement high">High</span></td>
                          <td><span className="sd-schedule yes">✓ Yes</span></td>
                          <td><span className="sd-risk low">Low</span></td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}

          {/* ==================== SPONSORSHIPS TAB ==================== */}
          {activeTab === 'sponsorships' && (
            <div className="sd-sponsorships">
              <div className="sd-sponsorship-summary">
                <div className="sd-summary-card">
                  <h4>Total Investment</h4>
                  <p className="sd-big-number">${stats.totalInvested.toLocaleString()}</p>
                </div>
                <div className="sd-summary-card">
                  <h4>Active Sponsorships</h4>
                  <p className="sd-big-number">{sponsorships.filter(s => s.status === 'Active').length}</p>
                </div>
                <div className="sd-summary-card">
                  <h4>Completed</h4>
                  <p className="sd-big-number">{sponsorships.filter(s => s.status === 'Completed').length}</p>
                </div>
              </div>

              <div className="sd-card">
                <h3>💼 Sponsorship Records</h3>
                <div className="sd-table-container">
                  <table className="sd-table">
                    <thead>
                      <tr>
                        <th>Project</th>
                        <th>Type</th>
                        <th>Amount</th>
                        <th>Start Date</th>
                        <th>End Date</th>
                        <th>Status</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {sponsorships.map(s => (
                        <tr key={s.id}>
                          <td><strong>{s.project}</strong></td>
                          <td>{s.type}</td>
                          <td className="sd-amount">${s.amount.toLocaleString()}</td>
                          <td>{s.startDate}</td>
                          <td>{s.endDate}</td>
                          <td><span className={`sd-status-pill ${s.status.toLowerCase()}`}>{s.status}</span></td>
                          <td><button className="sd-btn-sm">Manage</button></td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}

          {/* ==================== SETTINGS TAB ==================== */}
          {activeTab === 'settings' && (
            <div className="sd-settings">
              <div className="sd-settings-grid">
                <div className="sd-card">
                  <h3>🏢 Company Information</h3>
                  <div className="sd-form">
                    <div className="sd-form-group">
                      <label>Company Name</label>
                      <input type="text" value={profile.companyName} readOnly />
                    </div>
                    <div className="sd-form-group">
                      <label>Industry</label>
                      <input type="text" value={profile.industry} readOnly />
                    </div>
                    <div className="sd-form-group">
                      <label>Website</label>
                      <input type="text" value={profile.website} readOnly />
                    </div>
                    <div className="sd-form-group full">
                      <label>Description</label>
                      <textarea value={profile.description} readOnly></textarea>
                    </div>
                  </div>
                </div>

                <div className="sd-card">
                  <h3>👤 Contact Information</h3>
                  <div className="sd-form">
                    <div className="sd-form-group">
                      <label>Contact Name</label>
                      <input type="text" value={profile.contactName} readOnly />
                    </div>
                    <div className="sd-form-group">
                      <label>Email</label>
                      <input type="email" value={profile.email} readOnly />
                    </div>
                    <div className="sd-form-group">
                      <label>Phone</label>
                      <input type="tel" value={profile.phone} readOnly />
                    </div>
                  </div>
                </div>

                <div className="sd-card">
                  <h3>🎯 Sponsorship Preferences</h3>
                  <div className="sd-form">
                    <div className="sd-form-group full">
                      <label>Areas of Interest</label>
                      <div className="sd-tags">
                        {profile.areasOfInterest.map((area, i) => (
                          <span key={i} className="sd-tag">{area}</span>
                        ))}
                      </div>
                    </div>
                    <div className="sd-form-group">
                      <label>Budget Range</label>
                      <input type="text" value={profile.budgetRange} readOnly />
                    </div>
                  </div>
                </div>

                <div className="sd-card">
                  <h3>🔔 Notification Preferences</h3>
                  <div className="sd-settings-list">
                    <div className="sd-setting-item">
                      <span>Email Notifications</span>
                      <label className="sd-toggle">
                        <input type="checkbox" checked={profile.notificationPreferences.email} readOnly />
                        <span className="sd-toggle-slider"></span>
                      </label>
                    </div>
                    <div className="sd-setting-item">
                      <span>Milestone Updates</span>
                      <label className="sd-toggle">
                        <input type="checkbox" checked={profile.notificationPreferences.milestones} readOnly />
                        <span className="sd-toggle-slider"></span>
                      </label>
                    </div>
                    <div className="sd-setting-item">
                      <span>Message Alerts</span>
                      <label className="sd-toggle">
                        <input type="checkbox" checked={profile.notificationPreferences.messages} readOnly />
                        <span className="sd-toggle-slider"></span>
                      </label>
                    </div>
                    <div className="sd-setting-item">
                      <span>Weekly Digest</span>
                      <label className="sd-toggle">
                        <input type="checkbox" checked={profile.notificationPreferences.weeklyDigest} readOnly />
                        <span className="sd-toggle-slider"></span>
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

        </div>
      </main>

      {/* ==================== PROJECT DETAILS MODAL ==================== */}
      {selectedProject && (
        <div className="sd-modal-overlay" onClick={() => setSelectedProject(null)}>
          <div className="sd-modal large" onClick={e => e.stopPropagation()}>
            <button className="sd-modal-close" onClick={() => setSelectedProject(null)}>×</button>
            
            <div className="sd-modal-header">
              <div>
                <h2>{selectedProject.title}</h2>
                <p>{selectedProject.team} • {selectedProject.category}</p>
              </div>
              <span className={`sd-status-badge ${selectedProject.status}`}>{selectedProject.status}</span>
            </div>

            <div className="sd-modal-body">
              <div className="sd-modal-section">
                <h4>📋 Description</h4>
                <p>{selectedProject.description}</p>
              </div>

              <div className="sd-modal-row">
                <div className="sd-modal-section">
                  <h4>👥 Team Members</h4>
                  <div className="sd-team-list">
                    {selectedProject.students.map((student, i) => (
                      <span key={i} className="sd-team-member">{student}</span>
                    ))}
                  </div>
                </div>
                <div className="sd-modal-section">
                  <h4>👨‍🏫 Mentor</h4>
                  <div className="sd-mentor-info">
                    <div className="sd-mentor-avatar">{selectedProject.mentor.avatar}</div>
                    <div>
                      <strong>{selectedProject.mentor.name}</strong>
                      <span>{selectedProject.mentor.expertise}</span>
                    </div>
                  </div>
                </div>
              </div>

              <div className="sd-modal-section">
                <h4>🎯 Milestones</h4>
                <div className="sd-milestones">
                  {selectedProject.milestones.map((milestone, i) => (
                    <div key={i} className={`sd-milestone ${milestone.status}`}>
                      <div className="sd-milestone-icon">
                        {milestone.status === 'completed' && '✅'}
                        {milestone.status === 'in-progress' && '🔄'}
                        {milestone.status === 'pending' && '⏳'}
                      </div>
                      <div className="sd-milestone-content">
                        <span className="sd-milestone-name">{milestone.name}</span>
                        <span className="sd-milestone-date">{milestone.date}</span>
                      </div>
                    </div>
                  ))}
                </div>
              </div>

              <div className="sd-modal-section">
                <h4>💰 Funding</h4>
                <div className="sd-funding-info">
                  <div className="sd-funding-stat">
                    <span className="label">Total Funding</span>
                    <span className="value">${selectedProject.funding.toLocaleString()}</span>
                  </div>
                  <div className="sd-funding-stat">
                    <span className="label">Used</span>
                    <span className="value">${selectedProject.fundingUsed.toLocaleString()}</span>
                  </div>
                  <div className="sd-funding-stat">
                    <span className="label">Remaining</span>
                    <span className="value">${(selectedProject.funding - selectedProject.fundingUsed).toLocaleString()}</span>
                  </div>
                </div>
              </div>
            </div>

            <div className="sd-modal-footer">
              <button className="sd-btn secondary" onClick={() => openChat(selectedProject.team)}>💬 Message Team</button>
              <button className="sd-btn primary">📊 View Full Report</button>
            </div>
          </div>
        </div>
      )}

      {/* ==================== REQUEST DETAILS MODAL ==================== */}
      {selectedRequest && (
        <div className="sd-modal-overlay" onClick={() => setSelectedRequest(null)}>
          <div className="sd-modal" onClick={e => e.stopPropagation()}>
            <button className="sd-modal-close" onClick={() => setSelectedRequest(null)}>×</button>
            
            <div className="sd-modal-header">
              <div>
                <h2>{selectedRequest.title}</h2>
                <p>{selectedRequest.team} • {selectedRequest.category}</p>
              </div>
              <span className={`sd-priority-badge ${selectedRequest.priority}`}>{selectedRequest.priority}</span>
            </div>

            <div className="sd-modal-body">
              <div className="sd-modal-section">
                <h4>💡 Pitch</h4>
                <p>{selectedRequest.pitch}</p>
              </div>

              <div className="sd-modal-section">
                <h4>🔴 Problem Statement</h4>
                <p>{selectedRequest.problem}</p>
              </div>

              <div className="sd-modal-section">
                <h4>💚 Proposed Solution</h4>
                <p>{selectedRequest.solution}</p>
              </div>

              <div className="sd-modal-row">
                <div className="sd-modal-section">
                  <h4>📦 Support Needed</h4>
                  <div className="sd-tags">
                    {selectedRequest.supportType.map((type, i) => (
                      <span key={i} className="sd-tag">{type}</span>
                    ))}
                  </div>
                </div>
                <div className="sd-modal-section">
                  <h4>💰 Requested Budget</h4>
                  <p className="sd-big-value">${selectedRequest.requestedBudget.toLocaleString()}</p>
                </div>
              </div>

              <div className="sd-modal-section">
                <h4>👥 Team Members</h4>
                <div className="sd-team-list">
                  {selectedRequest.students.map((student, i) => (
                    <span key={i} className="sd-team-member">{student}</span>
                  ))}
                </div>
              </div>
            </div>

            <div className="sd-modal-footer">
              <button className="sd-btn danger" onClick={() => handleRejectRequest(selectedRequest.id)}>✗ Reject</button>
              <button className="sd-btn secondary">📧 Request More Info</button>
              <button className="sd-btn success" onClick={() => handleApproveRequest(selectedRequest.id)}>✓ Approve</button>
            </div>
          </div>
        </div>
      )}

      {/* ==================== CHAT MODAL ==================== */}
      {showChat && (
        <div className="sd-modal-overlay" onClick={() => setShowChat(false)}>
          <div className="sd-modal chat-modal" onClick={e => e.stopPropagation()}>
            <div className="sd-chat-header">
              <h3>💬 Chat with {chatTarget}</h3>
              <button className="sd-modal-close" onClick={() => setShowChat(false)}>×</button>
            </div>
            <div className="sd-chat-body">
              <div className="sd-chat-message received">
                <p>Hello! Thank you for your sponsorship support!</p>
                <span className="sd-chat-time">10:30 AM</span>
              </div>
              <div className="sd-chat-message sent">
                <p>Great progress on the project! Keep up the good work.</p>
                <span className="sd-chat-time">10:35 AM</span>
              </div>
            </div>
            <div className="sd-chat-input">
              <input type="text" placeholder="Type your message..." />
              <button className="sd-btn primary">Send</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default SponsorDashboard;

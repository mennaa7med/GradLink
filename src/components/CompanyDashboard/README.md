# Company Dashboard - GradLink

A comprehensive, modern React dashboard for companies using the GradLink platform. Built with React, TailwindCSS, Framer Motion, and Recharts.

## Features

### 🏠 Dashboard Overview
- **KPI Cards**: Total Applicants, Projects, Accepted Resumes, Active This Month
- **Interactive Charts**: Monthly activity trends and project applications
- **Recent Activity Feed**: Real-time updates on company activities

### 📁 Projects Management
- **Project Table**: View all company projects with status, duration, and applicant count
- **Add Project Modal**: Create new projects with detailed requirements
- **Project Actions**: View, edit, and delete projects
- **Status Management**: Track project lifecycle (Active, Completed, Paused)

### 👥 Applicants Management
- **Applicant Cards**: Detailed applicant profiles with match percentages
- **Filter System**: Filter by status (All, Pending, Accepted, Rejected)
- **Match Scoring**: Visual match percentage with color-coded indicators
- **Quick Actions**: View resume and contact applicants directly

### 📊 Analytics & Insights
- **Multiple Chart Types**: Line charts, pie charts, bar charts, and area charts
- **Performance Metrics**: Application trends, acceptance rates, response times
- **Skills Analysis**: Most common skills among applicants
- **Project Performance**: Success rates and hiring statistics
- **AI Insights**: Automated recommendations and trend analysis

### ⚙️ Settings & Configuration
- **Company Profile**: Update company information, logo, and description
- **Security Settings**: Change password and account security
- **Notification Preferences**: Customize notification settings
- **Account Management**: Delete account with confirmation modal

## Technical Stack

- **React 18**: Modern React with hooks and functional components
- **TailwindCSS**: Utility-first CSS framework for responsive design
- **Framer Motion**: Smooth animations and transitions
- **Recharts**: Interactive charts and data visualization
- **Heroicons**: Beautiful SVG icons
- **React Router**: Client-side routing

## File Structure

```
src/components/CompanyDashboard/
├── Dashboard.jsx              # Main dashboard layout
├── Sidebar.jsx                # Navigation sidebar
├── Topbar.jsx                 # Top navigation bar
├── Content.jsx                # Dynamic content renderer
└── Pages/
    ├── DashboardOverview.jsx  # Dashboard overview page
    ├── Projects.jsx          # Projects management page
    ├── Applicants.jsx        # Applicants management page
    ├── Analytics.jsx         # Analytics and insights page
    └── Settings.jsx          # Settings and configuration page
```

## Usage

### Accessing the Dashboard
Navigate to `/company-dashboard-new` in your GradLink application to access the new company dashboard.

### Navigation
- Use the sidebar to navigate between different sections
- Click the hamburger menu to toggle sidebar visibility
- Use the topbar for notifications and profile management

### Key Interactions
- **Sidebar**: Click any menu item to switch pages
- **Projects**: Click "Add Project" to create new projects
- **Applicants**: Use filter buttons to view specific applicant statuses
- **Analytics**: Charts are interactive with hover tooltips
- **Settings**: Use tabs to navigate between different setting categories

## Customization

### Colors
The dashboard uses GradLink's brand colors:
- Primary Blue: `#0C2737`
- Dark Blue: `#012031`
- Accent Yellow: `#FFCB66`

### Animations
All animations use Framer Motion with consistent timing:
- Page transitions: 0.3s ease
- Hover effects: Scale 1.05
- Tap effects: Scale 0.95

### Responsive Design
- Mobile-first approach with TailwindCSS
- Breakpoints: sm (640px), md (768px), lg (1024px), xl (1280px)
- Sidebar collapses on mobile devices

## Future Enhancements

- **Real-time Notifications**: WebSocket integration for live updates
- **CSV Export**: Export applicant data and analytics
- **Direct Chat**: In-app messaging with applicants
- **AI Recommendations**: Machine learning insights for hiring
- **Advanced Filtering**: More sophisticated search and filter options
- **Bulk Actions**: Select multiple applicants for batch operations

## Development

### Prerequisites
- Node.js 16+
- npm or yarn
- React 18
- TailwindCSS 3+

### Installation
```bash
npm install
```

### Running the Dashboard
```bash
npm run dev
```

### Building for Production
```bash
npm run build
```

## Contributing

When contributing to the Company Dashboard:
1. Follow the existing code structure and naming conventions
2. Use TailwindCSS for styling
3. Add Framer Motion animations for smooth interactions
4. Ensure responsive design across all screen sizes
5. Test all functionality before submitting changes

## Support

For issues or questions regarding the Company Dashboard, please refer to the main GradLink documentation or contact the development team.

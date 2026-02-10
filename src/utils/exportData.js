/**
 * Export Data Utilities
 * Provides functions to export data in various formats (CSV, JSON, PDF)
 */

// Export data as CSV
export const exportToCSV = (data, filename = 'export.csv') => {
  if (!data || data.length === 0) {
    console.warn('No data to export');
    return;
  }

  // Get headers from first object
  const headers = Object.keys(data[0]);
  
  // Create CSV content
  const csvContent = [
    headers.join(','), // Header row
    ...data.map(row => 
      headers.map(header => {
        let value = row[header];
        
        // Handle null/undefined
        if (value === null || value === undefined) {
          value = '';
        }
        
        // Convert to string
        value = String(value);
        
        // Escape quotes and wrap in quotes if contains comma or newline
        if (value.includes(',') || value.includes('\n') || value.includes('"')) {
          value = `"${value.replace(/"/g, '""')}"`;
        }
        
        return value;
      }).join(',')
    )
  ].join('\n');

  // Create and download file
  downloadFile(csvContent, filename, 'text/csv;charset=utf-8;');
};

// Export data as JSON
export const exportToJSON = (data, filename = 'export.json') => {
  const jsonContent = JSON.stringify(data, null, 2);
  downloadFile(jsonContent, filename, 'application/json');
};

// Export tasks to CSV
export const exportTasksToCSV = (tasks) => {
  const formattedTasks = tasks.map(task => ({
    Title: task.title,
    Description: task.description || '',
    Status: task.status,
    Priority: task.priority || 'Normal',
    'Due Date': task.dueDate ? new Date(task.dueDate).toLocaleDateString() : '',
    'Created At': task.createdAt ? new Date(task.createdAt).toLocaleDateString() : '',
    Category: task.category || ''
  }));
  
  exportToCSV(formattedTasks, `tasks_${formatDate(new Date())}.csv`);
};

// Export applications to CSV
export const exportApplicationsToCSV = (applications) => {
  const formattedApps = applications.map(app => ({
    'Job Title': app.jobTitle || app.title,
    Company: app.companyName || app.company,
    Status: app.status,
    'Applied Date': app.appliedAt ? new Date(app.appliedAt).toLocaleDateString() : '',
    'Cover Letter': (app.coverLetter || '').substring(0, 100) + '...'
  }));
  
  exportToCSV(formattedApps, `applications_${formatDate(new Date())}.csv`);
};

// Export team members to CSV
export const exportTeamMembersToCSV = (members) => {
  const formatted = members.map(member => ({
    Name: member.name || member.fullName,
    Email: member.email,
    Role: member.role,
    Status: member.status || 'Active',
    'Joined Date': member.joinedAt ? new Date(member.joinedAt).toLocaleDateString() : ''
  }));
  
  exportToCSV(formatted, `team_members_${formatDate(new Date())}.csv`);
};

// Generate PDF from HTML element
export const exportToPDF = async (elementId, filename = 'export.pdf') => {
  try {
    // Dynamic import to avoid loading html2pdf if not needed
    const html2pdf = (await import('html2pdf.js')).default;
    
    const element = document.getElementById(elementId);
    if (!element) {
      console.error('Element not found:', elementId);
      return;
    }

    const opt = {
      margin: 10,
      filename: filename,
      image: { type: 'jpeg', quality: 0.98 },
      html2canvas: { scale: 2, useCORS: true },
      jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
    };

    await html2pdf().set(opt).from(element).save();
  } catch (error) {
    console.error('PDF export failed:', error);
    alert('PDF export requires html2pdf.js library. Please install it: npm install html2pdf.js');
  }
};

// Print-friendly PDF export (uses browser print)
export const printToPDF = (elementId) => {
  const element = document.getElementById(elementId);
  if (!element) {
    console.error('Element not found:', elementId);
    return;
  }

  // Store original body content
  const originalContent = document.body.innerHTML;
  const printContent = element.innerHTML;

  // Replace body with print content
  document.body.innerHTML = `
    <html>
      <head>
        <title>Print</title>
        <style>
          body { 
            font-family: Arial, sans-serif; 
            padding: 20px;
            color: #333;
          }
          @media print {
            body { 
              -webkit-print-color-adjust: exact;
              print-color-adjust: exact;
            }
          }
        </style>
      </head>
      <body>${printContent}</body>
    </html>
  `;

  // Print
  window.print();

  // Restore original content
  document.body.innerHTML = originalContent;
  window.location.reload();
};

// Export resume to PDF
export const exportResumeToPDF = () => {
  const resumeData = localStorage.getItem('resumeBuilderData');
  if (!resumeData) {
    alert('No resume data found. Please build your resume first.');
    return;
  }

  // Use browser print functionality
  window.print();
};

// Helper function to download file
const downloadFile = (content, filename, mimeType) => {
  const blob = new Blob([content], { type: mimeType });
  const url = URL.createObjectURL(blob);
  
  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  link.style.display = 'none';
  
  document.body.appendChild(link);
  link.click();
  
  document.body.removeChild(link);
  URL.revokeObjectURL(url);
};

// Helper function to format date
const formatDate = (date) => {
  return date.toISOString().split('T')[0];
};

// Export notifications to CSV
export const exportNotificationsToCSV = (notifications) => {
  const formatted = notifications.map(notif => ({
    Title: notif.title,
    Message: notif.message,
    Type: notif.type,
    Read: notif.isRead ? 'Yes' : 'No',
    Date: notif.createdAt ? new Date(notif.createdAt).toLocaleString() : ''
  }));
  
  exportToCSV(formatted, `notifications_${formatDate(new Date())}.csv`);
};

// Export materials progress to CSV
export const exportMaterialsProgressToCSV = (materials) => {
  const formatted = materials.map(mat => ({
    Title: mat.title,
    Type: mat.type,
    Field: mat.field,
    Progress: `${mat.progress || 0}%`,
    Status: mat.status,
    'Time Spent': mat.timeSpent ? `${mat.timeSpent} mins` : '0 mins',
    Rating: mat.rating || 'Not rated'
  }));
  
  exportToCSV(formatted, `learning_progress_${formatDate(new Date())}.csv`);
};

export default {
  exportToCSV,
  exportToJSON,
  exportToPDF,
  printToPDF,
  exportTasksToCSV,
  exportApplicationsToCSV,
  exportTeamMembersToCSV,
  exportResumeToPDF,
  exportNotificationsToCSV,
  exportMaterialsProgressToCSV
};
















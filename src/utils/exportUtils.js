/**
 * Export utilities for PDF and CSV generation
 */

/**
 * Export data to CSV format
 * @param {Array} data - Array of objects to export
 * @param {string} filename - Name of the file (without extension)
 * @param {Array} columns - Optional column configuration [{key, header}]
 */
export const exportToCSV = (data, filename = 'export', columns = null) => {
  if (!data || data.length === 0) {
    console.warn('No data to export');
    return;
  }

  // Determine columns
  const cols = columns || Object.keys(data[0]).map(key => ({ key, header: key }));
  
  // Create header row
  const headers = cols.map(col => `"${col.header}"`).join(',');
  
  // Create data rows
  const rows = data.map(item => {
    return cols.map(col => {
      let value = item[col.key];
      
      // Handle null/undefined
      if (value === null || value === undefined) {
        value = '';
      }
      
      // Handle dates
      if (value instanceof Date) {
        value = value.toLocaleDateString();
      }
      
      // Handle arrays
      if (Array.isArray(value)) {
        value = value.join('; ');
      }
      
      // Handle objects
      if (typeof value === 'object') {
        value = JSON.stringify(value);
      }
      
      // Escape quotes and wrap in quotes
      return `"${String(value).replace(/"/g, '""')}"`;
    }).join(',');
  });
  
  // Combine headers and rows
  const csv = [headers, ...rows].join('\n');
  
  // Create and trigger download
  const blob = new Blob(['\ufeff' + csv], { type: 'text/csv;charset=utf-8;' });
  downloadBlob(blob, `${filename}.csv`);
};

/**
 * Export data to PDF format using browser print
 * @param {string} title - Document title
 * @param {Array} data - Array of objects to export
 * @param {Array} columns - Column configuration [{key, header}]
 */
export const exportToPDF = (title, data, columns = null) => {
  if (!data || data.length === 0) {
    console.warn('No data to export');
    return;
  }

  const cols = columns || Object.keys(data[0]).map(key => ({ key, header: key }));
  
  // Create HTML table
  const tableHTML = `
    <!DOCTYPE html>
    <html>
    <head>
      <title>${title}</title>
      <style>
        body {
          font-family: Arial, sans-serif;
          padding: 20px;
          color: #333;
        }
        h1 {
          color: #0c2737;
          margin-bottom: 20px;
        }
        .export-date {
          color: #666;
          font-size: 12px;
          margin-bottom: 20px;
        }
        table {
          width: 100%;
          border-collapse: collapse;
          margin-top: 20px;
        }
        th {
          background: #0c2737;
          color: white;
          padding: 12px 8px;
          text-align: left;
          font-weight: 600;
        }
        td {
          padding: 10px 8px;
          border-bottom: 1px solid #ddd;
        }
        tr:nth-child(even) {
          background: #f9f9f9;
        }
        @media print {
          body { -webkit-print-color-adjust: exact; print-color-adjust: exact; }
        }
      </style>
    </head>
    <body>
      <h1>${title}</h1>
      <div class="export-date">Exported on: ${new Date().toLocaleString()}</div>
      <table>
        <thead>
          <tr>
            ${cols.map(col => `<th>${col.header}</th>`).join('')}
          </tr>
        </thead>
        <tbody>
          ${data.map(item => `
            <tr>
              ${cols.map(col => {
                let value = item[col.key];
                if (value === null || value === undefined) value = '-';
                if (value instanceof Date) value = value.toLocaleDateString();
                if (Array.isArray(value)) value = value.join(', ');
                if (typeof value === 'object') value = JSON.stringify(value);
                return `<td>${value}</td>`;
              }).join('')}
            </tr>
          `).join('')}
        </tbody>
      </table>
    </body>
    </html>
  `;
  
  // Open print window
  const printWindow = window.open('', '_blank');
  printWindow.document.write(tableHTML);
  printWindow.document.close();
  printWindow.focus();
  
  // Wait for content to load then print
  setTimeout(() => {
    printWindow.print();
  }, 250);
};

/**
 * Export tasks to formatted report
 */
export const exportTasksReport = (tasks, format = 'csv') => {
  const columns = [
    { key: 'name', header: 'Task Name' },
    { key: 'status', header: 'Status' },
    { key: 'priority', header: 'Priority' },
    { key: 'dueDate', header: 'Due Date' },
    { key: 'category', header: 'Category' },
    { key: 'assignee', header: 'Assigned To' }
  ];

  if (format === 'pdf') {
    exportToPDF('Tasks Report', tasks, columns);
  } else {
    exportToCSV(tasks, 'tasks_report', columns);
  }
};

/**
 * Export projects to formatted report
 */
export const exportProjectsReport = (projects, format = 'csv') => {
  const columns = [
    { key: 'title', header: 'Project Title' },
    { key: 'description', header: 'Description' },
    { key: 'category', header: 'Category' },
    { key: 'status', header: 'Status' },
    { key: 'createdAt', header: 'Created Date' }
  ];

  if (format === 'pdf') {
    exportToPDF('Projects Report', projects, columns);
  } else {
    exportToCSV(projects, 'projects_report', columns);
  }
};

/**
 * Export users/members report (for admin)
 */
export const exportUsersReport = (users, format = 'csv') => {
  const columns = [
    { key: 'fullName', header: 'Name' },
    { key: 'email', header: 'Email' },
    { key: 'role', header: 'Role' },
    { key: 'createdAt', header: 'Joined Date' },
    { key: 'lastLoginAt', header: 'Last Login' }
  ];

  if (format === 'pdf') {
    exportToPDF('Users Report', users, columns);
  } else {
    exportToCSV(users, 'users_report', columns);
  }
};

/**
 * Helper to download blob
 */
function downloadBlob(blob, filename) {
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
  URL.revokeObjectURL(url);
}

export default {
  exportToCSV,
  exportToPDF,
  exportTasksReport,
  exportProjectsReport,
  exportUsersReport
};
















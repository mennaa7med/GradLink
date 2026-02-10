import api from './client';

// ============== Job Applications ==============

// Apply for a job
export async function applyForJob(applicationData) {
  const { data } = await api.post('/api/applications/jobs', applicationData);
  return data;
}

// Get applications for a specific job (company view)
export async function getJobApplications(jobId) {
  const { data } = await api.get(`/api/applications/jobs/${jobId}`);
  return data;
}

// Get all applications for company's jobs
export async function getAllCompanyJobApplications() {
  const { data } = await api.get('/api/applications/jobs/company/all');
  return data;
}

// Update job application status
export async function updateJobApplicationStatus(applicationId, status, notes = null) {
  const { data } = await api.put(`/api/applications/jobs/${applicationId}/status`, { status, notes });
  return data;
}

// Check if user has applied for a job
export async function hasAppliedForJob(jobId) {
  const { data } = await api.get(`/api/applications/jobs/${jobId}/check`);
  return data;
}

// ============== Internship Applications ==============

// Apply for an internship
export async function applyForInternship(applicationData) {
  const { data } = await api.post('/api/applications/internships', applicationData);
  return data;
}

// Get all applications for company's internships
export async function getAllCompanyInternshipApplications() {
  const { data } = await api.get('/api/applications/internships/company/all');
  return data;
}

// Update internship application status
export async function updateInternshipApplicationStatus(applicationId, status, notes = null) {
  const { data } = await api.put(`/api/applications/internships/${applicationId}/status`, { status, notes });
  return data;
}

// Check if user has applied for an internship
export async function hasAppliedForInternship(internshipId) {
  const { data } = await api.get(`/api/applications/internships/${internshipId}/check`);
  return data;
}

// ============== My Applications (Student View) ==============

// Get all my applications
export async function getMyApplications() {
  const { data } = await api.get('/api/applications/my');
  return data;
}


















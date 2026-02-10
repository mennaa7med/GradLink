import api from './client';

// Get all sponsors
export async function listSponsors(params = {}) {
  const { data } = await api.get('/api/sponsors', { params });
  return data;
}

// Get sponsor by ID
export async function getSponsor(id) {
  const { data } = await api.get(`/api/sponsors/${id}`);
  return data;
}

// Create sponsor (Admin only)
export async function createSponsor(sponsorData) {
  const { data } = await api.post('/api/sponsors', sponsorData);
  return data;
}

// Update sponsor (Admin only)
export async function updateSponsor(id, sponsorData) {
  const { data } = await api.put(`/api/sponsors/${id}`, sponsorData);
  return data;
}

// Delete sponsor (Admin only)
export async function deleteSponsor(id) {
  await api.delete(`/api/sponsors/${id}`);
}

// Get all sponsor applications (Admin only)
export async function listApplications(params = {}) {
  const { data } = await api.get('/api/sponsors/applications', { params });
  return data;
}

// Get application by ID (Admin only)
export async function getApplication(id) {
  const { data } = await api.get(`/api/sponsors/applications/${id}`);
  return data;
}

// Submit sponsor application (Public)
export async function submitApplication(applicationData) {
  const { data } = await api.post('/api/sponsors/applications', applicationData);
  return data;
}

// Update application (Admin only)
export async function updateApplication(id, applicationData) {
  const { data } = await api.put(`/api/sponsors/applications/${id}`, applicationData);
  return data;
}

// Approve application (Admin only)
export async function approveApplication(id, createSponsor = true) {
  const { data } = await api.post(`/api/sponsors/applications/${id}/approve`, null, {
    params: { createSponsor }
  });
  return data;
}

// Reject application (Admin only)
export async function rejectApplication(id) {
  const { data } = await api.post(`/api/sponsors/applications/${id}/reject`);
  return data;
}

// Delete application (Admin only)
export async function deleteApplication(id) {
  await api.delete(`/api/sponsors/applications/${id}`);
}

// Get my applications (Authenticated users)
export async function getMyApplications() {
  const { data } = await api.get('/api/sponsors/applications/me');
  return data;
}

// Get sponsors statistics
export async function getSponsorsStats() {
  const { data } = await api.get('/api/sponsors/stats');
  return data;
}


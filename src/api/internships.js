import api from './client';

// Get all active internships (public)
export async function listInternships(params = {}) {
  const { data } = await api.get('/api/internships', { params });
  return data.internships || data;
}

// Get my internships (company)
export async function listMyInternships() {
  const { data } = await api.get('/api/internships/my');
  return data;
}

// Get single internship
export async function getInternship(id) {
  const { data } = await api.get(`/api/internships/${id}`);
  return data;
}

// Create internship
export async function createInternship(internshipData) {
  const { data } = await api.post('/api/internships', internshipData);
  return data;
}

// Update internship
export async function updateInternship(id, internshipData) {
  const { data } = await api.put(`/api/internships/${id}`, internshipData);
  return data;
}

// Delete internship
export async function deleteInternship(id) {
  await api.delete(`/api/internships/${id}`);
}

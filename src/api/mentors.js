import api from './client';

// Get all mentors
export async function listMentors() {
  const { data } = await api.get('/api/mentors');
  return data;
}

// Get mentor by ID
export async function getMentor(id) {
  const { data } = await api.get(`/api/mentors/${id}`);
  return data;
}

// Get mentors by specialization
export async function getMentorsBySpecialization(specialization) {
  const { data } = await api.get(`/api/mentors/specialization/${encodeURIComponent(specialization)}`);
  return data;
}

// Search mentors
export async function searchMentors(query) {
  const { data } = await api.get('/api/mentors/search', { params: { query } });
  return data;
}

// Create mentor (Admin only)
export async function createMentor(mentorData) {
  const { data } = await api.post('/api/mentors', mentorData);
  return data;
}

// Apply as mentor (Authenticated users)
export async function applyAsMentor(mentorData) {
  const { data } = await api.post('/api/mentors/apply', mentorData);
  return data;
}

// Update mentor
export async function updateMentor(id, mentorData) {
  const { data } = await api.put(`/api/mentors/${id}`, mentorData);
  return data;
}

// Delete mentor (Admin only)
export async function deleteMentor(id) {
  await api.delete(`/api/mentors/${id}`);
}


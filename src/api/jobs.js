import { api } from './client';

// Get all active jobs
export async function listJobs() {
  const res = await api.get('/api/jobs');
  return res.data;
}

// Get my jobs (company's jobs)
export async function listMyJobs() {
  const res = await api.get('/api/jobs/my');
  return res.data;
}

// Get single job
export async function getJob(id) {
  const res = await api.get(`/api/jobs/${id}`);
  return res.data;
}

// Create job
export async function createJob(data) {
  const res = await api.post('/api/jobs', data);
  return res.data;
}

// Update job
export async function updateJob(id, data) {
  const res = await api.put(`/api/jobs/${id}`, data);
  return res.data;
}

// Delete job
export async function deleteJob(id) {
  await api.delete(`/api/jobs/${id}`);
}















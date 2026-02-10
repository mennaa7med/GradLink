import api from './client';

// ==================== Overview ====================
export async function getOverview() {
  const { data } = await api.get('/api/sponsor-dashboard/overview');
  return data;
}

// ==================== Profile ====================
export async function getProfile() {
  const { data } = await api.get('/api/sponsor-dashboard/profile');
  return data;
}

export async function updateProfile(profileData) {
  const { data } = await api.put('/api/sponsor-dashboard/profile', profileData);
  return data;
}

// ==================== Projects ====================
export async function getProjects(filters = {}) {
  const params = new URLSearchParams();
  if (filters.status) params.append('status', filters.status);
  if (filters.category) params.append('category', filters.category);
  
  const { data } = await api.get('/api/sponsor-dashboard/projects', { params });
  return data;
}

export async function getProject(id) {
  const { data } = await api.get(`/api/sponsor-dashboard/projects/${id}`);
  return data;
}

export async function approveRejectProject(requestData) {
  const { data } = await api.post('/api/sponsor-dashboard/projects/approve-reject', requestData);
  return data;
}

export async function updateProjectProgress(id, progressData) {
  const { data } = await api.put(`/api/sponsor-dashboard/projects/${id}/progress`, progressData);
  return data;
}

// ==================== Funding ====================
export async function getFundingHistory() {
  const { data } = await api.get('/api/sponsor-dashboard/funding');
  return data;
}

export async function addFunding(fundingData) {
  const { data } = await api.post('/api/sponsor-dashboard/funding', fundingData);
  return data;
}

// ==================== Messages ====================
export async function getMessages(projectId = null) {
  const params = new URLSearchParams();
  if (projectId) params.append('projectId', projectId);
  
  const { data } = await api.get('/api/sponsor-dashboard/messages', { params });
  return data;
}

export async function sendMessage(messageData) {
  const { data } = await api.post('/api/sponsor-dashboard/messages', messageData);
  return data;
}

export async function markMessageAsRead(messageId) {
  await api.put(`/api/sponsor-dashboard/messages/${messageId}/read`);
}

// ==================== Available Projects ====================
export async function getAvailableProjects(filters = {}) {
  const params = new URLSearchParams();
  if (filters.category) params.append('category', filters.category);
  if (filters.search) params.append('search', filters.search);
  
  const { data } = await api.get('/api/sponsor-dashboard/available-projects', { params });
  return data;
}

export async function sponsorProject(projectData) {
  const { data } = await api.post('/api/sponsor-dashboard/sponsor-project', projectData);
  return data;
}

// ==================== Registration ====================
export async function registerSponsor(registrationData) {
  const { data } = await api.post('/api/auth/register/sponsor', registrationData);
  return data;
}


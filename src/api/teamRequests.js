import api from './client';

// Get all team requests (with optional filters)
export const getTeamRequests = async (params = {}) => {
  const response = await api.get('/api/TeamRequests', { params });
  return response.data;
};

// Get a single team request by ID
export const getTeamRequest = async (id) => {
  const response = await api.get(`/api/TeamRequests/${id}`);
  return response.data;
};

// Get my team requests (requires auth)
export const getMyTeamRequests = async () => {
  const response = await api.get('/api/TeamRequests/my');
  return response.data;
};

// Create a new team request (requires auth)
export const createTeamRequest = async (data) => {
  const response = await api.post('/api/TeamRequests', data);
  return response.data;
};

// Update a team request (requires auth)
export const updateTeamRequest = async (id, data) => {
  const response = await api.put(`/api/TeamRequests/${id}`, data);
  return response.data;
};

// Delete a team request (requires auth)
export const deleteTeamRequest = async (id) => {
  const response = await api.delete(`/api/TeamRequests/${id}`);
  return response.data;
};

// Send a join request (requires auth)
export const sendJoinRequest = async (teamRequestId, data) => {
  const response = await api.post(`/api/TeamRequests/${teamRequestId}/join`, data);
  return response.data;
};

// Get join requests for a team request (owner only)
export const getJoinRequests = async (teamRequestId) => {
  const response = await api.get(`/api/TeamRequests/${teamRequestId}/join-requests`);
  return response.data;
};

// Update join request status (owner only)
export const updateJoinRequestStatus = async (joinRequestId, status) => {
  const response = await api.put(`/api/TeamRequests/join-requests/${joinRequestId}/status`, { status });
  return response.data;
};

// Get stats
export const getStats = async () => {
  const response = await api.get('/api/TeamRequests/stats');
  return response.data;
};

export default {
  getTeamRequests,
  getTeamRequest,
  getMyTeamRequests,
  createTeamRequest,
  updateTeamRequest,
  deleteTeamRequest,
  sendJoinRequest,
  getJoinRequests,
  updateJoinRequestStatus,
  getStats
};







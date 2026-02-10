import api from './client';

// Get all tasks
export const listTasks = async (filters = {}) => {
  const params = new URLSearchParams();
  if (filters.status) params.append('status', filters.status);
  if (filters.category) params.append('category', filters.category);
  if (filters.priority) params.append('priority', filters.priority);
  if (filters.person) params.append('person', filters.person);
  if (filters.search) params.append('search', filters.search);
  
  const queryString = params.toString();
  const url = queryString ? `/api/tasks?${queryString}` : '/api/tasks';
  const { data } = await api.get(url);
  return data;
};

// Get a single task
export const getTask = async (id) => {
  const { data } = await api.get(`/api/tasks/${id}`);
  return data;
};

// Create a new task
export const createTask = async (taskData) => {
  const { data } = await api.post('/api/tasks', taskData);
  return data;
};

// Update a task
export const updateTask = async (id, taskData) => {
  await api.put(`/api/tasks/${id}`, taskData);
};

// Delete a task
export const deleteTask = async (id) => {
  await api.delete(`/api/tasks/${id}`);
};

// Subtasks
export const createSubtask = async (taskId, name) => {
  const { data } = await api.post(`/api/tasks/${taskId}/subtasks`, { name });
  return data;
};

export const updateSubtask = async (taskId, subtaskId, updates) => {
  await api.put(`/api/tasks/${taskId}/subtasks/${subtaskId}`, updates);
};

export const deleteSubtask = async (taskId, subtaskId) => {
  await api.delete(`/api/tasks/${taskId}/subtasks/${subtaskId}`);
};

// Stats and helpers
export const getTaskStats = async () => {
  const { data } = await api.get('/api/tasks/stats');
  return data;
};

export const getPeople = async () => {
  const { data } = await api.get('/api/tasks/people');
  return data;
};

// Task Attachments
export const getTaskAttachments = async (taskId) => {
  const { data } = await api.get(`/api/tasks/${taskId}/attachments`);
  return data;
};

export const uploadTaskAttachment = async (taskId, file, description = '') => {
  const formData = new FormData();
  formData.append('file', file);
  if (description) {
    formData.append('description', description);
  }
  const { data } = await api.post(`/api/tasks/${taskId}/attachments`, formData, {
    headers: { 'Content-Type': 'multipart/form-data' }
  });
  return data;
};

export const deleteTaskAttachment = async (taskId, attachmentId) => {
  await api.delete(`/api/tasks/${taskId}/attachments/${attachmentId}`);
};

export const getAttachmentDownloadUrl = (taskId, attachmentId) => {
  const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5080';
  return `${baseUrl}/api/tasks/${taskId}/attachments/${attachmentId}/download`;
};


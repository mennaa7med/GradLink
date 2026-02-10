import api from './client';

export async function listProjects() {
  const { data } = await api.get('/api/projects');
  return data;
}

export async function createProject(payload) {
  const { data } = await api.post('/api/projects', payload);
  return data;
}

export async function updateProject(id, payload) {
  const { data } = await api.put(`/api/projects/${id}`, payload);
  return data;
}

export async function deleteProject(id) {
  const { data } = await api.delete(`/api/projects/${id}`);
  return data;
}

export async function listTasks(projectId) {
  try {
    const { data } = await api.get(`/api/projects/${projectId}/tasks`);
    return data;
  } catch (e) {
    if (e.response?.status === 404) return [];
    throw e;
  }
}

export async function createTask(projectId, payload) {
  const { data } = await api.post(`/api/projects/${projectId}/tasks`, payload);
  return data;
}

export async function updateTask(projectId, taskId, payload) {
  const { data } = await api.put(`/api/projects/${projectId}/tasks/${taskId}`, payload);
  return data;
}

export async function deleteTask(projectId, taskId) {
  const { data } = await api.delete(`/api/projects/${projectId}/tasks/${taskId}`);
  return data;
}


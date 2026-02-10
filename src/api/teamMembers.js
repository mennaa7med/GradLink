import api from './client';

// Get team members for a project
export async function listTeamMembers(projectId) {
  const { data } = await api.get(`/api/teammembers/project/${projectId}`);
  return data;
}

// Get team member by ID
export async function getTeamMember(id) {
  const { data } = await api.get(`/api/teammembers/${id}`);
  return data;
}

// Create team member
export async function createTeamMember(memberData) {
  const { data } = await api.post('/api/teammembers', memberData);
  return data;
}

// Update team member
export async function updateTeamMember(id, memberData) {
  const { data } = await api.put(`/api/teammembers/${id}`, memberData);
  return data;
}

// Delete team member
export async function deleteTeamMember(id) {
  await api.delete(`/api/teammembers/${id}`);
}

// Search team members in a project
export async function searchTeamMembers(projectId, query) {
  const { data } = await api.get(`/api/teammembers/project/${projectId}/search`, {
    params: { query }
  });
  return data;
}

// Get team stats for a project
export async function getTeamStats(projectId) {
  const { data } = await api.get(`/api/teammembers/project/${projectId}/stats`);
  return data;
}

// Get tasks for a team member
export async function getMemberTasks(memberId) {
  const { data } = await api.get(`/api/teammembers/${memberId}/tasks`);
  return data;
}

// Create task for team member
export async function createMemberTask(taskData) {
  const { data } = await api.post('/api/teammembers/tasks', taskData);
  return data;
}

// Update team member task
export async function updateMemberTask(taskId, taskData) {
  const { data } = await api.put(`/api/teammembers/tasks/${taskId}`, taskData);
  return data;
}

// Delete team member task
export async function deleteMemberTask(taskId) {
  await api.delete(`/api/teammembers/tasks/${taskId}`);
}


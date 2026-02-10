import api from './client';

// Get admin dashboard stats
export async function getAdminStats() {
  const { data } = await api.get('/api/admin/stats');
  return data;
}

// Get all users with pagination
export async function listUsers(page = 1, pageSize = 20) {
  const { data } = await api.get('/api/admin/users', {
    params: { page, pageSize }
  });
  return data;
}


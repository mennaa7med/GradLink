import api from './client';

// Get user by ID
export async function getUser(id) {
  const { data } = await api.get(`/api/users/${id}`);
  return data;
}

// Get current user profile
export async function getCurrentUser() {
  const { data } = await api.get('/api/users/me');
  return data;
}

// Update current user profile
export async function updateProfile(profileData) {
  const { data } = await api.put('/api/users/me', profileData);
  return data;
}

// Upload avatar
export async function uploadAvatar(file) {
  const formData = new FormData();
  formData.append('file', file);
  
  const { data } = await api.post('/api/users/me/avatar', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
  return data;
}

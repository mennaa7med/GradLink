import api from './client';

// Get all user's resumes
export async function listResumes() {
  const { data } = await api.get('/api/resumes');
  return data;
}

// Get resume by ID
export async function getResume(id) {
  const { data } = await api.get(`/api/resumes/${id}`);
  return data;
}

// Upload resume
export async function uploadResume(file) {
  const formData = new FormData();
  formData.append('file', file);
  
  const { data } = await api.post('/api/resumes/upload', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
  return data;
}

// Delete resume
export async function deleteResume(id) {
  await api.delete(`/api/resumes/${id}`);
}


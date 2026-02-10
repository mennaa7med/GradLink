import api from './client';

// Get all user's conversations
export async function listConversations() {
  const { data } = await api.get('/api/conversations');
  return data;
}

// Get conversation by ID
export async function getConversation(id) {
  const { data } = await api.get(`/api/conversations/${id}`);
  return data;
}

// Create a new conversation
export async function createConversation(recipientId) {
  const { data } = await api.post('/api/conversations', { recipientId });
  return data;
}


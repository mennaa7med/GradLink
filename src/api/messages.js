import api from './client';

// Get unread messages count
export async function getUnreadMessagesCount() {
  const { data } = await api.get('/api/conversations/unread-count');
  return data;
}

// Get recent unread messages
export async function getUnreadMessages(limit = 5) {
  const { data } = await api.get('/api/conversations/unread-messages', {
    params: { limit }
  });
  return data;
}

// Get all conversations
export async function getConversations() {
  const { data } = await api.get('/api/conversations');
  return data;
}

// Get single conversation with messages
export async function getConversation(id) {
  const { data } = await api.get(`/api/conversations/${id}`);
  return data;
}

// Create or get existing conversation
export async function createOrGetConversation(recipientId) {
  const { data } = await api.post('/api/conversations', { recipientId });
  return data;
}
















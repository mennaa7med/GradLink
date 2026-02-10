import api from './client';

// Test chat connection
export async function testChat() {
  const { data } = await api.get('/api/chatproxy/test');
  return data;
}

// Send chat message to AI
export async function sendChatMessage(message) {
  const { data } = await api.post('/api/chatproxy/chat', { message });
  return data;
}


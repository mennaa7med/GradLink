import api from './client';

// Get all notifications
export async function getNotifications(params = {}) {
  const { data } = await api.get('/api/notifications', { params });
  return data;
}

// Get notification count
export async function getNotificationCount() {
  const { data } = await api.get('/api/notifications/count');
  return data;
}

// Mark notification as read
export async function markAsRead(notificationId) {
  await api.put(`/api/notifications/${notificationId}/read`);
}

// Mark all notifications as read
export async function markAllAsRead() {
  await api.put('/api/notifications/read-all');
}

// Delete notification
export async function deleteNotification(notificationId) {
  await api.delete(`/api/notifications/${notificationId}`);
}

// Clear all read notifications
export async function clearReadNotifications() {
  await api.delete('/api/notifications/clear-read');
}


















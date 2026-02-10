import api from './client';

// Login
export async function login(email, password) {
  const { data } = await api.post('/api/auth/login', { email, password });
  return data;
}

// Register
export async function register(userData) {
  const { data } = await api.post('/api/auth/register', userData);
  return data;
}

// Logout
export async function logout() {
  await api.post('/api/auth/logout');
}

// Refresh token
export async function refreshToken(refreshToken) {
  const { data } = await api.post('/api/auth/refresh', { refreshToken });
  return data;
}

// Forgot password - request reset
export async function forgotPassword(email) {
  const { data } = await api.post('/api/auth/forgot-password', { email });
  return data;
}

// Reset password with token
export async function resetPassword(token, newPassword) {
  const { data } = await api.post('/api/auth/reset-password', { token, newPassword });
  return data;
}

// Verify reset token
export async function verifyResetToken(token) {
  const { data } = await api.get('/api/auth/verify-reset-token', { params: { token } });
  return data;
}

// Resend email verification
export async function resendVerification(email) {
  const { data } = await api.post('/api/auth/resend-verification', { email });
  return data;
}

// Register sponsor
export async function registerSponsor(sponsorData) {
  const { data } = await api.post('/api/auth/register/sponsor', sponsorData);
  return data;
}

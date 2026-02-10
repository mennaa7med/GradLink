import api from './client';

/**
 * Get OAuth provider configuration
 */
export const getOAuthProviders = async () => {
  const { data } = await api.get('/api/oauth/providers');
  return data;
};

/**
 * Login with social provider (using access token from OAuth callback)
 */
export const socialLogin = async (provider, accessToken) => {
  const { data } = await api.post('/api/oauth/social-login', {
    provider,
    accessToken
  });
  return data;
};

/**
 * Exchange authorization code for tokens
 */
export const exchangeCode = async (provider, code, redirectUri) => {
  const { data } = await api.post('/api/oauth/exchange-code', {
    provider,
    code,
    redirectUri
  });
  return data;
};

/**
 * Initiate Google OAuth flow
 */
export const initiateGoogleLogin = (clientId, redirectUri) => {
  const params = new URLSearchParams({
    client_id: clientId,
    redirect_uri: redirectUri,
    response_type: 'code',
    scope: 'openid email profile',
    access_type: 'offline',
    prompt: 'consent'
  });
  
  window.location.href = `https://accounts.google.com/o/oauth2/v2/auth?${params}`;
};

/**
 * Initiate GitHub OAuth flow
 */
export const initiateGitHubLogin = (clientId, redirectUri) => {
  const params = new URLSearchParams({
    client_id: clientId,
    redirect_uri: redirectUri,
    scope: 'user:email'
  });
  
  window.location.href = `https://github.com/login/oauth/authorize?${params}`;
};
















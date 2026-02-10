export function parseJwt(token) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  } catch {
    return null;
  }
}

export function getUserIdFromToken() {
  const access = localStorage.getItem('accessToken');
  if (!access) return null;
  const payload = parseJwt(access);
  // prefer NameIdentifier or sub (Guid)
  return payload?.nameid || payload?.sub || null;
}



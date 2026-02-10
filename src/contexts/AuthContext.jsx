import React, { createContext, useContext, useEffect, useMemo, useState } from 'react';
import api from '../api/client';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const access = localStorage.getItem('accessToken');
    if (access) {
      // Fetch current user profile
      api.get('/api/users/me')
        .then(({ data }) => {
          setUser(data);
          localStorage.setItem('userRole', data.role || 'Student');
        })
        .catch(() => {
          // Token invalid, clear storage
          localStorage.removeItem('accessToken');
          localStorage.removeItem('refreshToken');
          setUser(null);
        })
        .finally(() => setLoading(false));
    } else {
      setLoading(false);
    }
  }, []);

  const login = async (email, password) => {
    const { data } = await api.post('/api/auth/login', { email, password });
    localStorage.setItem('accessToken', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    
    // Save user info from login response
    if (data.user) {
      setUser(data.user);
      localStorage.setItem('userId', data.user.id);
      localStorage.setItem('userName', data.user.fullName);
      localStorage.setItem('userRole', data.user.roles?.[0] || 'Student');
      
      // Save all roles
      if (data.user.roles) {
        localStorage.setItem('userRoles', JSON.stringify(data.user.roles));
      }
      
      // Save company name if user is a company
      if (data.user.companyName) {
        localStorage.setItem('companyName', data.user.companyName);
      }
    }
    
    // Return the response so caller can access user info
    return data;
  };

  // Login with OAuth tokens (for Google, GitHub, etc.)
  const loginWithToken = (accessToken, refreshToken, userData) => {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    
    if (userData) {
      setUser(userData);
      localStorage.setItem('user', JSON.stringify(userData));
      localStorage.setItem('userId', userData.id);
      localStorage.setItem('userName', userData.fullName);
      localStorage.setItem('userRole', userData.roles?.[0] || 'Student');
      
      if (userData.roles) {
        localStorage.setItem('userRoles', JSON.stringify(userData.roles));
      }
    }
  };

  const register = async (email, password, fullName, phoneNumber = '', role = 'Student', companyName = '', extendedData = {}) => {
    // Build registration payload with all fields
    const registrationData = {
      email,
      password,
      fullName,
      phoneNumber,
      role,
      companyName,
      // Spread extended data (includes mentor-specific, student-specific fields)
      ...extendedData
    };

    // Remove null/undefined values to keep payload clean
    const cleanedData = Object.fromEntries(
      Object.entries(registrationData).filter(([_, v]) => v != null && v !== '')
    );

    const { data } = await api.post('/api/auth/register', cleanedData);
    
    // Return the response - registration now requires OTP verification
    // User will be redirected to verify-otp page
    return data;
  };

  const logout = async () => {
    try { await api.post('/api/auth/logout'); } catch {}
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userRole');
    setUser(null);
  };

  const value = useMemo(() => ({ user, login, loginWithToken, register, logout, loading }), [user, loading]);

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}



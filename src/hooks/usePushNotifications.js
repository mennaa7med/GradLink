import { useState, useEffect, useCallback } from 'react';
import api from '../api/client';

/**
 * Custom hook for managing push notifications
 */
export const usePushNotifications = () => {
  const [permission, setPermission] = useState('default');
  const [isSupported, setIsSupported] = useState(false);
  const [isSubscribed, setIsSubscribed] = useState(false);
  const [subscription, setSubscription] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Check browser support
  useEffect(() => {
    const supported = 'serviceWorker' in navigator && 'PushManager' in window;
    setIsSupported(supported);
    
    if (supported) {
      setPermission(Notification.permission);
    }
  }, []);

  // Check existing subscription
  useEffect(() => {
    const checkSubscription = async () => {
      if (!isSupported) return;

      try {
        const registration = await navigator.serviceWorker.ready;
        const existingSub = await registration.pushManager.getSubscription();
        
        if (existingSub) {
          setSubscription(existingSub);
          setIsSubscribed(true);
        }
      } catch (err) {
        console.error('Failed to check subscription:', err);
      }
    };

    checkSubscription();
  }, [isSupported]);

  // Register service worker
  const registerServiceWorker = useCallback(async () => {
    if (!isSupported) return null;

    try {
      const registration = await navigator.serviceWorker.register('/sw.js', {
        scope: '/'
      });
      console.log('Service Worker registered:', registration);
      return registration;
    } catch (err) {
      console.error('Service Worker registration failed:', err);
      throw err;
    }
  }, [isSupported]);

  // Request notification permission
  const requestPermission = useCallback(async () => {
    if (!isSupported) {
      setError('Push notifications are not supported in this browser');
      return false;
    }

    try {
      const result = await Notification.requestPermission();
      setPermission(result);
      return result === 'granted';
    } catch (err) {
      setError('Failed to request notification permission');
      return false;
    }
  }, [isSupported]);

  // Subscribe to push notifications
  const subscribe = useCallback(async () => {
    if (!isSupported) {
      setError('Push notifications are not supported');
      return false;
    }

    setLoading(true);
    setError(null);

    try {
      // Request permission first
      const hasPermission = await requestPermission();
      if (!hasPermission) {
        setError('Notification permission denied');
        setLoading(false);
        return false;
      }

      // Register service worker
      const registration = await registerServiceWorker();
      if (!registration) {
        throw new Error('Failed to register service worker');
      }

      // Wait for service worker to be ready
      await navigator.serviceWorker.ready;

      // Get VAPID public key from backend
      const { data: config } = await api.get('/api/notifications/push/config');
      
      if (!config.vapidPublicKey) {
        // Push notifications not configured on backend
        console.log('VAPID key not configured - skipping push subscription');
        setLoading(false);
        return false;
      }

      // Convert VAPID key
      const applicationServerKey = urlBase64ToUint8Array(config.vapidPublicKey);

      // Subscribe
      const pushSubscription = await registration.pushManager.subscribe({
        userVisibleOnly: true,
        applicationServerKey
      });

      // Save subscription to backend
      await api.post('/api/notifications/push/subscribe', {
        endpoint: pushSubscription.endpoint,
        keys: {
          p256dh: btoa(String.fromCharCode.apply(null, new Uint8Array(pushSubscription.getKey('p256dh')))),
          auth: btoa(String.fromCharCode.apply(null, new Uint8Array(pushSubscription.getKey('auth'))))
        }
      });

      setSubscription(pushSubscription);
      setIsSubscribed(true);
      setLoading(false);
      
      console.log('Push subscription successful');
      return true;
    } catch (err) {
      console.error('Push subscription failed:', err);
      setError(err.message || 'Failed to subscribe to push notifications');
      setLoading(false);
      return false;
    }
  }, [isSupported, requestPermission, registerServiceWorker]);

  // Unsubscribe from push notifications
  const unsubscribe = useCallback(async () => {
    if (!subscription) return false;

    setLoading(true);
    setError(null);

    try {
      await subscription.unsubscribe();
      
      // Notify backend
      await api.post('/api/notifications/push/unsubscribe', {
        endpoint: subscription.endpoint
      });

      setSubscription(null);
      setIsSubscribed(false);
      setLoading(false);
      
      console.log('Push unsubscription successful');
      return true;
    } catch (err) {
      console.error('Push unsubscription failed:', err);
      setError(err.message || 'Failed to unsubscribe from push notifications');
      setLoading(false);
      return false;
    }
  }, [subscription]);

  // Send test notification (for debugging)
  const sendTestNotification = useCallback(async () => {
    try {
      await api.post('/api/notifications/push/test');
      return true;
    } catch (err) {
      console.error('Failed to send test notification:', err);
      return false;
    }
  }, []);

  return {
    isSupported,
    permission,
    isSubscribed,
    loading,
    error,
    subscribe,
    unsubscribe,
    requestPermission,
    sendTestNotification
  };
};

// Helper function to convert VAPID key
function urlBase64ToUint8Array(base64String) {
  const padding = '='.repeat((4 - base64String.length % 4) % 4);
  const base64 = (base64String + padding)
    .replace(/-/g, '+')
    .replace(/_/g, '/');

  const rawData = window.atob(base64);
  const outputArray = new Uint8Array(rawData.length);

  for (let i = 0; i < rawData.length; ++i) {
    outputArray[i] = rawData.charCodeAt(i);
  }
  return outputArray;
}

export default usePushNotifications;
















import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import './Logout.css';

const Logout = () => {
  const navigate = useNavigate();
  const [animateOut, setAnimateOut] = useState(false);

  useEffect(() => {
    setAnimateOut(true); // شغّل الانيميشن

    const timer = setTimeout(() => {
      // امسح بيانات المستخدم (اختياري)
      localStorage.clear();
      sessionStorage.clear();

      // انتقل إلى صفحة signin وأعد تحميلها
      navigate('/signin');
      window.location.reload(); // Reload بعد التوجيه
    }, 1500); // مدة الأنيميشن

    return () => clearTimeout(timer);
  }, [navigate]);

  return (
    <div className={`logout-container ${animateOut ? 'fade-out' : ''}`}>
      <div className="logout-box">
        <h2>You’ve successfully logged out</h2>
        <p className="thank-msg">
          Thanks for using <span className="brand">GradLink</span>! We hope to see you again soon.
        </p>
        <p className="redirect-msg">Redirecting...</p>
      </div>
    </div>
  );
};

export default Logout;

import React from 'react';
import { useLocation, Link } from 'react-router-dom';

// Everything here is inactive unless the build sets VITE_DEMO_MODE=true.
export const DEMO_MODE = import.meta.env.VITE_DEMO_MODE === 'true';

const BLOCKED_MESSAGE = 'Disabled in the read-only demo.';
const WRITE_LABEL =
  /^\s*[+\u2795]?\s*(new|add|edit|delete|remove|create|apply|upload|send|save|submit|post|invite|join|export|publish|update|register|sign\s?up|reply|rate|follow|book|pay|analy[sz]e|screen|match)\b/i;

function toast(text) {
  let el = document.getElementById('demo-toast');
  if (!el) {
    el = document.createElement('div');
    el.id = 'demo-toast';
    el.setAttribute('role', 'alert');
    el.style.cssText =
      'position:fixed;left:50%;bottom:48px;transform:translateX(-50%);z-index:100000;background:#212529;color:#fff;padding:10px 16px;border-radius:8px;font-size:14px;box-shadow:0 4px 16px rgba(0,0,0,.3);max-width:90vw;text-align:center';
    document.body.appendChild(el);
  }
  el.textContent = text;
  el.style.display = 'block';
  clearTimeout(el._t);
  el._t = setTimeout(() => (el.style.display = 'none'), 3500);
}

export function showDemoBlocked() {
  toast(BLOCKED_MESSAGE);
}

// Capture-phase guards: stop write-looking buttons and all form submits before the app handles them.
export function installDemoGuards() {
  if (!DEMO_MODE || typeof document === 'undefined' || window.__demoGuards) return;
  window.__demoGuards = true;
  document.addEventListener(
    'click',
    (e) => {
      const el = e.target.closest && e.target.closest('button, a, [role="button"], input[type="submit"]');
      if (!el || el.closest('[data-demo-allow]')) return;
      const label = (el.getAttribute('aria-label') || el.textContent || el.value || '').trim();
      const isDeleteOrEdit = el.className && /(delete|edit|remove)/i.test(String(el.className));
      if (WRITE_LABEL.test(label) || isDeleteOrEdit || (el.type === 'submit' && el.form)) {
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
        toast(BLOCKED_MESSAGE);
      }
    },
    true
  );
  document.addEventListener(
    'submit',
    (e) => {
      if (e.target.closest && e.target.closest('[data-demo-allow]')) return;
      e.preventDefault();
      e.stopPropagation();
      toast(BLOCKED_MESSAGE);
    },
    true
  );
}

export function DemoBanner() {
  const { pathname } = useLocation();
  const samples = /^\/(opportunities|career)/i.test(pathname);
  if (!DEMO_MODE) return null;
  return (
    <div
      role="status"
      style={{ position: 'fixed', bottom: 0, left: 0, right: 0, zIndex: 99999, background: '#fff3cd', color: '#664d03', textAlign: 'center', padding: '6px 12px', fontSize: 13, borderTop: '1px solid #ffe69c' }}
    >
      Read-only demo with sample data. Saving, uploading, messaging and sign-up are disabled.
      {samples && ' Listings on this page are fictional examples, not open positions.'}
    </div>
  );
}

// Replaces pages that need writes, uploads, CV processing or chat.
export function DemoUnavailable({ children }) {
  if (!DEMO_MODE) return children;
  return (
    <div style={{ maxWidth: 560, margin: '15vh auto', padding: 24, textAlign: 'center', fontFamily: 'sans-serif' }}>
      <h2>Not available in the demo</h2>
      <p>
        This feature (sign-up, messaging, chat, CV analysis, uploads or editing) is turned off in the read-only demo.
        You can still browse the sample dashboard, projects, tasks, materials, mentors and sponsors.
      </p>
      <p>
        <Link to="/dashboard" data-demo-allow>Back to the demo dashboard</Link>
      </p>
    </div>
  );
}

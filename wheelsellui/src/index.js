// src/index.js (Update this file)

import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css'; // Your main CSS file
import App from './App';
import { AuthProvider } from './context/AuthContext'; // <-- Import AuthProvider

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <AuthProvider>
      <App />
    </AuthProvider>
  </React.StrictMode>
);
// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CssBaseline } from '@mui/material';

// Layout
import Navbar from './components/Layout/Navbar';

// Pages
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import SearchPage from './pages/SearchPage';
import CarDetailsPage from './pages/CarDetailsPage';
import CreateAdPage from './pages/CreateAdPage';
import ProfilePage from './pages/ProfilePage';
// import NotFoundPage from './pages/NotFoundPage'; // If you add a 404 page

const App = () => {
  return (
    <Router>
      <CssBaseline /> {/* MUI CSS Reset */}
      <Navbar />
      <main>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/car/:id" element={<CarDetailsPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          
          {/* Protected Routes (You might want to wrap these with a component to enforce login) */}
          <Route path="/create-ad" element={<CreateAdPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          {/* <Route path="*" element={<NotFoundPage />} /> */}
        </Routes>
      </main>
    </Router>
  );
};

export default App;
import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { TextField, Button, Container, Typography, Box, Alert } from '@mui/material';
import apiClient from '../api/apiClient';
import { PRIMARY_YELLOW, DARK_GRAY } from '../theme/colors';

// *** TEMPORARY MOCK AUTH CONTEXT ***
// We'll replace this with a real context hook later.
const useAuth = () => {
    // This function will eventually store the token and update the global state.
    const login = (token) => {
        console.log("Mock Login: Token received and stored (TBD).", token);
        // For now, let's just save a flag in local storage to simulate being logged in.
        localStorage.setItem('isAuthenticated', 'true');
    };
    return { login };
};
// **********************************

const LoginPage = () => {
    const navigate = useNavigate();
    const { login } = useAuth(); // Use the mock auth hook

    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });

    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            // API Endpoint: POST /api/auth/login
            const response = await apiClient.post('/auth/login', formData);
            
            // Assuming your ASP.NET Core API returns an object with a token (e.g., 'token' or 'jwtToken')
            // If your API sets an HttpOnly cookie, response.data might be empty, and the server handles session.
            const token = response.data.token || 'SUCCESS_TOKEN'; 

            login(token); // Store the token/session state
            
            console.log('Login Success:', response.data);
            
            // Navigate to the Profile page or Home page upon successful login
            navigate('/profile'); 

        } catch (err) {
            // Check for specific error message from the backend (e.g., 400 Bad Request)
            const errorMessage = err.response?.data?.title || 'Login failed. Check your email and password.';
            setError(errorMessage);
            console.error('Login Error:', err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Container component="main" maxWidth="xs" sx={{ py: 8 }}>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    backgroundColor: 'white',
                    p: 4,
                    borderRadius: 2,
                    boxShadow: 3
                }}
            >
                <Typography component="h1" variant="h4" sx={{ color: DARK_GRAY, mb: 3 }}>
                    Sign In
                </Typography>

                {error && <Alert severity="error" sx={{ width: '100%', mb: 2 }}>{error}</Alert>}

                <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1, width: '100%' }}>
                    <TextField
                        name="email"
                        required
                        fullWidth
                        label="Email Address"
                        autoFocus
                        value={formData.email}
                        onChange={handleChange}
                        type="email"
                        margin="normal"
                    />
                    <TextField
                        name="password"
                        required
                        fullWidth
                        label="Password"
                        type="password"
                        value={formData.password}
                        onChange={handleChange}
                        margin="normal"
                    />

                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        disabled={loading}
                        sx={{ mt: 3, mb: 2, backgroundColor: PRIMARY_YELLOW, color: DARK_GRAY, fontWeight: 'bold', 
                              '&:hover': { backgroundColor: '#FFC800' } }}
                    >
                        {loading ? 'Signing In...' : 'Sign In'}
                    </Button>
                    
                    <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                        <Link to="/register" style={{ color: DARK_GRAY }}>
                            Don't have an account? Sign Up
                        </Link>
                    </Box>
                </Box>
            </Box>
        </Container>
    );
};

export default LoginPage;
import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { TextField, Button, Container, Typography, Box, Grid, Alert } from '@mui/material';
import apiClient from '../api/apiClient';
import { PRIMARY_YELLOW, DARK_GRAY } from '../theme/colors';

const RegisterPage = () => {
    const navigate = useNavigate();
    
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        firstName: '',
        lastName: '',
        country: '',
        city: '',
        phoneNumber: ''
    });

    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
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
        setSuccess('');
        setLoading(true);

        // Basic client-side validation check
        if (!formData.email || !formData.password || !formData.firstName || !formData.phoneNumber) {
            setError('Please fill in all required fields.');
            setLoading(false);
            return;
        }

        try {
            // The endpoint is /api/auth/register, which is resolved by apiClient
            const response = await apiClient.post('/auth/register', formData);
            
            setSuccess('Registration successful! Redirecting to login...');
            
            // Log the response and navigate to the login page after a delay
            console.log('Registration Success:', response.data);
            setTimeout(() => {
                navigate('/login');
            }, 2000);

        } catch (err) {
            // Error handling for 400 Bad Request, etc.
            const errorMessage = err.response?.data?.title || err.response?.data?.errors?.Password?.[0] || 'Registration failed. Please check your input.';
            setError(errorMessage);
            console.error('Registration Error:', err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Container component="main" maxWidth="md" sx={{ py: 4 }}>
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
                    Create Account
                </Typography>

                {error && <Alert severity="error" sx={{ width: '100%', mb: 2 }}>{error}</Alert>}
                {success && <Alert severity="success" sx={{ width: '100%', mb: 2 }}>{success}</Alert>}

                <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1, width: '100%' }}>
                    
                    {/* ACCOUNT DETAILS */}
                    <Typography variant="h6" sx={{ color: DARK_GRAY, mb: 1, mt: 2 }}>
                        Account Details
                    </Typography>
                    <Grid container spacing={2}>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                name="email"
                                required
                                fullWidth
                                label="Email Address"
                                autoFocus
                                value={formData.email}
                                onChange={handleChange}
                                type="email"
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                name="password"
                                required
                                fullWidth
                                label="Password"
                                type="password"
                                value={formData.password}
                                onChange={handleChange}
                            />
                        </Grid>
                    </Grid>
                    
                    {/* PERSONAL DETAILS */}
                    <Typography variant="h6" sx={{ color: DARK_GRAY, mb: 1, mt: 3 }}>
                        Personal Details
                    </Typography>
                    <Grid container spacing={2}>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                name="firstName"
                                required
                                fullWidth
                                label="First Name"
                                value={formData.firstName}
                                onChange={handleChange}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                name="lastName"
                                fullWidth
                                label="Last Name"
                                value={formData.lastName}
                                onChange={handleChange}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                name="phoneNumber"
                                required
                                fullWidth
                                label="Phone Number"
                                value={formData.phoneNumber}
                                onChange={handleChange}
                            />
                        </Grid>
                    </Grid>

                    {/* LOCATION DETAILS */}
                    <Typography variant="h6" sx={{ color: DARK_GRAY, mb: 1, mt: 3 }}>
                        Location
                    </Typography>
                    <Grid container spacing={2}>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                name="country"
                                fullWidth
                                label="Country"
                                value={formData.country}
                                onChange={handleChange}
                            />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField
                                name="city"
                                fullWidth
                                label="City"
                                value={formData.city}
                                onChange={handleChange}
                            />
                        </Grid>
                    </Grid>

                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        disabled={loading}
                        sx={{ mt: 3, mb: 2, backgroundColor: PRIMARY_YELLOW, color: DARK_GRAY, fontWeight: 'bold', 
                              '&:hover': { backgroundColor: '#FFC800' } }}
                    >
                        {loading ? 'Registering...' : 'Register'}
                    </Button>
                    
                    <Grid container justifyContent="flex-end">
                        <Grid item>
                            <Link to="/login" style={{ color: DARK_GRAY }}>
                                Already have an account? Sign In
                            </Link>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
        </Container>
    );
};

export default RegisterPage;
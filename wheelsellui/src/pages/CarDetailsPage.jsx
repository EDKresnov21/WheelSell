import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Container, Box, Typography, Grid, Paper, Divider, Button, CircularProgress, Alert } from '@mui/material';
import ContactMailIcon from '@mui/icons-material/ContactMail';
import apiClient from '../api/apiClient';
import { useAuth } from '../context/AuthContext'; // To check authentication status
import { PRIMARY_YELLOW, DARK_GRAY } from '../theme/colors';

// Mock data structure for a single car
const mockCarData = {
    id: 1,
    title: '2023 Toyota Camry LE',
    make: 'Toyota',
    model: 'Camry',
    year: 2023,
    price: 32500,
    mileage: 5000,
    engineType: '2.5L I4',
    transmissionType: 'Automatic',
    description: 'Immaculate condition, single owner, low mileage. Perfect family sedan with great fuel economy and safety features.',
    city: 'Los Angeles',
    country: 'USA',
    // Seller Contact Info (Crucial for conditional display)
    sellerName: 'Jane Doe',
    sellerPhone: '+1-555-123-4567',
    sellerEmail: 'jane.doe@example.com',
    photos: [
        'https://via.placeholder.com/800x600?text=Car+Photo+1',
        'https://via.placeholder.com/800x600?text=Car+Photo+2',
        'https://via.placeholder.com/800x600?text=Car+Photo+3',
    ],
};

const CarDetailsPage = () => {
    const { id } = useParams(); // Get the car ID from the URL
    const { isAuthenticated } = useAuth(); // Check if the user is logged in
    
    const [car, setCar] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [showContacts, setShowContacts] = useState(false);

    useEffect(() => {
        const fetchCar = async () => {
            setLoading(true);
            setError('');
            try {
                // API Endpoint: GET /api/cars/{id}
                const response = await apiClient.get(`/cars/${id}`);
                setCar(response.data);
                
                // --- MOCK DATA FALLBACK ---
                // For testing before the API is ready:
                if (!response.data) {
                     setCar(mockCarData);
                }
                // --------------------------

            } catch (err) {
                setError('Failed to load car details. Listing may not exist.');
                // Fallback for mock data when API call fails
                setCar(mockCarData); 
            } finally {
                setLoading(false);
            }
        };

        fetchCar();
    }, [id]);
    
    // Function to handle showing contacts
    const handleShowContacts = () => {
        if (isAuthenticated) {
            setShowContacts(true);
        } else {
            // If not logged in, redirect them to the login page
            alert("You must be signed in to view seller contact details.");
            // navigate('/login'); // We could use useNavigate to redirect here
        }
    };


    if (loading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
                <CircularProgress sx={{ color: PRIMARY_YELLOW }} />
            </Box>
        );
    }

    if (error || !car) {
        return <Alert severity="error" sx={{ mt: 3 }}>{error || 'Listing not found.'}</Alert>;
    }

    // --- RENDER CAR DETAILS ---
    return (
        <Container maxWidth="xl" sx={{ py: 4 }}>
            <Typography variant="h3" sx={{ color: DARK_GRAY, mb: 1, fontWeight: 700 }}>
                {car.title}
            </Typography>
            <Typography variant="h6" color="text.secondary" sx={{ mb: 3 }}>
                {car.city}, {car.country} | {car.make} {car.model}
            </Typography>

            <Grid container spacing={4}>
                {/* LEFT COLUMN: MEDIA & DESCRIPTION (70%) */}
                <Grid item xs={12} lg={8}>
                    {/* 1. Media Gallery (Placeholder for Carousel) */}
                    <Paper elevation={4} sx={{ mb: 4 }}>
                        <Box 
                            sx={{ 
                                height: 450, 
                                backgroundColor: DARK_GRAY, 
                                color: 'white', 
                                display: 'flex', 
                                alignItems: 'center', 
                                justifyContent: 'center', 
                                borderRadius: 1 
                            }}
                        >
                            <Typography variant="h5">
                                Photos/Video Gallery Placeholder ({car.photos?.length || 0} items)
                            </Typography>
                        </Box>
                    </Paper>

                    {/* 2. Description */}
                    <Paper elevation={2} sx={{ p: 3, mb: 4 }}>
                        <Typography variant="h5" sx={{ color: DARK_GRAY, mb: 2, fontWeight: 600 }}>
                            Description
                        </Typography>
                        <Typography variant="body1">
                            {car.description}
                        </Typography>
                    </Paper>

                    {/* 3. Specifications */}
                    <Paper elevation={2} sx={{ p: 3 }}>
                        <Typography variant="h5" sx={{ color: DARK_GRAY, mb: 2, fontWeight: 600 }}>
                            Specifications
                        </Typography>
                        <Grid container spacing={2}>
                            <Grid item xs={6}><Typography>Year:</Typography></Grid>
                            <Grid item xs={6}><Typography sx={{ fontWeight: 600 }}>{car.year}</Typography></Grid>
                            <Grid item xs={6}><Typography>Mileage:</Typography></Grid>
                            <Grid item xs={6}><Typography sx={{ fontWeight: 600 }}>{car.mileage.toLocaleString('en-US')} km</Typography></Grid>
                            <Grid item xs={6}><Typography>Engine:</Typography></Grid>
                            <Grid item xs={6}><Typography sx={{ fontWeight: 600 }}>{car.engineType}</Typography></Grid>
                            <Grid item xs={6}><Typography>Transmission:</Typography></Grid>
                            <Grid item xs={6}><Typography sx={{ fontWeight: 600 }}>{car.transmissionType}</Typography></Grid>
                        </Grid>
                    </Paper>
                </Grid>

                {/* RIGHT COLUMN: PRICE & CONTACT (30%) */}
                <Grid item xs={12} lg={4}>
                    <Paper elevation={4} sx={{ p: 3, position: 'sticky', top: 20 }}>
                        
                        {/* 1. Price */}
                        <Typography variant="h4" color="text.secondary">Asking Price</Typography>
                        <Typography variant="h2" sx={{ color: PRIMARY_YELLOW, fontWeight: 900, mb: 3 }}>
                            ${car.price.toLocaleString('en-US')}
                        </Typography>
                        
                        <Divider sx={{ mb: 3 }} />

                        {/* 2. Contact Section (Conditional Display) */}
                        <Typography variant="h5" sx={{ color: DARK_GRAY, mb: 2, fontWeight: 600 }}>
                            Seller Contact
                        </Typography>

                        {!showContacts ? (
                            // Button state (initial)
                            <Button
                                fullWidth
                                variant="contained"
                                onClick={handleShowContacts}
                                startIcon={<ContactMailIcon />}
                                sx={{ 
                                    backgroundColor: PRIMARY_YELLOW, 
                                    color: DARK_GRAY, 
                                    fontSize: '1.1rem',
                                    p: 1.5,
                                    fontWeight: 700,
                                    '&:hover': { backgroundColor: '#FFC800' }
                                }}
                            >
                                {isAuthenticated ? 'Show Contacts' : 'Sign In to View Contacts'}
                            </Button>
                        ) : (
                            // Display Contacts state (after clicking or if user is logged in)
                            <Box>
                                <Typography variant="body1">
                                    **Name:** {car.sellerName}
                                </Typography>
                                <Typography variant="body1" sx={{ mt: 1 }}>
                                    **Phone:** <a href={`tel:${car.sellerPhone}`}>{car.sellerPhone}</a>
                                </Typography>
                                <Typography variant="body1" sx={{ mt: 1 }}>
                                    **Email:** <a href={`mailto:${car.sellerEmail}`}>{car.sellerEmail}</a>
                                </Typography>
                            </Box>
                        )}
                        
                    </Paper>
                </Grid>
            </Grid>
        </Container>
    );
};

export default CarDetailsPage;
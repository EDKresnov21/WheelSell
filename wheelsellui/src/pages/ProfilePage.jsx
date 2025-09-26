import React, { useState, useEffect } from 'react';
import { Container, Box, Typography, Grid, TextField, Button, Alert, CircularProgress, Paper } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CarCard from '../components/Layout/CarCard.jsx'; // Reusing the CarCard component
import apiClient from '../api/apiClient';
import { PRIMARY_YELLOW, DARK_GRAY } from '../theme/colors';

// Mock data for initial testing
const mockUserData = {
    firstName: 'Alex',
    lastName: 'Johnson',
    email: 'alex.j@wheelsell.com',
    phoneNumber: '+1-555-987-6543',
    city: 'New York',
    country: 'USA',
};

const mockUserListings = [
    { id: 101, title: '2023 BMW M3 Competition', make: 'BMW', year: 2023, mileageKm: 8000, price: 85000, photoUrl: 'https://via.placeholder.com/300x200?text=BMW+M3', city: 'New York' },
    { id: 102, title: '2019 Porsche 911 Carrera', make: 'Porsche', year: 2019, mileageKm: 25000, price: 110000, photoUrl: 'https://via.placeholder.com/300x200?text=Porsche+911', city: 'New York' },
];

const ProfilePage = () => {
    const [userData, setUserData] = useState(mockUserData);
    const [listings, setListings] = useState(mockUserListings);
    const [isEditing, setIsEditing] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    // 1. Fetch User Data and Listings on component mount
    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            setError('');
            try {
                // Fetch User Profile: GET /api/user/profile
                const profileResponse = await apiClient.get('/user/profile');
                setUserData(profileResponse.data || mockUserData); // Use mock data as fallback

                // Fetch User Listings: GET /api/user/listings
                const listingsResponse = await apiClient.get('/user/listings');
                setListings(listingsResponse.data || mockUserListings); // Use mock data as fallback

            } catch (err) {
                setError('Failed to load profile or listings. Are you logged in?');
                console.error('Profile Fetch Error:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    // Handle form changes
    const handleInputChange = (e) => {
        setUserData({
            ...userData,
            [e.target.name]: e.target.value
        });
    };

    // 2. Handle Profile Update
    const handleSave = async () => {
        setLoading(true);
        setError('');
        setSuccess('');
        try {
            // API Endpoint: PUT /api/user/profile
            await apiClient.put('/user/profile', userData);
            setSuccess('Profile updated successfully!');
            setIsEditing(false); // Exit edit mode
        } catch (err) {
            setError('Failed to update profile. Please try again.');
            console.error('Profile Update Error:', err);
        } finally {
            setLoading(false);
        }
    };
    
    // 3. Placeholder for Delete Listing functionality
    const handleDeleteListing = (carId) => {
        if (window.confirm(`Are you sure you want to delete listing #${carId}?`)) {
            // TODO: Call API endpoint: apiClient.delete(`/cars/delete/${carId}`);
            alert(`Listing ${carId} deleted (Mock action).`); 
            // Update local state by removing the deleted listing
            setListings(listings.filter(car => car.id !== carId));
        }
    };

    if (loading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
                <CircularProgress sx={{ color: PRIMARY_YELLOW }} />
            </Box>
        );
    }

    return (
        <Container component="main" maxWidth="lg" sx={{ py: 4 }}>
            <Typography variant="h3" sx={{ color: DARK_GRAY, mb: 4, fontWeight: 700 }}>
                My Profile Dashboard
            </Typography>
            
            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
            {success && <Alert severity="success" sx={{ mb: 2 }}>{success}</Alert>}

            {/* SECTION A: PROFILE DETAILS */}
            <Paper elevation={4} sx={{ p: 4, mb: 5 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                    <Typography variant="h5" sx={{ color: DARK_GRAY, fontWeight: 600 }}>
                        Account Information
                    </Typography>
                    <Button 
                        variant="contained" 
                        onClick={() => isEditing ? handleSave() : setIsEditing(true)} 
                        disabled={loading}
                        startIcon={isEditing ? <SaveIcon /> : <EditIcon />}
                        sx={{ backgroundColor: PRIMARY_YELLOW, color: DARK_GRAY, '&:hover': { backgroundColor: '#FFC800' } }}
                    >
                        {isEditing ? 'Save Changes' : 'Edit Profile'}
                    </Button>
                </Box>
                
                <Grid container spacing={3}>
                    {/* Input Fields */}
                    <Grid item xs={12} sm={6}>
                        <TextField fullWidth label="First Name" name="firstName" value={userData.firstName} onChange={handleInputChange} disabled={!isEditing} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <TextField fullWidth label="Last Name" name="lastName" value={userData.lastName} onChange={handleInputChange} disabled={!isEditing} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <TextField fullWidth label="Email" name="email" value={userData.email} disabled /> {/* Email often cannot be edited */}
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <TextField fullWidth label="Phone Number" name="phoneNumber" value={userData.phoneNumber} onChange={handleInputChange} disabled={!isEditing} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <TextField fullWidth label="City" name="city" value={userData.city} onChange={handleInputChange} disabled={!isEditing} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <TextField fullWidth label="Country" name="country" value={userData.country} onChange={handleInputChange} disabled={!isEditing} />
                    </Grid>
                </Grid>
            </Paper>

            {/* SECTION B: MY LISTINGS */}
            <Typography variant="h4" sx={{ color: DARK_GRAY, mb: 3, fontWeight: 700 }}>
                My Active Listings ({listings.length})
            </Typography>
            
            {listings.length === 0 && (
                 <Alert severity="info">You currently have no active listings. Start selling your car now!</Alert>
            )}

            <Grid container spacing={4}>
                {listings.map((car) => (
                    <Grid item xs={12} md={6} lg={4} key={car.id}>
                        <CarCard car={car} /> 
                        {/* Management Buttons */}
                        <Box sx={{ mt: 1, display: 'flex', justifyContent: 'space-between' }}>
                            <Button size="small" variant="outlined" sx={{ color: DARK_GRAY, borderColor: DARK_GRAY }} 
                                component="a" href={`/edit-ad/${car.id}`} // Link to the Edit Ad page
                            >
                                Edit
                            </Button>
                            <Button size="small" color="error" variant="outlined" onClick={() => handleDeleteListing(car.id)}>
                                Delete
                            </Button>
                        </Box>
                    </Grid>
                ))}
            </Grid>
        </Container>
    );
};

export default ProfilePage;
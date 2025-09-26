import React, { useState, useEffect } from 'react';
import { Container, Grid, Box, Typography, TextField, Button, CircularProgress, Alert, MenuItem } from '@mui/material';
import CarCard from '../components/Layout/CarCard.jsx';
import apiClient from '../api/apiClient';
import { PRIMARY_YELLOW, DARK_GRAY } from '../theme/colors';

// Define filter options for dropdowns
const transmissionOptions = ['Automatic', 'Manual', 'Any'];
const fuelOptions = ['Gasoline', 'Diesel', 'Electric', 'Hybrid', 'Any'];

// Initial state for all search parameters
const initialSearchParams = {
    make: '',
    model: '',
    minPrice: '',
    maxPrice: '',
    minYear: '',
    maxYear: '',
    transmission: 'Any', // Default to 'Any'
    fuelType: 'Any',     // Default to 'Any'
    city: '',
    page: 1, // Add pagination support
    pageSize: 9, // Display 9 cars per page
};

const SearchPage = () => {
    const [cars, setCars] = useState([]);
    const [searchParams, setSearchParams] = useState(initialSearchParams);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [totalResults, setTotalResults] = useState(0); // For pagination

    // Function to fetch cars from the API
    const fetchCars = async (params) => {
        setLoading(true);
        setError('');
        
        // Filter out empty or 'Any' parameters for cleaner query string
        const cleanedParams = Object.fromEntries(
            Object.entries(params).filter(([_, v]) => v && v !== 'Any')
        );

        const query = new URLSearchParams(cleanedParams).toString();
        
        try {
            // API Endpoint: GET /api/cars?make=...&minPrice=...
            const response = await apiClient.get(`/cars?${query}`);
            
            // Assuming your ASP.NET Core API returns results and total count (e.g., in headers or a response object)
            // For now, we'll use a simplified structure:
            setCars(response.data.items || response.data || []);
            setTotalResults(response.data.totalCount || (response.data?.length || 0)); 

        } catch (err) {
            setError('Failed to fetch car listings. Check API connection.');
            console.error('Fetch Cars Error:', err);
            setCars([]);
            setTotalResults(0);
        } finally {
            setLoading(false);
        }
    };

    // Use a debounced effect in a real app, but for simplicity, we call fetchCars directly
    useEffect(() => {
        // Trigger API call when searchParams change
        fetchCars(searchParams);
    }, [searchParams]); 

    // Handle standard input/select changes
    const handleFilterChange = (e) => {
        setSearchParams(prev => ({
            ...prev,
            [e.target.name]: e.target.value,
            page: 1, // Reset to first page on any filter change
        }));
    };
    
    // Handle pagination button clicks
    const handlePageChange = (newPage) => {
        setSearchParams(prev => ({
            ...prev,
            page: newPage
        }));
    };

    const handleClearFilters = () => {
        setSearchParams(initialSearchParams);
    };
    
    // Calculate total pages for the pagination control
    const totalPages = Math.ceil(totalResults / searchParams.pageSize);

    return (
        <Container maxWidth="xl" sx={{ mt: 3 }}>
            <Typography variant="h3" sx={{ color: DARK_GRAY, mb: 1, fontWeight: 700 }}>
                Car Listings
            </Typography>
            <Typography variant="h6" color="text.secondary" sx={{ mb: 4 }}>
                Found {totalResults} vehicles matching your criteria.
            </Typography>

            <Grid container spacing={4}>
                {/* FILTER SIDEBAR (25% width) */}
                <Grid item xs={12} md={3}>
                    <Box sx={{ p: 3, backgroundColor: 'white', borderRadius: 2, boxShadow: 3 }}>
                        <Typography variant="h5" sx={{ mb: 3, fontWeight: 600 }}>Refine Search</Typography>

                        {/* Text Filter: Make */}
                        <TextField
                            fullWidth
                            label="Make"
                            name="make"
                            value={searchParams.make}
                            onChange={handleFilterChange}
                            margin="normal"
                        />
                        
                        {/* Text Filter: Model */}
                        <TextField
                            fullWidth
                            label="Model"
                            name="model"
                            value={searchParams.model}
                            onChange={handleFilterChange}
                            margin="normal"
                        />
                        
                        {/* Select Filter: Transmission */}
                        <TextField
                            select
                            fullWidth
                            label="Transmission"
                            name="transmission"
                            value={searchParams.transmission}
                            onChange={handleFilterChange}
                            margin="normal"
                        >
                            {transmissionOptions.map((option) => (
                                <MenuItem key={option} value={option}>
                                    {option}
                                </MenuItem>
                            ))}
                        </TextField>

                        {/* Select Filter: Fuel Type */}
                        <TextField
                            select
                            fullWidth
                            label="Fuel Type"
                            name="fuelType"
                            value={searchParams.fuelType}
                            onChange={handleFilterChange}
                            margin="normal"
                        >
                            {fuelOptions.map((option) => (
                                <MenuItem key={option} value={option}>
                                    {option}
                                </MenuItem>
                            ))}
                        </TextField>
                        
                        {/* Range Filter: Price */}
                        <Typography variant="subtitle1" sx={{ mt: 2, mb: 1, fontWeight: 500 }}>Price Range ($)</Typography>
                        <Grid container spacing={2}>
                            <Grid item xs={6}>
                                <TextField size="small" label="Min" name="minPrice" value={searchParams.minPrice} onChange={handleFilterChange} type="number" />
                            </Grid>
                            <Grid item xs={6}>
                                <TextField size="small" label="Max" name="maxPrice" value={searchParams.maxPrice} onChange={handleFilterChange} type="number" />
                            </Grid>
                        </Grid>

                        {/* Range Filter: Year */}
                        <Typography variant="subtitle1" sx={{ mt: 2, mb: 1, fontWeight: 500 }}>Year Range</Typography>
                        <Grid container spacing={2}>
                            <Grid item xs={6}>
                                <TextField size="small" label="From" name="minYear" value={searchParams.minYear} onChange={handleFilterChange} type="number" />
                            </Grid>
                            <Grid item xs={6}>
                                <TextField size="small" label="To" name="maxYear" value={searchParams.maxYear} onChange={handleFilterChange} type="number" />
                            </Grid>
                        </Grid>
                        
                        {/* Clear Filters Button */}
                        <Button
                            fullWidth
                            variant="outlined"
                            onClick={handleClearFilters}
                            sx={{ mt: 3, borderColor: DARK_GRAY, color: DARK_GRAY, '&:hover': { backgroundColor: '#eee' } }}
                        >
                            Clear Filters
                        </Button>
                    </Box>
                </Grid>

                {/* CAR RESULTS AREA (75% width) */}
                <Grid item xs={12} md={9}>
                    {loading && (
                        <Box sx={{ display: 'flex', justifyContent: 'center', py: 5 }}>
                            <CircularProgress sx={{ color: PRIMARY_YELLOW }} />
                        </Box>
                    )}
                    
                    {error && <Alert severity="error" sx={{ mb: 3 }}>{error}</Alert>}
                    
                    {!loading && cars.length === 0 && !error && (
                        <Alert severity="info">No cars found matching your criteria. Try clearing the filters.</Alert>
                    )}

                    {/* Car Card Display */}
                    {!loading && cars.length > 0 && (
                        <Grid container spacing={3}>
                            {cars.map((car) => (
                                <Grid item xs={12} sm={6} lg={4} key={car.id}>
                                    <CarCard car={car} />
                                </Grid>
                            ))}
                        </Grid>
                    )}
                    
                    {/* Pagination Controls */}
                    {!loading && totalPages > 1 && (
                        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4, gap: 2 }}>
                            <Button
                                variant="outlined"
                                onClick={() => handlePageChange(searchParams.page - 1)}
                                disabled={searchParams.page === 1}
                            >
                                Previous
                            </Button>
                            <Typography sx={{ display: 'flex', alignItems: 'center' }}>
                                Page {searchParams.page} of {totalPages}
                            </Typography>
                            <Button
                                variant="outlined"
                                onClick={() => handlePageChange(searchParams.page + 1)}
                                disabled={searchParams.page >= totalPages}
                            >
                                Next
                            </Button>
                        </Box>
                    )}
                </Grid>
            </Grid>
        </Container>
    );
};

export default SearchPage;
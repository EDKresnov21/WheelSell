// src/components/CarCard.jsx

import React from 'react';
import { Card, CardContent, CardMedia, Typography, Box, Divider } from '@mui/material';
import { Link } from 'react-router-dom';
import { PRIMARY_YELLOW, DARK_GRAY } from '../../theme/colors';

const CarCard = ({ car }) => {
    // Mock data structure, adjust based on your API response
    const { id, title, year, mileageKm, price, photoUrl, city } = car; 

    return (
        <Card 
            component={Link} 
            to={`/car/${id}`} // Link to the specific car details page
            sx={{ 
                textDecoration: 'none',
                height: '100%',
                transition: '0.3s',
                '&:hover': {
                    boxShadow: 8, // Lift on hover
                    transform: 'translateY(-2px)'
                }
            }}
        >
            <CardMedia
                component="img"
                height="200"
                image={photoUrl || 'https://via.placeholder.com/300x200?text=No+Image'} // Placeholder if no image
                alt={title}
                sx={{ objectFit: 'cover' }}
            />
            <CardContent>
                <Typography gutterBottom variant="h6" component="div" sx={{ fontWeight: 600, color: DARK_GRAY }}>
                    {title}
                </Typography>
                
                <Divider sx={{ my: 1 }} />

                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
                    <Typography variant="body2" color="text.secondary">
                        Year: {year}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                        Mileage: {mileageKm} km
                    </Typography>
                </Box>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    Location: {city || 'N/A'}
                </Typography>

                {/* Price is the primary focus, colored yellow */}
                <Typography 
                    variant="h5" 
                    sx={{ color: PRIMARY_YELLOW, fontWeight: 800, mt: 2 }}
                >
                    ${price ? price.toLocaleString('en-US') : 'N/A'} 
                </Typography>
            </CardContent>
        </Card>
    );
};

export default CarCard;
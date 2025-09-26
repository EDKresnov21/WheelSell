import React from 'react';
import { Box, Typography, Button, Container, Grid, Paper } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import PersonOutlineIcon from '@mui/icons-material/PersonOutline';
import { Link } from 'react-router-dom';
import { PRIMARY_YELLOW, DARK_GRAY } from '../theme/colors';

// Component for the feature tiles on the home page
const FeatureTile = ({ title, description, to, icon: IconComponent }) => (
    <Paper 
        elevation={6} // Stronger shadow for better visibility
        sx={{ 
            p: 3, 
            textAlign: 'center', 
            height: '100%',
            transition: 'transform 0.3s, box-shadow 0.3s',
            borderTop: `5px solid ${PRIMARY_YELLOW}`, // Yellow accent line
            '&:hover': { 
                transform: 'translateY(-8px)', // Lift effect on hover
                boxShadow: `0 10px 20px rgba(0, 0, 0, 0.2)` 
            }
        }}
    >
        <IconComponent sx={{ fontSize: 48, color: PRIMARY_YELLOW, mb: 1.5 }} />
        <Typography variant="h6" gutterBottom sx={{ fontWeight: 600 }}>{title}</Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
            {description}
        </Typography>
        <Button 
            component={Link} 
            to={to} 
            variant="outlined"
            sx={{ 
                color: DARK_GRAY, 
                borderColor: DARK_GRAY,
                '&:hover': { 
                    backgroundColor: PRIMARY_YELLOW, 
                    borderColor: PRIMARY_YELLOW,
                    color: DARK_GRAY,
                    fontWeight: 'bold'
                }
            }}
        >
            Go to Section
        </Button>
    </Paper>
);


const HomePage = () => {
    return (
        <Box>
            {/* 1. HERO SECTION - Main Banner (Dark Gray background, Yellow CTA) */}
            <Box 
                sx={{ 
                    minHeight: '450px', 
                    backgroundColor: DARK_GRAY, 
                    color: 'white', 
                    display: 'flex', 
                    flexDirection: 'column',
                    justifyContent: 'center', 
                    alignItems: 'center', 
                    textAlign: 'center',
                    mb: 6,
                    py: 6,
                    borderRadius: 1 // Slight rounding for modern look
                }}
            >
                <Container maxWidth="md">
                    <Typography variant="h2" gutterBottom sx={{ fontWeight: 800, color: PRIMARY_YELLOW }}>
                        Find Your Perfect Ride
                    </Typography>
                    <Typography variant="h5" sx={{ mb: 4, color: '#aaa' }}>
                        Thousands of verified car listings available instantly.
                    </Typography>
                    
                    {/* Main Yellow CTA Button */}
                    <Button 
                        variant="contained" 
                        size="large" 
                        component={Link} 
                        to="/search"
                        startIcon={<SearchIcon />}
                        sx={{ 
                            backgroundColor: PRIMARY_YELLOW, 
                            color: DARK_GRAY,
                            fontSize: '1.2rem',
                            padding: '10px 30px',
                            fontWeight: 700,
                            transition: '0.3s',
                            '&:hover': { 
                                backgroundColor: '#FFC800', 
                                transform: 'scale(1.05)'
                            }
                        }}
                    >
                        Start Car Search
                    </Button>
                </Container>
            </Box>

            {/* 2. FEATURE TILES */}
            <Container maxWidth="lg">
                <Typography variant="h4" gutterBottom align="center" sx={{ mb: 5, color: DARK_GRAY, fontWeight: 700 }}>
                    Our Key Features
                </Typography>
                <Grid container spacing={4}>
                    
                    {/* Tile 1: Search */}
                    <Grid item xs={12} md={4}>
                        <FeatureTile 
                            title="Advanced Search" 
                            description="Filter by make, model, year, and price. Find exactly what you need with our detailed tools." 
                            to="/search" 
                            icon={SearchIcon}
                        />
                    </Grid>

                    {/* Tile 2: Sell */}
                    <Grid item xs={12} md={4}>
                        <FeatureTile 
                            title="List Your Vehicle" 
                            description="Fast and easy listing process. Add up to 10 photos and 5 videos to maximize visibility." 
                            to="/create-ad" 
                            icon={AddCircleOutlineIcon}
                        />
                    </Grid>
                    
                    {/* Tile 3: Profile */}
                    <Grid item xs={12} md={4}>
                        <FeatureTile 
                            title="Personal Account" 
                            description="Manage your listings, update profile details, and track your favorited cars." 
                            to="/profile" 
                            icon={PersonOutlineIcon}
                        />
                    </Grid>
                    
                </Grid>
            </Container>
        </Box>
    );
};

export default HomePage;
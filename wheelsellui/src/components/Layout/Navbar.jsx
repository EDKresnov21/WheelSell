// src/components/Layout/Navbar.jsx (Update this file)

import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext'; // <-- Use the real hook
import { PRIMARY_YELLOW, DARK_GRAY } from '../../theme/colors';

const Navbar = () => {
    const { isAuthenticated, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/'); // Redirect to home after logging out
    };

    return (
        <AppBar position="static" sx={{ backgroundColor: DARK_GRAY }}>
            <Toolbar>
                <Typography variant="h6" component={Link} to="/" sx={{ flexGrow: 1, textDecoration: 'none', color: PRIMARY_YELLOW, fontWeight: 'bold' }}>
                    WheelSell
                </Typography>

                <Box sx={{ display: 'flex', gap: 2 }}>
                    {/* Search Button */}
                    <Button color="inherit" component={Link} to="/search" sx={{ color: PRIMARY_YELLOW }}>
                        Search Cars
                    </Button>
                    
                    {isAuthenticated ? (
                        <>
                            {/* Logged In State */}
                            <Button variant="contained" component={Link} to="/profile" sx={{ backgroundColor: PRIMARY_YELLOW, color: DARK_GRAY }}>
                                Profile
                            </Button>
                            <Button color="inherit" onClick={handleLogout}>
                                Sign Out
                            </Button>
                        </>
                    ) : (
                        <>
                            {/* Guest State */}
                            <Button color="inherit" component={Link} to="/login">
                                Sign In
                            </Button>
                            <Button color="inherit" component={Link} to="/register">
                                Register
                            </Button>
                        </>
                    )}
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Navbar;
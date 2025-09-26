// src/components/Layout/MainLayout.jsx

import { Outlet } from 'react-router-dom';
import Navbar from './Navbar';
import Footer from './Footer';
import { Box } from '@mui/material';
import { LIGHT_GRAY } from '../../theme/colors';

const MainLayout = () => {
    return (
        <Box sx={{ minHeight: '100vh', display: 'flex', flexDirection: 'column', backgroundColor: LIGHT_GRAY }}>
            <Navbar />
            
            {/* Основной контент страницы */}
            <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
                <Outlet /> 
            </Box>

            <Footer />
        </Box>
    );
};

export default MainLayout;
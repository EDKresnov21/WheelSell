// src/components/Layout/Footer.jsx

import { Box, Typography } from '@mui/material';
import { DARK_GRAY } from '../../theme/colors';

const Footer = () => {
    return (
        <Box 
            component="footer" 
            sx={{ 
                p: 2, 
                mt: 'auto', 
                backgroundColor: DARK_GRAY, 
                color: 'white', 
                textAlign: 'center' 
            }}
        >
            <Typography variant="body2">
                © {new Date().getFullYear()} WheelSell. Все права защищены.
            </Typography>
        </Box>
    );
};

export default Footer;
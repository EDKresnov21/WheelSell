import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Box, Typography, TextField, Button, Grid, Alert, CircularProgress, Paper } from '@mui/material';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import apiClient from '../api/apiClient';
import { PRIMARY_YELLOW, DARK_GRAY } from '../theme/colors';

const initialAdData = {
    title: '',
    make: '',
    model: '',
    year: '',
    price: '',
    mileage: '',
    engineType: '',
    transmissionType: '',
    description: '',
    city: '',
    country: '',
};

const CreateAdPage = () => {
    const navigate = useNavigate();
    const [adData, setAdData] = useState(initialAdData);
    const [files, setFiles] = useState([]); // Array to store selected files
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    // Handles text input changes
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setAdData({
            ...adData,
            [name]: value
        });
    };

    // Handles file selection (critical part for media upload)
    const handleFileChange = (e) => {
        // e.target.files is a FileList object. Convert it to an array.
        setFiles(Array.from(e.target.files));
    };
    
    // --- Submission Logic: Using FormData for Files ---
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');
        setLoading(true);

        // 1. Create FormData object
        const formData = new FormData();

        // 2. Append all text fields from adData
        Object.keys(adData).forEach(key => {
            formData.append(key, adData[key]);
        });
        
        // 3. Append all files (Photos/Videos)
        files.forEach((file, index) => {
            // Note: Use the exact key your backend expects for file collections (e.g., 'Files' or 'Images')
            formData.append('Files', file); 
        });

        try {
            // API Endpoint: POST /api/cars/create
            // Important: We must not set 'Content-Type': 'application/json' for FormData. 
            // Axios handles setting 'multipart/form-data' automatically.
            const response = await apiClient.post('/cars/create', formData);
            
            setSuccess('Car listing successfully created! Redirecting to search...');
            console.log('Ad Creation Success:', response.data);
            
            setTimeout(() => {
                navigate('/search');
            }, 2500);

        } catch (err) {
            const errorMessage = err.response?.data?.title || 'Ad creation failed. Please check all fields and file sizes.';
            setError(errorMessage);
            console.error('Ad Creation Error:', err.response || err);
        } finally {
            setLoading(false);
        }
    };
    // ----------------------------------------------------

    return (
        <Container component="main" maxWidth="lg" sx={{ py: 4 }}>
            <Paper elevation={4} sx={{ p: 4 }}>
                <Typography component="h1" variant="h3" sx={{ color: DARK_GRAY, mb: 4, fontWeight: 700 }}>
                    Create New Listing
                </Typography>

                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                {success && <Alert severity="success" sx={{ mb: 2 }}>{success}</Alert>}

                <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
                    
                    {/* SECTION 1: Vehicle Details */}
                    <Typography variant="h5" sx={{ color: DARK_GRAY, mb: 2, borderBottom: '2px solid #eee' }}>
                        Vehicle Details
                    </Typography>
                    <Grid container spacing={3}>
                        <Grid item xs={12} sm={6}>
                            <TextField fullWidth required label="Listing Title" name="title" value={adData.title} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField fullWidth required label="Price ($)" name="price" type="number" value={adData.price} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={4}>
                            <TextField fullWidth required label="Make" name="make" value={adData.make} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={4}>
                            <TextField fullWidth required label="Model" name="model" value={adData.model} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={4}>
                            <TextField fullWidth required label="Year" name="year" type="number" value={adData.year} onChange={handleInputChange} />
                        </Grid>
                    </Grid>

                    {/* SECTION 2: Specifications and Location */}
                    <Typography variant="h5" sx={{ color: DARK_GRAY, mt: 4, mb: 2, borderBottom: '2px solid #eee' }}>
                        Specs & Location
                    </Typography>
                    <Grid container spacing={3}>
                        <Grid item xs={12} sm={4}>
                            <TextField fullWidth required label="Mileage (km)" name="mileage" type="number" value={adData.mileage} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={4}>
                            <TextField fullWidth required label="Engine Type" name="engineType" value={adData.engineType} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={4}>
                            <TextField fullWidth required label="Transmission Type" name="transmissionType" value={adData.transmissionType} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField fullWidth required label="City" name="city" value={adData.city} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                            <TextField fullWidth required label="Country" name="country" value={adData.country} onChange={handleInputChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                required
                                label="Description"
                                name="description"
                                multiline
                                rows={4}
                                value={adData.description}
                                onChange={handleInputChange}
                            />
                        </Grid>
                    </Grid>

                    {/* SECTION 3: Media Upload */}
                    <Typography variant="h5" sx={{ color: DARK_GRAY, mt: 4, mb: 2, borderBottom: '2px solid #eee' }}>
                        Photos & Videos
                    </Typography>
                    <input 
                        accept="image/*,video/*" 
                        style={{ display: 'none' }} 
                        id="media-upload-button" 
                        multiple 
                        type="file" 
                        onChange={handleFileChange}
                    />
                    <label htmlFor="media-upload-button">
                        <Button 
                            variant="outlined" 
                            component="span" 
                            startIcon={<FileUploadIcon />}
                            sx={{ color: DARK_GRAY, borderColor: DARK_GRAY }}
                        >
                            Upload Files (Max 15 total)
                        </Button>
                    </label>
                    <Box sx={{ mt: 2 }}>
                        {files.length > 0 ? (
                            <Typography variant="body1">
                                **{files.length}** file(s) selected.
                            </Typography>
                        ) : (
                            <Typography variant="body2" color="text.secondary">
                                No files selected.
                            </Typography>
                        )}
                        {/* You can add a preview of the file names here */}
                    </Box>

                    {/* SUBMIT BUTTON */}
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        disabled={loading}
                        size="large"
                        sx={{ mt: 5, backgroundColor: PRIMARY_YELLOW, color: DARK_GRAY, fontWeight: 'bold', 
                              '&:hover': { backgroundColor: '#FFC800' } }}
                    >
                        {loading ? <CircularProgress size={24} sx={{ color: DARK_GRAY }} /> : 'Publish Listing'}
                    </Button>
                </Box>
            </Paper>
        </Container>
    );
};

export default CreateAdPage;
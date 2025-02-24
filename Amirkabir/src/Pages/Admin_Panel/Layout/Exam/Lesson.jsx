import Modal from '../../../../Components/Modal/modal';
import ThemeProvider from '../../../../Components/Theme/ThemeProvider';
import React, { useState } from 'react';
import { TextField, Button, Select, MenuItem, InputLabel, FormControl, Typography, Box } from '@mui/material';

const CourseInformationForm = (props) => {
  const [url, setUrl] = useState('');
  const [level, setLevel] = useState('');
  const [image, setImage] = useState(null);
  const [dragOver, setDragOver] = useState(false);
  const [courseName, setCourseName] = useState('');

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    
    if (file) {
      setImage(URL.createObjectURL(file));
    }
  };

  const handleDrop = (e) => {
    e.preventDefault();
    const file = e.dataTransfer.files[0];
    
    if (file) {
      setImage(URL.createObjectURL(file));
    }
    
    setDragOver(false);
  };

  const handleDragOver = (e) => {
    e.preventDefault();
    setDragOver(true);
  };

  const handleSubmit = () => {
    const formData = new FormData();
    formData.append('courseName', courseName);
    formData.append('url', url);
    formData.append('level', level);
    
    console.log('Form submitted:', {
      courseName,
      url,
      level,
      image,
    });
  };

  return (
    <ThemeProvider>
      <Modal handleSubmit={props.handleSubmit}>
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            height: '100vh',
            background: 'linear-gradient(45deg, #ffffff 40%, #97e7e9)',
            padding: 3,
          }}
        >
          <Box
            sx={{
              maxWidth: 600,
              width: '100%',
              padding: 4,
              borderRadius: '8px',
              backgroundColor: '#ffffff',
              boxShadow: '0 -4px 20px rgba(0, 0, 0, 0.1)',
              transition: 'all 0.3s ease',
              '&:hover': {
                boxShadow: '0 -8px 30px rgba(0, 0, 0, 0.2)',
              },
            }}
          >
            <Typography
              variant="h5"
              gutterBottom
             
              sx={{
                fontWeight: 'bold',
                color: '#333',
                textAlign: 'center',
                marginBottom: 3,
                fontFamily: "'Roboto', sans-serif",
              }}
            >
              اطلاعات دوره
            </Typography>

            <TextField
              fullWidth
              label="نام دوره"
              value={courseName}
              onChange={(e) => setCourseName(e.target.value)}
              margin="normal"
             
              sx={{
                backgroundColor: '#f9f9f9',
                borderRadius: '4px',
                marginBottom: 2,
                '& .MuiInputBase-root': {
                  borderRadius: '6px',
                },
              }}
            />

            <TextField
              fullWidth
              label="لینک دوره"
              value={url}
              onChange={(e) => setUrl(e.target.value)}
              margin="normal"
              helperText="https://example.com/your-course"
            
              sx={{
                backgroundColor: '#f9f9f9',
                borderRadius: '4px',
                marginBottom: 2,
                '& .MuiInputBase-root': {
                  borderRadius: '6px',
                },
              }}
            />

            <FormControl fullWidth margin="normal" sx={{ backgroundColor: '#f9f9f9', borderRadius: '4px', marginBottom: 2 }}>
              <InputLabel>سطح</InputLabel>
              <Select
                value={level}
                onChange={(e) => setLevel(e.target.value)}
                sx={{
                  '& .MuiSelect-icon': {
                    color: '#1976d2',
                  },
                }}
              >
                <MenuItem value="مبتدی">مبتدی</MenuItem>
                <MenuItem value="متوسط">متوسط</MenuItem>
                <MenuItem value="پیشرفته">پیشرفته</MenuItem>
              </Select>
            </FormControl>

            <Box
              sx={{
                marginTop: 3,
                border: dragOver ? '2px solid #1976d2' : '2px dashed #cccccc',
                backgroundColor: dragOver ? '#e3f2fd' : '#f9f9f9',
                borderRadius: '8px',
                padding: 4,
                textAlign: 'center',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                transition: 'all 0.3s ease',
                minHeight: 150,
              }}

              onDrop={handleDrop}
              onDragOver={handleDragOver}
              onDragLeave={() => setDragOver(false)}
            >
              <Typography
                variant="body2"
                sx={{ marginBottom: 2, color: '#555', fontWeight: 'bold', fontSize: '16px' }}
              >
                تصویر را اینجا بکشید یا از کتابخانه انتخاب کنید
              </Typography>
              <Typography variant="caption" sx={{ color: '#aaa', marginBottom: 2 }}>
                JPG, PNG حداکثر ۲ گیگابایت
              </Typography>
             
              <Button
                variant="outlined"
                component="label"
                startIcon={<CloudUpload />}
               
                sx={{
                  textTransform: 'none',
                  marginBottom: 2,
                  borderColor: '#1976d2',
                  color: '#1976d2',
                  '&:hover': {
                    borderColor: '#1565c0',
                    color: '#1565c0',
                  },
                }}
              >
                بارگذاری تصویر
                
                <input
                  type="file"
                  hidden
                  onChange={handleImageUpload}
                  accept="image/png, image/jpeg"
                />
              </Button>

              {image && (
                <Box
                  sx={{
                    marginTop: 2,
                    border: '1px solid #ccc',
                    borderRadius: '8px',
                    overflow: 'hidden',
                    width: '100%',
                    maxWidth: 250,
                    transition: 'transform 0.3s ease, box-shadow 0.3s ease',
                    '&:hover': {
                      transform: 'scale(1.05)',
                      boxShadow: '0 -4px 15px rgba(0, 0, 0, 0.2)',
                    },
                  }}
                >
                  <img src={image} alt="Preview" style={{ width: '100%', height: 'auto' }} />
                </Box>
              )}
            </Box>

            <Button
              variant="contained"
              color="primary"
              fullWidth
              
              sx={{
                marginTop: 3,
                fontWeight: 'bold',
                textTransform: 'none',
                padding: '12px 0',
                borderRadius: '8px',
                '&:hover': {
                  backgroundColor: '#1565c0',
                },
              }}
              
              onClick={handleSubmit}
            >
              ایجاد
            </Button>
          </Box>
        </Box>
      </Modal>
    </ThemeProvider>
  );
};

export default CourseInformationForm;
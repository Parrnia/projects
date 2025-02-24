import Validation from '../../Add_Validation';
import Modal from '../../../../Components/Modal/modal';
import Alert from '../../../../Components/Inputs/Alert';
import TextInput from '../../../../Components/Inputs/TextField';
import contentImage from '../../../../Images/Admin/Content.jpg';
import { convertToPersianDate } from '../../../../Components/Date/Jalaali';
import React, { useState } from 'react';
import {Add, Edit, CloudUpload } from '@mui/icons-material';
import { Box, Button, Typography, Divider, List, ListItem, IconButton, Tooltip } from '@mui/material';

const Add_Lesson = (props) => {
  const [open, setOpen] = useState(false);
  const [errors, setErrors] = useState({});
  const [lessons, setLessons] = useState([]);
  const [dragOver, setDragOver] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const token = localStorage.getItem('token');
  const emailOrPhone = localStorage.getItem('emailOrPhone');

  const [values, setValues] = useState({
    title: "",
    videoLink: "",
    photoFile: null,
  });
   
  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setValues((prevValues) => ({ ...prevValues, [name]: value }));
  };

  const handleAddLesson = () => {        
    if(!props.id)
      setOpen(true);
    else
      setIsModalOpen(true);
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    
    setValues({
      title: "",
      videoLink: "",
      photoFile: null,
    });
    
    setErrors({});
  };

  const handleImageUpload = (e) => {
    const uploadedFile = e.target.files[0];
    
    if (uploadedFile)
      setValues((prevValues) => ({ ...prevValues, photoFile: uploadedFile }));
  };

  const handleDrop = (e) => {
    e.preventDefault();
    const uploadedFile = e.dataTransfer.files[0];
    
    if (uploadedFile)
      setValues((prevValues) => ({ ...prevValues, photoFile: uploadedFile }));
    
    setDragOver(false);
  };
  
  const handleDragOver = (e) => {
    e.preventDefault();
    setDragOver(true);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    const currentTime = new Date().toLocaleString();
    const date = convertToPersianDate(currentTime);

    const [err, isValid] = Validation(values);
    setErrors(err);

    if (isValid) 
    {
      const newLesson = {
        title: values.title,
        url: values.videoLink,
        icon: values.photoFile ? URL.createObjectURL(values.photoFile) : contentImage,
        updated: `${date[2]} ${date[1]} ${date[0]}`,
      };
  
      setLessons((prevLessons) => [...prevLessons, newLesson]);   
      handleModalClose();
    }
  };

  return (
    <Box sx={{backgroundColor: '#f9f9f9', borderRadius: 1 }}>
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          px: { xs: 1, sm: 1, md: 2, lg: 2, xl: 2 },
        }}
      >
        <Typography variant="subtitle1">
          دروس
        </Typography>
        <Button
          variant="contained"
          startIcon={<Add />}
          
          sx={{
            height: '40px',
            minWidth: '120px'
          }}
          
          onClick={handleAddLesson}
        >
          افزودن درس
        </Button>
      </Box>
      
      <Divider sx={{ marginBottom: 1, marginTop: { xs: '5%', md: '2%', xl:'2%' } }} />
      
      <List sx={{ borderRadius: 2, padding: 1, maxHeight: 240, overflowY: 'auto' }}>
        {lessons.map((lesson, index) => (
          <ListItem
            key={index}
              
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              border: '1px solid #ddd',
              borderRadius: 1,
              mb: 1,
              padding: 1.5,
            }}
          >
            <Box display="flex" alignItems="center">
              <img
                src={lesson.icon}
                alt={lesson.title}
                
                style={{
                  width: 50,
                  height: 50,
                  marginRight: 16,
                  marginLeft: 16,
                  borderRadius: 4
                }}
              />
              
              <Box sx={{ textAlign: 'right' }}>
                <Typography variant="body1" fontWeight="bold">
                  {lesson.title}
                </Typography>
                
                <Typography variant="body2" color="textSecondary">
                  به روز شده در تاریخ {lesson.updated}
                </Typography>
              </Box>
            </Box>
            <Tooltip title="ویرایش درس">
              <IconButton onClick={() => console.log('Edit clicked')}>
                <Edit />
              </IconButton>
            </Tooltip>
          </ListItem>
        ))}
      </List>

      {isModalOpen && (
        <Modal 
          open={isModalOpen} 
          onClose={handleModalClose}
          component={ 
            <Box>
              <Typography variant="h6" gutterBottom sx={{ fontWeight: 'bold', marginBottom: 3 }}>
                اطلاعات درس            
              </Typography>
              
              <TextInput
                name="title"
                label="نام درس"
                onChange={handleInputChange}
                value={values.title}
                error={errors.title}
                marginBottom={2}
              />
              
              <TextInput
                label="لینک درس"
                name="videoLink"
                onChange={handleInputChange}
                value={values.videoLink}
                error={errors.videoLink}
              />

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
                }}
                
                onDrop={handleDrop}
                onDragOver={handleDragOver}
                onDragLeave={() => setDragOver(false)}
              >
                <Typography variant="body2" sx={{ marginBottom: 2 }}>
                  برای آپلود تصویر، آن را اینجا بکشید یا کلیک کنید
                </Typography>
                
                <Button
                  variant="outlined"
                  component="label"
                  startIcon={<CloudUpload />}
                  sx={{ marginBottom: 2 }}
                >
                  بارگذاری تصویر
                  
                  <input
                    type="file"
                    hidden
                    onChange={handleImageUpload}
                    accept="image/png, image/jpeg"
                  />
                </Button>

                {values.photoFile && (
                  <Typography variant="body2" color="textSecondary" sx={{ marginTop: 2 }}>
                    فایل آپلود شده : {values.photoFile.name}
                  </Typography>
                )}

              </Box>

              <Button
                variant="contained"
                fullWidth
                sx={{ marginTop: 3 }}
                onClick={handleSubmit}
              >
                ایجاد
              </Button>
            </Box>
          }
        />
      )}
      
      <Alert open={open} setOpen={setOpen} severity="error" text="لطفا ابتدا دوره را ایجاد نمایید" />
    </Box>
  );
};

export default Add_Lesson;
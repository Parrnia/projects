import Validation from '../../Add_Validation';
import Add_Lesson from "../Lessons/Add_Lesson";
import Alert from '../../../../Components/Inputs/Alert';
import courseImage from '../../../../Images/Admin/Course.jpg';
import TextInput from '../../../../Components/Inputs/TextField';
import LayoutSidebar from '../../../../Components/Layout/LayoutSidebar';
import { AddCourseServices } from '../../../../Services/AddCourseServices';
import React, { useState, useRef } from 'react';
import { Box, Button, Typography, IconButton, Tooltip, Divider, Card, CardMedia, CardContent, Grid } from '@mui/material';
import { FormatBold, FormatItalic, FormatUnderlined, Title, FormatAlignLeft, FormatAlignRight, CloudUpload } from '@mui/icons-material';

const toolbarIcons = [
  { icon: <FormatAlignRight />, label: 'Align Right', action: 'justifyRight' },
  { icon: <FormatAlignLeft />, label: 'Align Left', action: 'justifyLeft' },
  { icon: <Title />, label: 'Title', action: 'formatBlock', value: 'h1' },
  { icon: <FormatUnderlined />, label: 'Underline', action: 'underline' },
  { icon: <FormatItalic />, label: 'Italic', action: 'italic' },
  { icon: <FormatBold />, label: 'Bold', action: 'bold' },
];

const Add_Course = () => {
  const [values, setValues] = useState({
    title: "",
    description: "",
    videoLink: "",
    photoFile: "",
  });

  const editorRef = useRef(null);
  const [id, setId] = useState("");
  const [open, setOpen] = useState(false);
  const [errors, setErrors] = useState({});
  const [dragOver, setDragOver] = useState(false);
  const [showError, setShowError] = useState(false);
  const [messageCreate, setMessageCreate] = useState("");

  const token = localStorage.getItem('token');
  const emailOrPhone = localStorage.getItem('emailOrPhone');

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setValues((prevValues) => ({ ...prevValues, [name]: value }));
  };

  const handleSubmitCourse = async (event) => {
    event.preventDefault();
    const text = editorRef.current.innerText;
    setValues((prevValues) => ({ ...prevValues, description: text }));

    const [err, isValid] = Validation(values);
    setErrors(err);

    if (isValid) 
    {
      try 
      {
        const data = new FormData();
        data.append('Title', values.title);
        data.append('Description', values.description || "توضیح ...");
        data.append('Type', "a");
        data.append('Context', "a");
        data.append('VideoLink', values.videoLink);
        data.append('DependentCourseId', "a");
        data.append('ShortContext', "a");

        const photoFileUrl = values.photoFile || courseImage;
        const res = await fetch(photoFileUrl);

        if (!res.ok) 
        {
          throw new Error(`Failed to fetch photo from URL: ${res.statusText}`);
        }

        const blob = await res.blob();
        data.append('PhotoFile', blob, 'photo.jpg');

        const response = await AddCourseServices(emailOrPhone, token, data);

        if (response.status == 201) 
        {
          setOpen(true);
          setMessageCreate("دوره با موفقیت ایجاد شد");
          setId(response.data.id);
          setShowError(false);
        }
        else
        {
          setOpen(true);
          setShowError(true);
          setMessageCreate(response.response.data.Message);
        }
      } 
      catch (error) 
      {
        setShowError(true);
      }
    } 
    else 
    {
      setShowError(true);
    }
  };

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    
    if (file) {
      setValues((prevValues) => ({ ...prevValues, photoFile: URL.createObjectURL(file) }));
    }
  };

  const handleDrop = (e) => {
    e.preventDefault();
    const file = e.dataTransfer.files[0];
    
    if (file) {
      setValues((prevValues) => ({ ...prevValues, photoFile: URL.createObjectURL(file) }));
    }
    
    setDragOver(false);
  };

  const handleDragOver = (e) => {
    e.preventDefault();
    setDragOver(true);
  };

  const handleToolbarAction = (action, value = null) => {
    if (editorRef.current) {
      editorRef.current.focus();
      document.execCommand(action, false, value);
    }
  };

  return (
    <LayoutSidebar
      component={
        <Box sx={{ display: 'flex',  flexDirection: 'column', gap: 0, p: { xs: 1, md: 5, xl: 8}, pb: 0 }}>
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              mb: 2,
              width: {xl:'70%', lg:'58%', md:'75%'},
              mr: { md: 7, lg: 18, xl:0 }
            }}
          >
            <Typography variant="h3" fontWeight="bold" justifyContent="center">
              افزودن دوره
            </Typography>
            
            <Box>
              <Button
                variant="contained"
                sx={{ height: '40px', minWidth: '120px'}}
                onClick={handleSubmitCourse}
              >
                ثبت
              </Button>
            </Box>
          </Box>

          <Grid container spacing={1} sx={{ justifyContent: { xs: 'center', sm: 'center', md: 'center', xl: 'flex-start' } }}>
            <Grid item xs={12} md={9} lg={7} xl={9}>
              <Box sx={{ p: { xs: 4, md: 4, xl:5 }, backgroundColor: '#f9f9f9', borderRadius: 1, width: '100%', maxWidth: '1000px' }}>
                <Typography variant="subtitle1" gutterBottom>
                  اطلاعات پایه    
                </Typography>
                
                <Divider sx={{ marginBottom: '3%' }} />

                <TextInput
                  name="title"
                  fullWidth
                  label="عنوان"
                  placeholder="عنوان"
                  onChange={handleInputChange}
                  value={values.title}
                  error={errors.title}
                  sx={{ mb: 2 }}
                />

                <Typography sx={{ marginTop: '3%'}}>توصیف</Typography>
                <Box sx={{
                  border: '1px solid #ddd', 
                  backgroundColor: '#fff', 
                  boxShadow: '0 1px 3px rgba(0,0,0,0.1)', 
                  minHeight: 150,
                  padding: 2, 
                  width: '100%' 
                }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', borderBottom: '1px solid #ddd', padding: 0.5, gap: 1,  justifyContent: { xs: 'center', sm: 'flex-end', md: 'flex-end', xl: 'flex-end' }  }}>
                    {toolbarIcons.map((tool, index) => (
                      <Tooltip key={index} title={tool.label} placement="top">
                        <IconButton 
                          sx={{
                            padding: 0.5, 
                            display: (tool.action === 'italic') ? { xs: 'none', xl: 'inline-flex' } : 'inline-flex'
                          }} 
                          
                          onClick={() => handleToolbarAction(tool.action, tool.value)}
                        >
                          {tool.icon}
                        </IconButton>
                      </Tooltip>
                    ))}
                  </Box>

                  <Box ref={editorRef} contentEditable suppressContentEditableWarning sx={{ minHeight: 120, padding: 1, outline: 'none', cursor: 'text', fontSize: '0.875rem' }}></Box>
                </Box>
              </Box>

              <Box sx={{ p: 3, backgroundColor: '#f9f9f9', borderRadius: 1, width: '100%', maxWidth: '1000px', mt: 3 }}>
                <Add_Lesson id={id}/>
              </Box>  
            </Grid>
            
            <Grid item xs={12} md={9} lg={7} xl={3}>
              <Card sx={{ borderRadius: 1 }}>
                <CardMedia
                  component="img"
                  src={values.photoFile || courseImage}
                  height="200"
                  alt="Course Image"
                />
                
                <CardContent>
                  <TextInput
                    fullWidth
                    name="videoLink"
                    placeholder="لینک ویدئو"
                    value={values.videoLink}
                    onChange={handleInputChange}
                    error={errors.videoLink}
                  />
                </CardContent>
                
                <CardContent>
                  <Box sx={{ border: dragOver ? '2px solid #1976d2' : '2px dashed #cccccc', borderRadius: 2, padding: 3, textAlign: 'center' }}
                    onDrop={handleDrop}
                    onDragOver={handleDragOver}
                    onDragLeave={() => setDragOver(false)}
                  >
                    <Typography variant="body2" sx={{ marginBottom: 2 }}>
                      برای آپلود تصویر، آن را اینجا بکشید یا کلیک کنید
                    </Typography>
                    <input type="file" hidden onChange={handleImageUpload} accept="image/*" id="image-upload" />
                    <label htmlFor="image-upload">
                      <Button variant="outlined" component="span" startIcon={<CloudUpload />}>
                        بارگذاری تصویر
                      </Button>
                    </label>
                  </Box>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
          
          <Alert
            open={open}
            setOpen={setOpen}
            severity={showError ? "error" : "success"} 
            text={messageCreate} 
          />
        </Box>
      }
    />
  );
};

export default Add_Course;
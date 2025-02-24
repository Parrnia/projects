import { ProfileServices } from "../../../Services/ProfileServices";
import { convertToPersianDate } from "../../../Components/Date/Jalaali";
import { Divider } from "@mui/material";
import React, { useState, useEffect } from 'react';
import { Box, Grid, Typography, Avatar } from '@mui/material';
import CalendarMonthTwoToneIcon from '@mui/icons-material/CalendarMonthTwoTone';

const Profile_Info = (props) => {
  const token = localStorage.getItem('token');
  const [profileData, setProfileData] = useState(null);
  const emailOrPhone = localStorage.getItem('emailOrPhone');
  const date = convertToPersianDate(profileData?.createTime);

  useEffect(() => {
    const fetchProfileData = async () => {

      if (token && emailOrPhone) 
      {
        try 
        {
          const response = await ProfileServices(emailOrPhone, token);
          setProfileData(response.data);
          console.log(response.data);
        } 
        catch
        {
        }
      } 
    };

    fetchProfileData();
  }, [token]);

  return (
    <Box
      sx={{
        p: 1,
        backgroundColor: "background.default",
        display: "flex",
        justifyContent: "center"
      }}
    >
      <Box
        sx={{
          width: "100%",
          border: "none"
        }}
      >
        <Grid container spacing={2} alignItems="center" justifyContent="flex-end">
          <Grid item xs>
            <Typography variant="h5" color="text.primary" fontWeight={700}>
              {profileData?.firstName} {profileData?.lastName}
            </Typography>
            
            <Typography
              variant="body2"
              color="text.secondary"
              sx={{ mt: 1, display: 'flex', alignItems: 'center' }}
            >
              <CalendarMonthTwoToneIcon style={{ fontSize: '20px' }} />
              <span>
                عضو شده در تاریخ 
                <span style={{ fontSize: '15px', marginRight: '5px' }}>{date[2]}</span> 
                <span style={{  marginRight: '5px' }}>{date[1]}</span>
                <span style={{ fontSize: '15px', marginRight: '5px' }}>{date[0]}</span> 
              </span>
            </Typography>
          </Grid>
        
          <Grid item>
            <Box
              sx={{
                position: "relative",
                width: 100,
                height: 100,
              }}
            >
              <Avatar
                src="https://via.placeholder.com/100" 
                alt="User Avatar"
                sx={{
                  width: 100,
                  height: 100,
                  border: theme => `3px solid ${theme.palette.primary.main}`,
                }}
              />

              <Box    
                sx={{
                  position: "absolute",
                  bottom: -5,
                  right: -5,
                  width: 30,
                  height: 30,
                  backgroundColor: theme => theme.palette.primary.main,
                  borderRadius: "50%",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  boxShadow: "0 0 5px rgba(0, 0, 0, 0.2)",
                  transition: "transform 0.2s ease-in-out, background-color 0.2s ease-in-out",
                  '&:hover': {
                    transform: 'scale(1.1)',
                    backgroundColor: theme => theme.palette.primary.dark,
                  }
                }}
              >
                <Typography
                  variant="caption"
                  color={theme => theme.palette.primary.contrastText}
                  fontWeight="bold"
                  
                  sx={{
                    fontSize: '18px',
                    cursor: 'pointer',
                    transition: 'transform 0.3s',
                  }}
                >
                  +
                </Typography>
              </Box>
            </Box>
          </Grid>
        </Grid>
        <Divider sx={{ mt: 2, borderWidth: 1 }} />
      </Box>
    </Box>
  );
};

export default Profile_Info;
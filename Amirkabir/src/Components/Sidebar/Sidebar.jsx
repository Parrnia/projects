import ThemeProvider from '../Theme/ThemeProvider';
import Amirkabir from "../../Images/Sidebar/Amirkabir.png";
import logout_icon from "../../Images/Sidebar/logout-icon.png";
import course_icon from "../../Images/Sidebar/courses-icon.png";
import profile_icon from "../../Images/Sidebar/profile-icon.png";
import missions_icon from "../../Images/Sidebar/missions-icon.png";
import settings_icon from "../../Images/Sidebar/settings-icon.png";
import exercise_icon from "../../Images/Sidebar/exercise-icon.png";
import leaderboard_icon from "../../Images/Sidebar/leaderboard-icon.png";
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import MenuIcon from '@mui/icons-material/Menu';
import { styled, useTheme } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';
import { Box, Drawer, List, Typography, ListItemButton, ListItemIcon, ListItemText, IconButton } from '@mui/material';

const drawerWidths = {
  xs: 240,
  sm: 260,
  md: 300
};

const StyledDrawer = styled(Drawer)(({ theme, width }) => ({
  '& .MuiDrawer-paper': {
    width: width,
    boxSizing: 'border-box',
    backgroundColor: theme.palette.background.paper,
    color: theme.palette.text.primary,
  },
}));

const StyledListItemButton = styled(ListItemButton)(({ theme }) => ({
  borderRadius: theme.shape.borderRadius,
  margin: theme.spacing(1, 2),
  '&:hover': {
    backgroundColor: theme.palette.action.hover,
  },
}));

export default function Sidebar() {
  const navigate = useNavigate();
  const theme = useTheme();
  const isLargeScreen = useMediaQuery(theme.breakpoints.up('md'));
  const [open, setOpen] = useState(false);

  const handleDrawerToggle = () => {
    setOpen(!open);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.setItem('isLogin', '0');
    localStorage.removeItem('emailOrPhone');
    navigate('/');
  };

  const menuItems = [
    { text: 'پروفایل', icon: profile_icon, action: () => navigate('/profile') },
    { text: 'دوره ها', icon: course_icon, action: () => navigate('/courses') },
    { text: 'تمرین', icon: exercise_icon, action: () => navigate('/exercise') },
    { text: 'لیدربورد', icon: leaderboard_icon, action: () => navigate('/leaderboard') },
    { text: 'ماموریت ها', icon: missions_icon, action: () => navigate('/missions') },
    { text: 'تنظیمات', icon: settings_icon, action: () => navigate('/settings') },
    { text: 'خروج', icon: logout_icon, action: handleLogout },
  ];

  const drawerContent = (
    <Box>
      <Box
        sx={{
          display: 'flex',
          padding: theme.spacing(3),
          justifyContent: 'center',
          alignItems: 'center',
        }}
      >
        <img src={Amirkabir} alt="Amirkabir Logo" style={{ width: '80%' }} />
      </Box>

      <List>
        {menuItems.map((item, index) => (
          <StyledListItemButton key={index} onClick={item.action}>
            <ListItemIcon>
              <img src={item.icon} alt={item.text} style={{ width: 24, height: 24 }} />
            </ListItemIcon>
            <ListItemText
              primary={
                <Typography
                  variant="body2"
                  sx={{
                    fontSize: '1rem',
                    textAlign: 'right',
                  }}
                >
                  {item.text}
                </Typography>
              }
            />
          </StyledListItemButton>
        ))}
      </List>
    </Box>
  );

  const drawerWidth = isLargeScreen ? drawerWidths.md : drawerWidths.sm;

  return (
    <ThemeProvider>
      {isLargeScreen ? (
        <StyledDrawer variant="permanent" anchor="right" width={drawerWidth}>
          {drawerContent}
        </StyledDrawer>
      ) : (
        <>
          <IconButton
            onClick={handleDrawerToggle}
            
            sx={{
              position: 'fixed',
              top: theme.spacing(2),
              right: theme.spacing(2),
              zIndex: theme.zIndex.drawer + 1,
            }}
          >
            <MenuIcon />
          </IconButton>
          <StyledDrawer
            variant="temporary"
            anchor="right"
            open={open}
            onClose={handleDrawerToggle}
            
            ModalProps={{
              keepMounted: true
            }}

            width={drawerWidth}
          >
            {drawerContent}
          </StyledDrawer>
        </>
      )}
    </ThemeProvider>
  );
}
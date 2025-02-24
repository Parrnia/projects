import { Stack } from '@mui/material';
import React, { useState } from "react";
import WestIcon from "@mui/icons-material/West";
import { useTheme } from '@mui/material/styles';
import DiamondIcon from "@mui/icons-material/Diamond";
import FavoriteIcon from "@mui/icons-material/Favorite";
import FlameIcon from "@mui/icons-material/LocalFireDepartment";
import { Box, Typography, List, ListItem, ListItemAvatar, ListItemText, Avatar, Tabs, Tab, Divider, IconButton } from "@mui/material";

const Follower_Following = () => {
  const followers = [
    { name: "ثنا مشایخ" },
    { name: "هانیه باقری" },
    { name: "فاطمه کاظمی" },
  ];

  const theme = useTheme();
  const [tabIndex, setTabIndex] = useState(0);

  const handleTabChange = (event, newValue) => {
    setTabIndex(newValue);
  };

  return (
    <Box
      sx={{
        width: "100%",
      }}
    >
      <Box sx={{ width: "100%" }}>
        <Box sx={{ width: "100%", display: 'flex', justifyContent: 'space-evenly', paddingBottom: 2 }}>
          <IconButton>
            <FlameIcon sx={{ color: "error.main", fontSize: 25 }} />
            <Typography sx={{ fontSize: 18, marginLeft: 0.5 }}>3</Typography>
          </IconButton>

          <IconButton>
            <DiamondIcon sx={{ color: "primary.main", fontSize: 25 }} />
            <Typography sx={{ fontSize: 18, marginLeft: 0.5 }}>1224</Typography>
          </IconButton>

          <IconButton>
            <FavoriteIcon sx={{ color: "error.main", fontSize: 25 }} />
            <Typography sx={{ fontSize: 18, marginLeft: 0.5 }}>3</Typography>
          </IconButton>
        </Box>
      </Box>
    
      <Box
        sx={{
          width: "100%",
          bgcolor: "#fff",
          borderRadius: 1,
          boxShadow: 3,
          padding: 2,
        }}
      >
        <Tabs
          value={tabIndex}
          onChange={handleTabChange}
          aria-label="Followers and Following Tabs"
          sx={{ width: '100%' }}
        >
          <Tab
            label={<span>34<br />دنبال کننده ‌ها</span>}
            
            sx={{
              whiteSpace: "normal", 
              lineHeight: "1.5",
              width: '50%',
              textAlign: 'center'
            }}
          />
          
          <Tab
              label={<span>4<br />دنبال شونده‌ها</span>}
            
              sx={{
                whiteSpace: "normal", 
                lineHeight: "1.5",
                width: '50%',
                textAlign: 'center'
              }}
          />
        </Tabs>

        <List>
          {followers.map((follower, index) => (
            <ListItem key={index}>
              <ListItemAvatar>
                <Avatar src={follower.avatar} />
              </ListItemAvatar>
              <ListItemText 
                primary={follower.name} 
                sx={{ textAlign: 'right' }} 
              />
            </ListItem>
          ))}
        </List>

        <Divider />

        <Stack direction="row" spacing={1} alignItems="center" sx={{ cursor: "pointer", textAlign: "right", marginTop: 1 }}>
          <Typography variant="body2" color="primary" sx={{ fontWeight: 'bold' }}>
            مشاهده 31 دنبال‌کننده دیگر
          </Typography>
          <IconButton sx={{ padding: 0, color: theme.palette.primary.main }}>
            <WestIcon />
          </IconButton>
        </Stack>
      </Box>
    </Box>
  );
};

export default Follower_Following;

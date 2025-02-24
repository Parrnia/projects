import Sidebar from '../../Components/Sidebar/Sidebar';
import Amirkabir from "../../Images/Firend/Amirkabir.png";
import ThemeProvider from '../../Components/Theme/ThemeProvider';
import { SearchFriendServices } from '../../Services/SearchFriendServices';
import React, { useState, useEffect } from 'react';
import { Search as SearchIcon, PersonAdd as FollowIcon } from '@mui/icons-material';
import { TextField, Box, Divider, InputAdornment, IconButton, Button, Avatar, Typography, List, ListItem, ListItemAvatar, ListItemText } from '@mui/material';

const SearchFriend = () => {
  const [email, setEmail] = useState('');
  const [verified, setVerified] = useState(false);
  const [searchResults, setSearchResults] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    const token = localStorage.getItem('token');
    const email = localStorage.getItem('emailOrPhone');

    if (email) setEmail(email);
    if (token) setVerified(token);
  }, []);

  const handleSearch = async () => {
    try 
    {
      if (searchQuery) 
      {
        const response = await SearchFriendServices(email, verified, searchQuery);

        if (response.status === 200) 
        {
          const formattedResults = response.data.map((item) => ({
            id: item.id,
            name: `${item.firstName} ${item.lastName}`,
            avatarUrl: item.profilePicture || '',
          }));

          setSearchResults(formattedResults);
        }
      }
    } 
    catch (error) 
    {
    }
  };

  return (
    <>
      <Sidebar />
      
      <ThemeProvider>
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', p: 2 }}>
          <Box
            sx={{
              display: 'flex',
              flexDirection: { xs: 'column', sm: 'row' },
              alignItems: 'center',
              width: '100%',
              gap: 2,
            }}
          >
            <TextField
              variant="outlined"
              placeholder="نام یا نام کاربری"
              size="medium"
              fullWidth
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <IconButton>
                      <SearchIcon />
                    </IconButton>
                  </InputAdornment>
                ),
                sx: { height: '50px', padding: 0, textAlign: 'right' },
              }}
            />
            <Button
              variant="contained"
              sx={{ height: '50px', minWidth: '100px' }}
              onClick={handleSearch}
            >
              جستجو
            </Button>
          </Box>

          <Divider sx={{ my: 4, width: '100%' }} />

          <List sx={{ width: '100%', direction: 'rtl' }}>
            {searchResults.map((result) => (
              <ListItem
                key={result.id}
                
                sx={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  p: 1,
                  border: '1px solid #e0e0e0',
                  borderRadius: '8px',
                  mb: 1,
                  flexDirection: { xs: 'column', sm: 'row' },
                }}
              >
                <ListItemAvatar>
                  <Avatar src={result.avatarUrl} alt={result.name} />
                </ListItemAvatar>
                <ListItemText
                  primary={<Typography sx={{ fontWeight: 'bold', textAlign: 'right' }}>{result.name}</Typography>}
                  secondary={<Typography sx={{ textAlign: 'right' }}>{result.username}</Typography>}
                />
                <Button
                  variant="outlined"
                  startIcon={<FollowIcon />}
                  sx={{ minWidth: '80px', alignSelf: { xs: 'flex-start', sm: 'center' } }}
                >
                  دنبال کردن
                </Button>
              </ListItem>
            ))}
          </List>

          {!searchResults.length &&
            <Box sx={{ width: '100%', display: 'flex', justifyContent: 'center', mt: 4 }}>
              <img src={Amirkabir} alt="Amirkabir" />
            </Box>
          }
        </Box>
      </ThemeProvider>
    </>
  );
};

export default SearchFriend;
import React from "react";
import Add_Friends from "./Add_Friends";
import SearchFriend from "./SearchFirend";
import Sidebar from "../../Components/Sidebar/Sidebar";
import ThemeProvider from "../../Components/Theme/ThemeProvider";
import DiamondIcon from "@mui/icons-material/Diamond";
import FavoriteIcon from "@mui/icons-material/Favorite";
import { Box, Typography, IconButton } from "@mui/material";
import FlameIcon from "@mui/icons-material/LocalFireDepartment";

const Friend = () => {
  return (
    <ThemeProvider>
      <Box sx={{ display: "flex", flexDirection: "row", width: "100%", flexWrap: 'wrap' }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            width: { xs: "100%", sm: "0%", md: "19%" },
          }}
        />
        
        <Sidebar />

        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            width: { xs: "100%", sm: "50%", md: "50%" },
            padding: "16px",
            gap: 2,
            p:5,
            marginLeft: "3%",
          }}
        >
          <SearchFriend />
        </Box>

        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            marginLeft: "2%",
            width: { xs: "100%", sm: "25%", md: "25%" },
            padding: "16px",
            p:6,
            justifyContent: "center",
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
        
          <Add_Friends title="راه ارتباطی دیگر" />
        </Box>
      </Box>
    </ThemeProvider>
  );
};

export default Friend;
import React from "react";
import { Box } from "@mui/material";
import Add_Friends from "../Firend/Add_Friends";
import Profile_Info from "./Layout/Profile_Info";
import Profile_Status from "./Layout/Profile_Status";
import Sidebar from "../../Components/Sidebar/Sidebar";
import Follower_Following from "./Layout/Follower_Following";
import ThemeProvider from "../../Components/Theme/ThemeProvider";
import Profile_Achievements from "./Layout/Profile_Achievements";

const Profile = () => {
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
          <Profile_Info />
          <Profile_Status />
          <Profile_Achievements />
        </Box>

        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            marginLeft: "2%",
            width: { xs: "100%", sm: "25%", md: "25%" },
            padding: "16px",
            gap: 6,
            p:6,
            justifyContent: "center",
          }}
        >
          <Follower_Following />
          <Add_Friends title="افزودن دوستان" inviteFirend={true} />
        </Box>
      </Box>
    </ThemeProvider>
  );
};

export default Profile;
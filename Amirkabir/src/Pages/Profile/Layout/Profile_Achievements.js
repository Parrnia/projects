import Achievement from "./Achievement/Achievement";
import React from "react";
import { Box, Typography, Paper } from "@mui/material";

const Profile_Achievements = () => {
  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        p: 1,
        flexDirection: "column",
        width: "100%",
      }}
    >
      <Box sx={{ width: "100%" }}>
        <Typography
          variant="h6"
          color="text.primary"
          fontWeight={700}
          mb={2}
          sx={{
            textAlign: "right",
          }}
        >
          دستاوردها
        </Typography>
      </Box>

      <Box sx={{ width: "100%" }}>
        <Paper
          elevation={3}
          sx={{
            p: 3,
            backgroundColor: "background.paper",
            borderRadius: 2,
            boxSizing: "border-box",
          }}
        >
          <Achievement title="عنوان دستاورد" progress={17} goal={30} mb={3} />
          <Achievement title="عنوان دستاورد" progress={17} goal={30} />
        </Paper>
      </Box>
    </Box>
  );
};

export default Profile_Achievements;
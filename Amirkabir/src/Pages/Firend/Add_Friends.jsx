import InviteFriends from "../InviteFriends/InviteFriends";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import WestIcon from "@mui/icons-material/West";
import { useTheme } from "@mui/material/styles";
import SearchIcon from "@mui/icons-material/Search";
import MailOutlineIcon from "@mui/icons-material/MailOutline";
import { Box, Typography, Stack, IconButton } from "@mui/material";

const Add_Friends = (props) => {
  const theme = useTheme();
  const navigate = useNavigate();
  const [showInviteFriends, setShowInviteFriends] = useState(false);

  const handleInviteClick = () => {
    setShowInviteFriends(true);
  };

  return (
    <Box
      sx={{
        width: "100%",
        bgcolor: theme.palette.background.paper,
        borderRadius: 1,
        boxShadow: 3,
        border: `1px solid ${theme.palette.divider}`,
        padding: "3%",
        display: "flex",
        flexDirection: "column",
        gap: 2,
      }}
    >
      <Typography
        variant="h6"

        sx={{
          textAlign: "right",
          fontWeight: "bold",
          color: "black",
        }}
      >
        {props.title}
      </Typography>

      {props.inviteFirend && (
        <Stack
          direction="row"
          alignItems="center"

          sx={{
            padding: 1,
            border: "none",
            borderRadius: 1,
            cursor: "pointer",
            "&:hover": {
              backgroundColor: theme.palette.action.hover,
            },
            gap: 0.5,
            justifyContent: "space-between",
          }}
          
          onClick={() => navigate("/searchfriend")}
        >
          <Stack direction="row" alignItems="center" gap={0.5}>
            <IconButton sx={{ color: theme.palette.primary.main, padding: 0 }}>
              <SearchIcon />
            </IconButton>
            <Typography sx={{ fontWeight: "bold" }}>پیدا کردن دوستان</Typography>
          </Stack>

          <IconButton
            sx={{
              padding: 0,
              color: theme.palette.primary.main,
            }}
          >
            <WestIcon />
          </IconButton>
        </Stack>
      )}

      <Stack
        direction="row"
        alignItems="center"
        sx={{
          padding: 1,
          border: "none",
          borderRadius: 1,
          cursor: "pointer",
          "&:hover": {
            backgroundColor: theme.palette.action.hover,
          },
          gap: 0.5,
          justifyContent: "space-between",
        }}
        onClick={handleInviteClick}
      >
        <Stack direction="row" alignItems="center" gap={0.5}>
          <IconButton sx={{ color: theme.palette.primary.main, padding: 0 }}>
            <MailOutlineIcon />
          </IconButton>
          <Typography sx={{ fontWeight: "bold" }}>دعوت از دوستان</Typography>
        </Stack>

        <IconButton
          sx={{
            padding: 0,
            color: theme.palette.primary.main,
          }}
        >
          <WestIcon />
        </IconButton>
      </Stack>

      {showInviteFriends && <InviteFriends open={showInviteFriends} onClose={() => setShowInviteFriends(false)} />}
    </Box>
  );
};

export default Add_Friends;
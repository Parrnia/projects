import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import img from './A.png';
import img1 from './f.png'; // Example second image
import img2 from './f1.png'; // Example third image
import "./azmon.css";
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Button,
  LinearProgress,
} from "@mui/material";
import FavoriteIcon from "@mui/icons-material/Favorite";

const Final = () => {
  const navigate = useNavigate(); // For navigation
  const location = useLocation(); // Get the location object to access passed state

  // Get heartCount from location state
  const heartCount = location.state?.heartCount || 0; // Default to 0 if no heartCount is passed

  return (
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(180deg, #8DDBDB 0%, rgba(141, 219, 219, 0) 100%)", // New gradient background
        backgroundSize: "cover",
        backgroundRepeat: "no-repeat",
        backgroundAttachment: "fixed",
        display: "flex",
        flexDirection: "column",
      }}
    >
      {/* Header */}
      <AppBar position="fixed" sx={{ backgroundColor: "#FFFFFF", color: "#fff" }}>
        <Toolbar sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
          {/* Left Side: Heart Icon */}
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FavoriteIcon sx={{ color: "red", mr: 1 }} />
            <Typography variant="body1" sx={{ color: "black" }}>{heartCount}</Typography>
          </Box>

          {/* Center: Progress Bar */}
          <Box sx={{ flex: 1, mx: 2 }}>
            <LinearProgress
              variant="determinate"
              value={100} // Full progress bar
              sx={{
                height: 10,
                borderRadius: 5,
                backgroundColor: "#e0e0e0",
                "& .MuiLinearProgress-bar": {
                  backgroundColor: "#ffc400",
                },
              }}
            />
          </Box>

          {/* Right Side: Title */}
          <Typography variant="h6" sx={{ textAlign: "left", whiteSpace: "nowrap", color: "#0DBBBF" }}>
            امیرکبیر
          </Typography>
        </Toolbar>
      </AppBar>

      {/* Main Content */}
      <Box
        sx={{
          flexGrow: 1,
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
          textAlign: "center",
          mt: 10,
          mb: 8,
        }}
      >
        {/* Heart counter */}
        <Box
  sx={{
    backgroundColor: "#fff",
    borderRadius: 5,
    boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.1)",
    px: 6,  // Increase horizontal padding
    py: 4,  // Increase vertical padding
    maxWidth: "60%", // Make the box wider
    textAlign: "center",
    mb: 6, // Increase margin-bottom
  }}
>


  {/* Render hearts with larger size */}
  <Box sx={{ display: "flex", justifyContent: "center", gap: 2 }}>
    {[...Array(5)].map((_, index) => (
      <FavoriteIcon
        key={index}
        sx={{
          color: index < heartCount ? "red" : "transparent",
          border: "2px solid red", // Increase border size
          borderRadius: "50%",
          padding: "5px", // Make padding larger
          fontSize: "2.5rem", // Increase heart icon size
        }}
      />
    ))}
  </Box>
  <Typography
  variant="h1"
  sx={{
    color: "#333",
    mt: 3,
    fontSize: "1.8rem",
    fontFamily: "'Vazir', sans-serif", // استفاده از فونت فارسی (مثال: Vazir)
    fontWeight: 700, // ضخامت فونت
    lineHeight: 1.6, // ارتفاع خط
    textAlign: "center", // متن وسط‌چین
    textShadow: "1px 1px 2px rgba(0, 0, 0, 0.1)", // سایه متن
  }}
>
  شما ۱۰۰ امتیاز به دست آوردید!
</Typography>

<Typography
  variant="h1"
  sx={{
    color: "#333",
    mt: 3,
    fontSize: "1.8rem",
    fontFamily: "'Vazir', sans-serif", // استفاده از فونت فارسی (مثال: Vazir)
    fontWeight: 700, // ضخامت فونت
    lineHeight: 1.6, // ارتفاع خط
    textAlign: "center", // متن وسط‌چین
    textShadow: "1px 1px 2px rgba(0, 0, 0, 0.1)", // سایه متن
  }}
>
  {heartCount} تعداد قلب ها
</Typography>
</Box>

      </Box>

      {/* Footer */}
      <Box
        sx={{
          position: "fixed",
          bottom: 0,
          width: "100%",
          backgroundColor: "#f5f5f5",
          borderTop: "1px solid #e0e0e0",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          px: 3,
          py: 2,
        }}
      >
     
        <Button
          variant="contained"
          color="secondary"
          className="b2"
          onClick={() => navigate("/profile")}
        >
        بازگشت به صفحه پروفایل
        </Button>
      </Box>
    </Box>
  );
};

export default Final;

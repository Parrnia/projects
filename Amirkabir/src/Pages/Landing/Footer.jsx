import React from "react";
import { Box, Typography, Grid, Divider, Button, IconButton } from "@mui/material";
import TelegramLogo from "../../Images/Landing/icons8-telegram-240.png";
import InstagramLogo from "../../Images/Landing/icons8-instagram-256.png";
import TwitterLogo from "../../Images/Landing/icons8-twitter-240.png";
import YouTubeLogo from "../../Images/Landing/icons8-youtube-240.png";

const Footer = () => {
  return (
    <Box sx={{ backgroundColor: "#2d3748", padding: "20px 40px", textAlign: "center" }}>
      <Grid container spacing={3} sx={{ justifyContent: "space-between", flexWrap: "wrap", marginBottom: "20px" }}>
        <Grid item xs={12} sm={6} md={3}>
          <Typography variant="h6" sx={{ fontSize: "22px", fontWeight: "bold", marginBottom: "10px", color: "white" }}>
            امیرکبیر
          </Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>
            امیرکبیر اولین و بروزترین وبسایت آموزشی رایگان در سطح ایران است که همیشه تلاش کرده تا بتواند جدیدترین و بروزترین آموزش ها و مقالات را در اختیار علاقه مندان قرار دهد.
          </Typography>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Typography variant="h6" sx={{ fontSize: "22px", fontWeight: "bold", marginBottom: "10px", color: "white" }}>
            بخش های سایت
          </Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>درباره امیرکبیر</Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>ارتباط با ما</Typography>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Typography variant="h6" sx={{ fontSize: "22px", fontWeight: "bold", marginBottom: "10px", color: "white" }}>
            دسترسی سریع
          </Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>اخبار</Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>تالار گفت و گو</Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>میکروکست</Typography>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Typography variant="h6" sx={{ fontSize: "22px", fontWeight: "bold", marginBottom: "10px", color: "white" }}>
            ارتباط با ما
          </Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>ایمیل : example@gmail.com</Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>اینستاگرام : example</Typography>
          <Typography sx={{ fontSize: "18px", lineHeight: 1.5, marginBottom: "8px", color: "white" }}>شماره تماس : 1234567</Typography>
        </Grid>
      </Grid>

      <Divider sx={{ margin: "20px 0", backgroundColor: "white" }} />

      <Box sx={{ display: "flex", justifyContent: "center", gap: "15px", marginBottom: "15px" }}>
        <IconButton><img src={TelegramLogo} alt="Telegram" style={{ width: "32px", height: "32px" }} /></IconButton>
        <IconButton><img src={InstagramLogo} alt="Instagram" style={{ width: "32px", height: "32px" }} /></IconButton>
        <IconButton><img src={TwitterLogo} alt="Twitter" style={{ width: "32px", height: "32px" }} /></IconButton>
        <IconButton><img src={YouTubeLogo} alt="YouTube" style={{ width: "32px", height: "32px" }} /></IconButton>
      </Box>

      <Button sx={{ backgroundColor: "#1976d2", color: "white", padding: "10px 20px", borderRadius: "8px", fontSize: "18px", fontWeight: "bold", '&:hover': { backgroundColor: "#155a9e" } }}>
        دانلود اپلیکشن
      </Button>
    </Box>
  );
};

export default Footer;

import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import img2 from "./f1.png";
import img from "./A.png";
import { Card, CardContent } from "@mui/material";
import { Diamond, GpsFixed } from "@mui/icons-material";
import "./azmon.css";
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Button,
  LinearProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  CircularProgress,
} from "@mui/material";
import FavoriteIcon from "@mui/icons-material/Favorite";
import { fetchUserExamResults } from "../../Services/azmonresultsevice";
import { fetchUserScore } from '../../Services/azmonscoreService';

const Azmon1 = () => {
  const navigate = useNavigate();
  const location = useLocation();
  
  // Ensure location.state is properly initialized
  const { heartCount = 0, examId } = location.state || {};

  const token = localStorage.getItem("token");
  const username = localStorage.getItem("emailOrPhone");

  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [examResults, setExamResults] = useState(null);
  const [error, setError] = useState(null);
  const [userScore, setUserScore] = useState(null);

  const handleOpen = async () => {
    setLoading(true);
    setError(null);
    const examIdFromStorage = examId;
    console.log(examIdFromStorage);
    try {
      const response = await fetchUserExamResults(username, token, examIdFromStorage);
      console.log("Fetched exam results:", response);
      setExamResults(response[0]);
    } catch (err) {
      setError("خطا در دریافت نتایج آزمون! لطفاً دوباره تلاش کنید.");
    } finally {
      setLoading(false);
      setOpen(true);
    }
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleFinal = async () => {
    try {
      const scoreResponse = await fetchUserScore(username, token);
      setUserScore(scoreResponse.points);
   
      navigate("/final", { state: { heartCount: scoreResponse.hearts } });
    } catch (err) {
      console.error("Error fetching user score:", err);
    }
  };

  // Calculate the percentage of correct answers
  const calculateCorrectPercentage = () => {
    if (!examResults || !examResults.userAnswers) return 0;
    const totalQuestions = examResults.userAnswers.length;
    const correctAnswers = examResults.userAnswers.filter(answer => answer.isCorrectAnswer).length;
    return ((correctAnswers / totalQuestions) * 100).toFixed(2);
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(180deg, #8DDBDB 0%, rgba(141, 219, 219, 0) 100%)",
        display: "flex",
        flexDirection: "column",
      }}
    >
      <Box
        component="img"
        src={img}
        alt="Left Decorative"
        sx={{
          position: "absolute",
          left: "5%",
          top: "75%",
          transform: "translateY(-50%)",
          width: 150,
          height: "auto",
        }}
      />

      <AppBar position="fixed" sx={{ backgroundColor: "#FFFFFF", color: "#000" }}>
        <Toolbar sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FavoriteIcon sx={{ color: "red", mr: 1 }} />
            <Typography variant="body1">{heartCount}</Typography>
          </Box>

          <Box sx={{ flex: 1, mx: 2 }}>
            <LinearProgress
              variant="determinate"
              value={100}
              sx={{
                height: 10,
                borderRadius: 5,
                backgroundColor: "#e0e0e0",
                "& .MuiLinearProgress-bar": { backgroundColor: "#ffc400" },
              }}
            />
          </Box>

          <Typography variant="h6" sx={{ color: "#0DBBBF" }}>
            امیرکبیر
          </Typography>
        </Toolbar>
      </AppBar>

      <Box
        sx={{
          flexGrow: 1,
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
          mt: 10,
          mb: 8,
        }}
      >
        <Box
          sx={{
            backgroundColor: "#fff",
            borderRadius: 5,
            boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.1)",
            px: 4,
            py: 3,
            maxWidth: "50%",
            textAlign: "center",
          }}
        >
          <Typography variant="h5" sx={{ mb: 3, color: "#333" }}>
            شما این دوره با موفقیت به اتمام رسانید
          </Typography>
          <Box
      display="flex"
      justifyContent="center"
      alignItems="center"
      gap={2}
      sx={{ flexWrap: "wrap" }}
    >
      {/* Card 1 */}
      <Card sx={{ width: 200, textAlign: "center", backgroundColor: "#4dd0e1" }}>
        <CardContent>
          <Typography variant="h6" sx={{ color: "#fff" }}>
            مجموع امتیازات
          </Typography>
          <Diamond sx={{ fontSize: 40, color: "#fff", marginY: 1 }} />
          <Typography variant="h5" sx={{ color: "#fff" }}>
            {userScore || 100}
          </Typography>
        </CardContent>
      </Card>

      {/* Card 2 */}
      <Card sx={{ width: 200, textAlign: "center", backgroundColor: "#546e7a" }}>
        <CardContent>
          <Typography variant="h6" sx={{ color: "#fff" }}>
            درصد پاسخ‌دهی
          </Typography>
          <GpsFixed sx={{ fontSize: 40, color: "#fff", marginY: 1 }} />
          <Typography variant="h5" sx={{ color: "#fff" }}>
            {calculateCorrectPercentage()}%
          </Typography>
        </CardContent>
      </Card>
    </Box>
        </Box>
      </Box>

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
        <Button variant="contained" color="primary" onClick={handleOpen}>
          مرور نتایج آزمون
        </Button>
        <Button
          variant="contained"
          color="secondary"
          onClick={handleFinal}
        >
          پایان آزمون
        </Button>
      </Box>
      <Dialog open={open} onClose={handleClose}>
  <DialogTitle>نتایج آزمون</DialogTitle>
  <DialogContent>
    {loading ? (
      <CircularProgress />
    ) : error ? (
      <Typography color="error">{error}</Typography>
    ) : examResults?.userAnswers?.length > 0 ? (
      <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
        {examResults.userAnswers.map((answer, index) => (
          <Box
            key={index}
            sx={{
              backgroundColor: answer.isCorrectAnswer ? "#e6ffed" : "#f8d7da",
              padding: 2,
              borderRadius: 2,
              border: answer.isCorrectAnswer ? "1px solid #c3e6cb" : "1px solid #f5c6cb",
            }}
          >
            <Typography variant="body1">{`سوال ${index + 1}: ${answer.questionText}`}</Typography>
            <Typography variant="body2" color={answer.isCorrectAnswer ? "green" : "red"}>
              {answer.isCorrectAnswer ? "✅ پاسخ شما درست بود." : "❌ پاسخ شما نادرست بود."}
            </Typography>
            <Typography variant="body2">
              پاسخ صحیح: {" "}
              {answer.options.find((opt) => opt.id === answer.userAnswerOptionId)?.text || "نامشخص"}
            </Typography>
          </Box>
        ))}
      </Box>
    ) : (
      <Typography>نتایجی برای نمایش وجود ندارد.</Typography>
    )}
  </DialogContent>
  <DialogActions>
    <Button onClick={handleClose}>بستن</Button>
  </DialogActions>
</Dialog>
    </Box>
  );
};

export default Azmon1;
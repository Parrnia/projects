import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import img from './A.png';
import axios from "axios";
import { fetchExam } from '../../Services/QuestionService'; // Import services
import { submitUserAnswer } from '../../Services/submitanswerService';
import { fetchUserScore } from '../../Services/azmonscoreService';
import { useLocation } from "react-router-dom";

import img1 from './a1.png';
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Radio,
  RadioGroup,
  FormControlLabel,
  Card,
  CardContent,
  LinearProgress,
  IconButton,
} from "@mui/material";
import FavoriteIcon from "@mui/icons-material/Favorite";
import CloseIcon from "@mui/icons-material/Close";

const Azmon = () => {
  const location = useLocation();
  const { examId } = location.state || {}; // دریافت examId از state

  const [selectedOption, setSelectedOption] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [videoLink, setVideoLink] = useState("");
  const [heartCount, setHeartCount] = useState(5);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [isAlreadyPassedPopupOpen, setIsAlreadyPassedPopupOpen] = useState(false); // New popup state
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [answerSubmitted, setAnswerSubmitted] = useState(false);
  const [answers, setAnswers] = useState([]);
  const [examId1, setExamId1] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [questions, setQuestions] = useState([]);
  const navigate = useNavigate();
  const [feedback, setFeedback] = useState(null);
  const [isNextButtonDisabled, setIsNextButtonDisabled] = useState(false); // New state for disabling next button
  const [showLessonButton, setShowLessonButton] = useState(false); // New state for showing lesson button

  useEffect(() => {
    const fetchExamDetails = async () => {
      try {
        const token = localStorage.getItem("token");
        const username = localStorage.getItem("emailOrPhone");
        const examIdFromStorage = examId; // Replace with dynamic ID if needed

        // Fetch exam data
        const examData = await fetchExam(examIdFromStorage, username, token);
        setExamId1(examIdFromStorage ); // Assuming the API returns an examId in the response
        setQuestions(examData.questions); // Set the questions from the API response
      } catch (error) {
        setError(error.message || "An error occurred while fetching exam details.");
      } finally {
        setLoading(false);
      }
    };

    fetchExamDetails();
  }, []);

  useEffect(() => {
    const fetchHearts = async () => {
      try {
        const token = localStorage.getItem("token");
        const username = localStorage.getItem("emailOrPhone");

        // Fetch user score
        const userData = await fetchUserScore(username, token);
        setHeartCount(userData.hearts); // Update hearts from API response
      } catch (error) {
        console.error("Error fetching user score:", error.message);
      }
    };

    fetchHearts();
  }, []);

  useEffect(() => {
    if (heartCount === 0) {
      setIsPopupOpen(true);
    }
  }, [heartCount]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  const handleOptionChange = (event) => {
    setSelectedOption(event.target.value);
  };

  const checkAnswer = async () => {
    const token = localStorage.getItem("token");
    const username = localStorage.getItem("emailOrPhone");
    const question = questions[currentQuestionIndex];

    try {
      // Submit user answer
      const response = await submitUserAnswer(
        examId,
        question.id,
        parseInt(selectedOption.replace("option", "")),
        username,
        token
      );

      if (response.isPassed) {
        // Correct answer
        setAnswers((prevAnswers) => [
          ...prevAnswers,
          {
            question: question.text,
            userAnswer: selectedOption,
            isCorrect: true,
          },
        ]);
        setFeedback("correct");
        setAnswerSubmitted(true);
        setShowLessonButton(true); // Show lesson button
      } else {
        // Incorrect answer
        setAnswers((prevAnswers) => [
          ...prevAnswers,
          {
            question: question.text,
            userAnswer: selectedOption,
            isCorrect: false,
          },
        ]);
        setFeedback("incorrect");
        setVideoLink(response.videoLink);
        setIsModalOpen(true);
        setHeartCount((prevHeartCount) => prevHeartCount - 1);
        setIsNextButtonDisabled(true); // Disable next button
      }
    } catch (error) {
      if (error.response?.status === 500 && error.response.data?.Message === "شما قبلا این آزمون را قبول شده اید.") {
        setIsAlreadyPassedPopupOpen(true);
      } else {
        console.error("Error submitting answer:", error.message);
      }
    }
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setIsNextButtonDisabled(false); // Enable next button after closing modal
  };

  const closePopup = () => {
    setIsPopupOpen(false);
    navigate("/profile"); // Navigate to profile page after closing the popup
  };

  const previousQuestion = () => {
    if (!answerSubmitted && currentQuestionIndex > 0) {
      setCurrentQuestionIndex(currentQuestionIndex - 1);
      setSelectedOption(null);
      setFeedback(null);
      setAnswerSubmitted(false);
      setShowLessonButton(false); // Hide lesson button
    }
  };
  
  const nextQuestion = () => {
    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
      setSelectedOption(null);
      setFeedback(null);
      setAnswerSubmitted(false);
      setShowLessonButton(false); // Hide lesson button
    } else {
      navigate("/azmon1", { state: { answers, heartCount } });
    
    }
  };
  
  const handleVideoWatched = () => {
    setIsModalOpen(false);
    setAnswerSubmitted(true);
    setIsNextButtonDisabled(false); // Enable next button after watching video
  };

  return (
    <Box
      sx={{
        position: "relative",
        minHeight: "100vh",
        backgroundImage: "linear-gradient(to right, #f5f7fa, #e2e8f0)",
        display: "flex",
        flexDirection: "column",
      }}
    >
      <AppBar position="fixed" sx={{ backgroundColor: "#FFFFFF", color: "#fff" }}>
        <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FavoriteIcon sx={{ color: "red", mr: 1 }} />
            <Typography variant="body1" sx={{ color: "black" }}>{heartCount}</Typography>
          </Box>
          <Box sx={{ flex: 1, mx: 2 }}>
            <LinearProgress
              variant="determinate"
              value={(currentQuestionIndex / questions.length) * 100}
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
          <Typography variant="h6" sx={{ color: "#0DBBBF" }}>
            امیرکبیر
          </Typography>
        </Toolbar>
      </AppBar>

      <Box
        sx={{
          flexGrow: 1,
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          mt: 10,
          mb: 8,
        }}
      >
        <Card sx={{ width: "80%", maxWidth: 600, p: 3 }}>
          <CardContent>
            <Typography
              variant="h6"
              sx={{
                mb: 2,
                fontWeight: "bold",
                fontFamily: "'Tahoma', sans-serif",
                lineHeight: 1.5,
              }}
            >
              {questions[currentQuestionIndex]?.text}
            </Typography>
            <RadioGroup
              name="quiz-options"
              value={selectedOption}
              onChange={handleOptionChange}
            >
              {questions[currentQuestionIndex]?.options.map((option, index) => (
                <FormControlLabel
                  key={option.id}
                  value={`option${option.id}`}
                  control={<Radio />}
                  label={option.text}
                />
              ))}
            </RadioGroup>
            <Button
              variant="contained"
              color="primary"
              sx={{ mt: 2 }}
              onClick={checkAnswer}
              disabled={selectedOption === null}
            >
              پاسخ
            </Button>
            {showLessonButton && (
              <Button
                variant="outlined"
                color="secondary"
                sx={{ mt: 2, ml: 2 }}
                onClick={() => setIsModalOpen(true)}
              >
                مشاهده درسنامه
              </Button>
            )}
          </CardContent>
        </Card>
      </Box>

      <Box
        sx={{
          position: "fixed",
          bottom: 0,
          width: "100%",
          backgroundColor: feedback === "correct" ? "#4CAF50" : feedback === "incorrect" ? "#F44336" : "#f5f5f5",
          px: 3,
          py: 2,
          display: "flex",
          justifyContent: "space-between",
          transition: "background-color 0.3s ease",
        }}
      >
        <Dialog open={isModalOpen} onClose={closeModal} maxWidth="md" fullWidth>
          <DialogTitle sx={{ textAlign: "center", fontSize: "1.5rem", fontWeight: "bold" }}>
            درسنامه
          </DialogTitle>
          <DialogContent>
            <Box
              sx={{
                width: "100%",
                height: "400px",
                mb: 2,
                mx: "auto",
              }}
            >
              <iframe
                src={videoLink}
                width="100%"
                height="100%"
                frameBorder="0"
                allowFullScreen
              />
            </Box>
            <Typography variant="body2" sx={{ textAlign: "center" }}>
              لطفاً ویدیو را مشاهده کنید و سپس به سوال بعدی بروید.
            </Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={closeModal} color="secondary">
              بستن
            </Button>
          </DialogActions>
        </Dialog>

        <Dialog open={isPopupOpen} onClose={closePopup}>
          <DialogTitle>
            <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
              <Typography variant="h6">قلب‌ها تمام شدند</Typography>
              <IconButton onClick={closePopup}>
                <CloseIcon />
              </IconButton>
            </Box>
          </DialogTitle>
          <DialogContent>
            <Box sx={{ display: "flex", justifyContent: "center", mb: 2 }}>
              <img src={img1} alt="No Hearts Left" style={{ width: "80%" }} />
            </Box>
            <Typography variant="body2">قلب‌ها تمام شدند، لطفاً دوباره امتحان کنید!</Typography>
          </DialogContent>
          <DialogActions>
            <Button onClick={closePopup} color="secondary">بستن</Button>
          </DialogActions>
        </Dialog>

        <Dialog
          open={isAlreadyPassedPopupOpen}
          onClose={() => setIsAlreadyPassedPopupOpen(false)}
          sx={{
            "& .MuiDialog-paper": {
              borderRadius: 3,
              maxWidth: "500px",
              width: "90%",
              p: 3,
            },
          }}
        >
          <DialogTitle
            sx={{
              textAlign: "center",
              fontSize: "1.5rem",
              fontWeight: "bold",
              color: "#0DBBBF",
            }}
          >
            اطلاعیه
          </DialogTitle>
          <DialogContent
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              textAlign: "center",
              gap: 2,
            }}
          >
            <Typography
              variant="body1"
              sx={{
                fontSize: "1.2rem",
                fontFamily: "'Vazir', sans-serif",
                color: "#333",
              }}
            >
              شما قبلا این آزمون را قبول شدید.
            </Typography>
            <Box
              component="img"
              src={img1}
              alt="قبول شده‌اید"
              sx={{
                width: "80%",
                maxWidth: "300px",
                borderRadius: 2,
                boxShadow: "0 4px 10px rgba(0, 0, 0, 0.2)",
              }}
            />
          </DialogContent>
          <DialogActions
            sx={{
              justifyContent: "center",
              pt: 2,
            }}
          >
            <Button
              onClick={() => navigate("/profile")}
              variant="contained"
              color="primary"
              sx={{
                fontSize: "1rem",
                px: 4,
                py: 1,
                borderRadius: 3,
                backgroundColor: "#0DBBBF",
                "&:hover": {
                  backgroundColor: "#0CA6A8",
                },
              }}
            >
              بستن
            </Button>
          </DialogActions>
        </Dialog>

        <Box
          component="img"
          src={img}
          alt="تصویر"
          sx={{
            position: "absolute",
            bottom: "80px",
            left: "110px",
            width: "120px",
            height: "auto",
            borderRadius: "8px",
            boxShadow: "0 4px 8px rgba(0, 0, 0, 0.2)",
            "@media (max-width: 600px)": {
              width: "80px",
            },
          }}
        />

  {/* دکمه "بازگشت به صفحه پروفایل" */}
  <Button
    variant="outlined"
    color="inherit" // رنگ پیش‌فرض
    onClick={() => navigate("/profile")} // هدایت به صفحه پروفایل
    sx={{
      backgroundColor: "#f5f5f5", // رنگ پس‌زمینه
      "&:hover": {
        backgroundColor: "#e0e0e0", // تغییر رنگ هنگام هاور
      },
    }}
  >
    بازگشت به پروفایل
  </Button>

  {/* دکمه "سوال قبلی" */}
  <Button
    variant="outlined"
    onClick={previousQuestion}
    disabled={currentQuestionIndex === 0 || answerSubmitted} // غیر فعال کردن دکمه قبلی
  >
    سوال قبلی
  </Button>

  {/* دکمه "سوال بعدی" یا "پایان آزمون" */}
  {currentQuestionIndex < questions.length - 1 ? (
    <Button
      variant="contained"
      color="primary"
      onClick={nextQuestion}
      disabled={isNextButtonDisabled}
    >
      سوال بعدی
    </Button>
  ) : (
    <Button
      variant="contained"
      color="secondary"
      onClick={() => navigate("/azmon1", { state: { answers, heartCount, examId } })}
    >
      پایان آزمون
    </Button>
  )}

</Box>
    </Box>
  );
};

export default Azmon;
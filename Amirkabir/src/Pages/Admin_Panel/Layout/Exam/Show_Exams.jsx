import LayoutSidebar from "../../../../Components/Layout/LayoutSidebar";
import { useNavigate } from "react-router-dom";
import React, { useState, useEffect, useRef } from "react";
import { Edit as EditIcon, Search as SearchIcon, AccessTime as AccessTimeIcon, Visibility as VisibilityIcon } from "@mui/icons-material";
import { Container, Grid, Card, CardContent, CardMedia, Typography, Box, CircularProgress, IconButton, TextField, InputAdornment, Button, Tooltip } from "@mui/material";

const courses = Array.from({ length: 20 }).map((_, i) => ({
  title: `آزمون شماره ${i + 1}`,
  details: "25 تکمیل شده",
  image: `https://images.unsplash.com/1/type-away-numero-dos.jpg?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D`,
  rating: (Math.random() * 5).toFixed(1),
  views: Math.floor(Math.random() * 10000),
  lastUpdated: `${Math.floor(Math.random() * 12 + 1)} ماه قبل`,
  status: Math.random() > 0.5 ? "منتشر شده" : "پیش‌نویس",
}));

const Show_Exams = () => {
  const [loading, setLoading] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [visibleCourses, setVisibleCourses] = useState(6);
  const [filterStatus, setFilterStatus] = useState("همه");
  const [filteredCourses, setFilteredCourses] = useState(courses);

  const navigate = useNavigate();
  const observerRef = useRef(null);

  const handleEditClick = () => {
    navigate("/adminpanel:editexam");
  };

  useEffect(() => {
    let filtered = courses;

    if (searchQuery !== "") 
    {
      filtered = filtered.filter((course) =>
        course.title.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    if (filterStatus !== "همه") 
    {
      filtered = filtered.filter((course) => course.status === filterStatus);
    }

    setFilteredCourses(filtered);
  }, [searchQuery, filterStatus]);

  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        if (entries[0].isIntersecting && visibleCourses < filteredCourses.length) 
        {
          setLoading(true);
          setVisibleCourses((prev) => Math.min(prev + 6, filteredCourses.length));
        }
      },
      { threshold: 1.0 }
    );

    if (observerRef.current) {
      observer.observe(observerRef.current);
    }

    return () => {
      if (observerRef.current) {
        observer.unobserve(observerRef.current);
      }
    };
  }, [visibleCourses, filteredCourses]);

  useEffect(() => {
    if (visibleCourses >= filteredCourses.length && loading) {
      setLoading(false);
    }
  }, [visibleCourses, loading, filteredCourses]);

  return (
    <LayoutSidebar
      component={
        <Container sx={{ marginTop: 4 }}>
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              mb: 3,
            }}
          >
            <Typography variant="h3" fontWeight="bold">
              آزمون ها
            </Typography>
            <Button
              variant="contained"
              sx={{ height: "40px", minWidth: "120px" }}
              onClick={() => navigate("/adminpanel/addexam")} 
            >
              افزودن آزمون
            </Button>
          </Box>

          <Box
            sx={{
              display: "flex",
              flexDirection: { xs: "column", sm: "row" },
              alignItems: "center",
              width: "100%",
              gap: 2,
              mb: 3,
              maxWidth: "800px",
              mx: "auto",
            }}
          >
            <TextField
              variant="outlined"
              placeholder="جستجوی آزمون ها"
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
                sx: { height: "50px", padding: 0 },
              }}
            />
          </Box>

          <Grid container spacing={3} sx={{ mt: 2 }}>
            {filteredCourses.slice(0, visibleCourses).map((course, index) => (
              <Grid item xs={12} sm={6} md={4} key={index}>
                <Card
                  sx={{
                    position: "relative",
                    borderRadius: "16px",
                    overflow: "hidden",
                    boxShadow: "0 8px 16px rgba(0, 0, 0, 0.1)",
                    transition: "transform 0.3s ease, box-shadow 0.3s ease",
                    "&:hover": {
                      transform: "scale(1.05)",
                      boxShadow: "0 16px 24px rgba(0, 0, 0, 0.15)",
                    },
                  }}
                >
                  <Tooltip title="ویرایش آزمون">
                    <IconButton
                      size="small"
                      
                      sx={{
                        position: "absolute",
                        top: 12,
                        right: 12,
                        backgroundColor: "#ffffff",
                        boxShadow: "0 2px 6px rgba(0, 0, 0, 0.1)",
                        "&:hover": { backgroundColor: "#f0f0f0" },
                      }}
                      
                      onClick={handleEditClick}
                    >
                      <EditIcon fontSize="small" />
                    </IconButton>
                  </Tooltip>

                  <CardMedia
                    component="img"
                    image={course.image}
                    alt={course.title}
                    
                    sx={{
                      width: "100%",
                      height: 140,
                      objectFit: "cover",
                    }}
                  />

                  <CardContent
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      alignItems: "center",
                      padding: "16px",
                      gap: "12px"
                    }}
                  >
                    <Typography
                      variant="h6"
                      
                      sx={{
                        fontWeight: "bold",
                        textAlign: "center",
                        color: "primary.main",
                        wordBreak: "break-word",
                      }}
                    >
                      {course.title}
                    </Typography>

                    <Typography
                      variant="body2"
                      
                      sx={{
                        color: "text.secondary",
                        textAlign: "center",
                        fontSize: "0.875rem"
                      }}
                    >
                      {course.details}
                    </Typography>

                    <Box
                      sx={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                        width: "100%",
                        gap: 1,
                        mt: 1
                      }}
                    >
                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          gap: 0.5,
                          color: "text.secondary"
                        }}
                      >
                        <AccessTimeIcon fontSize="small" />
                        <Typography variant="caption">{course.lastUpdated}</Typography>
                      </Box>

                      <Box
                        sx={{
                          display: "flex",
                          alignItems: "center",
                          gap: 0.5,
                          color: "text.secondary",
                        }}
                      >
                        <VisibilityIcon fontSize="small" />
                        <Typography variant="caption">{course.views} بازدید</Typography>
                      </Box>
                    </Box>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>

          {loading && (
            <Box display="flex" justifyContent="center" my={3}>
              <CircularProgress />
            </Box>
          )}

          <Box ref={observerRef} sx={{ height: 20 }} />
        </Container>
      }
    />
  );
};

export default Show_Exams;
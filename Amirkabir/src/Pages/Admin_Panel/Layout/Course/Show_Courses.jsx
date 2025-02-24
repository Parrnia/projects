import LayoutSidebar from "../../../../Components/Layout/LayoutSidebar";
import Tooltip from "@mui/material/Tooltip";
import { useNavigate } from "react-router-dom";
import React, { useState, useEffect, useRef } from "react";
import { Visibility as VisibilityIcon, Edit as EditIcon, Search as SearchIcon, Article as ArticleIcon } from "@mui/icons-material";
import { Container, Grid, Card, CardContent, CardMedia, Typography, Box, CircularProgress, IconButton, Rating, TextField, InputAdornment, Button } from "@mui/material";

const courses = Array.from({ length: 20 }).map((_, i) => ({
  title: `دوره شماره ${i + 1}`,
  details: "توضیحات دوره...",
  image: `https://images.unsplash.com/photo-1453733190371-0a9bedd82893?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D`,
  rating: (Math.random() * 5).toFixed(1),
  views: Math.floor(Math.random() * 10000),
  lastUpdated: `شامل ${Math.floor(Math.random() * 12 + 1)} درس`,
  status: Math.random() > 0.5 ? "published" : "draft",
}));

const Show_Courses = () => {
  const observerRef = useRef(null);
  const [loading, setLoading] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [filterStatus, setFilterStatus] = useState("all");
  const [visibleCourses, setVisibleCourses] = useState(6);
  const [filteredCourses, setFilteredCourses] = useState(courses);

  const navigate = useNavigate();

  const handleEditClick = () => {
    navigate("/adminpanel/editcourse");
  };

  useEffect(() => {
    let filtered = courses;

    if (searchQuery !== "") {
      filtered = filtered.filter((course) =>
        course.title.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    if (filterStatus !== "all") {
      filtered = filtered.filter((course) => course.status === filterStatus);
    }

    setFilteredCourses(filtered);
  }, [searchQuery, filterStatus]);

  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        if (entries[0].isIntersecting && visibleCourses < filteredCourses.length) {
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
              دوره‌ها
            </Typography>
            <Button
              variant="contained"
              sx={{ height: "40px", minWidth: "120px" }}
              onClick={() => navigate("/adminpanel/addcourse")} 
            >
              افزودن دوره
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
              placeholder="جستجوی دوره‌ها"
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
                    height: "100%",
                    display: "flex",
                    flexDirection: "column",
                    backgroundColor: "#f5f5f5",
                    boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
                    position: "relative",
                    transition: "transform 0.3s ease, box-shadow 0.3s ease",
                    "&:hover": {
                      transform: "scale(1.05)",
                      boxShadow: "0 8px 12px rgba(0, 0, 0, 0.15)",
                    },
                  }}
                >
                  <CardMedia
                    component="img"
                    height="160"
                    image={course.image}
                    alt={course.title}
                  />
                  
                  <CardContent sx={{ flexGrow: 1, padding: 2 }}>
                    <Typography
                      variant="h6"
                      sx={{ fontWeight: "bold", color: "primary.main" }}
                      gutterBottom
                    >
                      {course.title}
                    </Typography>

                    <Typography variant="body2" sx={{ color: "text.secondary" }}>
                      {course.details}
                    </Typography>

                    <Box
                      sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        mt: 2,
                      }}
                    >
                      <Typography
                        variant="body2"
                        sx={{ color: "text.secondary", display: "flex", alignItems: "center" }}
                      >
                        {course.views}
                        <VisibilityIcon sx={{ fontSize: 16, mr: 0.5 }} />
                      </Typography>
                      
                      <Box sx={{ display: "flex", alignItems: "center" }}>
                        <Typography
                          variant="body2"
                          sx={{ ml: 0.5, color: "text.secondary" }}
                        >
                          ({course.rating})
                        </Typography>
                        
                        <Rating
                          name={`rating-${index}`}
                          value={parseFloat(course.rating)}
                          precision={0.1}
                          dir="ltr" 
                          readOnly
                          size="small"
                        />
                      </Box>
                    </Box>

                    <Box
                      sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        mt: 2,
                      }}
                    >
                      <Typography
                        variant="body2"
                        sx={{ color: "text.secondary", display: "flex", alignItems: "center", gap: 0.5 }}
                      >
                        <ArticleIcon sx={{ fontSize: 18, mr: 0.5 }} />
                        {course.lastUpdated}
                      </Typography>
                    
                      <Tooltip title="ویرایش دوره">
                        <IconButton size="small" color="primary" onClick={handleEditClick}>
                          <EditIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>
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

export default Show_Courses;
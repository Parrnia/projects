import React, { useEffect, useState } from "react";
import Slider from "react-slick";
import styles from "./EventsBox.module.css"; // Import EventsBox module
import sliderStyles from "./slider.module.css"; // Import Slider module
import { fetchCourses } from '../../Services/userCourses'; // Fetch courses function
import slid from "../../Images/Landing/slider.png";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import img from "../../Images/Landing/OBJECT.png";
import { Card, CardContent, Typography, CardMedia, Box, Container } from "@mui/material"; // Material-UI Components

const RecentEvents = () => {
  const [courses, setCourses] = useState([]);

  const token = localStorage.getItem('token');
  const email = localStorage.getItem('emailOrPhone');

  useEffect(() => {
    const loadCourses = async () => {
      try {
        const courseData = await fetchCourses(email, token);
        if (courseData && courseData.length > 0) {
          setCourses(courseData);
        }
      } catch (error) {
        console.error('Failed to fetch courses:', error);
      }
    };

    loadCourses();
  }, [email, token]);

  const settings = {
    dots: false,
    infinite: true,
    speed: 500,
    slidesToShow: 3,
    slidesToScroll: 1,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
        },
      },
      {
        breakpoint: 768,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
        },
      },
    ],
  };

  return (
    <div className={styles.dorehbox1}>

               
      <div className={styles.container}>
      <Box className={styles.textContainer} display="flex" alignItems="center">
                        <Typography variant="h4" className={styles.title}>جدیدترین دوره ها</Typography>
                        <img src={img} className={styles.simg} alt="Course" />
                    </Box>
     
        <div className={sliderStyles.slider}>
          <Slider {...settings}>
            {courses.map((course, index) => (
              <div key={index} className={sliderStyles.card}>
                <img
                  src={course.photo || slid}
                  alt={course.title}
                  className={sliderStyles["card-image"]}
                  onError={(e) => { e.target.src = slid; }} // Fallback image
                />
                <h3>{course.title}</h3>
                <p>{course.description}</p>
                <div className={sliderStyles.info}>
                  <span>👥 {course.numberOfUser}</span>
                  <span>📚 {course.numberOfContent}</span>
                </div>
                {/* Add link or button to course details */}
              </div>
            ))}
          </Slider>
        </div>
      </div>
    </div>
  );
};

export default RecentEvents;

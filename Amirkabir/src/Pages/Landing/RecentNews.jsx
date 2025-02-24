// src/components/Recentnews.js

import React from "react";
import Slider from "react-slick";
import styles from "./EventsBox.module.css"; // Import EventsBox module
import sliderStyles from "./slider.module.css"; // Import Slider module
import slid from "../../Images/Landing/slider.png";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

const Recentnews = () => {
  const slides = [
    {
      title: "آموزش نوسان‌گیری روزانه",
      description: "در این قسمت به طور مختصر دوره را در سه خط توضیح می‌دهیم.",
      image: slid,
      info: { students: 145, lessons: 24, duration: "01:47:00" },
      link: "/course-details/daily-trading",
    },
    {
      title: "آموزش تحلیل تکنیکال",
      description: "دوره‌ای کامل برای یادگیری تحلیل تکنیکال به زبان ساده.",
      image: slid,
      info: { students: 98, lessons: 18, duration: "02:15:00" },
      link: "/course-details/technical-analysis",
    },
    {
      title: "آموزش اصول معامله‌گری",
      description: "آموزش اصول و قواعد معامله‌گری برای مبتدیان.",
      image: slid,
      info: { students: 200, lessons: 30, duration: "03:00:00" },
      link: "/course-details/trading-basics",
    },
  ];

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
        <div className={styles["textContainer"]}>
          <p className={styles.title}>از گوشه و اطراف سرمایه گذاری</p>
        </div>
        <div className={sliderStyles.slider}>
          <Slider {...settings}>
            {slides.map((slide, index) => (
              <div key={index} className={sliderStyles.card}>
                <img
                  src={slide.image}
                  alt={slide.title}
                  className={sliderStyles["card-image"]}
                />
                <h3>{slide.title}</h3>
                <p>{slide.description}</p>
                <div className={sliderStyles.info}>
                  <span>👥 {slide.info.students}</span>
                  <span>📚 {slide.info.lessons}</span>
                </div>
                {/* Link or button to course details */}
              </div>
            ))}
          </Slider>
        </div>
      </div>
    </div>
  );
};

export default Recentnews;

// src/components/Slid.js

import React from 'react';
import Slider from 'react-slick';
import styles from './slider.module.css';  // Import the modular CSS
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import slid from "../../Images/Landing/slider.png";

const Slid = () => {
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
    <div className={styles.slider}>
      <Slider {...settings}>
        {slides.map((slide, index) => (
          <div key={index} className={styles.card}>
            <img src={slide.image} alt={slide.title} className={styles["card-image"]} />
            <h3>{slide.title}</h3>
            <p>{slide.description}</p>
            <div className={styles.info}>
              <span>👥 {slide.info.students}</span>
              <span>📚 {slide.info.lessons}</span>
              <span>⏱️ {slide.info.duration}</span>
            </div>
            <div className={styles.line}></div>
            <a href={slide.link} className={styles.btn}>
              <span>←</span> مشاهده اطلاعات دوره
            </a>
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default Slid;

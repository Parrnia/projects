// src/components/LandingPage.js
import { Grid, Typography, Box } from "@mui/material";

import React from "react";
import amirKabirImg from "../../Images/Landing/amirkabir.PNG"
import './landing.css'; // Import the CSS file for styling
import EventsBox from "./eventBox";
import BazarSarmayeImg from "../../Images/Landing/bazarSarmaye.PNG"
import AmirKabirDownloadImg from "../../Images/Landing/AmirKabirDownload.PNG"
import RecentNews from "./RecentNews";
import Footer from "./Footer";
import Header from "./header";
import userImage from "../../Images/Landing/img.png"
import Slid from "./slider";
import main2 from '../../Images/Landing/main2.png'
import img1 from '../../Images/Landing/Education.png'
import img2 from '../../Images/Landing/Award5.png'
import img3 from '../../Images/Landing/Award4.png'
import img4 from '../../Images/Landing/Website.png'
import Recentnews from "./RecentNews";
const LandingPage = () => {
    return (
        <div className="landing-page">


            {/* Header Section */}
            <Header>

            </Header>

            <div className="BazarSarmaye-Image">
                <img
                    src={BazarSarmayeImg} // Use the imported image
                    alt="Landing Page Visual"
                    className="landing-image"
                />

            </div>

            <main className="main-content">
    {/* About AmirKabir Section */}
    <div className="content2">
    
      <div className="about-section">
        <div className="text-section">
          <h1 className="title">درباره امیرکبیر</h1>
          <p className="description">
            امیرکبیر اولین و بروزترین وبسایت آموزشی رایگان در سطح ایران است که
            همیشه تلاش کرده تا بتواند جدیدترین و بروزترین آموزش‌ها و مقالات را
            در اختیار علاقه‌مندان بازار سرمایه قرار دهد.
          </p>
        </div>
        <div className="image-section">
          <img
            src={amirKabirImg}
            alt="AmirKabir"
            className="about-image"
          />
        </div>
      </div>
    </div>
    <div className="hide-on-mobile">
                <EventsBox>

                </EventsBox>
</div>
                <div className="container1">
      <div className="content1">
        <h1>
          اپلیکیشن <span className="highlight">امیرکبیر</span>
        </h1>
        <ul className="features">
  <li>
    <img src={img1} alt="Education" />
    <span>یادگیری استراتژی‌های مناسب</span>
  </li>
  <li>
    <img src={img2} alt="Analytics" />
    <span>دریافت اعتبار و جوایز با دیدن آموزش‌ها</span>
  </li>
  <li>
    <img src={img4} alt="Global" />
    <span>بهترین اپلیکیشن رایگان آموزشی بازار سرمایه</span>
  </li>
  <li>
    <img src={img3} alt="Certificate" />
    <span>بهترین شیوه آموزشی</span>
  </li>
</ul>

        <button className="download">همین الان دانلودش کن</button>
      </div>
      <div className="phone-image">
      <img
          src={main2}
          alt="Landing Page Visual"
          className="landing-image"
        />
      </div>
    </div>
            </main>
            <div className="hide-on-mobile">
  <RecentNews />
</div>
            <div className="BazarSarmaye-Image">
                <img
                    src={userImage} // Use the imported image
                    alt="Landing Page Visual"
                    className="landing-image"
                />
            </div>

            <Footer>

            </Footer>
        </div>

    );
};

export default LandingPage;

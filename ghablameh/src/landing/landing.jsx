import Navbarparent from '../components/navbarparent';
import './landing.css';
import './style.css';
import React from 'react';
import shape from './images/bg.png'
import feature1 from './images/feature-1.png';
import feature2 from './images/feature-2.png';
import aboutBanner from './images/about-banner.png';
import img1 from "./images/team.svg"
import { Link } from 'react-router-dom';
const Landing = () => {
  return (

    <div>
      <Navbarparent />
      <section class="wrapper">
        <div class="container">
          <div class="grid-cols-2">
            <div class="grid-item-1">
              <h1 class="main-heading inline-block text-center">
                به سامانه قابلمه خوش آمدید
              </h1>
              <p class="info-text">
                برای دیدن امکانات سایت ثبت نام کنید یا وارد شوید
              </p>

              <div class="btn_wrapper">
                {/* <button class="btn view_more_btn">
                 ثبت نام
              </button> */}
                <Link to='/signup' class="btn d_btn text-center"> ثبت نام </Link>
                <Link to='/login' class="btn view_more_btn text-center">ورود</Link>
                {/* <button ></button> */}
              </div>
            </div>
            <div class="grid-item-2">
              <div class="team_img_wrapper">
                <img src={img1} alt="team-img" />
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* HERO SECTION */}


      {/* ABOUT SECTION */}
      <section className="about" id="about">
        <div className="container">
          <div className="about-top">
            <h2 className="h2 section-title">قابلمه</h2>
            <p className="section-text">
              این سامانه ، سامانه رزرو غذا است که کارفرمایان بخش های مختلف میتوانند منوی هفتگی خود را تعریف کنند
              تا افراد بخش های مرتبط بتوانند غذای مورد نظر خود را رزرو کنند
            </p>
            <ul className="about-list">
              <li>
                <div className="about-card">
                  <div className="card-icon">
                    <ion-icon name="briefcase-outline"></ion-icon>
                  </div>
                  <h3 className="h3 card-title">جامع بودن</h3>
                  <p className="card-text">
                    قابلمه به تمام شرکت ها و بخش های صنعتی و آکادمیک خدمات خود را ارائه می دهد
                  </p>
                </div>
              </li>
              <li>
                <div className="about-card">
                  <div className="card-icon">
                    <ion-icon name="chatbubbles-outline"></ion-icon>
                  </div>
                  <h3 className="h3 card-title">رزو غذا</h3>
                  <p className="card-text">
                    تمام کاربران به راحتی میتوانند غذا خود را
                    <br />
                    در بوفه موردنظر رزرو کنند
                  </p>
                </div>
              </li>
              <li>
                <div className="about-card">
                  <div className="card-icon">
                    <ion-icon name="rocket-outline"></ion-icon>
                  </div>
                  <h3 className="h3 card-title">تعریف منوی هفتگی</h3>
                  <p className="card-text">
                    تمامی سازمان ها میتوانند منوی هفتگی مورد نظر خود را برای بوفه های خود تعریف کنند
                  </p>
                </div>
              </li>
            </ul>
          </div>
          <div className="about-bottom">
            <figure className="about-bottom-banner">
              <img src={aboutBanner} alt="about banner" className="about-banner" />
            </figure>
            <div className="about-bottom-content">
              <h2 className="h2 section-title"> با نظرات خود ما را یاری کنید</h2>
              <p className="section-text">
                اگر نظر یا انتقادی درباره ی نحوه کارکرد سایت دارید با ما در میان بگذارید تا با کمک شما عملکرد خود را بهبود دهیم
              </p>

            </div>
          </div>
        </div>
      </section>

      {/* FEATURES SECTION */}
      <section className="features" id="features">
        <div>




        </div>

      </section>

    </div>
  );
};

export default Landing;
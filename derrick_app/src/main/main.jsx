import { useState  , useRef } from 'react';
import styles from './main.module.css';
import NavBar from './navbar/Navbar';
import Footer from './footer/Footer';
import Slider from 'react-slick';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import style from'./style.module.css'; // Assuming this is where you place the custom styles
import logomain from '../../public/assest/logo.png'
import logo1 from '../../public/assest/logo1.png'
import logo2 from '../../public/assest/logo2.png'
import logo3 from '../../public/assest/logo3.png'
import line from '../../public/assest/curved-lines.png'
import atm from '../../public/assest/atm-card.png'
import choose from '../../public/assest/pick.png'
import trust from '../../public/assest/trust.png'
import baner from '../../public/assest/baner.svg'

import s1 from '../../public/assest/s1.svg'
import s2 from '../../public/assest/s2.svg'
import s3 from '../../public/assest/s3.svg'
import s4 from '../../public/assest/s4.svg'
import s5 from '../../public/assest/s5.svg'
import s6 from '../../public/assest/s6.svg'
import s7 from '../../public/assest/s7.svg'
import s8 from '../../public/assest/s8.svg'
import './slider.css'

function Main() {
  const containerRef = useRef(null);
  let isMouseDown = false;
  let startX;
  let scrollLeft;

  const handleMouseDown = (e) => {
    isMouseDown = true;
    startX = e.pageX - containerRef.current.offsetLeft;
    scrollLeft = containerRef.current.scrollLeft;
  };

  const handleMouseLeave = () => {
    isMouseDown = false;
  };

  const handleMouseUp = () => {
    isMouseDown = false;
  };

  const handleMouseMove = (e) => {
    if (!isMouseDown) return;
    e.preventDefault();
    const x = e.pageX - containerRef.current.offsetLeft;
    const walk = (x - startX) * 3;
    containerRef.current.scrollLeft = scrollLeft - walk;
  };
  const [isActive, setIsActive] = useState(false);

  const toggleActiveClass = () => {
    setIsActive(!isActive);
  };

  const removeActive = () => {
    setIsActive(false);
  };

  const settings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
  };

  return (
    <div className={styles.background}>
      <NavBar />
      <div className="slider">
  <Slider {...settings} >
    <div>
      <img src={s1} alt="Slider Image 1" />
    </div>
    <div>
      <img src={s2} alt="Slider Image 2" />
    </div>
    <div>
      <img src={s3} alt="Slider Image 3" />
    </div>
    <div>
      <img src={s4} alt="Slider Image 4" />
    </div>
  </Slider>


</div>

      <div className={styles.baner} >
        
      <img className={styles.imgbaner}  src={baner} alt="Background Image 1" />
      </div>

      <div className={styles.baner2}>
        <div className={styles.bannertext}>
          <p>
            وبسایت دریک پی یک پلتفرم زنجیره تامین مالی است 
            که با کمک به بهبود وضعیت جامعه در انواع زمینه های تولید تا فروش به فروشندگان و مصرف کنندگان کمک می بخشد. پلترفم دریک پی زیر مجموعه شرکت زنجیره تامین فناوری هستی می باشد که در تیر ماه 1403 شروع به کار کرده است و ورود بخش های مختلفی را در اینده ای نزدیک پیش بینی می کند.
          </p>
        </div>
        <div className={styles.bannerBackground}>
          <img className={styles.logo1} src={logo1} alt="Background Image 1" />
          <img className={styles.logo2} src={logo2} alt="Background Image 2" />
          <img className={styles.logo3} src={logo3} alt="Background Image 3" />
        </div>
        <img src={logomain} alt="توضیح تصویر" className={styles.bannerImage} />
      </div>

      <div
      ref={containerRef}
      className={styles.container}
      onMouseDown={handleMouseDown}
      onMouseLeave={handleMouseLeave}
      onMouseUp={handleMouseUp}
      onMouseMove={handleMouseMove}
    >
      <div className={styles.cardt}>
      <div className={styles.bannertext2}>
          <p>چرا دریک پی ؟</p>
        </div>
      </div>
      <div className={styles.card}>
      <div className={styles.bannerBackground}></div>
        <div className={styles.box}>
          <img src={line} className={styles.line} />
          <img src={atm} className={styles.atm} />
          <h2 className={styles.credit}>اعتبار</h2>
          <p className={styles.credittext}>
            شما می توانید با داشتن دریک پی و داشتن اعتبار از این پلتفرم مالی در بخش های مختلف زیر مجموعه این پلتفرم خرید وسرمایه گذاری هایی داشته باشید
          </p>
        </div>
      </div>
      <div className={styles.card}>
      <div className={styles.bannerBackground}></div>
        <div className={styles.box}>
          <img src={line} className={styles.line} />
          <img src={trust} className={styles.atm} />
          <h2 className={styles.credit}>اعتماد</h2>
          <p className={styles.credittext}>
          با اعتماد به دریک پی تمامی اطلاعات شما در پیش ما به صورت محرمانه باقی می ماند و در هر لحظه که شما نیازمند اعتبار باشید میتوانید در کمتر زمان ان را دریافت کنید
          </p>
      </div>
      </div>
      <div className={styles.card}>
      <div className={styles.bannerBackground}></div>
        <div className={styles.box}>
          <img src={line} className={styles.line} />
          <img src={choose} className={styles.atm} />
          <h2 className={styles.credit}>انتخاب</h2>
          <p className={styles.credittext}>
          شما با اتنخاب ما  می توانید بدون مراجعه حضوری برای دریافت  واستفاده از تسهیلات تستن شده در بانک های معتبر ایران از دریک پی استفاده کنید
          </p>
          </div>
   
    </div>
    </div>
      <div className="slider">
        <Slider {...settings}>
          <div>
            <img src={s5} alt="Slider Image 1" />
          </div>
          <div>
            <img   src={s6}  alt="Slider Image 2" />
          </div>
          <div>
            <img  src={s7}  alt="Slider Image 3" />
          </div>
          <div>
            <img  src={s8}  alt="Slider Image 3" />
          </div>
        </Slider>
      </div>
      <Footer />
    </div>
  );
}

export default Main;
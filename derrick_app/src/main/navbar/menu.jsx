import React, { useState } from 'react';
import styles from'./menu.module.css'; 
import logo from '../assest/Logo_Menue.png';
import credit from '../assest/credit-card.png';
import document from '../assest/document.png';
import badge from '../assest/badge-check.png';
import wallet from '../assest/wallet.png';
import orbital from '../assest/orbital.png'; 
import newImage2 from '../assest/ellipse.png'; 
import newImage1 from '../assest/group.png';
import newImage from '../assest/orbital1.png';
import instagram from '../assest/instagram.png';
import linkdin from '../assest/linkedin.png';
import telegrm from '../assest/telegram.png';
const Menu = () => {
    const [showAlternateContent, setShowAlternateContent] = useState(false);
    const [showLogisticsLinks, setShowLogisticsLinks] = useState(false);
    const [activeTrade, setActiveTrade] = useState(false); // برای لینک تجارت
    const [activeLogistics, setActiveLogistics] = useState(false); // برای لینک مدیریت لجستیک
    const [fadeOut, setFadeOut] = useState(false); // متغیر جدید برای انیمیشن محو شدن

    const handleTradeClick = () => {
        setFadeOut(true);
        setTimeout(() => {
          setShowAlternateContent(prev => !prev);
          setActiveTrade(prev => !prev);
          setFadeOut(false);
        }, 500);
      };
    
      const handleLogisticsClick = () => {
        setFadeOut(true);
        setTimeout(() => {
          setShowLogisticsLinks(prev => !prev);
          setActiveLogistics(prev => !prev);
          setFadeOut(false);
        }, 500);
      };
    
 

    return (
        <div className={`${styles.menu} ${fadeOut ? styles.fadeOut : styles.fadeIn}`}>
        <div className="d-flex justify-content-end align-items-center">
          <span className={styles.navbartext}>دریک پی</span>
          <img src={logo} alt="Logo" className="navbar-brand me-2" />
        </div>
        <label className={styles.menulabel}>دسترسی سریع</label>

        <img src={document} className={styles.menuimg} alt="Document" />
        <a href="#" className={styles.menulink}>برات الکترونیک</a>
        <img src={credit} className={styles.menuimg1} alt="Credit" />
        <a href="#" className={styles.menulink1}>دریک کارت</a>
        <img src={wallet} className={styles.menuimg2} alt="Wallet" />
        <a href="#" className={styles.menulink2}>کیف پول دریک</a>
        <img src={badge} className={styles.menuimg3} alt="Badge" />
        <a href="#" className={styles.menulink3}>کارت گارانتی</a>

        <label className={styles.menulabel1}>منو</label>
        <a 
          href="#" 
          onClick={handleTradeClick} 
          className={`${styles.menulink4} ${activeTrade ? styles.active : ''}`} 
        >
          تجارت
        </a>


        {showAlternateContent ? (
          <div>
            <a href="#" className={styles.menulink5}>بانک</a>
            <a href="#" className={styles.menulink6}>بیمه</a>
            <a 
              href="#" 
              onClick={handleLogisticsClick} 
              className={`${styles.menulink7} ${activeLogistics ? styles.active : ''}`} 
            >
              مدیریت لجستیک
            </a>

            {showLogisticsLinks && (
              <div>
                <a href="#" className={styles.menulink8}>خدمات فورواردر</a>
                <a href="#" className={styles.menulink9}>حمل ونقل</a>
              </div>
            )}

            <img src={newImage} className={styles.mainimg1} alt="New" />
          </div>
        ) : (
          <div>
            <a href="#" className={styles.menulink15}>معدن و کشاورزی</a>
            <a href="#" className={styles.menulink16}>گردشگری و صنایع دستی</a>
            <a href="#" className={styles.menulink17}>صنعت و خدمات</a>
            <img src={orbital} className={styles.mainimg} alt="Orbital" />
          </div>
        )}
        <div className={styles.socialcontainer}>
          <img src={instagram} className={styles.social} id={styles.social1} alt="Instagram" />
          <img src={linkdin} className={styles.social} id={styles.social2} alt="LinkedIn" />
          <img src={telegrm} className={styles.social} id={styles.social3} alt="Telegram" />
        </div>
      </div>
    );
};

export default Menu;
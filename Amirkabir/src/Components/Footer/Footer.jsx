import React from "react";
import styles from "./Footer.module.css"; // Import the CSS module

// Import your logo images here
import TelegramLogo from "../../Images/Landing/icons8-telegram-240.png";
import InstagramLogo from "../../Images/Landing/icons8-instagram-256.png";
import TwitterLogo from "../../Images/Landing/icons8-twitter-240.png";
import YouTubeLogo from "../../Images/Landing/icons8-youtube-240.png";

const Footer = () => {
    return (
        <footer className={styles.footer}>
            <div className={styles.footercolumns}>
                <div className={styles.footercolumn}>
                    <h2 className={styles.fh}>امیرکبیر</h2>
                    <p className={styles.fp}>امیرکبیر اولین و بروزترین وبسایت آموزشی رایگان در سطح ایران است که همیشه تلاش کرده تا بتواند جدیدترین و بروزترین آموزش ها و مقالات را در اختیار علاقه مندان قرار دهد.</p>
                </div>
                <div className={styles.footercolumn1}>
                    <h4>بخش های سایت</h4>
                    <p>درباره امیرکبیر</p>
                    <p>ارتباط با ما</p>
                </div>
                <div className={styles.footercolumn1}>
                    <h4>دسترسی سریع</h4>
                    <p>اخبار</p>
                    <p>تالار گفت و گو</p>
                    <p>میکروکست</p>
                </div>
                <div className={styles.footercolumn1}>
                    <h4>ارتباط با ما</h4>
                    <p>ایمیل : example@gmail.com</p>
                    <p>اینستاگرام : example</p>
                    <p>شماره تماس : 1234567</p>
                </div>
            </div>

            {/* Separator Line */}
            <hr className={styles.footerseparator} />

            {/* Social Media and Download Section */}
            <div className={styles.footerbottom}>
                <div className={styles.socialicons}>
                    <img src={TelegramLogo} alt="Telegram" className={styles.socialicon} />
                    <img src={InstagramLogo} alt="Instagram" className={styles.socialicon} />
                    <img src={TwitterLogo} alt="Twitter" className={styles.socialicon} />
                    <img src={YouTubeLogo} alt="YouTube" className={styles.socialicon} />
                </div>
                <button className={styles.downloadbutton}>دانلود اپلیکشن</button>
            </div>
        </footer>
    );
};

export default Footer;

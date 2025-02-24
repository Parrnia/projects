import React from 'react';
import { faPhone } from '@fortawesome/free-solid-svg-icons';
import styles from './Footer.module.css';
import { faEnvelopeOpen } from '@fortawesome/free-solid-svg-icons';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { faTelegramPlane } from '@fortawesome/free-brands-svg-icons';
import { faFacebookF } from '@fortawesome/free-brands-svg-icons';
import { faTwitter } from '@fortawesome/free-brands-svg-icons';
import { faInstagram } from '@fortawesome/free-brands-svg-icons';
import { faTelegram } from '@fortawesome/free-brands-svg-icons';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const Footer = () => {
  return (
    <footer className={styles.footer}>
      <div className={styles.footerleft}>
      
        <p className={styles.about}>
          <span>درباره شرکت</span> Ut congue augue non tellus bibendum, in varius tellus condimentum. In scelerisque nibh tortor, sed rhoncus odio condimentum in. Sed sed est ut sapien ultrices eleifend. Integer tellus est, vehicula eu lectus tincidunt, ultricies feugiat leo. Suspendisse tellus elit, pharetra in hendrerit ut, aliquam quis augue. Nam ut nibh mollis, tristique ante sed, viverra massa.
        </p>
        <div className={styles.icons}>
          <a href="#"><FontAwesomeIcon icon={faFacebookF} size="lg" /></a>
          <a href="#"><FontAwesomeIcon icon={faTwitter} size="lg" /></a>
          <a href="#"><FontAwesomeIcon icon={faInstagram} size="lg" /></a>
        </div>
      
      </div>
      <div className={styles.footercenter}>
  <div className={styles.contactInfo}>
    <div className={styles.contactItem}>
      <FontAwesomeIcon icon={faMapMarkerAlt} size="lg" />
      <p>نام و آدرس خیابان شهر، کشور</p>
    </div>
    <div className={styles.contactItem}>
      <FontAwesomeIcon icon={faPhone} size="lg" />
      <p>(+00) 0000 000 000</p>
    </div>
    <div className={styles.contactItem}>
      <FontAwesomeIcon icon={faEnvelopeOpen} size="lg" />
      <p><a href="#">office@company.com</a></p>
    </div>
  </div>
</div>
      <div className={styles.footerright}>
        <h2>لوگوی شرکت<span></span></h2>
        <p className={styles.menu}>
          <a href="#">خانه</a> |
          <a href="#">درباره</a> |
          <a href="#">خدمات</a> |
          <a href="#">نمونه کارها</a> |
          <a href="#">اخبار</a> |
          <a href="#">تماس</a>
        </p>
        <p className={styles.name}>نام شرکت &copy; 2016</p>
      </div>
    </footer>
  );
};

export default Footer;
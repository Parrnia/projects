import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import logo1 from '../assest/Logo_Menue.png';
import './Navbar.css';
import Modal from 'react-modal';
import Menu from './Menu';
import { Link } from 'react-router-dom';
import styles from'./menu.module.css'; 
import logo from '../assest/Logo_Menue.png';
import credit from '../assest/credit-card.png';
import document1 from '../assest/document.png';
import badge from '../assest/badge-check.png';
import wallet from '../assest/wallet.png';
import orbital from '../assest/orbital.png'; 
import newImage2 from '../assest/ellipse.png'; 
import newImage1 from '../assest/group.png';
import newImage from '../assest/orbital1.png';
import instagram from '../assest/instagram.png';
import linkdin from '../assest/linkedin.png';
import telegrm from '../assest/telegram.png';

const Navbar = () => {
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [showAlternateContent, setShowAlternateContent] = useState(false);
  const [showLogisticsLinks, setShowLogisticsLinks] = useState(false);
  const [activeTrade, setActiveTrade] = useState(false);
  const [activeLogistics, setActiveLogistics] = useState(false);
  const [fadeOut, setFadeOut] = useState(false);
  const [isSliding, setIsSliding] = useState(false);

  const openModal = () => {
    setModalIsOpen(true);
  };

  const closeModal = () => {
    setModalIsOpen(false);
  };

  const handleTradeClick = () => {
    setIsSliding(true);
    setFadeOut(true);
    setTimeout(() => {
      setShowAlternateContent(prev => !prev);
      setActiveTrade(prev => !prev);
      setFadeOut(false);
      setIsSliding(false);
    }, 500);
  };

  const handleLogisticsClick = () => {
    setIsSliding(true);
    setFadeOut(true);
    setTimeout(() => {
      setShowLogisticsLinks(prev => !prev);
      setActiveLogistics(prev => !prev);
      setFadeOut(false);
      setIsSliding(false);
    }, 500);
  };

  useEffect(() => {
    Modal.setAppElement('#root');
  }, []);

  return (
    <nav className="navbar navbar-expand-lg navbar-light">
      <div className="container-fluid">
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <div className="d-flex align-items-center">
            <img src={logo1} alt="Logo" className="navbar-brand me-2" />
            <span className="navbar-text">دریک پی</span>
          </div>
          <ul className="navbar-nav ms-auto">
            <li className="nav-item">
              <a className="nav-link" href="#">خانه</a>
            </li>
            <li className="nav-item">
              <a className="nav-link" href="#">درباره ما</a>
            </li>
            <li className="nav-item">
              <a className="nav-link" href="#">تماس با ما</a>
            </li>
            <li className="nav-item">
              <Link to="/log2" className="nav-link">ثبت نام/ ورود</Link>
            </li>
          </ul>
          <span
            id="navicon"
            style={{
              cursor: 'pointer',
              fontSize: '32px',
              position: 'absolute',
              top: '45px',
              left: '35px',
              color: '#5d62ff',
            }}
            className="navbar-toggler-icon"
            onClick={openModal}
          ></span>
          <Modal
            isOpen={modalIsOpen}
            onRequestClose={closeModal}
            contentLabel="Example Modal"
            appElement={document.getElementById('root')}
            style={{
              content: {
                width: '95%',
                maxWidth: '1200px',
                height: 'auto',
                maxHeight: '100%',
                margin: 'auto',
                padding: '20px',
                borderRadius: '8px',
                zIndex: 1000,
                left: '10px',
                animation: isSliding ? (fadeOut ? 'modalClose 0.5s forwards' : 'modalOpen 0.5s forwards') : (modalIsOpen ? 'modalOpen 0.5s forwards' : 'modalClose 0.5s forwards') ,
              },
              overlay: {
                backgroundColor: 'rgba(0, 0, 0, 0.5)',
                zIndex: 999,
              },
            }}
          >
           <div className={`${styles.menu} ${fadeOut ? styles.fadeOut : ''}`}>
              <div className="d-flex justify-content-end align-items-center">
                <span className={styles.navbartext}>دریک پی</span>
                <img src={logo} alt="Logo" className="navbar-brand me-2" />
              </div>
              <label className={styles.menulabel}>دسترسی سریع</label>

              <img src={document1} className={styles.menuimg} alt="Document" />
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
              
            </div>
            <div className={styles.socialcontainer}>
                <img src={instagram} className={styles.social} id={styles.social1} alt="Instagram" />
                <img src={linkdin} className={styles.social} id={styles.social2} alt="LinkedIn" />
                <img src={telegrm} className={styles.social} id={styles.social3} alt="Telegram" />
              </div>
            <span
              onClick={closeModal}
              style={{
                cursor: 'pointer',
                fontSize: '32px',
                position: 'absolute',
                top: '20px',
                left: '20px',
                color: '#5d62ff',
                zIndex: 1001,
              }}
            >
              ✕
            </span>
          </Modal>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
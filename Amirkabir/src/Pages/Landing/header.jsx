import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import styles from './header.module.css';
import img1 from '../../Images/Landing/Login.png';
import img from '../../Images/Landing/ProfileAdd1.png';
import avatar from './avatar.png';
import Login_SignUp from '../Login_SignUp/Login_SignUp';
import { ProfileServices } from '../../Services/ProfileServices';

const Header = () => {
    const login = localStorage.getItem('isLogin');
    const [isLogin, setIsLogin] = useState(true);
    const [openModal, setOpenModal] = useState(false);
    const [profile, setProfile] = useState(null);
    const [isMenuOpen, setIsMenuOpen] = useState(false); // وضعیت منو
    const navigate = useNavigate();

    useEffect(() => {
        if (login == 1) {
            const username = localStorage.getItem('username');
            const token = localStorage.getItem('token');
            fetchProfile(username, token);
        }
    }, [login]);

    const fetchProfile = async (username, token) => {
        try {
            const profileData = await ProfileServices(username, token);
            setProfile(profileData);
        } catch (error) {
            console.error('Failed to fetch profile:', error);
        }
    };

    const handleOpenModal = (isLogin) => {
        setIsLogin(isLogin);
        setOpenModal(true);
    };

    const handleCloseModal = () => {
        setOpenModal(false);
    };

    const handleLogout = () => {
        localStorage.removeItem('isLogin');
        localStorage.removeItem('username');
        localStorage.removeItem('token');
        setProfile(null);
        setOpenModal(false);
    };

    const handleProfileClick = () => {
        navigate('/profile');
    };

    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen); // تغییر وضعیت منو
    };

    return (
        <header className={styles.header}>
            <div className={styles.headerContainer}>
                {login != 1 ? (
                    <div className={styles.signUp}>
                        <button
                            className={styles.signUpButton}
                            onClick={() => handleOpenModal(false)}
                        >
                            عضویت
                            <img src={img} alt="Icon" className={styles.buttonIcon} />
                        </button>
                        <button
                            className={styles.loginButton}
                            onClick={() => handleOpenModal(true)}
                        >
                            ورود
                            <img src={img1} alt="Icon" className={styles.buttonIcon} />
                        </button>
                    </div>
                ) : (
                    <div className={styles.userInfo}>
                        <div className={styles.profileSection} onClick={handleProfileClick} style={{ cursor: 'pointer' }}>
                            {profile?.profilePicture ? (
                                <img 
                                    src={profile.profilePicture} 
                                    alt="Profile" 
                                    className={styles.profileImage} 
                                />
                            ) : (
                                <div className={styles.profileDefault}>
                                   <img src={avatar} alt="Icon" className={styles.profileImage} />
                                </div>
                            )}
                            <div className={styles.profileDetails}>
                                <span className={styles.profileName}>
                                    {profile?.firstName} {profile?.lastName}
                                </span>
                            </div>
                        </div>
                        <button 
                            className={styles.loginButton} 
                            onClick={handleLogout}
                        >
                            خروج
                            <img src={img1} alt="Icon" className={styles.buttonIcon} />
                        </button>
                    </div>
                )}
                <div className={styles.searchContainer}>
                    <input
                        type="text"
                        placeholder="دنبال چی میگردی؟"
                        className={styles.searchInput}
                    />
                </div>

                <div className={styles.textRight}>
                    <p>امیرکبیر</p>
                </div>

                {/* دکمه همبرگر برای حالت موبایل */}
                <button className={styles.hamburger} onClick={toggleMenu}>
                    <span className={styles.hamburgerLine}></span>
                    <span className={styles.hamburgerLine}></span>
                    <span className={styles.hamburgerLine}></span>
                </button>
            </div>

            {/* منوی ناوبری */}
            <nav className={`${styles.headerNav} ${isMenuOpen ? styles.open : ''}`}>
                <button className={styles.navButton}>خبرها</button>
                <button className={styles.navButton}>بلاگ</button>
                <button className={styles.navButton}>دوره ها</button>
                <button className={styles.navButton}>امیرکبیر</button>
            </nav>

            {openModal &&
                <Login_SignUp
                    open={openModal}
                    onClose={handleCloseModal}
                    selectedTab={isLogin ? 0 : 1}
                />
            }
        </header>
    );
};

export default Header;
import React, {useEffect, useState} from 'react';
import styles from './UserCourseHeader.module.css';
import crystal from '../../Images/UserCourses/Crystal.svg'
import fire from '../../Images/UserCourses/Fire.svg'
import heart from '../../Images/UserCourses/Heart.svg'
import {convertToPersianDate} from "../../Components/Date/Jalaali";
import {ProfileServices} from "../../Services/ProfileServices";
const Header = () => {
    const token = localStorage.getItem('token');
    // const token = 'c0d73d71-058a-47e5-9776-8df7816a62a2';
    const [profileData, setProfileData] = useState(null);
    const emailOrPhone = localStorage.getItem('emailOrPhone');
    useEffect(() => {
        const fetchProfileData = async () => {

            if (token && emailOrPhone) {
                try {
                    const response = await ProfileServices(emailOrPhone, token);
                    setProfileData(response.data);
                } catch {
                }
            }
        };

        fetchProfileData();
    }, [token]);
    return (
        <header className={styles.headerContainer}>
            {/* Right Section - Search */}
            {/*<div className={styles.searchContainer}>*/}
            {/*    <input*/}
            {/*        type="text"*/}
            {/*        placeholder="جستجو..."*/}
            {/*        className={styles.searchInput}*/}
            {/*    />*/}
            {/*</div>*/}

            {/*/!* Center Section - Button *!/*/}
            {/*<div className={styles.buttonContainer}>*/}
            {/*    <button className={styles.mainButton}>افزودن دوره جدید</button>*/}
            {/*</div>*/}

            {/* Left Section - Numbers and Images */}
            <div className={styles.infoContainer}>
                <div className={styles.infoItem}>
                    <img src={fire} alt="Image 1" className={styles.image} />
                    <span className={styles.number}>{profileData?.dailyChain}</span>
                </div>
                <div className={styles.infoItem}>
                    <img src={crystal} alt="Image 2" className={styles.image} />
                    <span className={styles.number}>{profileData?.points}</span>
                </div>
                <div className={styles.infoItem}>
                    <img src={heart} alt="Image 3" className={styles.image} />
                    <span className={styles.number}>{profileData?.hearts}</span>
                </div>
            </div>
        </header>
    );
};

export default Header;

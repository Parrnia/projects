import React from 'react';
import styles from './SideBar.module.css';

function Sidebar({ open, onClose }) {
    return (
        <div className={`${styles.sidebarContainer} ${open ? styles.open : ''}`}>
            <div className={styles.header} dir="rtl">
                <h3 className={styles.headertext}>امیرکبیر</h3>

            </div>
            <ul className={styles.menuList} dir="rtl">
                <li className={styles.menuItem}>
                    <span className={styles.icon}>📥</span>
                    <span className={styles.text}>پروفایل</span>
                </li>
                <li className={styles.menuItem}>
                    <span className={styles.icon}>📤</span>
                    <span className={styles.text}>تمرین</span>
                </li>
                <li className={styles.menuItem}>
                    <span className={styles.icon}>⭐</span>
                    <span className={styles.text}>دوره ها</span>
                </li>
                <li className={styles.menuItem}>
                    <span className={styles.icon}>📝</span>
                    <span className={styles.text}>تنظیمات</span>
                </li>
                <li className={styles.menuItem}>
                    <span className={styles.icon}>🗑️</span>
                    <span className={styles.text}>خروج</span>
                </li>
            </ul>
        </div>
    );
}

export default Sidebar;

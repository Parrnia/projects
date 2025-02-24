import React, { useEffect, useState } from 'react';
import UserCourseHeader from './UserCourseHeader';
import { fetchCourses } from '../../Services/userCourses';
import styles from './UserCourses.module.css';
import amirkabir from '../../Images/UserCourses/AmirKabir.svg';
import DailyMissions from "../Courses/DailyMissions";
import Leaderboard from "../Courses/LeaderBoard";
import Sidebar from "../../Components/Sidebar/Sidebar";
import { useNavigate } from 'react-router-dom';
import {Box} from "@mui/material";

const mockData = [
    {
        title: "دوره مقدماتی برنامه نویسی",
        description: "این دوره مبانی برنامه نویسی را آموزش می‌دهد.",
        numberOfContent: 20,
        numberOfUser: 150,
        image: amirkabir,
    },
    {
        title: "دوره پیشرفته طراحی وب",
        description: "طراحی صفحات وب پیشرفته و کاربردی.",
        numberOfContent: 15,
        numberOfUser: 100,
        image: amirkabir,
    },
    {
        title: "آموزش داده‌کاوی",
        description: "مفاهیم و کاربردهای داده‌کاوی.",
        numberOfContent: 10,
        numberOfUser: 200,
        image: amirkabir,
    },
    {
        title: "آموزش داده‌کاوی",
        description: "مفاهیم و کاربردهای داده‌کاوی.",
        numberOfContent: 10,
        numberOfUser: 200,
        image: amirkabir,
    },
];

function UserCourses() {
    const navigate = useNavigate();
    const [courses, setCourses] = useState([]);

    const token = localStorage.getItem('token');
    // const token = '90d0d84c-59ed-443a-8418-4d329e4ccfb4';
    const email = localStorage.getItem('emailOrPhone');

    useEffect(() => {
        const loadCourses = async () => {
            try {
                const courseData = await fetchCourses(email, token);
                if (courseData && courseData.length > 0) {
                    setCourses(courseData);
            
                } 
            } catch (error) {
                console.error('Failed to fetch courses:', error);
   
            }
        };

        loadCourses();
    }, []);

    const handleContinueClick = (course) => {
        localStorage.setItem('courseId', course.id);
        navigate('/course');
    };

    return (



        <div className={styles.container}>

            <Sidebar/>
            {/* Header */}
            <UserCourseHeader />

            {/* Layout Wrapper */}
            <div className={styles.layout}>
                {/* Main Content */}
                <main className={styles.mainContent}>
                    <div className={styles.coursesGrid} dir="rtl">
                        {courses.map((course) => (
                            <div key={course.title} className={styles.courseCard}>
                                <img
                                    src={course.photo}
                                    alt={course.title}
                                    className={styles.courseImage}
                                    loading="lazy"
                                    onError={(e) => {
                                        e.target.src = amirkabir; // Fallback image
                                    }}
                                />
                                <div className={styles.courseDetails}>
                                    <h3 className={styles.courseTitle}>{course.title}</h3>
                                    <p className={styles.courseDescription}>{course.description}</p>
                                    <div className={styles.courseInfo}>
                                        <p>
                                            <strong>تعداد محتوا:</strong> {course.numberOfContent}
                                        </p>
                                        <p>
                                            <strong>تعداد کاربران:</strong> {course.numberOfUser}
                                        </p>
                                    </div>
                                </div>
                                <div className={styles.courseActions}>
                                    <button
                                        className={styles.actionButton}
                                        onClick={() => handleContinueClick(course)}
                                    >
                                        ادامه دادن
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                </main>

            {/* Left Sidebar */}
            <div className={styles.leftColumn}>
                    <div className={styles.dailyMissions}>
                        <h4 className={styles.sectionTitle}>ماموریت‌های روزانه</h4>
                        <DailyMissions />
                    </div>
                    <div className={styles.leaderBoard}>
                        <h4 className={styles.sectionTitle}>جدول امتیازات</h4>
                        <Leaderboard />
                    </div>
                </div>

                {/* Right Sidebar */}

            </div>
        </div>
    );
}

export default UserCourses;

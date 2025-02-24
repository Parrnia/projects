import React, {useEffect, useState} from "react";
import styles from "./Courses.module.css";
import Header from "../UserCourses/UserCourseHeader";
import DailyMissions from "./DailyMissions";
import Leaderboard from "./LeaderBoard";
import Stepper from "./Stepper";
import Sidebar from "../../Components/Sidebar/Sidebar";
import {Box} from "@mui/material";
import {GetMyCourseContentServices} from "../../Services/GetMyCourseContentServices";
import {maxWidth} from "@mui/system";


function Courses() {

    const [courseContentData, setCourseContentData] = useState(null);
    const token = localStorage.getItem('token');
    // const token = 'c0d73d71-058a-47e5-9776-8df7816a62a2';
    const emailOrPhone = localStorage.getItem('emailOrPhone');
    const contentId = localStorage.getItem('courseId');
    // const contentId = 1;

    useEffect(() => {
        const fetchMyCourseContentData = async () => {
            if (token && emailOrPhone && contentId) {
                const response = await GetMyCourseContentServices(emailOrPhone, token, contentId);
                setCourseContentData(response?.data);
                console.log(response?.data);
            }
        };
        fetchMyCourseContentData();
    }, [token]);

    return (
        <Box sx={{ display: "flex", flexDirection: "row", width: "100%", flexWrap: 'wrap' }}>

            <Sidebar/>
            <Box
                // sx={{
                //     flexGrow: 1,
                //     marginRight: {},
                //    //width: { xs/ "100%", sm: "50%", md: "50%" },
                //
                //     transition: 'margin-right 0.3s ease',
                // }}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    // width: { xs: "100%", sm: "100%", md: "80%" },
                    width: maxWidth,
                    padding: "16px",
                    gap: 2,
                    p:5,
                    marginRight: { md: "260px" , lg:"300px"}
                }}
            >

                <div className={styles.container} dir="rtl">
                    {/* <Sidebar /> */}
                    <Header/>

                    {/* Header Section */}
                    <div className={styles.header}>
                        <h3>{courseContentData?.title}</h3>
                        
                        <div className={styles.details}>
                            <span className={styles.videos}>🎥 {courseContentData?.length} ویدئو</span>
                        </div>
                    </div>

                    {/* Main Content Section */}
                    <div className={styles.content}>
                        {/* Map Section */}
                        <div className={styles.map}>
                    
                            <h4 className={styles.sectionTitle}>مسیر ویدئوها</h4>
                            <Stepper steps={courseContentData}/>
                            
                        </div>

                        {/* Right Column: Daily Missions and Leaderboard */}
                        <div className={styles.rightColumn}>
                            <div className={styles.dailyMissions}>
                                <h4 className={styles.sectionTitle}>ماموریت‌های روزانه</h4>
                                <DailyMissions/>
                            </div>

                            <div className={styles.leaderBoard}>
                                <h4 className={styles.sectionTitle}>جدول امتیازات</h4>
                                <Leaderboard/>
                            </div>
                        </div>
                    </div>

                    {/* <Footer /> */}
                </div>
            </Box>
        </Box>
    );
}

export default Courses;

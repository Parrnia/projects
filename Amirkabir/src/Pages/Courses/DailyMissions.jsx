import React from "react";
import styles from "./DailyMissions.module.css";

const tasks = [
    { title: "100 امتیاز کسب کن", progress: 100, goal: 100 },
    { title: "در یک آزمون 90% از سوالات پاسخ صحیح بده", progress: 0, goal: 1 },
    { title: "دو دوره را به اتمام رسان", progress: 2, goal: 2 },
];

function DailyMissions() {
    return (
        <div className={styles.dailyMissions}>
            <h3>ماموریت های روزانه</h3>
            <ul>
                {tasks.map((task, index) => (
                    <li key={index} className={styles.task}>
                        <span>{task.title}</span>
                        <div className={styles.progressBar}>
                            <div
                                className={styles.filled}
                                style={{ width: `${(task.progress / task.goal) * 100}%` }}
                            ></div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default DailyMissions;

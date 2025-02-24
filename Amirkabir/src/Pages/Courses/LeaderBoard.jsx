import React from "react";
import styles from "./LeaderBoard.module.css";
import amirKabir from "../../Images/UserCourses/AmirKabir.svg"; // Replace with your actual image paths


const dummyData = [
    { name: "هانیه باقری", score: 3456, image: amirKabir },
    { name: "علی رضایی", score: 3210, image: amirKabir },
    { name: "سارا احمدی", score: 2987, image: amirKabir },
    { name: "حمید کاظمی", score: 2875, image: amirKabir },
];

function Leaderboard() {
    return (
        <div className={styles.leaderboard}>
            <h3>برترین ها</h3>
            <ul>
                {dummyData.map((user, index) => (
                    <li key={index} className={styles.item}>
                        <span className={styles.rank}>{index + 1}</span>
                        <img
                            src={user.image}
                            alt={`${user.name}'s avatar`}
                            className={styles.avatar}
                        />
                        <span className={styles.name}>{user.name}</span>
                        <span className={styles.score}>{user.score} امتیاز</span>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default Leaderboard;

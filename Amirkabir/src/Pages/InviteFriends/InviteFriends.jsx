import React, { useState, useEffect } from "react";
import styles from "./InviteFriends.module.css";
import amirkabir from "../../Images/UserCourses/AmirKabir.svg";
import {ProfileServices} from "../../Services/ProfileServices";

function InviteFriends(props) {
    const token = localStorage.getItem('token');
    const [refralcode, setRefralCode] = useState(null);
    const emailOrPhone = localStorage.getItem('emailOrPhone');
    useEffect(() => {
        const fetchProfileData = async () => {

            if (token && emailOrPhone) {
                try {
                    const response = await ProfileServices(emailOrPhone, token);
                    setRefralCode(response.data.refCode);
                } catch {
                }
            }
        };

        fetchProfileData();
    }, [token]);

    const handleClose = () => {
        props.onClose();
    };

    const handleCopyLink = () => {
        navigator.clipboard.writeText(refralcode);
        alert("Link copied to clipboard!");
    };

    return (
        <div dir="rtl">
            {props.open && (
                <div className={styles.dialogBackdrop}>
                    <div className={styles.dialog}>
                        <div className={styles.dialogHeader}>
                            <h2 className={styles.dialogTitle}>دعوت از دوستان</h2>
                            <button
                                className={styles.closeButton}
                                onClick={handleClose}
                                aria-label="Close"
                            >
                                &times;
                            </button>
                        </div>
                        <div className={styles.dialogContent}>
                            <img
                                className={styles.avatar}
                                src={amirkabir}
                                alt="Invite illustration"
                            />
                            <p className={styles.inviteMessage}>دوستات رو دعوت کن!</p>
                            <div className={styles.linkContainer}>
                                <input
                                    type="text"
                                    value={refralcode}
                                    readOnly
                                    className={styles.inviteLink}
                                />
                                <button className={styles.copyButton} onClick={handleCopyLink}>
                                    کپی کردن رفرنس کد
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default InviteFriends;

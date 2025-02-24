import React, { useState } from "react";
import styles from "./Stepper.module.css";
import lockIcon from "../../Images/Course/Lock.svg";
import unlockIcon from "../../Images/Course/unlock.png"; // آیکون قفل باز
import timeIcon from "../../Images/Course/time.svg";
import { useNavigate } from "react-router-dom";
import { Getcontentdetails } from '../../Services/contentdetails';

const CustomStepper = ({ steps }) => {
    const [activeStep, setActiveStep] = useState(0);
    const navigate = useNavigate();

    const handleStepClick = (stepIndex) => {
        setActiveStep(stepIndex);
    };

    const handleExamButtonClick = async (contentId) => {
        const token = localStorage.getItem("token");
        const username = localStorage.getItem("emailOrPhone");
     
        try {
            const response = await Getcontentdetails(username, token, contentId);
            const examId = response.examId;
            console.log(examId);
            navigate("/azmon", { state: { examId } });
        } catch (error) {
            console.error("خطا در دریافت examId:", error);
        }
    };

    return (
        <div className={styles.backgroundMap}>
            {steps
                ?.filter((step) => step.isRegistered)
                .map((step, index) => (
                    <div
                        key={index}
                        role="listitem"
                        className={`${styles.stepItem} ${
                            index <= activeStep ? styles.activeStep : ""
                        }`}
                        onClick={() => handleStepClick(index)}
                    >
                        <div className={styles.stepContent}>
                            <div className={styles.stepIcon}>
                                {/* تغییر آیکون بر اساس وجود آزمون */}
                                <img 
                                    src={step.isExamPassed ?   unlockIcon : lockIcon} 
                                    alt="Step Icon" 
                                />
                            </div>
                            <div className={styles.stepText}>
                                <h5>{step.contentTitle}</h5>

                                {/* نمایش دکمه "شرکت در آزمون" به صورت شرطی */}
                                {!step.isExamPassed && step.isRegistered && step.hasExam && (
                                    <button
                                        className={styles.examButton}
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            handleExamButtonClick(step.contentId);
                                        }}
                                    >
                                        شرکت در آزمون
                                    </button>
                                )}
                                <div className={styles.duration}>
                                    <p>{step.duration}</p>
                                    <img src={timeIcon} alt="Step Icon" />
                                </div>
                            </div>
                        </div>
                    </div>
                ))}
        </div>
    );
};

export default CustomStepper;
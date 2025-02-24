import Login from '../Login/login';
import Signup from '../SignUp/Signup';
import Modal from '../../Components/Modal/modal';
import ForgotPass from '../ForgotPass/ForgotPass';
import Signup_Information from '../SignUp/Signup_Information';
import ThemeProvider from '../../Components/Theme/ThemeProvider';
import VerificationCode from '../VerificationCode/VerificationCode';
import React, { useState } from 'react';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import { Typography, Box, Tab, Tabs, IconButton } from '@mui/material';

const Login_SignUp = (props) => {
    const [verified, setVerified] = useState(false);
    const [showForgotModal, setShowForgotModal] = useState(false);
    const [verificationToken, setVerificationToken] = useState("");
    const [selectedTab, setSelectedTab] = useState(props.selectedTab);
    const [isSignupInfoModal, setIsSignupInfoModal] = useState(false);
    const [showVerificationModal, setShowVerificationModal] = useState(false);

    const handleChange = (event, newValue) => {
        setSelectedTab(newValue);
    };

    const handlePasswordResetSuccess = () => {
        setSelectedTab(0);
        setShowForgotModal(false);
        setShowVerificationModal(false);
    };

    const closeModal = (bool) => {
        setShowForgotModal(false);
        setVerified(false);
        setVerificationToken("");
        setIsSignupInfoModal(false);
        setShowVerificationModal(false);

        if(bool)
            props.onClose();
    };

    const handleVerificationSuccess = () => {
        if (selectedTab === 1)
            setIsSignupInfoModal(true);
        else
            setShowForgotModal(true);
    };

    const currentComponent = selectedTab === 0 
        ? <Login openForgotModal={() => setShowForgotModal(true)} />
        : (
            <Signup
                token={verificationToken}
                showVerificationModal={() => setShowVerificationModal(true)}
            />);

    return (
        <ThemeProvider>
            {!showForgotModal && !showVerificationModal && !isSignupInfoModal ?
                <Modal component={
                    <Box sx={{ p: 2 }}>
                        <Tabs
                            value={selectedTab}
                            onChange={handleChange}
                            textColor="inherit"
                            indicatorColor="primary"
                            sx={{ mb: 3 }}
                        >
                            <Tab label="ورود" sx={{ width: '50%' }} />
                            <Tab label="ثبت نام" sx={{ width: '50%' }} />
                        </Tabs>

                        <Typography variant="h5" sx={{ fontWeight: 'bold', my: 2, textAlign: 'center' }}>
                            امیرکبیر
                        </Typography>

                        {currentComponent}  
                    </Box>
                } 
                open={props.open} 
                onClose={() => closeModal(true)} /> 
                : (
                    <Modal
                        open={props.open}
                        onClose={() => closeModal(true)}
                        component={
                            verified ? (
                                isSignupInfoModal ? (
                                    <Signup_Information 
                                        onClose={() => closeModal(true)}
                                        token={verificationToken} 
                                    />
                                ) : (
                                    <ForgotPass
                                        token={verificationToken}
                                        passwordReset={handlePasswordResetSuccess}
                                    />
                                )
                            ) : (showForgotModal && !showVerificationModal?
                                    <>
                                        <IconButton
                                            sx={{ position: 'absolute', top: 8, right: 8 }}
                                            onClick={() => closeModal(false)}
                                        >
                                            <ArrowForwardIcon />
                                        </IconButton>
                                        <Typography
                                            variant="h6"
                                     
                                            sx={{
                                                mb: 3,
                                                textAlign: 'center',
                                                color: '#333',
                                            }}
                                        >
                                            بازیابی رمزعبور
                                        </Typography>
                                        
                                        <Signup
                                            p={2}
                                            token={verificationToken}
                                            showVerificationModal={() => setShowVerificationModal(true)}
                                        />
                                    </>
                                :
                                    <VerificationCode
                                        closeVerificationModal={() => closeModal(false)}
                                        setVerified={setVerified}
                                        setVerificationToken={setVerificationToken}
                                        onVerificationSuccess={handleVerificationSuccess}
                                    />)
                        }
                    />
                )
            }
        </ThemeProvider>
    );
};

export default Login_SignUp;
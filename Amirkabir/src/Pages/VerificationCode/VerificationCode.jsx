import { CheckOtp } from '../../Services/CheckOtp';
import { SentOtpRetry } from '../../Services/SentOtpRetry';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import React, { useState, useEffect, useRef } from 'react';
import { Typography, Box, TextField, Button, Grid, IconButton } from '@mui/material';

const VerificationCode = (props) => {
    const email = localStorage.getItem('emailOrPhone');
    const [code, setCode] = useState(['', '', '', '']);
    const [canResend, setCanResend] = useState(false);
    const [error, setError] = useState(false);
    const [timer, setTimer] = useState(30);
    const inputRefs = useRef([]);

    useEffect(() => {
        const countdown = setInterval(() => {
            setTimer((prev) => {
                if (prev === 1) {
                    setCanResend(true);
                    clearInterval(countdown);
                    return 0;
                }
                return prev - 1;
            });
        }, 1000);

        return () => clearInterval(countdown);
    }, [timer === 0]);

    const handleCodeChange = (value, index) => {
        const newCode = [...code];
        newCode[index] = value.slice(0, 1);
        setCode(newCode);

        if (value && index > 0)
            inputRefs.current[index - 1].focus();
    };

    const handleKeyDown = (event, index) => {
        if (event.key === 'Backspace' && code[index] === '' && index < code.length - 1) {
            inputRefs.current[index + 1]?.focus();
        }
    };

    const handleResend = async () => {
        setTimer(30);
        setError(false);
        setCanResend(false);

        try 
        {
            const response = await SentOtpRetry(email);
        } 
        catch (error) {
            console.error(error);
        }
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        try 
        {
            const response = await CheckOtp(code, email);

            if(response.status === 200)
            {
                setError(false);
                props.setVerified(true);
                props.setVerificationToken(response.data);
                props.onVerificationSuccess();
            }
            else
            {
                setError(true);
            }
        } 
        catch (error) 
        {
            setError(true);
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit}>
            <IconButton
                sx={{ position: 'absolute', top: 8, right: 8 }}
                onClick={props.closeVerificationModal}
            >
                <ArrowForwardIcon />
            </IconButton>

            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                <Typography variant="h5" sx={{ fontWeight: 'bold', my: 2, textAlign: 'center' }}>
                    امیرکبیر
                </Typography>

                <Typography sx={{ mb: 1 }}>
                    کد تایید را وارد کنید
                </Typography>
                <Typography sx={{ mb: 1, fontSize: '13px' }}>
                    کد چهار رقمی ارسال شده جهت ساخت حساب کاربری را وارد کنید
                </Typography>

                <Grid container spacing={1} justifyContent="center">
                    {code.map((digit, index) => (
                        <Grid item key={index} xs={2}>
                            <TextField
                                variant="outlined"
                                inputProps={{ style: { textAlign: 'center' } }}
                                value={digit}
                                onChange={(e) => handleCodeChange(e.target.value, index)}
                                onKeyDown={(e) => handleKeyDown(e, index)}
                                inputRef={(el) => (inputRefs.current[index] = el)}

                                sx={{
                                    borderColor: error ? 'red' : 'initial',
                                    '& .MuiOutlinedInput-notchedOutline': {
                                        borderColor: error ? 'red' : 'initial',
                                    },
                                }}
                            />
                        </Grid>
                    ))}
                </Grid>

                <Box sx={{ textAlign: 'center', mt: 2 }}>
                    <Typography variant="body2" color="textSecondary">
                        {timer > 0 ? `00:${timer.toString().padStart(2, '0')}` : ''}
                    </Typography>

                    {canResend && (
                        <Typography
                            variant="body2"
                            color="primary"
                            onClick={handleResend}
                            sx={{ cursor: 'pointer', textDecoration: 'underline' }}
                        >
                            پیام دریافت نکردید؟ ارسال دوباره
                        </Typography>
                    )}
                </Box>

                <Button
                    type="submit"
                    variant="contained"
                    sx={{ mt: 1, height: '48px', fontSize: '16px' }}
                >
                    تایید
                </Button>

                {error && (
                    <Typography sx={{ mt: 2, color: 'red', textAlign: 'center' }}>
                       کد وارد شده نامعتبر است
                    </Typography>
                )}
            </Box>
        </Box>
    );
};

export default VerificationCode;
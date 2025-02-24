import Validation from './SignupValidation';
import { SendOtp } from '../../Services/SendOtp';
import { Captcha } from '../../Services/Captcha';
import TextInput from '../../Components/Inputs/TextField';
import React, { useState, useEffect } from 'react';
import { Button, Box, Typography, CircularProgress } from '@mui/material';

const Signup = (props) => {
    const [values, setValues] = useState({
        email: '',
        code: '',
        token: '',
        captchaImage: '',
    });

    const [errors, setErrors] = useState({});
    const [showError, setShowError] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [isCaptchaLoading, setIsCaptchaLoading] = useState(false);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setValues((prevValues) => ({ ...prevValues, [name]: value }));
    };

    const fetchCaptcha = async () => {
        setIsCaptchaLoading(true);
        
        try 
        {
            const captchaData = await Captcha();
            
            if (captchaData) 
            {
                setValues((prevValues) => ({
                    ...prevValues,
                    token: captchaData.token,
                    captchaImage: captchaData.captchaImage,
                }));
            }
        } 
        catch (error) {
            console.error('Error fetching CAPTCHA:', error);
        } 
        finally {
            setIsCaptchaLoading(false);
        }
    };

    useEffect(() => {
        fetchCaptcha();
    }, []);

    const handleSubmit = async (event) => {
        event.preventDefault();

        const [err, isValid] = Validation(values);
        setErrors(err);

        if (!isValid) 
        {
            setShowError(true);
            return;
        }

        setIsSubmitting(true);
       
        try 
        {
            const response = await SendOtp(values.email, values.token, values.code);

            if (response.status === 200) 
            {
                localStorage.setItem('emailOrPhone', values.email);
                setShowError(false);
                props.showVerificationModal();
            } 
            else 
            {
                setErrors((prevErrors) => ({
                    ...prevErrors,
                    server: response.data.Message,
                }));
            
                setShowError(true);
            }
        } 
        catch (error) {
            console.error('Error sending OTP:', error);
        } 
        finally {
            setIsSubmitting(false);
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit}>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5, p:props.p }}>
                <TextInput
                    name="email"
                    label="ایمیل"
                    placeholder="آدرس ایمیل خود را وارد کنید"
                    value={values.email}
                    error={errors.email}
                    onChange={handleInputChange}
                />

                <Box
                    onClick={fetchCaptcha}
                    
                    sx={{
                        position: 'relative',
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        width: '70%',
                        height: '70px',
                        cursor: 'pointer',
                        backgroundColor: '#f3f3f3',
                        border: '1px solid #ccc',
                        borderRadius: '4px',
                        margin: '16px auto',
                        overflow: 'hidden',
                    }}
                >
                    {isCaptchaLoading ? (
                        <CircularProgress size={24} />
                    ) : values.captchaImage ? (
                        <img
                            src={values.captchaImage}
                            alt="Captcha"
                            style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                        />
                    ) : (
                        <Typography color="textSecondary">
                            برای بارگذاری CAPTCHA کلیک کنید
                        </Typography>
                    )}
                </Box>

                <TextInput
                    name="code"
                    label="کد امنیتی"
                    placeholder="کد امنیتی را وارد کنید"
                    value={values.code}
                    error={errors.code}
                    onChange={handleInputChange}
                />

                {errors.server && (
                    <Typography color="error" variant="body2">
                        {errors.server}
                    </Typography>
                )}

                <Button
                    type="submit"
                    variant="contained"
                    
                    sx={{
                        mt: 2,
                        height: '48px',
                        fontSize: '16px',
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        backgroundColor: 'primary.main',
                        '&:disabled': {
                            backgroundColor: 'primary.main',
                            color: '#fff',
                        },
                    }}
                    disabled={isSubmitting}
                >
                    {isSubmitting && <CircularProgress size={20} sx={{ mr: 2, color: '#fff' }} />}
                    دریافت کد       
                </Button>
            </Box>
        </Box>
    );
};

export default Signup;
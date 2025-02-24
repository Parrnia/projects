import Validation from './loginValidation';
import GoogleLoginButton from './GoogleLoginButton';
import TextField from '../../Components/Inputs/TextField';
import { LoginServices } from '../../Services/loginServices';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Typography, Box, Divider } from '@mui/material';

const Login = ({ openForgotModal }) => {
    const [values, setValues] = useState({
        emailOrPhone: "",
        password: "",
    });

    const navigate = useNavigate();
    const [errors, setErrors] = useState({});
    const [showError, setShowError] = useState(false);
    const [loginMessage, setLoginMessage] = useState("");
    const [showPassword, setShowPassword] = useState(false);

    const togglePassVisibility = () => setShowPassword(!showPassword);

    const handleInputChange = async (event, fromForgotPassword = false) => {
        if (fromForgotPassword) 
        {
            openForgotModal();
        } 
        else
        {
            const { name, value } = event.target;
            setValues(prevValues => ({ ...prevValues, [name]: value }));
        }
    };

    let data = {
        username: values.emailOrPhone,
        password: values.password,
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const [err, isValid] = Validation(values);
        setErrors(err);

        try 
        {
            if (isValid) 
            {
                const response = await LoginServices(data);

                if (response.status === 200) 
                {
                    localStorage.setItem('isLogin', '1');
                    localStorage.setItem('token', response.data.token);
                    localStorage.setItem('emailOrPhone', values.emailOrPhone);

                    if(response.data.role == "Admin")
                        navigate('/adminpanel/courses');
                    else
                        navigate('/profile');

                    setShowError(false);
                }
                else
                {
                    setLoginMessage("ورود ناموفق!");
                    setShowError(true);
                }
            }
            else
            {
                setShowError(true);
            }
        } 
        catch (error) 
        {
            setLoginMessage("ورود ناموفق!");
            setShowError(true);
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit}>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                <TextField
                    label="ایمیل یا شماره تلفن"
                    placeholder="ایمیل یا شماره تلفن خود را وارد کنید"
                    name="emailOrPhone"
                    value={values.emailOrPhone}
                    onChange={handleInputChange}
                    error={errors.emailOrPhone}
                />

                <TextField
                    label="رمز"
                    placeholder="رمز خود را وارد کنید"
                    name="password"
                    value={values.password}
                    onChange={handleInputChange}
                    error={errors.password}
                    type={showPassword ? "text" : "password"}
                    showPasswordToggle={togglePassVisibility}
                />

                <Button 
                    type="submit"
                    variant="contained" 
                    sx={{ mt: 1, height: '48px', fontSize: '16px' }}
                >
                    ورود
                </Button>

                <Typography sx={{ mt: 1, fontSize: '12px' }}>
                    رمز عبور خود را فراموش کرده‌اید؟{' '}
                    
                    <Typography 
                        component="a"
                        onClick={(event) => handleInputChange(event, true)}
                        sx={{ display: 'inline', color: '#00a2a7', fontSize: '12px' }}
                    >
                        بازیابی رمز عبور
                    </Typography>
                </Typography>

                <Divider sx={{ my: 1 }}>یا</Divider>

                <GoogleLoginButton />

                {loginMessage && (
                    <Typography sx={{ mt: 2, color: showError ? 'red' : 'green', textAlign: 'center' }}>
                        {loginMessage}
                    </Typography>
                )}
            </Box>
        </Box>
    );
};

export default Login;
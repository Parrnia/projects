
import { ForgotPassServices } from '../../Services/ForgotPassServices';
import TextInput from '../../Components/Inputs/TextField';
import Validation from './ForgotpassValidation';
import React, { useState } from 'react';
import EmailOutlinedIcon from '@mui/icons-material/EmailOutlined';
import { TextField, Button, Typography, Box, InputAdornment } from '@mui/material';

const ForgotPass = (props) => {
    const [values, setValues] = useState({
        newPass: '',
        confirmPass: '',
    });

    const [errors, setErrors] = useState({});
    const [showError, setShowError] = useState(false);
    const [showNewPass, setShowNewPass] = useState(false);
    const [showConfirmPass, setShowConfirmPass] = useState(false);
    const [resetMessage, setResetMessage] = useState("");
    const email = localStorage.getItem('emailOrPhone');

    const toggleNewPassVisibility = () => setShowNewPass(!showNewPass);
    const toggleConfirmPassVisibility = () => setShowConfirmPass(!showConfirmPass);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setValues(prevValues => ({ ...prevValues, [name]: value }));
    };

    let data = {
        password: values.newPass,
        confirmPassword: values.confirmPass,
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const [err, isValid] = Validation(values);
        setErrors(err);

        try 
        {
            if (isValid) 
            {
                const status = await ForgotPassServices(email, props.token, data);

                if (status === 200) 
                {
                    setResetMessage("تغییر رمز عبور با موفقیت انجام شد");
                    setShowError(false);

                    setTimeout(() => {
                        props.passwordReset();
                    }, 3000);
                }
                else
                {
                    setResetMessage("ایمیل مورد نظر یافت نشد");

                    setTimeout(() => {
                        props.passwordReset();
                    }, 3000);

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
            setShowError(true);
        }
      
    };

    return (
        <Box
            component="form"
            onSubmit={handleSubmit}

            sx={{
                margin: '0 auto',
                padding: '30px 25px',
            }}
        >
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                <Typography
                    variant="h6"
                    sx={{
                        mb: 3,
                        textAlign: 'center',
                        color: '#333',
                    }}
                >
                    بروزرسانی رمزعبور
                </Typography>

                <TextInput
                    label="رمز جدید"
                    name="newPass"
                    LockOutlinedIcon = "true" 
                    value={values.newPass}
                    onChange={handleInputChange}
                    error={errors.newPass}
                    type={showNewPass ? "text" : "password"}
                    showPasswordToggle={toggleNewPassVisibility}
                />

                <TextInput
                    label="تکرار رمز جدید"
                    name="confirmPass"
                    LockOutlinedIcon = "true" 
                    value={values.confirmPass}
                    onChange={handleInputChange}
                    error={errors.confirmPass}
                    type={showConfirmPass ? "text" : "password"}
                    showPasswordToggle={toggleConfirmPassVisibility}
                />

                <Typography variant="body1">
                    ایمیل
                </Typography>
                <TextField
                    disabled
                    placeholder={email}
                    fullWidth
                    
                    inputProps={{
                        style: {
                            height: '20px',
                            padding: '15px',
                            textAlign: 'left',
                            color: '#aaa',
                            fontSize: 'small'
                        }
                    }}

                    InputProps={{
                        startAdornment: (
                            <InputAdornment position="start" sx={{ pr: 0.5 }}>
                                <EmailOutlinedIcon />
                            </InputAdornment>
                        )
                    }}
                    sx={{
                        mt: 0,
                        '& .MuiInputBase-root': {
                            backgroundColor: '#f9f9f9',
                        }
                    }}
                />

                <Button 
                    type="submit"
                    variant="contained" 
                    sx={{ mt: 1, height: '48px', fontSize: '16px' }}
                >
                    بروزرسانی رمز جدید
                </Button>

                {resetMessage && (
                    <Typography sx={{ mt: 2, color: showError ? 'red' : 'green', textAlign: 'center' }}>
                        {resetMessage}
                    </Typography>
                )}

            </Box>
        </Box>
    );
};

export default ForgotPass;
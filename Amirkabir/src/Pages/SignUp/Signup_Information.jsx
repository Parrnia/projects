import { SignupServices } from '../../Services/signUpServices';
import TextInput from '../../Components/Inputs/TextField';
import Validation from './SignupValidation';
import React, { useState } from 'react';
import { Button, Box, Typography } from '@mui/material';

const SignupInfo = (props) => {
    const [values, setValues] = useState({
        SignupInfo:true,
        name:'',
        email: '',
        pass: '',
        confirmPass: '',
    });
    
    const [errors, setErrors] = useState({});
    const [showPass, setShowPass] = useState(false);
    const [showError, setShowError] = useState(false);
    const [signupMessage, setSignupMessage] = useState("");
    const [showConfirmPass, setShowConfirmPass] = useState(false);
    const email = localStorage.getItem('emailOrPhone');

    const togglePassVisibility = () => setShowPass(!showPass);
    const toggleConfirmPassVisibility = () => setShowConfirmPass(!showConfirmPass);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setValues(prevValues => ({ ...prevValues, [name]: value }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const [err, isValid] = Validation(values);
        setErrors(err);

        try
        {
            const nameParts = values.name.trim().split(' ');
            const firstName = nameParts[0] || '';
            const lastName = nameParts[1] || ''; 

            let data = {
                mobile: values.phone,
                firstName: firstName,
                lastName: lastName,
                password: values.pass,
                confirmPassword: values.confirmPass,
            };

            if (isValid)
            {

                const response = await SignupServices(email, props.token, data);

                if (response.status === 200) 
                {
                    setSignupMessage("ثبت نام با موفقیت انجام شد");
                    setShowError(false);

                    setTimeout(() => {
                        props.onClose();
                    }, 4000);
                }
            }
            else
            {
                setShowError(true);
            }
        }
        catch (error)
        {
            setSignupMessage("این ایمیل قبلا ثبت نام کرده است");
            setShowError(true);

            setTimeout(() => {
                props.onClose();
            }, 4000);
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit}>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                <Typography variant="h5" sx={{ fontWeight: 'bold', my: 2, textAlign: 'center' }}>
                    امیرکبیر
                </Typography>

                <Typography sx={{ fontWeight: 'bold' }}>
                    ثبت نام
                </Typography>

                <TextInput
                    icon="true"
                    name="name"
                    PermIdentityIcon="true"
                    label="نام و نام خانوادگی"
                    placeholder="نام خود را وارد کنید"
                    value={values.name}
                    error={errors.name}
                    onChange={handleInputChange}
                />

                <TextInput
                    icon="true"
                    name="phone"
                    label="شماره تماس"
                    placeholder="شماره تماس خود را وارد کنید"
                    PhoneEnabledOutlinedIcon = "true" 
                    value={values.phone}
                    error={errors.phone}
                    onChange={handleInputChange}
                />

                <TextInput
                    name="pass"
                    label="رمز"
                    LockOutlinedIcon = "true" 
                    placeholder="رمز را وارد کنید"
                    value={values.pass}
                    error={errors.pass}
                    onChange={handleInputChange}
                    type={showPass ? "text" : "password"}
                    showPasswordToggle={togglePassVisibility}
                />

                <TextInput
                    name="confirmPass"
                    LockOutlinedIcon = "true" 
                    label="تکرار رمز عبور"
                    placeholder="رمز را وارد کنید"
                    value={values.confirmPass}
                    error={errors.confirmPass}
                    onChange={handleInputChange}
                    type={showConfirmPass ? "text" : "password"}
                    showPasswordToggle={toggleConfirmPassVisibility}
                />

                <Button
                    type="submit"
                    variant="contained"
                    sx={{ mt: 2, height: '48px', fontSize: '16px' }}
                >
                    ثبت نام
                </Button>

                {signupMessage && (
                    <Typography sx={{ mt: 2, color: showError ? 'red' : 'green', textAlign: 'center' }}>
                        {signupMessage}
                    </Typography>
                )}
            </Box>
        </Box>
    );
};

export default SignupInfo;
import { GoogleLoginServices } from '../../Services/GoogleLoginServices';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import React, { useEffect, useState } from 'react';
import { Button, Typography, CircularProgress } from '@mui/material';

const GoogleLoginButton = () => {
    const navigate = useNavigate();
    const [errorMessage, setErrorMessage] = useState("");
    const [loading, setLoading] = useState(false);
    const clientId = '1037838162157-9luf9vl41etme613oe3gpphqskchg466.apps.googleusercontent.com';

    useEffect(() => {
        localStorage.clear();  
        loadGoogleScript();  
    }, []);

    const loadGoogleScript = () => {
        const script = document.createElement('script');
        script.src = "https://accounts.google.com/gsi/client?hl=fa"; 
        script.async = true;
        script.onload = () => initializeGoogleSignIn();
        document.body.appendChild(script);
    };

    const initializeGoogleSignIn = () => {
        window.google.accounts.id.initialize({
            client_id: clientId,
            callback: handleLoginSuccess,
        });
        window.google.accounts.id.renderButton(
            document.getElementById('google-signin-btn'),
            { theme: 'outline', size: 'large', text: 'signin_with' } 
        );
    };

    const sanitizeToken = (token) => token.replace(/[^\x20-\x7E]/g, '');

    const handleLoginSuccess = async (response) => {
        setLoading(true);
        try {
            const sanitizedToken = sanitizeToken(response.credential);
            const decodedToken = jwtDecode(sanitizedToken);
            const userEmail = decodedToken.email;

            await handleBackendLogin(sanitizedToken, userEmail);
        } catch (error) {
            setErrorMessage("ورود ناموفق!");  
        } finally {
            setLoading(false);
        }
    };

    const handleBackendLogin = async (token, userEmail) => {
        try {
            const res = await GoogleLoginServices(token); 
            
            if (res.status === 200 && res.data.isRegisterd) 
            {
                localStorage.setItem('isLogin', '1');
                localStorage.setItem('token', res.data.token);
                localStorage.setItem('emailOrPhone', userEmail);
                
                if(res.data.role == "Admin")
                    navigate('/adminpanel/courses');
                else
                    navigate('/profile');
            } 
            else 
            {
                setErrorMessage("ورود ناموفق!");  
            }
        } catch (error) {
            setErrorMessage("ورود ناموفق!"); 
        }
    };

    return (
        <div style={{ textAlign: 'center' }}>
            <div id="google-signin-btn"></div>
            {errorMessage && (
                <Typography sx={{ mt: 2, color: 'red', textAlign: 'center' }}>
                    {errorMessage} 
                </Typography>
            )}
        </div>
    );
};

export default GoogleLoginButton;
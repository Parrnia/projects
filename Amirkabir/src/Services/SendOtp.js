import Api from './Api';

export const SendOtp = async (email, token, code) => {
    try
    {
        let headers = {
            'captcha-token': token,
            'captcha-value': code
        };

        const SendOtp = await Api.post(`/User/SendOtp?Email=${encodeURIComponent(email)}`, null, { headers });

        return SendOtp;
    }
    catch (error)
    {
        return error.response;
    }
};
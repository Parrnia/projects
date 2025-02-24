import Api from './Api';

export const SentOtpRetry = async (email) => {
    try
    {
        const SendOtp = await Api.post(`/User/SentOtpRetry?Email=${encodeURIComponent(email)}`);
        return SendOtp;
    }
    catch (error)
    {
        return error.response;
    }
};
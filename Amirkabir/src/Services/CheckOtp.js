import Api from './Api';

export const CheckOtp = async (code, email) => {
    try
    {
        const otp = code.join('').split('').reduce((acc, char) => char + acc, '');

        const CheckOtp = await Api.post(`/User/CheckOtp?Email=${encodeURIComponent(email)}&otp=${encodeURIComponent(otp)}`);

        return CheckOtp;
    }
    catch (error)
    {
    }
};
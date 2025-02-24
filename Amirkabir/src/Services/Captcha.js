import Api from './Api';

export const Captcha = async () => {
    try 
    {
        const response = await Api.get('/Captcha');
        
        if (response?.data) 
        {
            return {
                token: response.data.token,
                captchaImage: `data:image/png;base64,${response.data.fileData}`,
            };
        }
    } 
    catch (error) 
    {
        console.error('Error fetching CAPTCHA:', error);
        return null;
    }
};
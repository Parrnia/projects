import Api from './Api';

export const GoogleLoginServices = async (token) => {
    try 
    {
        const response = await Api.post('/auth/google', {
            'token': token,
        },);
        return response;
    } 
    catch (error) 
    {
    }
};
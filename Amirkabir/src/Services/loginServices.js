import Api from './Api';

export const LoginServices = async (data) => {
    try 
    {
        const response = await Api.post('/User/Login',
            data, { headers: { "Content-Type": "application/json-patch+json" } });
    
        return response;
    } 
    catch (error) 
    {
    }
};
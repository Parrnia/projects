import Api from './Api';

export const ForgotPassServices = async (username, token, data) => {
    try 
    {
        let headers = {
            'Username': username,
            'Token': token
        };

        const response = await Api.post('/User/ForgetPasswordRegister', data, { headers });
        return response.status;
    } 
    catch (error) 
    {
    }
};
import Api from './Api';

export const SignupServices = async (username, token, data) => {
    try 
    {
        let headers = {
            'Username': username,
            'Token': token
        };

        const response = await Api.post('/User/Register', data, { headers });
        return response;
    } 
    catch (error) 
    {
    }
};
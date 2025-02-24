import Api from './Api';

export const ProfileServices = async (username, token) => {
    try 
    {
        let headers = {
            'Username': username,
            'Token': token
        };

        const response = await Api.get('/User/GetProfile', { headers });
        return response;
    } 
    catch (error) 
    {
    }
};
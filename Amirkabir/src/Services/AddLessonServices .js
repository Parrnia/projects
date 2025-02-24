import Api from './Api';

export const AddLessonServices = async (username, token, data) => {
    try 
    {
        let headers = {
            'Username': username,
            'Token': token,
            'Content-Type': 'multipart/form-data'
        };

        const response = await Api.post('Content/CreateContent', data, { headers });
        return response;
    } 
    catch (error) 
    {
        console.log(error);
    }
};
import Api from './Api';

export const AddCourseServices = async (username, token, data) => {
    try 
    {
        let headers = {
            'Username': username,
            'Token': token,
            'Content-Type': 'multipart/form-data'
        };

        const response = await Api.post('Course/CreateCourse', data, { headers });
        return response;
    } 
    catch (error) 
    {
        return error;
    }
};
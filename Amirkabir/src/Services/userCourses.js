import Api from './Api';

export const fetchCourses = async (userName = null, Token) => {
    try 
    {
        const response = await Api.get('/Course/GetAllCourses', {
            headers: {
                'accept': '*/*',
                'Username': userName,
                'Token': Token,
            },
        });
       console.log( response.data);
        return response.data;
    } 
    catch (error) {
        console.error('Error fetching courses:', error);
        throw error;
    }
};
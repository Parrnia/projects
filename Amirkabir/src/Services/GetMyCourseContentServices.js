import Api from './Api';

export const GetMyCourseContentServices = async (username, token, courseId) => {
    try {
        let headers = {
            'Username': username,
            'Token': token
        };

        const response = await Api.get('/Content/GetMyCourseContent', {headers, params: {courseId: 1}});
        return response;
    } catch (error) {
    }
};

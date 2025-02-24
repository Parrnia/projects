import Api from './Api'; 

export const fetchExam = async (examId, userName = null, token) => {
    console.log(token)    ;
  try {
    if (!token || !userName || !examId) {
      throw new Error("Missing required parameters: token, username, or examId.");
    }

    const response = await Api.get(`/Exam/GetExam/${examId}`, {
      headers: {
        'Username': userName,
        'Token': token
      }
    
    });
    console.log(response.data)    ;
    return response.data; 
    ;
  } catch (error) {
    console.error('Error fetching exam:', error);
    throw error;
  }
};

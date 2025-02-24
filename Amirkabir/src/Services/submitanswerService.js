import Api from './Api'; 

export const submitUserAnswer = async (examId, questionId, selectedOptionId, userName, token) => {
  try {

    if (!token || !userName || !examId || !questionId || !selectedOptionId) {
      throw new Error("Missing required parameters: token, username, examId, questionId, or selectedOptionId.");
    }

    const requestBody = {
      examId,
      questionId,
      selectedOptionId,
    };

    const response = await Api.post(`/Exam/SubmitUserAnswer`, requestBody, {
      headers: {
        'Username': userName,
        'Token': token,
      },
    });

    console.log('Answer submitted successfully:', response.data);
    return response.data; 
  } catch (error) {
    console.error('Error submitting user answer:', error);
    throw error; 
  }
};

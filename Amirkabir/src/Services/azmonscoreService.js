import Api from './Api'; 

export const fetchUserScore = async (userName = null, token) => {
  try {
    if (!token || !userName) {
      throw new Error("Missing required parameters: token or username.");
    }

    const response = await Api.get('/UserScore/GetScore', {
      headers: {
        'Username': userName,
        'Token': token,
      },
    });

    console.log(response.data);

    return response.data;
  } catch (error) {
    console.error('Error fetching user score:', error);
    throw error;  
  }
};

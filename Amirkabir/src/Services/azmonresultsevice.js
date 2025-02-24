import Api from './Api'; 

export const fetchUserExamResults = async (username = null, token = null, examId = null) => {
  try {
    // بررسی وجود پارامترهای ضروری
    if (!username || !token || examId === null) {
      throw new Error("Missing required parameters: username, token, or examId.");
    }

    // درخواست به API
    const response = await Api.get('/UserExamResult/GetUserExamResults', {
      headers: {
        'Username': username,
        'Token': token,
      },
      params: {
        examId, // اضافه کردن پارامتر به query
      },
    });

    console.log(response.data);

    return response.data; // بازگرداندن داده‌های پاسخ
  } catch (error) {
    console.error('Error fetching user exam results:', error);
    throw error; // پرتاب خطا برای مدیریت در فراخوانی
  }
};

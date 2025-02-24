import Api from './Api';

export const Getcontentdetails = async (username, token, contentId) => {
    try {
        // بررسی پارامترهای ورودی
        if (!username || !token || !contentId) {
            throw new Error("Missing required parameters: username, token, or contentId.");
        }

        // تنظیم هدرها
        const headers = {
            'Username': username,
            'Token': token
        };

        // ارسال درخواست به API
        const response = await Api.get('/Content/GetContentDetail', {
            headers: headers,
            params: { contentId: contentId } // ارسال contentId به عنوان پارامتر
        });

        // چاپ پاسخ برای دیباگ
        console.log(response.data);

        // بازگرداندن داده‌های پاسخ
        return response.data;
    } catch (error) {
        // ثبت خطا و پرتاب آن
        console.error('Error fetching content details:', error);
        throw error;
    }
};
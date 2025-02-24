import { toJalaali } from 'jalaali-js';

export const convertToPersianDate = (dateStr) => {
    const date = new Date(dateStr);
    
    if (!isNaN(date.getTime())) 
    {
        const year = date.getFullYear();
        const month = date.getMonth() + 1;
        const day = date.getDate();

        const persianDateObj = toJalaali(year, month, day);

        const persianMonths = [
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        ];

        const persianMonth = persianMonths[persianDateObj.jm - 1];

        return [
            persianDateObj.jy,
            persianMonth,
            persianDateObj.jd,
        ];
    } 
    else
        return ["Invalid Date"];
};
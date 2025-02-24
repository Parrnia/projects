import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'amountToWords',
})
export class AmountToWordsPipe implements PipeTransform {
  transform(value: string | number, type: 'rial' | 'toman' = 'rial'): string {
    if (value === null || value === undefined || value === '') {
      return '';
    }

    if (type === 'toman') {
      return wordifyRialsInTomans(value);
    } else {
      return wordifyRials(value);
    }
  }
}

export function wordifyRialsInTomans(num: string | number): string {
  if (num === null || num === undefined || num === '') {
    return '';
  }
  if (typeof num == 'string') {
    var cleanNumber = toEnglishDigits(num);
    num = parseInt(cleanNumber);
  }

  const originalAmount = num;
  if (num >= 10 || num <= -10) {
    num = Math.floor(num / 10);
  } else {
    num = 0;
  }
  const haveRial = (originalAmount / 10).toString().split('.')[1];
  return (
    (num ? wordifyfa(num, 0) + ' تومان' : '') +
    (num && haveRial ? ' و ' : '') +
    (haveRial ? `${wordifyfa(haveRial, 0)} ریال` : '')
  );
}

export function wordifyRials(num: string | number): string {
  if (num === null || num === undefined || num === '') {
    return '';
  }
  return wordifyfa(num, 0) + ' ریال';
}

export function wordifyfa(input: string | number, level: number = 0): string {
  if (input === null) {
    return '';
  }

  let num: number = parseInt(toEnglishDigits(input));

  if (num < 0) {
    num = num * -1;
    return 'منفی ' + wordifyfa(num, level);
  }
  if (num === 0) {
    if (level === 0) {
      return 'صفر';
    } else {
      return '';
    }
  }

  let result = '';
  const yekan = ['یک', 'دو', 'سه', 'چهار', 'پنج', 'شش', 'هفت', 'هشت', 'نه'],
    dahgan = ['بیست', 'سی', 'چهل', 'پنجاه', 'شصت', 'هفتاد', 'هشتاد', 'نود'],
    sadgan = [
      'یکصد',
      'دویست',
      'سیصد',
      'چهارصد',
      'پانصد',
      'ششصد',
      'هفتصد',
      'هشتصد',
      'نهصد',
    ],
    dah = [
      'ده',
      'یازده',
      'دوازده',
      'سیزده',
      'چهارده',
      'پانزده',
      'شانزده',
      'هفده',
      'هیجده',
      'نوزده',
    ];

  if (level > 0) {
    result += ' و ';
    level -= 1;
  }

  if (num < 10) {
    result += yekan[num - 1];
  } else if (num < 20) {
    result += dah[num - 10];
  } else if (num < 100) {
    result += dahgan[Math.floor(num / 10) - 2] + wordifyfa(num % 10, level + 1);
  } else if (num < 1000) {
    result +=
      sadgan[Math.floor(num / 100) - 1] + wordifyfa(num % 100, level + 1);
  } else if (num < 1000000) {
    result +=
      wordifyfa(Math.floor(num / 1000), level) +
      ' هزار' +
      wordifyfa(num % 1000, level + 1);
  } else if (num < 1000000000) {
    result +=
      wordifyfa(Math.floor(num / 1000000), level) +
      ' میلیون' +
      wordifyfa(num % 1000000, level + 1);
  } else if (num < 1000000000000) {
    result +=
      wordifyfa(Math.floor(num / 1000000000), level) +
      ' میلیارد' +
      wordifyfa(num % 1000000000, level + 1);
  } else if (num < 1000000000000000) {
    result +=
      wordifyfa(Math.floor(num / 1000000000000), level) +
      ' تریلیارد' +
      wordifyfa(num % 1000000000000, level + 1);
  }

  return result;
}

export function toEnglishDigits(input: string | number): string {
  return input
    .toString()
    .replace(/[۰-۹]/g, (d: string) =>
      String.fromCharCode(d.charCodeAt(0) - 1728)
    );
}
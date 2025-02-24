import { Injectable } from '@angular/core';
import { CreditDto } from '../../web-api-client';
import * as momentJalaali from "moment-jalaali";
import { Credit } from '../../interfaces/credit';
@Injectable({
  providedIn: 'root'
})
export class CreditMapperService {

  constructor() { }

  //#region DatabaseToFront
  mapCredites(creditDtos: CreditDto[]) {
    let credits: Credit[] = [];
    creditDtos.forEach(c => {
      credits.push(this.mapCredit(c));
    })
    return credits;
  }
  mapCredit(creditDto: CreditDto) {
    let credit = new Credit();
    credit.id = creditDto?.id ?? 0;
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    credit.date = momentJalaali(creditDto.date).format("jD jMMMM jYYYY");
    credit.modifierUserName = creditDto.modifierUserName!;
    credit.orderToken = creditDto.orderToken ?? '';
    credit.value = creditDto.value!;
    return credit;
  }
  //#endregion
  
}

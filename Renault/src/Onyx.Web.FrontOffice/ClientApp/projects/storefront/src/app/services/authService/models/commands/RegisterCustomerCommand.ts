import { CustomerTypeEnum } from '../../../../web-api-client';
import { PersonType } from '../entities/PersonType';



export class RegisterCustomerCommand {
  firstName!: string;
  lastName!: string;
  userName!: string;
  nationalCode!: string;
  phoneNumber!: string;
  email!: string;
  password!: string;
  confirmPassword!: string;
  personType!: PersonType;
  customerType!: CustomerTypeEnum;
  captchaId!: string;
  captchaCode!: string;
}

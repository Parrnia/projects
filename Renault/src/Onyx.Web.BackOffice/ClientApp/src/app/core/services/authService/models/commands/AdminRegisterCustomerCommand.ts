import { CustomerTypeEnum } from "src/app/web-api-client";
import { PersonType } from "../entities/PersonType";



export class AdminRegisterCustomerCommand {
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
}

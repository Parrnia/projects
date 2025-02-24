import { CustomerTypeEnum } from "src/app/web-api-client";
import { PersonType } from "../entities/PersonType";


export class UpdateCustomerCommandAuth {
  id?: string;
  firstName!: string;
  lastName!: string;
  nationalCode!: string;
  email!: string;
  profileImageId!: string;
  personType!: PersonType;
  customerType!: CustomerTypeEnum;
}

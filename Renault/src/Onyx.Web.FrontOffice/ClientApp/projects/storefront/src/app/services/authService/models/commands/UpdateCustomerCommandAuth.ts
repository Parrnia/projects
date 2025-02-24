import { CustomerTypeEnum } from '../../../../web-api-client';
import { PersonType } from '../entities/PersonType';



export class UpdateCustomerCommandAuth {
  userId?: string;
  firstName!: string;
  lastName!: string;
  nationalCode!: string;
  email!: string;
  profileImageId!: string;
  personType!: PersonType;
  customerType!: CustomerTypeEnum;
}

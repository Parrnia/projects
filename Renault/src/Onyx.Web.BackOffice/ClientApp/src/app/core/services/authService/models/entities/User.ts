

export class User {
  id!: string;
  userName!: string;
  firstName!: string;
  lastName!: string;
  nationalCode!: string;
  phoneNumber!: string;
  email?: string;
  isActive!: boolean;
  roles!: string[];
  registerDateTime!: string;
  personType!: string;
  customerType!: string;
  credit!: number;
  maxCredit!: number;
  lockoutEnabled!: boolean;
}
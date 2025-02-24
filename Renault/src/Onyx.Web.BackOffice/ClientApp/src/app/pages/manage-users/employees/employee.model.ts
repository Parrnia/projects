export class EmployeeModel {
  id!: string;
  userName!: string;
  firstName!: string;
  lastName!: string;
  nationalCode!: string;
  phoneNumber!: string;
  email!: string;
  isActive!: boolean;
  roles!: string;
  registerDateTime!: string;
  personType!: string;
  lockoutEnabled!: boolean;
  company!: string | undefined;
  avatar!: string;
  avatarImage!: string;
  state!: string;
}

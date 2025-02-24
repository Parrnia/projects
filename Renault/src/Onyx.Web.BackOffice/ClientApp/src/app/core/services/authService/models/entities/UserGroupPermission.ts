import { Group } from "./Group";
import { Permission } from "./Permission";


export class UserGroupPermission {
  group!: Group;
  permissions!: Permission[];
}
import { Group } from "src/app/core/services/authService/models/entities/Group";
import { Permission } from "src/app/core/services/authService/models/entities/Permission";

export interface PermissionGroupsModel {
  id: string;
  slug: string;
  name: string;
  category: string;
  service: string;
  state: number;
  }
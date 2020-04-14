import { Role } from "../Roles/Role";

export interface IUser {

  userName: string;
  phoneNumber: string;
  email: string;
  role: Role;
}

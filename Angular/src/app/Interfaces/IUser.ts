import { Role } from "../Roles/Role";

export interface IUser {

  id: string;
  userName: string;
  phoneNumber: string;
  email: string;
  role: Role;
}

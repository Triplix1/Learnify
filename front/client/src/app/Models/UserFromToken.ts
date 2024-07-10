import { Role } from "./Role";

export interface UserFromToken {
    email: string;
    username: string;
    imageUrl: string;
    role: Role;
}
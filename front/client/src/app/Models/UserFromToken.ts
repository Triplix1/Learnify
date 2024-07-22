import { Role } from "./Role";

export interface UserFromToken {
    id: string;
    email: string;
    username: string;
    name: string;
    surname: string;
    imageUrl: string;
    role: Role;
}
import { Role } from "./enums/Role";

export interface UserFromToken {
    id: number;
    email: string;
    username: string;
    name: string;
    surname: string;
    imageUrl: string;
    role: Role;
}
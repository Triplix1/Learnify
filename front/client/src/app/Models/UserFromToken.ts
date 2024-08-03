import { Role } from "./Role";

export interface UserFromToken {
    id: number;
    email: string;
    username: string;
    name: string;
    surname: string;
    imageUrl: string;
    role: Role;
}
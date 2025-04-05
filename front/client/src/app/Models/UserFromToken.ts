import { UserType } from "./enums/UserType";

export interface UserFromToken {
    id: number;
    email: string;
    username: string;
    name: string;
    surname: string;
    imageUrl: string;
    role: UserType;
}
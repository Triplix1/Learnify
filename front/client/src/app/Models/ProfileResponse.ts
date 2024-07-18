import { UserType } from "./UserType";

export interface ProfileResponse {
    id: string;
    username: string;
    email: string;
    type: UserType;
    name: string;
    surname: string;
    company?: string;
    cardNumber?: string;
    photoUrl?: string;
    photoPublicId?: string;
}
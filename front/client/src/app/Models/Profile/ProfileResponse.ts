import { UserType } from "../enums/UserType";

export interface ProfileResponse {
    id: number;
    userName: string;
    email: string;
    type: UserType;
    name: string;
    surname: string;
    company?: string;
    cardNumber?: string;
    imageUrl?: string;
    imageBlobName?: string;
    imageContainerName?: string;
}
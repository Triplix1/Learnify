export interface ProfileUpdateRequest {
    id: number;
    name: string;
    surname: string;
    cardNumber: string;
    company: string;
    file: File;
}
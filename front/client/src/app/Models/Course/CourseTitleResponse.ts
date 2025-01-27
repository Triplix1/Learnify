import { PrivateFileDataResponse } from "../File/PrivateFileDataResponse";

export interface CourseTitleResponse {
    id: number;
    title: string;
    description: string;
    photo: PrivateFileDataResponse;
}
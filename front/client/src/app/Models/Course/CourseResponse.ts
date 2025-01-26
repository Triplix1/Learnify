import { Language } from "../enums/Language";
import { PrivateFileDataResponse } from "../File/PrivateFileDataResponse";
import { ParagraphResponse } from "./Paragraph/ParagraphResponse";

export interface CourseResponse {
    id: number;
    authorId: number;
    photo: PrivateFileDataResponse;
    video: PrivateFileDataResponse;
    name: string;
    price: number;
    isPublished: boolean;
    primaryLanguage: string;
    paragraphs: ParagraphResponse[];
}
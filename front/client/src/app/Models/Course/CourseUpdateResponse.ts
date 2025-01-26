import { PrivateFileDataResponse } from "../File/PrivateFileDataResponse";
import { ParagraphResponse } from "./Paragraph/ParagraphResponse";

export class CourseUpdateResponse {
    id: number;
    authorId: number;
    photo: PrivateFileDataResponse;
    video: PrivateFileDataResponse;
    name: string;
    description: string;
    price: number;
    isPublished: boolean;
    primaryLanguage: string;
    paragraphs: ParagraphResponse[];
}
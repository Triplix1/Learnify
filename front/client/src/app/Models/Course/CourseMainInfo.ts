import { PrivateFileDataResponse } from "../File/PrivateFileDataResponse";
import { ParagraphResponse } from "./Paragraph/ParagraphResponse";

export class CourseMainInfo {
    name: string;
    description: string;
    price: number
    primaryLanguage: string
    userHasBoughtThisCourse: boolean;
    video: PrivateFileDataResponse;
    paragraphs: ParagraphResponse[];
}
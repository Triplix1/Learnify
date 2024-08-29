import { Language } from "../enums/Language";
import { ParagraphResponse } from "./Paragraph/ParagraphResponse";

export interface CourseResponse {
    id: number;
    authorId: number;
    name: string;
    price: number;
    isPublished: boolean;
    language: Language;
    paragraphs: ParagraphResponse[];
}
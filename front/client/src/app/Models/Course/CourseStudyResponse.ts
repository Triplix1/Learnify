import { ParagraphResponse } from "./Paragraph/ParagraphResponse";

export interface CourseStudyResponse {
    id: number;
    authorId: number;
    name: string;
    paragraphs: ParagraphResponse[];
}
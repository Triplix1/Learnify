import { ParagraphPublishedResponse } from "../Paragraph/ParagraphPublishedResponse";

export interface LessonDeletedResponse {
    paragraphPublishedResponse: ParagraphPublishedResponse;
    unpublishedParagraph: boolean;
}
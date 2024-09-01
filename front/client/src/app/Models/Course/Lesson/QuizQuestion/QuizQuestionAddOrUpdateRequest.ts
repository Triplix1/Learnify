import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";

export interface QuizQuestionAddOrUpdateRequest {
    media: AttachmentResponse;
    question: string;
    options: string[];
    correctAnswer: number;
}
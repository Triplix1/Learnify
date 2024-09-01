import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";

export interface QuizQuestionUpdateResponse {
    media: AttachmentResponse;
    question: string;
    options: string[];
    correctAnswer: number;
}
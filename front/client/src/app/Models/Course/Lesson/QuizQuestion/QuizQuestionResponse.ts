import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";

export interface QuizQuestionResponse {
    media: AttachmentResponse;
    question: string;
    options: string[];
}
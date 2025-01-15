import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";
import { AnswerAddOrUpdateRequest } from "./Anwers/AnswerAddOrUpdateRequest";

export interface QuizQuestionAddOrUpdateRequest {
    lessonId: string;
    quizId: string;
    media: AttachmentResponse;
    question: string;
}
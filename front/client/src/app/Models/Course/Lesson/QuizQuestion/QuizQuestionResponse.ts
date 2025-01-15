import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";
import { AnswersResponse } from "./Anwers/AnswersResponse";

export interface QuizQuestionResponse {
    quizId: string;
    media: AttachmentResponse;
    question: string;
    answers: AnswersResponse;
}
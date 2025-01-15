import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";
import { AnswersUpdateResponse } from "./Anwers/AnswersUpdateResponse";

export interface QuizQuestionUpdateResponse {
    quizId: string;
    media: AttachmentResponse;
    question: string;
    answers: AnswersUpdateResponse;
}
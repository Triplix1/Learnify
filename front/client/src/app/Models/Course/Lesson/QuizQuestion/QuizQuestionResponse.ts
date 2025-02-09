import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";
import { AnswersResponse } from "./Anwers/AnswersResponse";
import { UserLessonQuizAnswerResponse } from "./Anwers/UserLessonQuizAnswerResponse";

export interface QuizQuestionResponse {
    id: string;
    media: AttachmentResponse;
    question: string;
    answers: AnswersResponse;
    userAnswer: UserLessonQuizAnswerResponse;
}
import { QuizValidateRequest } from "./QuizValidateRequest";

export interface AnswersValidateRequest {
    lessonId: string;
    quizValidateRequests: QuizValidateRequest[];
}
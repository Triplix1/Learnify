import { AnswersUpdateResponse } from "./Anwers/AnswersUpdateResponse";
import { CurrentLessonUpdatedResponse } from "../CurrentLessonUpdatedResponse";

export interface QuizQuestionUpdatedResponse {
    id: string;
    question: string;
    answers: AnswersUpdateResponse;
    currentLessonUpdated: CurrentLessonUpdatedResponse;
}
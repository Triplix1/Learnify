import { CurrentLessonUpdatedResponse } from "../../CurrentLessonUpdatedResponse";

export interface AnswersUpdatedResponse {
    options: string[];
    correctAnswer: number;
    currentLessonUpdated: CurrentLessonUpdatedResponse;
}
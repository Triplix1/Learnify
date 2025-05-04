import { CurrentLessonUpdatedResponse } from "./Course/Lesson/CurrentLessonUpdatedResponse";

export interface QuizDeleted {
    index: number;
    currentLessonUpdated: CurrentLessonUpdatedResponse;
}
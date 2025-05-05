import { LessonDeletedResponse } from "./LessonDeletedResponse";

export interface LessonDeletedEvent {
    lessonId: string;
    lessonDeletedResponse: LessonDeletedResponse;
}
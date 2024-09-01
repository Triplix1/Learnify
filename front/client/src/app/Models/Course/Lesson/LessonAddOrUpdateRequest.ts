import { QuizQuestionAddOrUpdateRequest } from "./QuizQuestion/QuizQuestionAddOrUpdateRequest";
import { VideoAddOrUpdateRequest } from "./Video/VideoAddOrUpdateRequest";

export interface LessonAddOrUpdateRequest {
    id: string | null;
    paragraphId: number;
    title: string;
    editedLessonId: string;
    video: VideoAddOrUpdateRequest;
    quizzes: QuizQuestionAddOrUpdateRequest[];
}
import { QuizQuestionResponse } from "./QuizQuestion/QuizQuestionResponse";
import { QuizQuestionUpdateResponse } from "./QuizQuestion/QuizQuestionUpdateResponse";
import { VideoResponse } from "./Video/VideoResponse";

export interface LessonUpdateResponse {
    id: string | null;
    paragraphId: number;
    title: string;
    editedLessonId: string;
    isDraft: boolean;
    video: VideoResponse;
    quizzes: QuizQuestionUpdateResponse[];
}
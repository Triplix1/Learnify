import { Language } from "../../enums/Language";
import { QuizQuestionResponse } from "./QuizQuestion/QuizQuestionResponse";
import { QuizQuestionUpdateResponse } from "./QuizQuestion/QuizQuestionUpdateResponse";
import { VideoResponse } from "./Video/VideoResponse";

export interface LessonUpdateResponse {
    id: string | null;
    paragraphId: number;
    title: string;
    editedLessonId: string;
    originalLessonId: string;
    isDraft: boolean;
    primaryLanguage: Language;
    video: VideoResponse;
    quizzes: QuizQuestionUpdateResponse[];
}
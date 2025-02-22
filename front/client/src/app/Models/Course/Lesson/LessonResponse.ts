import { Language } from "../../enums/Language";
import { QuizQuestionResponse } from "./QuizQuestion/QuizQuestionResponse";
import { VideoResponse } from "./Video/VideoResponse";

export interface LessonResponse {
    id: string | null;
    paragraphId: number;
    title: string;
    video: VideoResponse;
    primaryLanguage: Language;
    quizzes: QuizQuestionResponse[];
}
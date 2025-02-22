import { Language } from "../../enums/Language";
import { VideoAddOrUpdateRequest } from "./Video/VideoAddOrUpdateRequest";

export interface LessonAddOrUpdateRequest {
    id: string | null;
    paragraphId: number;
    title: string;
    editedLessonId: string;
    primaryLanguage: Language;
    video: VideoAddOrUpdateRequest;
}
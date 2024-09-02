import { Language } from "src/app/Models/enums/Language";

export interface SubtitlesUpdateRequest {
    id: number;
    fileId: number | null;
    language: Language;
}
import { Language } from "src/app/Models/enums/Language";

export interface SubtitlesResponse {
    id: number;
    subtitleFileId: number | null;
    transcriptionFileId: number | null;
    language: Language;
}
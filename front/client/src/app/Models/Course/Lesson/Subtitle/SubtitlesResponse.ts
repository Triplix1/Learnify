import { Language } from "src/app/Models/enums/Language";

export interface SubtitlesResponse {
    id: number;
    fileId: number | null;
    language: string;
}
import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";
import { Language } from "src/app/Models/enums/Language";
import { SubtitlesResponse } from "../Subtitle/SubtitlesResponse";

export interface VideoResponse {
    attachment: AttachmentResponse;
    primaryLanguage: string;
    subtitles: SubtitlesResponse[];
}
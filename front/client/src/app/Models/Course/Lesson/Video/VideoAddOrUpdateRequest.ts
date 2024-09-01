import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";
import { Language } from "src/app/Models/enums/Language";

export interface VideoAddOrUpdateRequest {
    attachment: AttachmentResponse;
    primaryLanguage: Language;
    subtitles: Language[];
}
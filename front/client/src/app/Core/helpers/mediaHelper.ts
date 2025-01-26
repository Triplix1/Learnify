import { MediaType } from "src/app/Models/enums/MediaType";

export function getMediaType(contentType: string): MediaType {
    if (contentType.startsWith('image/')) {
        return MediaType.Image;
    } else if (contentType.startsWith('video/')) {
        return MediaType.Video;
    }
    return MediaType.Unknown;
}
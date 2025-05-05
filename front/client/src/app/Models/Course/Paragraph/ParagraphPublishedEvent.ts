import { ParagraphPublishedResponse } from "./ParagraphPublishedResponse";

export interface ParagraphPublishedEvent {
    paragraphId: number;
    paragraphPublishedResponse: ParagraphPublishedResponse;
}
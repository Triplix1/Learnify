import { Language } from "../enums/Language";

export interface CourseUpdateRequest {
    id: number;
    name: string;
    price: number;
    primaryLanguage: Language;
}
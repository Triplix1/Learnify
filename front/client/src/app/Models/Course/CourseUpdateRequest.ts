import { Language } from "../enums/Language";

export interface CourseUpdateRequest {
    id: number;
    name: string;
    description: string;
    price: number;
    primaryLanguage: Language;
}
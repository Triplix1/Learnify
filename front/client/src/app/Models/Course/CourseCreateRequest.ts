import { Language } from "src/app/Models/enums/Language";

export interface CourseCreateRequest {
    name: string;
    price: number;
    primaryLanguage: Language;
}
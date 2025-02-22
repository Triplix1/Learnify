import { Language } from "src/app/Models/enums/Language";

export function convertStringToLanguage(language: string) {
    return Language[language as keyof typeof Language]
}
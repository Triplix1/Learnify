import { LessonTitleResponse } from "./Course/Lesson/LessonTitleResponse";

export interface SidebarItem {
    title: string;
    expanded: boolean;
    items: string[];
}
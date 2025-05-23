import { AttachmentResponse } from "src/app/Models/Attachment/AttachmentResponse";
import { AnswersUpdateResponse } from "./Anwers/AnswersUpdateResponse";
import { CurrentLessonUpdatedResponse } from "../CurrentLessonUpdatedResponse";

export interface QuizQuestionUpdateResponse {
    id: string;
    question: string;
    answers: AnswersUpdateResponse;
}
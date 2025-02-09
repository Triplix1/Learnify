import { QuizQuestionResponse } from "../QuizQuestionResponse";

export interface QuizItem {
    quizResponse: QuizQuestionResponse;
    selectedAnswer: number;
    isCorrect: boolean;
}
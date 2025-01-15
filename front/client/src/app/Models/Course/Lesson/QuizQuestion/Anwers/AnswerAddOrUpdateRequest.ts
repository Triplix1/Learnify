export interface AnswerAddOrUpdateRequest {
    lessonId: string;
    quizId: string;
    options: string[];
    correctAnswer: number;
}
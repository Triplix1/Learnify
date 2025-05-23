﻿using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;

namespace Learnify.Core.Dto.Course.QuizQuestion;

public class QuizQuestionResponse
{
    public string Id { get; set; }
    public string Question { get; set; }
    public AnswersResponse Answers { get; set; }
    public UserLessonQuizAnswerResponse UserAnswer { get; set; }
}
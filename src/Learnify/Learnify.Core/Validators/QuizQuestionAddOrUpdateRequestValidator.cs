using FluentValidation;
using Learnify.Core.Dto.Course.QuizQuestion;

namespace Learnify.Core.Validators;

public class QuizQuestionAddOrUpdateRequestValidator: AbstractValidator<QuizQuestionAddOrUpdateRequest>
{
    public QuizQuestionAddOrUpdateRequestValidator()
    {
        RuleFor(l => l.LessonId).NotNull();
    }
}
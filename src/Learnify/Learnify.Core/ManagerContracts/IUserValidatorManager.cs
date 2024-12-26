﻿namespace Learnify.Core.ManagerContracts;

public interface IUserValidatorManager
{
    Task ValidateAuthorOfCourseAsync(int courseId, int userId,
        CancellationToken cancellationToken = default);

    Task ValidateAuthorOfParagraphAsync(int paragraphId, int userId,
        CancellationToken cancellationToken = default);

    Task ValidateAuthorOfLessonAsync(string lessonId, int userId,
        CancellationToken cancellationToken = default);
}
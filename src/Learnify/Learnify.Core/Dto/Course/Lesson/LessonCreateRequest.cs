﻿namespace Learnify.Core.Dto.Course.Lesson;

public class LessonCreateRequest
{
    /// <summary>
    /// Id reference to content of lesson, which stores in NoSql
    /// </summary>
    public string ContentId { get; set; }
}
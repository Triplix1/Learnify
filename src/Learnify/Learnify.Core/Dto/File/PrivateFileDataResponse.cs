﻿namespace Learnify.Core.Dto.File;

public class PrivateFileDataResponse
{
    public int Id { get; set; }
    public string ContentType { get; set; }
    public int? CourseId { get; set; }
}
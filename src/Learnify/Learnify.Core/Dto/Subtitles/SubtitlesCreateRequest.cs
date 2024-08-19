﻿using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Subtitles;

public class SubtitlesCreateRequest
{
    public int? FileId { get; set; } 
    public Language Language { get; set; }
}
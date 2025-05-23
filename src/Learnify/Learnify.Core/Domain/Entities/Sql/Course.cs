﻿using System.ComponentModel.DataAnnotations.Schema;
using Learnify.Core.Dto.Course.Interfaces;
using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities.Sql;

public class Course: BaseEntity<int>, ICourseUpdatable
{
    public int AuthorId { get; set; }
    public int? PhotoId { get; set; }
    public int? VideoId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPublished { get; set; }
    public Language PrimaryLanguage { get; set; }

    [ForeignKey(nameof(PhotoId))]
    public PrivateFileData Photo { get; set; }
    [ForeignKey(nameof(VideoId))]
    public PrivateFileData Video { get; set; }
    public User Author { get; set; }

    public ICollection<Paragraph> Paragraphs { get; set; }
    public ICollection<UserBought> UserBoughts { get; set; }
}
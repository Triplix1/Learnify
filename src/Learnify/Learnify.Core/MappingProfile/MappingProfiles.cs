using AutoMapper;
using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.Subtitles;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;
using Learnify.Core.Dto.Profile;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.MappingProfile;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        // Role
        CreateMap<RoleRequest, Role>()
            .ConvertUsing(src => Enum.Parse<Role>(src.ToString()));

        // User
        CreateMap<User, ProfileResponse>();
        CreateMap<ProfileUpdateRequest, User>();

        // Course
        CreateMap<CourseCreateRequest, Course>();
        CreateMap<CourseUpdateRequest, Course>();
        CreateMap<Course, CourseResponse>();
        CreateMap<CourseParams, EfFilter<Course>>();

        // Paragraph
        CreateMap<ParagraphCreateRequest, Paragraph>();
        CreateMap<ParagraphUpdateRequest, Paragraph>();
        CreateMap<Paragraph, ParagraphResponse>();

        // Lesson
        CreateMap<LessonUpdateRequest, Lesson>();
        CreateMap<LessonCreateRequest, Lesson>();
        CreateMap<Lesson, LessonResponse>();
        CreateMap<Lesson, LessonTitleResponse>();

        // Attachment
        CreateMap<Attachment, AttachmentResponse>().ReverseMap();

        // QuizQuestion
        CreateMap<QuizQuestion, QuizQuestionResponse>();

        // Subtitles
        CreateMap<SubtitleReference, SubtitlesReferenceResponse>();
        CreateMap<SubtitlesCreateRequest, Subtitle>();
        CreateMap<Subtitle, SubtitlesResponse>();

        // FileData
        CreateMap<PrivateFileData, PrivateFileDataResponse>();
        CreateMap<PrivateFileDataCreateRequest, PrivateFileData>();
        CreateMap<PrivateFileBlobCreateRequest, PrivateFileDataCreateRequest>()
            .ForMember(pd => pd.BlobName, p => p.MapFrom(pr => pr.BlobDto.Name))
            .ForMember(pd => pd.ContainerName, p => p.MapFrom(pr => pr.BlobDto.ContainerName));
    }
}
using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;
using Learnify.Core.Dto.Course.Video;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Messages;
using Learnify.Core.Dto.Params;
using Learnify.Core.Dto.Profile;
using Learnify.Core.Dto.Subtitles;
using Learnify.Core.Enums;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.MappingProfile;

public class MappingProfiles : Profile
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
        CreateMap<Course, CourseTitleResponse>();
        CreateMap<Course, CourseResponse>()
            .ForMember(c => c.PrimaryLanguage, c => c.MapFrom(cr => cr.PrimaryLanguage.ToString()));
        CreateMap<CourseParams, EfFilter<Course>>();

        // Paragraph
        CreateMap<ParagraphCreateRequest, Paragraph>();
        CreateMap<ParagraphUpdateRequest, Paragraph>();
        CreateMap<Paragraph, ParagraphResponse>();

        // Lesson
        CreateMap<LessonAddOrUpdateRequest, Lesson>();
        CreateMap<Lesson, LessonResponse>();
        CreateMap<Lesson, LessonUpdateResponse>();
        CreateMap<Lesson, LessonTitleResponse>();

        // Attachment
        CreateMap<Attachment, AttachmentResponse>().ReverseMap();

        // QuizQuestion
        CreateMap<QuizQuestion, QuizQuestionResponse>();
        CreateMap<QuizQuestionAddOrUpdateRequest, QuizQuestion>()
            .ForMember(q => q.Id, q => q.MapFrom(s => s.QuizId));
        CreateMap<QuizQuestion, QuizQuestionUpdateResponse>();

        // Answers
        CreateMap<Answers, AnswersResponse>();
        CreateMap<Answers, AnswersUpdateResponse>();
        CreateMap<AnswerAddOrUpdateRequest, Answers>();

        // Subtitles
        CreateMap<SubtitlesCreateRequest, Subtitle>();
        CreateMap<Subtitle, SubtitlesResponse>();
        CreateMap<SubtitlesResponse, SubtitleInfo>();
        CreateMap<SubtitlesResponse, SubtitleReference>()
            .ForMember(s => s.SubtitleId, s => s.MapFrom(sr => sr.Id));

        // FileData
        CreateMap<PrivateFileData, PrivateFileDataResponse>();

        //Video
        CreateMap<VideoAddOrUpdateRequest, Video>();
        CreateMap<Video, VideoResponse>();

        //Message
        CreateMap<Message, MessageResponse>()
            .ForMember(m => m.SenderName, m => m.MapFrom(message => message.Sender.Name));
    }
}
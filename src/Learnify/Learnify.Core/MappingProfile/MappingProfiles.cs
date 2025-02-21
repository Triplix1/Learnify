using AutoMapper;
using Learnify.Contracts;
using Learnify.Core.Domain.Entities.NoSql;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto.Attachment;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Dto.Course.QuizQuestion;
using Learnify.Core.Dto.Course.QuizQuestion.Answers;
using Learnify.Core.Dto.Course.QuizQuestion.QuizAnswer;
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
        CreateMap<Course, CourseStudyResponse>();
        CreateMap<Course, CourseMainInfoResponse>()
            .ForMember(c => c.PrimaryLanguage, c => c.MapFrom(cr => cr.PrimaryLanguage.ToString()));;
        CreateMap<Course, CourseResponse>()
            .ForMember(c => c.PrimaryLanguage, c => c.MapFrom(cr => cr.PrimaryLanguage.ToString()));
        CreateMap<Course, CourseUpdateResponse>()
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
        CreateMap<Lesson, Lesson>();

        // Attachment
        CreateMap<Attachment, AttachmentResponse>().ReverseMap();

        // QuizQuestion
        CreateMap<QuizQuestion, QuizQuestionResponse>();
        CreateMap<QuizQuestionAddOrUpdateRequest, QuizQuestion>()
            .ForMember(q => q.Id, q => q.MapFrom(s => s.QuizId));
        CreateMap<QuizQuestion, QuizQuestionUpdateResponse>();
        
        //UserQuizAnswer
        CreateMap<UserQuizAnswer, UserQuizAnswerResponse>();
        CreateMap<UserQuizAnswerCreateRequest, UserQuizAnswer>();

        // Answers
        CreateMap<Answers, AnswersResponse>();
        CreateMap<Answers, AnswersUpdateResponse>();
        CreateMap<AnswerAddOrUpdateRequest, Answers>();

        // Subtitles
        CreateMap<SubtitlesCreateRequest, Subtitle>();
        CreateMap<Subtitle, SubtitleInfo>();
        CreateMap<Subtitle, SubtitlesResponse>();
        CreateMap<Subtitle, SubtitleReference>()
            .ForMember(s => s.SubtitleId, s => s.MapFrom(sb => sb.Id));
        CreateMap<SubtitlesResponse, SubtitleReference>()
            .ForMember(s => s.SubtitleId, s => s.MapFrom(sr => sr.Id));

        // FileData
        CreateMap<PrivateFileData, PrivateFileDataResponse>();
        CreateMap<GeneratedResponseUpdateRequest, PrivateFileCreateRequest>();
        CreateMap<GeneratedResponseUpdateRequest, PrivateFileData>();
        CreateMap<PrivateFileCreateRequest, PrivateFileData>();

        //Video
        CreateMap<VideoAddOrUpdateRequest, Video>()
            .ForMember(v => v.Subtitles, s => s.Ignore());
        CreateMap<Video, VideoResponse>()
            .ForMember(v => v.Subtitles, s => s.Ignore());

        //Message
        CreateMap<Message, MessageResponse>()
            .ForMember(m => m.SenderName, m => m.MapFrom(message => message.Sender.Name));
    }
}
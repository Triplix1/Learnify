﻿using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;

namespace Learnify.Infrastructure.Repositories.UnitsOfWork;


public class PsqUnitOfWork: IPsqUnitOfWork
{
    public PsqUnitOfWork(IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ICourseRepository courseRepository,
        IParagraphRepository paragraphRepository,
        IPrivateFileRepository privateFileRepository,
        IUserBoughtRepository userBoughtRepository,
        ISubtitlesRepository subtitlesRepository,
        IQuizRepository quizRepository,
        IUserQuizAnswerRepository userQuizAnswerRepository,
        IMeetingSessionRepository meetingSessionRepository,
        IMeetingConnectionRepository meetingConnectionRepository)
    {
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
        CourseRepository = courseRepository;
        ParagraphRepository = paragraphRepository;
        PrivateFileRepository = privateFileRepository;
        UserBoughtRepository = userBoughtRepository;
        SubtitlesRepository = subtitlesRepository;
        QuizRepository = quizRepository;
        UserQuizAnswerRepository = userQuizAnswerRepository;
        MeetingSessionRepository = meetingSessionRepository;
        MeetingConnectionRepository = meetingConnectionRepository;
    }

    public IParagraphRepository ParagraphRepository { get; }

    public IPrivateFileRepository PrivateFileRepository { get; }
    
    public IUserBoughtRepository UserBoughtRepository { get; }

    public ISubtitlesRepository SubtitlesRepository { get; }

    public IQuizRepository QuizRepository { get; }

    public IUserQuizAnswerRepository UserQuizAnswerRepository { get; }

    public IUserRepository UserRepository { get; }

    public IRefreshTokenRepository RefreshTokenRepository { get; }

    public ICourseRepository CourseRepository { get; }
    
    public IMeetingSessionRepository MeetingSessionRepository { get; }

    public IMeetingConnectionRepository MeetingConnectionRepository { get; }
}
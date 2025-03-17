using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.MeetingSession;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

public class MeetingSessionRepository : IMeetingSessionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MeetingSessionRepository(ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MeetingSessionResponse> GetMeetingSessionForCourseAsync(int courseId,
        CancellationToken cancellationToken = default)
    {
        var meetingSession = await _context.MeetingSessions.FirstOrDefaultAsync(ms => ms.CourseId == courseId,
            cancellationToken: cancellationToken);

        return _mapper.Map<MeetingSessionResponse>(meetingSession);
    }

    public async Task<MeetingSessionResponse> GetMeetingSessionAsync(string sessionId,
        CancellationToken cancellationToken = default)
    {
        var meetingSession = await _context.MeetingSessions.FirstOrDefaultAsync(ms => ms.SessionId == sessionId,
            cancellationToken: cancellationToken);

        return _mapper.Map<MeetingSessionResponse>(meetingSession);
    }

    public async Task<MeetingSessionResponse> CreateAsync(MeetingSessionCreateRequest meetingSessionCreateRequest,
        CancellationToken cancellationToken = default)
    {
        var meetingSession = _mapper.Map<MeetingSession>(meetingSessionCreateRequest);

        _context.MeetingSessions.Add(meetingSession);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MeetingSessionResponse>(meetingSession);
    }

    public async Task<bool> DeleteAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var meetingSession = await _context.MeetingSessions.FindAsync([sessionId], cancellationToken);

        if (meetingSession == null)
            return false;

        _context.MeetingSessions.Remove(meetingSession);
        var changes = await _context.SaveChangesAsync(cancellationToken);

        return changes > 0;
    }
}
using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.MeetingConnection;
using Learnify.Infrastructure.Data;

namespace Learnify.Infrastructure.Repositories;

public class MeetingConnectionRepository : IMeetingConnectionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MeetingConnectionRepository(ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MeetingConnectionResponse> CreateMeetingConnectionAsync(
        MeetingConnectionCreateRequest meetingConnectionCreateRequest)
    {
        var meetingConnection = _mapper.Map<MeetingConnection>(meetingConnectionCreateRequest);

        _context.MeetingConnections.Add(meetingConnection);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<MeetingConnectionResponse>(meetingConnection);
        return response;
    }

    public async Task<bool> DeleteMeetingConnectionAsync(string connectionId)
    {
        var connection = await _context.MeetingConnections.FindAsync(connectionId);

        if (connection is null)
            return false;

        _context.Remove(connection);
        return await _context.SaveChangesAsync() > 0;
    }
}
using MassTransit;

namespace Profile.Core.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedConsumer>
{
    
    public async Task Consume(ConsumeContext<UserCreatedConsumer> context)
    {
        throw new NotImplementedException();
    }
}
using Messages.GRPC.Models;
using Grpc.Core;
namespace Messages.GRPC.Services
{
    public interface IRoomService
    {
        Task BroadcastMessageAsync(ChatMessage message);
        Task<int> AddUserToRoomAsync(User user);
        void ConnectUserStream(Guid userId, IAsyncStreamWriter<ChatMessage> stream);
    }
}
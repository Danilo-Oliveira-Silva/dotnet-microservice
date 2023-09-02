using Messages.GRPC.Models;
using Grpc.Core;
namespace Messages.GRPC.Services
{
    public interface IRoomService
    {
        Task BroadcastMessageAsync(string roomId, ChatMessage message);
        Task<string> AddUserToRoomAsync(User user, string roomId);
        void ConnectUserStream(string userId, string roomId, IAsyncStreamWriter<ChatMessage> stream);
        void CreateRoom(string roomId);
        
    }
}
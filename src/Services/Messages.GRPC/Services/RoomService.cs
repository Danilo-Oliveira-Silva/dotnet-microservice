
using Messages.GRPC.Models;
using Grpc.Core;

namespace Messages.GRPC.Services;

public class RoomService : IRoomService
{
    private Room _room;
    public RoomService()
    {
        _room = new Room(1);
    }

    public async Task BroadcastMessageAsync(ChatMessage message)
	{
        foreach(var user in _room.Users)
        {
            await user.Stream.WriteAsync(message);
        }
    }

    public Task<int> AddUserToRoomAsync(User user)
    {
        _room.AddUsers(user);
        return Task.FromResult(_room.Id);
    }

    public void ConnectUserStream(Guid userId, IAsyncStreamWriter<ChatMessage> stream)
    {
        foreach( var user in _room.Users)
        {
            if (user.Id == userId) {
                user.Stream = stream;
            }
        }
    }



}
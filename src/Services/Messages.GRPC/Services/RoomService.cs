
using Messages.GRPC.Models;
using Grpc.Core;

namespace Messages.GRPC.Services;

public class RoomService : IRoomService
{
    private List<Room> _rooms;
    public RoomService()
    {
        _rooms = new List<Room>();
    }

    public async Task BroadcastMessageAsync(string roomId, ChatMessage message)
	{
        Console.WriteLine("1: " + roomId);
        foreach (var room in _rooms)
        {
            Console.WriteLine("3 - " + room.RoomId);
            if (room.RoomId == roomId)
            {
                Console.WriteLine("2: " + roomId);
                foreach(var user in room.Users)
                {
                    Console.WriteLine("send message " + message + " to user " +user.Name);
                    await user.Stream.WriteAsync(message);
                }
            }
        }
    }

    public Task<string> AddUserToRoomAsync(User user, string roomId)
    {
        string roomFounded = "";
        IEnumerable<Room> filteredRooms = _rooms.Where(r => r.RoomId == roomId);
        if (filteredRooms.Count() > 0)
        {
            filteredRooms.First().AddUsers(user);
            roomFounded = filteredRooms.First().RoomId;
        }
        
        return Task.FromResult(roomFounded);
    }

    public void ConnectUserStream(string userId, string roomId, IAsyncStreamWriter<ChatMessage> stream)
    {
        foreach (var room in _rooms)
        {
            if (room.RoomId == roomId)
            {
                foreach( var user in room.Users)
                {
                    if (user.Id == userId) {
                        user.Stream = stream;
                    }
                }
            }
        
        }
    }

    public void CreateRoom(string roomId)
    {
        List<Room> filteredRooms = _rooms.Where(r => r.RoomId == roomId).ToList();
        if (filteredRooms.Count == 0)
        {
            Room newRoom = new Room(roomId);
            _rooms.Add(newRoom);
        }

    }



}
using System.Collections.Generic;

namespace Messages.GRPC.Models
{
    public class Room
    {
        public string RoomId { get; set; }   
        public List<User> Users { get; set; }

        public Room(string RoomIdp)
        {
            RoomId  = RoomIdp;
            Users = new List<User>();
        }

        public void AddUsers(User user)
        {
            Users.Add(user);
        }
        
    }
}
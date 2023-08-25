using System.Collections.Generic;

namespace Messages.GRPC.Models
{
    public class Room
    {
        public int Id { get; set; }   
        public List<User> Users { get; set; }

        public Room(int id)
        {
            Id = id;
            Users = new List<User>();
        }

        public void AddUsers(User user)
        {
            Users.Add(user);
        }
        
    }
}
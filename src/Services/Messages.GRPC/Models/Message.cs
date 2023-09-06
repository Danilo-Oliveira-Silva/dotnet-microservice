using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages.GRPC.Models
{
    public class Message
    {
        public string? RoomId { get; set; }
        public string? UserId { get; set; }
        public string? Text { get; set; }
    }
}
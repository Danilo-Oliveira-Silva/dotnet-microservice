using System;
using Grpc.Core;

namespace Messages.GRPC.Models
{
    public class User
    {
        public string? Id { get; set; }   
        public string? Name { get; set; }
        public IAsyncStreamWriter<ChatMessage>? Stream { get; set; }
    }
}
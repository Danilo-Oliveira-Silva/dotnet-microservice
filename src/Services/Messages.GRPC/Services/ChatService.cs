using Grpc.Core;
using Messages.GRPC;
using Messages.GRPC.Models;

namespace Messages.GRPC.Services
{
    public class ChatService : Chat.ChatBase
    {
        private readonly ILogger<ChatService> _logger;
        private readonly IProducer _producer;
        private IRoomService _roomService;
        public ChatService(ILogger<ChatService> logger, IRoomService roomService, IProducer producer)
        {
            _logger = logger;
            _roomService = roomService;
            _producer = producer;
        }

        public override async Task<JoinUserResponse> JoinUserChat(JoinUserRequest request, ServerCallContext context)
        {
            
            var newUser = new User {
                Id = request.UserId,
                Name = request.Name
            };
            _roomService.CreateRoom(request.RoomId);

            await _roomService.AddUserToRoomAsync(newUser, request.RoomId);
            return new JoinUserResponse { RoomId = request.RoomId };
        }

        public override async Task SendMessage(
            IAsyncStreamReader<ChatMessage> requestStream, 
            IServerStreamWriter<ChatMessage> responseStream,
            ServerCallContext context)
        {

            if (requestStream != null)
            {
                if (!await requestStream.MoveNext()) return;
            }

            _roomService.ConnectUserStream(requestStream.Current.UserId, requestStream.Current.RoomId, responseStream);
            //_logger.LogInformation("User Stream: "+requestStream.Current.UserId+"");
            

        
            try {
                while (await requestStream.MoveNext())
                {
                    
                    if (!string.IsNullOrEmpty(requestStream.Current.Text))
					{
                        Console.WriteLine("texto: " + requestStream.Current.Text);  
                        _producer.SendMessage(new Message{ RoomId = requestStream.Current.RoomId, UserId = requestStream.Current.UserId, Text = requestStream.Current.Text});
                        await _roomService.BroadcastMessageAsync(requestStream.Current.RoomId, requestStream.Current);
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("error " + ex.Message);
            }
            
        }
    }
}
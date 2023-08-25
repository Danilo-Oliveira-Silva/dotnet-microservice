using Grpc.Core;
using Messages.GRPC;
using Messages.GRPC.Models;

namespace Messages.GRPC.Services
{
    public class ChatService : Chat.ChatBase
    {
        private readonly ILogger<ChatService> _logger;
        private IRoomService _roomService;
        public ChatService(ILogger<ChatService> logger, IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        public override async Task<JoinUserResponse> JoinUserChat(JoinUserRequest request, ServerCallContext context)
        {
            var newUser = new User {
                Id = new Guid(request.Id),
                Name = request.Name
            };
            _logger.LogInformation("user: " + newUser.Name + " connected");
            _roomService.AddUserToRoomAsync(newUser);
            return new JoinUserResponse { RoomId = 1};
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

            _roomService.ConnectUserStream(new Guid(requestStream.Current.UserId), responseStream);
            _logger.LogInformation("User Stream: "+requestStream.Current.UserId+"");
            

        
            try {
                while (await requestStream.MoveNext())
                {
                    
                    if (!string.IsNullOrEmpty(requestStream.Current.Text))
					{
                        Console.WriteLine("texto: " + requestStream.Current.Text);  
                        await _roomService.BroadcastMessageAsync(requestStream.Current);
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("error " + ex.Message);
            }
        }
    }
}
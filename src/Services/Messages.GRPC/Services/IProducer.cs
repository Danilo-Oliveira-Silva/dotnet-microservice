namespace Messages.GRPC.Services
{
    public interface IProducer
    {
        void SendMessage<T> (T message);
    }
}
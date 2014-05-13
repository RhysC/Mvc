namespace CodeConverters.PersistentQueues
{
    public interface IMessageHandler<in T>
    {
        void Handle(T message);
    }
}

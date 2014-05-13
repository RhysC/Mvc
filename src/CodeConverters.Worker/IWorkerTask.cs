namespace CodeConverters.Worker
{
    public interface IWorkerTask<T>
    {
        WorkResult DoWork();
    }
}
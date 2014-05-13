namespace CodeConverters.Worker
{
    /// <summary>
    /// This mirrors the autofac IStartable, however that interface has special implication in that it auto starts, we want more control
    /// </summary>
    public interface IStartable
    {
        void Start();
    }
}
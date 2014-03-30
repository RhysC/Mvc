namespace CodeConverters.Mvc.Diagnostics
{
    public interface IHeartBeater
    {
        void LogHeartbeat(string clientName);
    }
}
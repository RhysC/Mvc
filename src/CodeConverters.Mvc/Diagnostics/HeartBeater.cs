using System;
using log4net;

namespace CodeConverters.Mvc.Diagnostics
{
    public class HeartBeater : IHeartBeater
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HeartBeater));
        private static DateTime _lastLogHeartbeat = DateTime.UtcNow.AddMinutes(-1);//Assumes only one consumer per process

        public void LogHeartbeat(string clientName)
        {
            NewRelic.Api.Agent.NewRelic.IncrementCounter(string.Format(NewRelicCounterFormats.WebHeartbeatEvent, clientName));
            if ((DateTime.UtcNow - _lastLogHeartbeat).TotalSeconds < 29)
                return;
            Logger.Info("Heartbeat");
            _lastLogHeartbeat = DateTime.UtcNow;
        }
    }
}
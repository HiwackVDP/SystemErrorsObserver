using SystemErrorsObserver.EventWatcher;

namespace SystemErrorsObserver
{
    public static class Program
    {
        private static readonly string[] eventLogs = new[]
        {
            "Application",
            "System"
        };

        public static void Main(string[] args)
        {
            // Test commandline : 
            // eventcreate /ID 1 /L APPLICATION /T ERROR  /SO MYEVENTSOURCE /D "My first log"

            foreach (string eventLog in eventLogs)
            {
                new EventLogWatcher(eventLog);
            }

            while (true) { }
        }
    }
}

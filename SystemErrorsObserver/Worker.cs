using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace SystemErrorsObserver
{
    public class Worker : BackgroundService
    {
        private readonly string[] eventLogs = new[]
        {
            "Application",
            "System"
        };

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (string eventLog in eventLogs)
                StartEventLogWatcher(eventLog);

            while (!stoppingToken.IsCancellationRequested)
            {}
        }

        private void StartEventLogWatcher(string eventLog)
        {
            var query = new EventLogQuery(eventLog, PathType.LogName);
            var watcher = new EventLogWatcher(query);

            watcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(SystemErrorHandler);
            watcher.Enabled = true;
        }

        private void SystemErrorHandler(object sender, EventRecordWrittenEventArgs e)
        {
            if(!e.EventRecord.Properties.Any())
            {
                throw new ArgumentException(nameof(e.EventRecord.Properties));
            }

            if(e.EventRecord.Level == 2)
            {
                Notifier.ShowNotification(e.EventRecord.ProviderName, (string)e.EventRecord.Properties[0].Value, e.EventRecord.LogName);
            }
        }
    }
}

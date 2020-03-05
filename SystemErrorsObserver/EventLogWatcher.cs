using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace SystemErrorsObserver
{
    public class EventLogWatcher
    {

        public EventLogWatcher(string eventLog)
        {
            Start(eventLog);
        }

        private void Start(string eventLog)
        {
            var query = new EventLogQuery(eventLog, PathType.LogName);
            var watcher = new System.Diagnostics.Eventing.Reader.EventLogWatcher(query);

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
                Notifier.ShowNotification(e.EventRecord.ProviderName, e.EventRecord.FormatDescription(), e.EventRecord.LogName);
            }
        }
    }
}

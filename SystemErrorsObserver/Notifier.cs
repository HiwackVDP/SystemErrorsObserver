using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace SystemErrorsObserver
{
    public static class Notifier
    {
        public static void ShowNotification(string cardTitle, string cardMessage, string eventLog)
        {
            //https://docs.microsoft.com/en-us/previous-versions/windows/apps/hh761494(v=win.10)
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var titleNodes = template.GetElementsByTagName("text");
            titleNodes.Item(0).InnerText = cardTitle;
            titleNodes.Item(1).InnerText = cardMessage;

            var notifier = ToastNotificationManager.CreateToastNotifier("System error notifier");
            var notification = new ToastNotification(template)
            {
                Priority = ToastNotificationPriority.High,
                Tag = eventLog,
                ExpiresOnReboot = false
            };
            notification.Activated += new TypedEventHandler<ToastNotification, object>(OpenEventLogs);
            notifier.Show(notification);
        }

        private static void OpenEventLogs(ToastNotification sender, object args)
        {
            Process.Start($"%SystemRoot%\\System32\\Winevt\\Logs\\{sender.Tag}.evtx");
        }
    }
}

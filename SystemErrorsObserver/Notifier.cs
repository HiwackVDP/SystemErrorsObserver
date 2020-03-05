using System.Diagnostics;
using System.IO;
using System.Reflection;
using SystemErrorsObserver.Helpers;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace SystemErrorsObserver
{
    public static class Notifier
    {
        public static void ShowNotification(string cardTitle, string cardMessage, string eventLog)
        {
            var notificationTemplate = BuildNotificationTemplate(cardTitle, cardMessage);

            var notifier = ToastNotificationManager.CreateToastNotifier("System error notifier");
            var notification = new ToastNotification(notificationTemplate)
            {
                Priority = ToastNotificationPriority.High,
                Tag = eventLog,
                ExpiresOnReboot = false
            };
            notification.Activated += new TypedEventHandler<ToastNotification, object>(OpenEventLogs);
            notifier.Show(notification);
        }

        private static Windows.Data.Xml.Dom.XmlDocument BuildNotificationTemplate(string cardTitle, string cardMessage)
        {
            //https://docs.microsoft.com/en-us/previous-versions/windows/apps/hh761494(v=win.10)
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

            var titleNodes = template.GetElementsByTagName("text");
            titleNodes.Item(0).InnerText = cardTitle;
            titleNodes.Item(1).InnerText = cardMessage;

            var imageNodes = template.GetElementsByTagName("image");
            var attr = template.CreateAttribute("src");

            attr.Value = Path.Combine(PathHelper.AssemblyDirectory, "error.png");

            var imageNode = imageNodes.Item(0);
            imageNode.Attributes.SetNamedItem(attr);
            return template;
        }

        private static void OpenEventLogs(ToastNotification sender, object args)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/C eventvwr /c:{sender.Tag}"
            };
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}

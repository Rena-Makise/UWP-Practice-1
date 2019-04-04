using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Set1_32_Agent.Background
{
    public sealed class Trigger : Windows.ApplicationModel.Background.IBackgroundTask
    {
        // IBackgroundTask를 위한 Run인터페이스 멤버를 구현한다
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                string value =
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("value") ?
                    (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["value"] :
                    string.Empty;
                Windows.Data.Xml.Dom.XmlDocument xml =
                    Windows.UI.Notifications.ToastNotificationManager
                    .GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
                xml.GetElementsByTagName("text")[0].InnerText = "Time Zone Changed";
                xml.GetElementsByTagName("text")[1].InnerText = value;
                // ToastNotificationManager는 백그라운드 Agent가 트리거될 때 ToastNotification을 생성한다.
                Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(
                    new Windows.UI.Notifications.ToastNotification(xml));
            }
            catch
            {

            }
        }
    }
}

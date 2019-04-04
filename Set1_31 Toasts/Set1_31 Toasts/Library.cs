using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

public class Item
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string Time { get; set; }
}

public class Library
{
    // 토스트 노티피케이션의 작동을 위한 ToastNotifier의 멤버들이다.
    private ToastNotifier _notifier = ToastNotificationManager.CreateToastNotifier();
    // identifier을 생성하기 위한 Random변수이다.
    private Random _random = new Random((int)DateTime.Now.Ticks);

    // GetScheduledToastNotifications을 이용하여 스케쥴된 토스트 알림을 생성하는 메소드이다.
    public void Init(ListBox display)
    {
        display.Items.Clear();
        IReadOnlyList<ScheduledToastNotification> list = _notifier.GetScheduledToastNotifications();
        foreach (ScheduledToastNotification item in list)
        {
            display.Items.Add(new Item
            {
                Id = item.Id,
                Time = item.Content.GetElementsByTagName("text")[0].InnerText,
                Content = item.Content.GetElementsByTagName("text")[1].InnerText,
            });
        }
    }

    // ListBox에 항목을 추가할 뿐만 아니라, 
    // ScheduledToastNotifications을 통해 TImeSpan으로 전달된 구체적인 DateTime에 토스트 알림을 스케쥴해준다.
    public void Add(ref ListBox display, string value, TimeSpan occurs)
    {
        DateTime when = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, occurs.Hours, occurs.Minutes, occurs.Seconds);
        if (when > DateTime.Now)
        {
            XmlDocument xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            xml.GetElementsByTagName("text")[0].InnerText = when.ToLocalTime().ToString();
            xml.GetElementsByTagName("text")[1].InnerText = value;
            ScheduledToastNotification toast = new ScheduledToastNotification(xml, when)
            {
                Id= _random.Next(1, 100000000).ToString()
            };
            _notifier.AddToSchedule(toast);
            display.Items.Add(new Item { Id = toast.Id, Content = value, Time = when.ToString() });
        }
    }

    // RemoveFromSchedule을 호출하여 de-schedule해주고, 해당 토스트 알림을 ListBox에서 제거해준다.
    public void Remove(ListBox display)
    {
        if (display.SelectedIndex > -1)
        {
            _notifier = ToastNotificationManager.CreateToastNotifier();
            try
            {
                _notifier.RemoveFromSchedule(_notifier.GetScheduledToastNotifications().Where(
                    p => p.Id.Equals(((Item)display.SelectedItem).Id)).SingleOrDefault());
            }
            catch (Exception)
            {
            }
            display.Items.RemoveAt(display.SelectedIndex);
        }
    }
}
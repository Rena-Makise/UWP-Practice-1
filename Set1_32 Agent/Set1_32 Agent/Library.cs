using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

public class Library
{
    private IBackgroundTaskRegistration _registration;

    public void Save(string value)
    {
        ApplicationData.Current.LocalSettings.Values["value"] = value;
    }

    // 이미 실행중인 경우 Background Agent를 가져오는 메소드이다.
    public bool Init()
    {
        if (BackgroundTaskRegistration.AllTasks.Count > 0)
        {
            _registration = BackgroundTaskRegistration.AllTasks.Values.First();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> Toggle()
    {
        // 이미 실행중이 경우 등록을 취소한다.
        if (BackgroundTaskRegistration.AllTasks.Count > 0)
        {
            _registration = BackgroundTaskRegistration.AllTasks.Values.First();
            _registration.Unregister(true);
            _registration = null;
            return false;
        }
        else
        {
            try
            {
                // SystemTriggerType.TimeZoneChange의 SystemTrigger를 사용하여
                // BackgroundTaskBuilder로 백그라운드 작업을 만들고 등록
                await BackgroundExecutionManager.RequestAccessAsync();
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder
                {
                    Name = typeof(Set1_32_Agent.Background.Trigger).FullName
                };
                builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
                builder.TaskEntryPoint = builder.Name;
                builder.Register();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
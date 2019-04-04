// Windows.Stoarge의 필수 기능을 사용하겠다.
using Windows.Storage;

public class Libray
{
    // key 매개변수를 취한다.
    // 이 key는 ApplicationData.Current.LocalSettings.Values에 항목이 있는지 여부를 확인하기 위해 먼저 사용
    // 이 값이 null이 아니면 값이 리턴, null이면 empty string이 리턴될것.
    public string LoadSetting(string key)
    {
        if (ApplicationData.Current.LocalSettings.Values[key] != null)
        {
            return ApplicationData.Current.LocalSettings.Values[key].ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    // key와 value를 파라미터로 받아 ApplicationData.Current.LocalSettings에 저장
    public void SaveSetting(string key, string value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = value;
    }
}
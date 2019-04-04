using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

public class Library
{
    // 미디어가 재생되는동안 사용되는 이벤트와 대리자이다.
    public delegate void PlayingEvent();
    public event PlayingEvent Playing;

    private DispatcherTimer _timer = new DispatcherTimer();

    // 미디어가 재생되는 때 사용되는 DispatcherTimer를 설정해주는 메소드이다.
    public void Init()
    {
        _timer.Tick += (object sender, object e) =>
        {
            // 널 조건부 연산자(?.)
            // Playing이 null인지 검사하여 그 결과가 참이면 null을, 아닐경우 Invoke()를 실행.
            Playing?.Invoke();
        };
    }

    public void Timer(bool enabled)
    {
        if (enabled) _timer.Start(); else _timer.Stop();
    }

    // URI로부터 비디오 소스를 가져와서 MediaElement에 담는 메소드이다.
    public void Go(ref MediaElement display, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                display.Source = new Uri(value, UriKind.Absolute);
                display.Play();
            }
            catch
            {
            }
        }
    }
}
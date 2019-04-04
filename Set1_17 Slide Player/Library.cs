using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

public class Library
{
    // 애플리케이션에서 해당 상태가 일어났을때 알려주는 이벤트와 이벤트의 대리자 선언
    public delegate void PlayingEvent(BitmapImage image, int index);
    public event PlayingEvent Playing;
    public delegate void StoppedEvent();
    public event StoppedEvent Stopped;

    // 슬라이드는 비트맵이미지의 리스트로 표현된다.
    private List<BitmapImage> _list = new List<BitmapImage>();
    private int _index = 0;
    private bool _paused = false;

    // 몇몇 속성은 아래 세가지로 표현된다.
    public bool Isplaying { get; set; }
    public int Speed { get; set; }
    public int Position { get; set; }

    // 이미지가 담긴 URI를 typed in 했을 때 핸들링
    public void Go(ref Image display, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                display.Source = new BitmapImage(new Uri(value));
            }
            catch
            {
            }
        }
    }

    // BitmapImage로 구성된 슬라이드에서 더하거나 뺼때 사용된다
    public double Add(string value)
    {
        _list.Add(new BitmapImage(new Uri(value)));
        return _list.Count - 1;
    }
    public double Remove(int index)
    {
        if (index >= 0 && index < _list.Count)
        {
            _list.RemoveAt(index);
        }
        return _list.Count - 1;
    }

    // BitmapImage의 리스트를 슬라이드쇼로 구성해서 플레이한다.
    // 뭔가 딜레이 방식이 문제가 있는것 같다. 잘 작동하지 않는다.
    // Delay방식에서 Timespan.FromSeconds를 통해 값을 전달해야 제대로 동작한다!!
    public async void Play()
    {
        if (_list.Any() && (!_paused || !Isplaying))
        {
            Isplaying = true;
            _paused = false;
            while (Isplaying)
            {
                if (_list.Count > 0)
                {
                    if (_index < _list.Count)
                    {
                        Playing(_list[_index], _index);
                        _index += 1;
                    }
                    else
                    {
                        this.Stop();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(Speed));
            }
        }
    }

    // 슬라이드쇼를 잠시 멈춘다.
    public void Pause()
    {
        if (_list.Any() && Isplaying)
        {
            _paused = true;
            Isplaying = false;
        }
    }

    // 슬라이드쇼를 멈추고 초기화
    public void Stop()
    {
        if (_list.Any())
        {
            _index = 0;
            _paused = false;
            Isplaying = false;
            Stopped();
        }
    }
}
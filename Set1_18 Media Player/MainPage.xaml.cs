using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace Set1_18_Media_Player
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Library library = new Library();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            library.Init();
            library.Playing += () =>
             {
                 Position.Value = (int)Display.Position.TotalMilliseconds;
             };
        }

        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            library.Go(ref Display, Value.Text, e);
        }

        private void Position_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Display != null && Position != null)
            {
                Display.Position = TimeSpan.FromMilliseconds(Position.Value);
            }
        }

        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Display != null && Volume != null)
            {
                Display.Volume = Volume.Value;
            }
        }

        private void Display_MediaOpened(object sender, RoutedEventArgs e)
        {
            Position.Maximum = (int)Display.NaturalDuration.TimeSpan.TotalMilliseconds;
            Display.Play();
            Play.Icon = new SymbolIcon(Symbol.Pause);
            Play.Label = "Pause";
        }

        private void Display_MediaEnded(object sender, RoutedEventArgs e)
        {
            Play.Icon = new SymbolIcon(Symbol.Play);
            Play.Label = "Play";
            Display.Stop();
            Position.Value = 0;
        }

        private void Display_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            library.Timer(Display.CurrentState == MediaElementState.Playing);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Display.CurrentState == MediaElementState.Playing)
            {
                Display.Pause();
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
            }
            else
            {
                Display.Play();
                Play.Icon = new SymbolIcon(Symbol.Pause);
                Play.Label = "Pause";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Display.Stop();
        }
    }
}

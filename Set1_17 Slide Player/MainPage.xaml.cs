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

namespace Set1_17_Slide_Player
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
            library.Speed = (int)Speed.Value;
            library.Playing += (Windows.UI.Xaml.Media.Imaging.BitmapImage image, int index) =>
              {
                  Display.Source = image;
                  Position.Value = index;
              };
            library.Stopped += () =>
              {
                  Play.Icon = new SymbolIcon(Symbol.Play);
                  Play.Label = "Play";
                  Display.Source = null;
                  Position.Value = 0;
              };
        }

        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            library.Go(ref Display, Value.Text, e);
        }

        private void Position_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            library.Speed = (int)Position.Value;
        }

        private void Speed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Speed != null)
            {
                library.Speed = (int)Speed.Value;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Position.Maximum = library.Add(Value.Text);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Position.Maximum = library.Remove((int)Position.Value);
        }

        // CommandBar의 버튼 아이콘과 라벨을 바꾸는 과정.
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (library.Isplaying)
            {
                library.Pause();
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
            }
            else
            {
                library.Play();
                Play.Icon = new SymbolIcon(Symbol.Pause);
                Play.Label = "Pause";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            library.Stop();
        }
    }
}

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

namespace Set1_11_Web_Browser
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

        // 키스트로크에 대한 추가 이벤트 정보와 함께 TextBox의 텍스트를 전달한다.
        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            library.Go(ref Display, Value.Text, e);
        }

        // 텍스트박스를 현재 웹페이지 주소로 채운다
        private void Display_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                Value.Text = args.Uri.ToString();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            library.Back(ref Display);
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            library.Forward(ref Display);
        }

        // Refresh와 Stop은 WebView 함수의 메소드들이다.
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Display.Refresh();
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Display.Stop();
        }
    }
}

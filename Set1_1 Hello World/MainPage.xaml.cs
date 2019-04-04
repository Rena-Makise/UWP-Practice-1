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

namespace Set1_1_Hello_World
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // Page2에 뒤로가기 버튼을 통해 다시 되돌아왔을 때
            // 우리가 입력했던 정보를 페이지 캐시에 추가해서 정보를 백업할 수 있도록 설정
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        // 비동기방식이므로 async와 await이 필요!
        // UI의 응답성을 확보하기 위해 UWP앱용 .NET 라이브러리에 있는 많은 기능들이 비동기 메서드 방식으로 제공된다.
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await new Windows.UI.Popups.MessageDialog("Hello World").ShowAsync();
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {
            // Page클래스의 Frame속성을 이용하여 Page2.xaml의 콘텐츠를 표시
            // 그리고 동시에 name TextBox의 데이터를 전달한다.
            this.Frame.Navigate(typeof(Page2), name.Text);
        }
    }
}

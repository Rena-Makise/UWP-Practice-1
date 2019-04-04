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

namespace Set1_4_Image_Rotate
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        Library library = new Library();

        // 텍스트박스에 입력한 URL의 컨텐츠를 이미지의 소스로 설정하는 역할
        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Display.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(Value.Text));
            }
        }

        // 아래 셋은 Rotate 메소드에 어느축을 기준으로 회전할 것인지, 그리고 이미지를 전달하기 위한 역할
        private void Pitch_Click(object sender, RoutedEventArgs e)
        {
            library.Rotate("X", ref Display);
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            library.Rotate("Y", ref Display);
        }

        private void Yaw_Click(object sender, RoutedEventArgs e)
        {
            library.Rotate("Z", ref Display);
        }
    }
}

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

namespace Set1_3_Data_Input
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Libray library = new Libray();

        public MainPage()
        {
            this.InitializeComponent();
        }

        // 텍스트박스를 비워주는 역할
        private void New_Click(object sender, RoutedEventArgs e)
        {
            Email.Text = string.Empty;
            Website.Text = string.Empty;
            Telephone.Text = string.Empty;
        }

        // key를 이용해서 이전에 저장해 둔 값을 로드
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Email.Text = library.LoadSetting("Email");
            Website.Text = library.LoadSetting("Website");
            Telephone.Text = library.LoadSetting("Telephone");
        }

        // key에 값을 저장.
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            library.SaveSetting("Email", Email.Text);
            library.SaveSetting("Website", Website.Text);
            library.SaveSetting("Telephone", Telephone.Text);
        }
    }
}

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

namespace Set1_2_Command_Bar
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

        // Hide Other이나 Show Other을 클릭했을시에 트리거되는 이벤트이다.
        private void Show_Click(object sender, RoutedEventArgs e)
        {
            // Hide.Visibility가 Collapsed로 설정되어있으면 Visible로, 그렇지 않으면 Collapse로 설정.
            if (Hide.Visibility.Equals(Visibility.Collapsed))
            {
                Hide.Visibility = Visibility.Visible;
            }
            else
            {
                Hide.Visibility = Visibility.Collapsed;
            }
        }
    }
}

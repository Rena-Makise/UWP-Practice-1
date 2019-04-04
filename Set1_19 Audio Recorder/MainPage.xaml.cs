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

namespace Set1_19_Audio_Recorder
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

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            if (library.Recording)
            {
                library.Stop();
                Record.Icon = new SymbolIcon(Symbol.Memo);
                Record.Label = "Record";
            }
            else
            {
                library.Record();
                Record.Icon = new SymbolIcon(Symbol.Microphone);
                Record.Label = "Stop";
            }
        }

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            await library.Play(Dispatcher);
        }
    }
}

﻿using System;
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

namespace Set1_14_Rich_Editor
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

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            Bold.IsChecked = library.Bold(ref Display);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            Italic.IsChecked = library.Italic(ref Display);
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            Underline.IsChecked = library.Underline(ref Display);
        }

        private void Size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            library.Size(ref Display, ref Size);
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = library.Align(ref Display, Windows.UI.Text.ParagraphAlignment.Left);
            Centre.IsChecked = false;
            Right.IsChecked = false;
        }

        private void Centre_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = false;
            Centre.IsChecked = library.Align(ref Display, Windows.UI.Text.ParagraphAlignment.Center);
            Right.IsChecked = false;
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            Left.IsChecked = false;
            Centre.IsChecked = false;
            Right.IsChecked = library.Align(ref Display, Windows.UI.Text.ParagraphAlignment.Right);
        }

        private void Colour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            library.Colour(ref Display, ref Colour);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            library.NewAsync(Display);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            library.OpenAsync(Display);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            library.SaveAsync(Display);
        }
    }
}

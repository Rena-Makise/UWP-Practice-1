using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

public class Library
{
    // 방문한 웹 사이트로 되돌아 갈 수 있는지를 판단
    public void Back(ref WebView web)
    {
        if (web.CanGoBack)
        {
            web.GoBack();
        }
    }

    // 마찬가지.
    public void Forward(ref WebView web)
    {
        if (web.CanGoForward)
        {
            web.GoForward();
        }
    }

    // 키스트로크가 발생될때마다 트리거된다.
    // Enter 키스트로크가 발생하면 전달된 string값에 의해 결정된 Uri로 WebView를 Navigate한다.
    public void Go(ref WebView web, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                web.Navigate(new Uri(value));
            }
            catch
            {

            }
            web.Focus(FocusState.Keyboard);
        }
    }
}

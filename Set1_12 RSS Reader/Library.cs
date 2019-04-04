using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Web.Syndication;

public class Library
{
    // SyndicationClient는 SyndicationFeed를 이용하여 RSS feed를 수신한다.
    SyndicationClient _client = new SyndicationClient();
    SyndicationFeed _feed = new SyndicationFeed();

    // RetrieveFeedAsync를 이용해 RSS feed를 가져와서 ItemsControl의 ItemsSource에 피드 컨텐츠를 설정한다.
    private async void Load(ItemsControl list, Uri uri)
    {
        _client = new SyndicationClient();
        _feed = await _client.RetrieveFeedAsync(uri);
        list.ItemsSource = _feed.Items;
    }

    // 키스트로크가 발생할때마다 트리거되며, 엔터 키스트로크가 발생하면 TextBox의 텍스트를 사용하여 Load메서드를 호출한다.
    public void Go(ref ItemsControl list, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                Load(list, new Uri(value));
                list.Focus(FocusState.Keyboard);
            }
            catch
            {
            }
        }
    }
}
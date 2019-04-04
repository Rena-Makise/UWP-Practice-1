using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// pin된 Tile을 나타내는 클래스이다.
public class Item
{
    public string Id { get; set; }
    public string Content { get; set; }
    public Brush Colour { get; set; }
}

public class Library
{
    private Random _random = new Random((int)DateTime.Now.Ticks);

    // 문자열 색상을 Color로 변환한다.
    private Color FromString(string value)
    {
        return Color.FromArgb(
        Byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber));
    }

    // SecondaryTile의 아이템 리스트로 구성된 ListBox의 컨텐츠를 세팅하는 메소드이다
    public async void Init(ListBox display)
    {
        display.Items.Clear();
        IReadOnlyList<SecondaryTile> list = await SecondaryTile.FindAllAsync();
        foreach (SecondaryTile item in list)
        {
            display.Items.Add(new Item
            {
                Id = item.TileId,
                Content = item.DisplayName,
                Colour = new SolidColorBrush(item.VisualElements.BackgroundColor)
            });
        }
    }

    // ListBox에 아이템을 추가할 뿐만 아니라 윈도우 Start Menu에 SecondaryTile을 Pin한다.
    public async void Add(ListBox display, string value, ComboBox colour, object selection)
    {
        string id = _random.Next(1, 100000000).ToString();
        SecondaryTile tile = new SecondaryTile(id, value, id, new Uri("ms-appx:///"), TileSize.Default);
        Color background = FromString(((ComboBoxItem)colour.SelectedItem).Tag.ToString());
        tile.VisualElements.BackgroundColor = background;
        tile.VisualElements.ForegroundText = ForegroundText.Light;
        tile.VisualElements.ShowNameOnSquare150x150Logo = true;
        tile.VisualElements.ShowNameOnSquare310x310Logo = true;
        tile.VisualElements.ShowNameOnWide310x150Logo = true;
        await tile.RequestCreateAsync();
        display.Items.Add(new Item { Id = tile.TileId, Content = value, Colour = new SolidColorBrush(background) });
    }

    // 타일을 unpin하고 ListBox에서 해당 항목을 제거한다
    public async void Remove(ListBox display)
    {
        if (display.SelectedIndex > -1)
        {
            string id = ((Item)display.SelectedItem).Id;
            if (SecondaryTile.Exists(id))
            {
                SecondaryTile tile = new SecondaryTile(id);
                await tile.RequestDeleteAsync();
            }
            display.Items.RemoveAt(display.SelectedIndex);
        }
    }
}
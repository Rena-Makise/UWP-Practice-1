using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    private const int size = 3;
    // 가변배열 (Jagged Array). 2차원 배열과는 다르다. 가변 배열의 요소는 "배열"
    // 주사위에 새겨진 점을 표시
    private readonly byte[][] table =
    {               //  a, b, c, d, e, f,  g, h, i
        new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },   //0
        new byte[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 },   //1
        new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 1 },   //2
        new byte[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 },   //3
        new byte[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 },   //4
        new byte[] { 1, 0, 1, 0, 1, 0, 1, 0, 1 },   //5
        new byte[] { 1, 0, 1, 1, 0, 1, 1, 0, 1 },   //6
    };

    // 주사위에 필요한 숫자들을 만드는데 사용
    private Random _random = new Random((int)DateTime.Now.Ticks);

    // 각 주사위의 그리드에 점을 배치하는데 사용
    private void Add(ref Grid grid, int row, int column, byte opaity)
    {
        Ellipse dot = new Ellipse()
        {
            Fill = new SolidColorBrush(Colors.Black),
            Margin = new Thickness(5),
            Opacity = opaity,
        };
        dot.SetValue(Grid.ColumnProperty, column);
        dot.SetValue(Grid.RowProperty, row);
        grid.Children.Add(dot);
    }

    // 주사위 자신의 레이아웃을 생성하는데 사용
    private Grid Dice(int value)
    {
        Grid grid = new Grid()
        {
            Width = 100,
            Height = 100,
            Background = new SolidColorBrush(Colors.WhiteSmoke),
            Padding = new Thickness(5),
        };

        // Setup Grid
        for (int index = 0; (index < size); index++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        int count = 0;
        for (int row = 0; (row < size); row++)
        {
            for (int column = 0; (column < size); column++)
            {
                Add(ref grid, row, column, table[value][count]);
                count++;
            }
        }
        return grid;
    }

    // 주사위의 숫자를 선택하는데 사용
    private int Roll()
    {
        return _random.Next(1, 7);
    }

    // 전달된 그리드를 설정하여 주사위 단일 면을 지정
    public void New(ref Grid grid)
    {
        grid.Children.Clear();
        grid.Children.Add(Dice(Roll()));
    }
}
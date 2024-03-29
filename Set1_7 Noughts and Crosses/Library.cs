﻿using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    // Noughts and Crosses : 3목두기.....
    // Nought : 0, nothing
    private const string app_title = "Noughts and Crosses";
    private const char blank = ' ';
    private const char nought = 'O';
    private const char cross = 'X';
    private const int size = 3;

    // 필드는 보통 언더바로 시작되게 한다.
    // 그렇게 해야 지역 변수와 명확하게 구별되기 때문
    private bool _won = false;
    private char _piece = blank;
    // 게임을 표현할 것을 나타내는 2차원 배열
    private char[,] _board = new char[size, size];

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();

    }

    public async Task<bool> ConfirmAsync(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    // 게임에서 이기는 룰을 표현
    private bool Winner()
    {
        return
        (_board[0, 0] == _piece && _board[0, 1] == _piece && _board[0, 2] == _piece) ||
        (_board[1, 0] == _piece && _board[1, 1] == _piece && _board[1, 2] == _piece) ||
        (_board[2, 0] == _piece && _board[2, 1] == _piece && _board[2, 2] == _piece) ||
        (_board[0, 0] == _piece && _board[1, 0] == _piece && _board[2, 0] == _piece) ||
        (_board[0, 1] == _piece && _board[1, 1] == _piece && _board[2, 1] == _piece) ||
        (_board[0, 2] == _piece && _board[1, 2] == _piece && _board[2, 2] == _piece) ||
        (_board[0, 0] == _piece && _board[1, 1] == _piece && _board[2, 2] == _piece) ||
        (_board[0, 2] == _piece && _board[1, 1] == _piece && _board[2, 0] == _piece);
    }

    // 게임에서 무승부가 될 룰을 표현
    private bool Drawn()
    {
        return
        _board[0, 0] != blank && _board[0, 1] != blank && _board[0, 2] != blank &&
        _board[1, 0] != blank && _board[1, 1] != blank && _board[1, 2] != blank &&
        _board[2, 0] != blank && _board[2, 1] != blank && _board[2, 2] != blank;
    }

    // X 또는 O
    private Path Piece()
    {
        Path path = new Path
        {
            StrokeThickness = 5,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        if (_piece == cross)
        {
            LineGeometry line1 = new LineGeometry
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(60, 60),
            };
            LineGeometry line2 = new LineGeometry
            {
                StartPoint = new Point(60, 0),
                EndPoint = new Point(0, 60),
            };
            GeometryGroup linegroup = new GeometryGroup();
            linegroup.Children.Add(line1);
            linegroup.Children.Add(line2);
            path.Data = linegroup;
            path.Stroke = new SolidColorBrush(Colors.Red);
        }
        else if (_piece == nought)
        {
            EllipseGeometry ellipse = new EllipseGeometry
            {
                Center = new Point(30, 30),
                RadiusX = 30,
                RadiusY = 30,
            };
            path.Data = ellipse;
            path.Stroke = new SolidColorBrush(Colors.Blue);
        }
        return path;
    }

    private void Add(ref Grid grid, int row, int column)
    {
        Grid element = new Grid()
        {
            Height = 80,
            Width = 80,
            Margin = new Thickness(10),
            Background = new SolidColorBrush(Colors.WhiteSmoke),
        };
        element.Tapped += (object sender, TappedRoutedEventArgs e) =>
        {
            if (!_won)
            {
                element = (Grid)sender;
                if ((element.Children.Count < 1))
                {
                    element.Children.Add(Piece());
                    _board[(int)element.GetValue(Grid.RowProperty), (int)element.GetValue(Grid.ColumnProperty)] = _piece;
                }
                if (Winner())
                {
                    _won = true;
                    Show($"{_piece} wins!", app_title);
                }
                else if (Drawn())
                {
                    Show("Draw!", app_title);
                }
                else
                {
                    // Swap Players
                    _piece = (_piece == cross ? nought : cross);
                }
            }
            else
            {
                Show("Game Over!", app_title);
            }
        };
        element.SetValue(Grid.ColumnProperty, column);
        element.SetValue(Grid.RowProperty, row);
        grid.Children.Add(element);
    }

    // 게임의 시각적 레이아웃을 만드는데 사용
    // 어떻게 게임을 진행할지를 컨트롤하고, New에서 게임을 시작하는데 사용
    private void Layout(ref Grid grid)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();
        // Setup Grid
        for (int index = 0; (index < size); index++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        // Setup Board
        for (int row = 0; (row < size); row++)
        {
            for (int column = 0; (column < size); column++)
            {
                Add(ref grid, row, column);
                _board[row, column] = blank;
            }
        }
    }
    public async void New(Grid grid)
    {
        Layout(ref grid);
        _won = false;
        _piece = await ConfirmAsync("Who goes First?", app_title, nought.ToString(), cross.ToString()) ? nought : cross;
    }
}
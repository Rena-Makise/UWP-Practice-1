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

// 사용자 정의 컨트롤 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234236에 나와 있습니다.

namespace Set1_24_Carousel_Control
{
    public sealed partial class Carousel : UserControl
    {
        public Carousel()
        {
            this.InitializeComponent();
        }

        // Storyboard는 Carousel을 통해 회전시키는것을 나타낸다.
        private Windows.UI.Xaml.Media.Animation.Storyboard _animation = new Windows.UI.Xaml.Media.Animation.Storyboard();
        // BitmapImage의 List는 Carousel의 아이템들을 나타낸다.
        private List<Windows.UI.Xaml.Media.Imaging.BitmapImage> _list = new List<Windows.UI.Xaml.Media.Imaging.BitmapImage>();

        // Carousel을 세팅하기 위한 다양한 요소들이다.
        private Point _point;
        private Point _radius = new Point { X = -20, Y = 200 };
        // 스피드
        private double _speed = 0.01225;
        // 원근감
        private double _perspective = 55;
        // Carousel 중심의 위치
        private double _distance;

        // Carousel의 look과 feel을 생성하기 위한 메소드이다.
        private void Layout(ref Canvas display)
        {
            display.Children.Clear();
            for (int index = 0; index < _list.Count(); index++)
            {
                _distance = 1 / (1 - (_point.X / _perspective));
                Image item = new Image
                {
                    Width = 150,
                    Source = _list[index],
                    Tag = index * ((Math.PI * 2) / _list.Count),
                    RenderTransform = new ScaleTransform()
                };
                _point.X = Math.Cos((double)item.Tag) * _radius.X;
                _point.Y = Math.Cos((double)item.Tag) * _radius.Y;
                Canvas.SetLeft(item, _point.X - (item.Width - _perspective));
                Canvas.SetTop(item, _point.Y);
                item.Opacity = ((ScaleTransform)item.RenderTransform).ScaleX = ((ScaleTransform)item.RenderTransform).ScaleY = _distance;
                display.Children.Add(item);
            }
        }

        // 아이템들이 Carousel 주변을 움직이게 하기 위해
        // 즉 그냥 이미지를 회전시키기 위한 메소드이다.
        private void Rotate()
        {
            foreach (Image item in Display.Children)
            {
                double angle = (double)item.Tag;
                angle -= _speed;
                item.Tag = angle;
                _point.X = Math.Cos(angle) * _radius.X;
                _point.Y = Math.Sin(angle) * _radius.Y;
                Canvas.SetLeft(item, _point.X - (item.Width - _perspective));
                Canvas.SetTop(item, _point.Y);
                if (_radius.X >= 0)
                {
                    _distance = 1 * (1 - (_point.X / _perspective));
                    Canvas.SetZIndex(item, (int)(_point.X));
                }
                else
                {
                    _distance = 1 / (1 - (_point.X / _perspective));
                    Canvas.SetZIndex(item, (int)(_point.X));
                }
                item.Opacity = ((ScaleTransform)item.RenderTransform).ScaleX = ((ScaleTransform)item.RenderTransform).ScaleY = _distance;
            }
            _animation.Begin();
        }

        // BitmapImage를 Carousel의 아이템을 추가히기 위한 메소드이다.
        public void Add(Windows.UI.Xaml.Media.Imaging.BitmapImage image)
        {
            _list.Add(image);
            Layout(ref Display);
        }

        // Carousel에 최근 추가된 BitmapImage를 제거한다.
        public void RemoveLast()
        {
            if (_list.Any())
            {
                _list.Remove(_list.Last());
                Layout(ref Display);
            }
        }

        // BitmapImage의 List에 있는 아이템들을 초기화한다.
        public void New()
        {
            _list.Clear();
            Layout(ref Display);
        }

        // Carousel을 구성하고 생성하는 메소드이다.
        private void Display_Loaded(object sender, RoutedEventArgs e)
        {
            _animation.Completed += (object s, object obj) =>
             {
                 Rotate();
             };
            _animation.Begin();
        }
    }
}

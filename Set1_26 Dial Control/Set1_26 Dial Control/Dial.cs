using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Set1_26_Dial_Control
{
    public sealed class Dial : Control
    {
        public Dial()
        {
            this.DefaultStyleKey = typeof(Dial);
        }

        //  Grid는 다이얼의 손잡이(knob)을 나타낸다.
        private Grid _knob;
        // RotateTransform은 knob이 다이얼 주위를 회전하면서 나타내는 위치를 표현한다.
        private RotateTransform _value;
        private bool _hasCapture = false;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Dial), null);
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimu", typeof(double), typeof(Dial), null);
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(Dial), null);
        public static readonly DependencyProperty KnobProperty = DependencyProperty.Register("Knob", typeof(UIElement), typeof(Dial), null);
        public static readonly DependencyProperty FaceProperty = DependencyProperty.Register("Face", typeof(UIElement), typeof(Dial), null);

        // 다이얼을 나타내는 각각의 프로퍼티들이다.
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public UIElement Knob
        {
            get { return (UIElement)GetValue(KnobProperty); }
            set { SetValue(KnobProperty, value); }
        }
        public UIElement Face
        {
            get { return (UIElement)GetValue(FaceProperty); }
            set { SetValue(FaceProperty, value); }
        }

        // Knob의 UIElement가 어떻게 배치될 수 있는지를 계산하는 메소드이다.
        private double AngleQuadrant(double width, double height, Windows.Foundation.Point point)
        {
            double radius = width / 2;
            Windows.Foundation.Point centre = new Windows.Foundation.Point(radius, height / 2);
            Windows.Foundation.Point start = new Windows.Foundation.Point(0, height / 2);
            double triangleTop = Math.Sqrt(Math.Pow((point.X - centre.X), 2) + Math.Pow((centre.Y - point.Y), 2));
            double triangleHeight = (point.Y > centre.Y) ? point.Y - centre.Y : centre.Y - point.Y;
            return ((triangleHeight * Math.Sin(90)) / triangleTop) * 100;
        }

        // 다이얼 손잡이의 각도를 구하는데 사용된다.
        private double GetAngle(Windows.Foundation.Point point)
        {
            double diameter = _knob.ActualWidth;
            double height = _knob.ActualHeight;
            double radius = diameter / 2;
            double rotation = AngleQuadrant(diameter, height, point);
            if ((point.X > radius) && (point.Y <= radius))
            {
                rotation = 90.0 + (90.0 - rotation);
            }
            else if ((point.X > radius) && (point.Y > radius))
            {
                rotation = 180.0 + rotation;
            }
            else if ((point.X < radius) && (point.Y > radius))
            {
                rotation = 270.0 + (90.0 - rotation);
            }
            return rotation;
        }
        private void SetPosition(double rotation)
        {
            if (Minimum > 0 && Maximum > 0 && Minimum < 360 && Maximum <= 360)
            {
                if (rotation < Minimum) { rotation = Minimum; }
                if (rotation > Maximum) { rotation = Maximum; }
            }
            _value.Angle = rotation;
            Value = rotation;
        }

        // 컨트롤 자체의 모양이나 느낌을 초기화한다.
        // 필요한 모든 이벤트를 설정하는 데 사용된다.
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _knob = ((Grid)GetTemplateChild("Knob"));
            _value = ((RotateTransform)GetTemplateChild("DialValue"));
            if (Minimum > 0 && Minimum < 360) { SetPosition(Minimum); }
            _knob.PointerReleased += (object sender, PointerRoutedEventArgs e) =>
            {
                _hasCapture = false;
            };
            _knob.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
            {
                _hasCapture = true;
                SetPosition(GetAngle(e.GetCurrentPoint(_knob).Position));
            };
            _knob.PointerMoved += (object sender, PointerRoutedEventArgs e) =>
            {
                if (_hasCapture)
                {
                    SetPosition(GetAngle(e.GetCurrentPoint(_knob).Position));
                }
            };
            _knob.PointerExited += (object sender, PointerRoutedEventArgs e) =>
            {
                _hasCapture = false;
            };
        }
    }
}

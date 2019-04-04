using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WrapControl
{
    // 사이즈에 대한 너비와 높이, 그리고 Orientation이 포함된 OrientedSize라는 구조체를 정의한다.
    public partial class WrapPanel : Panel
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct OrientedSize
        {
            private Orientation orientation;
            private double direct;
            private double indirect;

            public Orientation Orientation
            {
                get { return orientation; }
            }

            public double Direct
            {
                get { return direct; }
                set { direct = value; }
            }

            public double Indirect
            {
                get { return indirect; }
                set { indirect = value; }
            }

            public double Width
            {
                get
                {
                    return (Orientation == Orientation.Horizontal) ? Direct : Indirect;
                }
                set
                {
                    if (Orientation == Orientation.Horizontal)
                    {
                        Direct = value;
                    }
                    else
                    {
                        Indirect = value;
                    }
                }
            }

            public double Height
            {
                get
                {
                    return (Orientation != Orientation.Horizontal) ? Direct : Indirect;
                }
                set
                {
                    if (Orientation != Orientation.Horizontal)
                    {
                        Direct = value;
                    }
                    else
                    {
                        Indirect = value;
                    }
                }
            }

            public OrientedSize(Orientation orientation)
                : this(orientation, 0.0, 0.0)
            {

            }

            public OrientedSize(Orientation orientation, double width, double height)
            {
                this.orientation = orientation;
                direct = 0.0;
                indirect = 0.0;
                this.Width = width;
                this.Height = height;
            }
        }

        private bool ignorePropertyChange;

        // 요소들이 서로 가까이 있는지를 확인하는 메소드이다.
        public static bool AreClose(double left, double right)
        {
            if (left == right)
            {
                return true;
            }
            double a = (Math.Abs(left) + Math.Abs(right) + 10.0) * 2.2204460492503131E-16;
            double b = left - right;
            return (-a < b) && (a > b);
        }

        // 요소들이 서로 멀리 떨어져있는지를 확인하는 메소드이다.
        public static bool IsGreaterThan(double left, double right)
        {
            return (left > right) && !AreClose(left, right);
        }

        // 패널에서 각 항목의 높이를 정의하는 프로퍼티이다.
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(WrapPanel),
                new PropertyMetadata(double.NaN, OnItemHeightOrWidthPropertyChanged));

        // 패널에서 각 항목의 너비를 정의하는 프로퍼티이다.
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(WrapPanel),
                new PropertyMetadata(double.NaN, OnItemHeightOrWidthPropertyChanged));

        // 패널에서 각 항목의 Orientation을 정의하는 프로퍼티이다.
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapPanel),
                new PropertyMetadata(Orientation.Horizontal, OnOrientationPropertyChanged));

        // 해당 프로퍼티들이 변경되었을때 조정하는 메소드이다.
        private static void OnOrientationPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            WrapPanel source = (WrapPanel)d;
            Orientation value = (Orientation)e.NewValue;
            if (source.ignorePropertyChange)
            {
                source.ignorePropertyChange = false;
                return;
            }
            if ((value != Orientation.Horizontal) && (value != Orientation.Vertical))
            {
                source.ignorePropertyChange = true;
                source.SetValue(OrientationProperty, (Orientation)e.OldValue);
                throw new ArgumentException("OnOrientationPropertyChanged InvalidValue", "value");
            }
            source.InvalidateMeasure();
        }

        // 해당 프로퍼티들이 변경되었을때 조정하는 메소드이다.
        private static void OnItemHeightOrWidthPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            WrapPanel source = (WrapPanel)d;
            double value = (double)e.NewValue;
            if (source.ignorePropertyChange)
            {
                source.ignorePropertyChange = false;
                return;
            }
            if (!double.IsNaN(value) && ((value <= 0.0) || double.IsPositiveInfinity(value)))
            {
                source.ignorePropertyChange = true;
                source.SetValue(e.Property, (double)e.OldValue);
                throw new ArgumentException("OnItemHeightOrWidthPropertyChanged InvalidValue", "value");
            }
            source.InvalidateMeasure();
        }

        // Panel의 레이아웃을 결정할 떄 사용되며 Panel내에서 항목을 적절히 배치해준다.
        // ArrangeLine을 이용하여 줄 바꿈이 필요할 때까지 요소를 배치한다.
        protected override Size MeasureOverride(Size constraint)
        {
            Orientation o = Orientation;
            OrientedSize lineSize = new OrientedSize(o);
            OrientedSize totalSize = new OrientedSize(o);
            OrientedSize maximumSize = new OrientedSize(o, constraint.Width, constraint.Height);
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            bool hasFixedWidth = !double.IsNaN(itemWidth);
            bool hasFixedHeight = !double.IsNaN(itemHeight);
            Size itemSize = new Size(hasFixedWidth ? itemWidth : constraint.Width,
                hasFixedHeight ? itemHeight : constraint.Height);
            foreach (UIElement element in Children)
            {
                element.Measure(itemSize);
                OrientedSize elementSize = new OrientedSize(o,
                    hasFixedWidth ? itemWidth : element.DesiredSize.Width,
                    hasFixedHeight ? itemHeight : element.DesiredSize.Height);
                if (IsGreaterThan(lineSize.Direct + elementSize.Direct, maximumSize.Direct))
                {
                    totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct);
                    totalSize.Indirect += lineSize.Indirect;
                    lineSize = elementSize;
                    if (IsGreaterThan(elementSize.Direct, maximumSize.Direct))
                    {
                        totalSize.Direct = Math.Max(elementSize.Direct, totalSize.Direct);
                        totalSize.Indirect += elementSize.Indirect;
                        lineSize = new OrientedSize(o);
                    }
                }
                else
                {
                    lineSize.Direct += elementSize.Direct;
                    lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect);
                }
            }
            totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct);
            totalSize.Indirect += lineSize.Indirect;
            return new Size(totalSize.Width, totalSize.Height);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            Orientation o = Orientation;
            OrientedSize lineSize = new OrientedSize(o);
            OrientedSize maximumSize = new OrientedSize(o, finalSize.Width, finalSize.Height);
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            bool hasFixedWidth = !double.IsNaN(itemWidth);
            bool hasFixedHeight = !double.IsNaN(itemHeight);
            double indirectOffset = 0;
            double? directDelta = (o == Orientation.Horizontal) ?
                (hasFixedWidth ? (double?)itemWidth : null) :
                (hasFixedHeight ? (double?)itemHeight : null);
            UIElementCollection children = Children;
            int count = children.Count;
            int lineStart = 0;
            for (int lineEnd = 0; lineEnd < count; lineEnd++)
            {
                UIElement element = children[lineEnd];
                OrientedSize elementSize = new OrientedSize(o,
                    hasFixedWidth ? itemWidth : element.DesiredSize.Width,
                    hasFixedHeight ? itemHeight : element.DesiredSize.Height);
                if (IsGreaterThan(lineSize.Direct + elementSize.Direct, maximumSize.Direct))
                {
                    ArrangeLine(lineStart, lineEnd, directDelta, indirectOffset, lineSize.Indirect);
                    indirectOffset += lineSize.Indirect;
                    lineSize = elementSize;
                    if (IsGreaterThan(elementSize.Direct, maximumSize.Direct))
                    {
                        ArrangeLine(lineEnd, ++lineEnd, directDelta, indirectOffset, elementSize.Indirect);
                        indirectOffset += lineSize.Indirect;
                        lineSize = new OrientedSize(o);
                    }
                    lineStart = lineEnd;
                }
                else
                {
                    lineSize.Direct += elementSize.Direct;
                    lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect);
                }
            }
            if (lineStart < count)
            {
                ArrangeLine(lineStart, count, directDelta, indirectOffset, lineSize.Indirect);
            }
            return finalSize;
        }

        // 줄을 재조정해준다.
        private void ArrangeLine(int lineStart, int lineEnd,
            double? directDelta, double indirectOffset, double indirectGrowth)
        {
            double directOffset = 0.0;
            Orientation o = Orientation;
            bool isHorizontal = o == Orientation.Horizontal;
            UIElementCollection children = Children;
            for (int index = lineStart; index < lineEnd; index++)
            {
                UIElement element = children[index];
                OrientedSize elementSize = new OrientedSize(o, element.DesiredSize.Width, element.DesiredSize.Height);
                double directGrowth = directDelta != null ? directDelta.Value : elementSize.Direct;
                Rect bounds = isHorizontal ?
                    new Rect(directOffset, indirectOffset, directGrowth, indirectGrowth) :
                    new Rect(indirectOffset, directOffset, indirectGrowth, directGrowth);
                element.Arrange(bounds);
                directOffset += directGrowth;
            }
        }
    }
}
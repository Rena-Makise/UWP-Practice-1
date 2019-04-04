using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    // Geolocator을 이용하여 현재 위치를 가져오는 메소드이다.
    public async Task<Geopoint> Position()
    {
        return (await new Geolocator().GetGeopositionAsync()).Coordinate.Point;
    }

    // MapControl 위에서 보일 타원요소의 시리즈들이다.
    public UIElement Marker()
    {
        Canvas marker = new Canvas();
        Ellipse outer = new Ellipse()
        {
            Width = 25,
            Height = 25,
            Margin = new Thickness(-12.5, -12.5, 0, 0),
            Fill = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240)),
        };
        Ellipse inner = new Ellipse()
        {
            Width = 20,
            Height = 20,
            Margin = new Thickness(-10, -10, 0, 0),
            Fill = new SolidColorBrush(Colors.Black),
        };
        Ellipse core = new Ellipse()
        {
            Width = 10,
            Height = 10,
            Margin = new Thickness(-5, -5, 0, 0),
            Fill = new SolidColorBrush(Colors.White),
        };
        marker.Children.Add(outer);
        marker.Children.Add(inner);
        marker.Children.Add(core);
        return marker;
    }

    public class OpenDownCommandBarVisualStateManager : VisualStateManager
    {
        protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            //replace OpenUp state change with OpenDown one and continue as normal
            if (!string.IsNullOrWhiteSpace(stateName) && stateName.EndsWith("OpenUp"))
            {
                stateName = stateName.Substring(0, stateName.Length - 6) + "OpenDown";
            }
            return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
        }
    }

    public class OpenDownCommandBar : CommandBar
    {
        public OpenDownCommandBar()
        {
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            if (layoutRoot != null)
            {
                VisualStateManager.SetCustomVisualStateManager(layoutRoot, new OpenDownCommandBarVisualStateManager());
            }
        }
    }
}
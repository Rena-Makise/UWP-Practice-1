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

namespace Set1_27_Flip_Control
{
    public sealed class FlipPanel : Control
    {
        public FlipPanel()
        {
            this.DefaultStyleKey = typeof(FlipPanel);
        }

        public static readonly DependencyProperty FrontContentProperty =
DependencyProperty.Register("FrontContent", typeof(object),
typeof(FlipPanel), null);

        public static readonly DependencyProperty BackContentProperty =
        DependencyProperty.Register("BackContent", typeof(object),
        typeof(FlipPanel), null);

        public static readonly DependencyProperty IsFlippedProperty =
        DependencyProperty.Register("IsFlipped", typeof(bool),
        typeof(FlipPanel), new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
        typeof(FlipPanel), null);

        // 컨트롤의 스타일 지정을 위한 프로퍼티들이다.
        public object FrontContent
        {
            get { return GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }
        public object BackContent
        {
            get { return GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }
        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // 컨트롤의 상태를 Normal과 Flipped 사이에서 서로 변경하는 메소드이다
        private void ChangeVisualState(bool useTransitions)
        {
            if (IsFlipped)
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Flipped", useTransitions);
            }
        }

        // ChangeVisualState를 이용하여 토글버튼을 통해 컨트롤의 상태를 전환하는 메소드이다.
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Windows.UI.Xaml.Controls.Primitives.ToggleButton flipButton =
                (Windows.UI.Xaml.Controls.Primitives.ToggleButton)GetTemplateChild("FlipButton");
            if (flipButton != null)
            {
                flipButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsFlipped = !IsFlipped;
                    ChangeVisualState(true);
                };
            }
            Windows.UI.Xaml.Controls.Primitives.ToggleButton flipButtonAlt =
                (Windows.UI.Xaml.Controls.Primitives.ToggleButton)GetTemplateChild("FlipButtonAlternative");
            if (flipButtonAlt != null)
            {
                flipButtonAlt.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsFlipped = !IsFlipped;
                    ChangeVisualState(true);
                };
            }
            ChangeVisualState(false);
        }
    }
}

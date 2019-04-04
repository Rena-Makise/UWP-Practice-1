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

namespace Set1_28_Expand_Control
{
    public sealed class ExpandPanel : Control
    {
        public ExpandPanel()
        {
            this.DefaultStyleKey = typeof(ExpandPanel);
        }
        private bool _useTransitions = true;
        // Panel이 collapse된 경우의 VisualState
        private VisualState _collapsedState;
        // Expand와 Collapse간의 스위칭 역할을 해 주는 togglebutton
        private Windows.UI.Xaml.Controls.Primitives.ToggleButton _toggleExpander;
        // Control의 content를 나타내는 FrameworkElement
        private FrameworkElement _contentElement;


        public static readonly DependencyProperty HeaderContentProperty =
        DependencyProperty.Register("HeaderContent", typeof(object), typeof(ExpandPanel), null);

        public static readonly DependencyProperty MainContentProperty =
        DependencyProperty.Register("MainContent", typeof(object), typeof(ExpandPanel), null);

        public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ExpandPanel), new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ExpandPanel), null);

        // 컨트롤의 header content 프로퍼티
        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        // 컨트롤의 main content의 프로퍼티
        public object MainContent
        {
            get { return GetValue(MainContentProperty); }
            set { SetValue(MainContentProperty, value); }
        }

        // 컨트롤이 collapsed 되었는지 Expanded 되었는지 여부 프로퍼티
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        // 컨트롤의 스타일링을 위한 프로퍼티
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // 컨트롤의 상태를 Collapsed와 Normal 사이에서 스위칭해주는 메소드
        private void ChangeVisualState(bool useTransitions)
        {
            if (IsExpanded)
            {
                if (_contentElement != null)
                {
                    _contentElement.Visibility = Visibility.Visible;
                }
                VisualStateManager.GoToState(this, "Expanded", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", useTransitions);
                _collapsedState = (VisualState)GetTemplateChild("Collapsed");
                if (_collapsedState == null)
                {
                    if (_contentElement != null)
                    {
                        _contentElement.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        // 컨트롤을 collapse시킬 수 있는 toggle button에 반응하여 
        // ChangeVisualState를 이용해 상태를 토글시켜주는 메소드
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _toggleExpander = (Windows.UI.Xaml.Controls.Primitives.ToggleButton)
                GetTemplateChild("ExpandCollapseButton");
            if (_toggleExpander != null)
            {
                _toggleExpander.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsExpanded = !IsExpanded;
                    _toggleExpander.IsChecked = IsExpanded;
                    ChangeVisualState(_useTransitions);
                };
            }
            _contentElement = (FrameworkElement)GetTemplateChild("Content");
            if (_contentElement != null)
            {
                _collapsedState = (VisualState)GetTemplateChild("Collapsed");
                if ((_collapsedState != null) && (_collapsedState.Storyboard != null))
                {
                    _collapsedState.Storyboard.Completed += (object sender, object e) =>
                    {
                        _contentElement.Visibility = Visibility.Collapsed;
                    };
                }
            }
            ChangeVisualState(false);
        }
    }
}

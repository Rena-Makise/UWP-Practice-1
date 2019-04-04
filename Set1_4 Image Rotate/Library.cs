using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

public class Library
{
    private bool _rotating = false;
    // 스토리보드는 DoubleAnimation의 파트로 이용된다.
    private Storyboard _rotation = new Storyboard();

    public void Rotate(string axis, ref Image target)
    {
        if (_rotating)
        {
            _rotation.Stop();
            _rotating = false;
        }
        else
        {
            // 0과 360 사이에서 움직이고 1초후에 시작해서 영원히 반복된다.
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0.0,
                To = 360.0,
                BeginTime = TimeSpan.FromSeconds(1),
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard.SetTarget(animation, target);
            // 각 축에 대한 PlaneProjection.Rotation값은 UIElement에 설정되며 이 경우에는 target으로 지정된 이미지가 대상이 된다.
            Storyboard.SetTargetProperty(animation, "(UIElement.Projection).(PlaneProjection.Rotation" + axis + ")");
            _rotation.Children.Clear();
            _rotation.Children.Add(animation);
            _rotation.Begin();
            _rotating = true;
        }
    }
}
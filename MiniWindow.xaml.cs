using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace KeySoundChan;

public partial class MiniWindow : Window
{
    public event Action? ExitMiniMode;
    
    public MiniWindow()
    {
        InitializeComponent();
    }
    
    public void SetImageSource(ImageSource? source)
    {
        DisplayImage.Source = source;
    }
    
    public void SetOpacity(double opacity)
    {
        DisplayImage.Opacity = opacity;
    }
    
    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
    
    private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        // 우클릭으로 미니 모드 종료
        ExitMiniMode?.Invoke();
    }
}


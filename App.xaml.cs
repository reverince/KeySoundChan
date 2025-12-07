using System.Windows;

namespace KeySoundChan;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // 미처리 예외 핸들러
        AppDomain.CurrentDomain.UnhandledException += (s, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            MessageBox.Show($"예기치 않은 오류가 발생했습니다:\n{ex?.Message}", 
                "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        };
        
        DispatcherUnhandledException += (s, args) =>
        {
            MessageBox.Show($"예기치 않은 오류가 발생했습니다:\n{args.Exception.Message}", 
                "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };
    }
}


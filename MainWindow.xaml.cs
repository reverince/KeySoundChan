using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;
using KeySoundChan.ViewModels;

namespace KeySoundChan;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private TaskbarIcon? _trayIcon;
    private MiniWindow? _miniWindow;
    private static readonly string[] SupportedAudioExtensions = { ".wav", ".mp3", ".m4a", ".ogg", ".flac" };
    private static readonly string[] SupportedImageExtensions = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
        
        // 미니 모드 변경 감지
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        
        LoadWindowSettings();
        InitializeTrayIcon();
    }
    
    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.IsMiniMode))
        {
            if (_viewModel.IsMiniMode)
            {
                OpenMiniWindow();
            }
            else
            {
                CloseMiniWindow();
            }
        }
        else if (e.PropertyName == nameof(MainViewModel.CurrentImageSource))
        {
            // 이미지 변경 시 미니 윈도우에도 반영
            _miniWindow?.SetImageSource(_viewModel.CurrentImageSource);
        }
        else if (e.PropertyName == nameof(MainViewModel.MiniImageOpacity))
        {
            // 불투명도 변경 시 미니 윈도우에도 반영
            _miniWindow?.SetOpacity(_viewModel.MiniImageOpacity / 100.0);
        }
    }
    
    private void OpenMiniWindow()
    {
        Hide(); // 메인 창 숨김
        
        _miniWindow = new MiniWindow();
        _miniWindow.SetImageSource(_viewModel.CurrentImageSource);
        _miniWindow.SetOpacity(_viewModel.MiniImageOpacity / 100.0);
        _miniWindow.ExitMiniMode += () =>
        {
            _viewModel.IsMiniMode = false;
        };
        _miniWindow.Show();
    }
    
    private void CloseMiniWindow()
    {
        _miniWindow?.Close();
        _miniWindow = null;
        
        Show(); // 메인 창 다시 표시
        Activate();
    }

    protected override void OnStateChanged(EventArgs e)
    {
        base.OnStateChanged(e);
        
        if (WindowState == WindowState.Minimized && _viewModel.MinimizeToTray)
        {
            Hide();
            _trayIcon?.ShowBalloonTip("KeySoundChan", "트레이로 최소화되었습니다.", BalloonIcon.Info);
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        SaveWindowSettings();
        _miniWindow?.Close();
        _trayIcon?.Dispose();
        _viewModel.Dispose();
        base.OnClosed(e);
    }

    #region 트레이 아이콘
    private void InitializeTrayIcon()
    {
        _trayIcon = new TaskbarIcon
        {
            ToolTipText = "KeySoundChan",
            Visibility = Visibility.Visible
        };
        
        // 아이콘 설정
        var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "wave.ico");
        if (File.Exists(iconPath))
        {
            _trayIcon.Icon = new Icon(iconPath);
        }
        
        // 컨텍스트 메뉴
        var contextMenu = new ContextMenu();
        
        var showItem = new MenuItem { Header = "열기" };
        showItem.Click += (s, e) => ShowMainWindow();
        contextMenu.Items.Add(showItem);
        
        var miniItem = new MenuItem { Header = "미니 모드" };
        miniItem.Click += (s, e) => 
        {
            if (_viewModel.HasImage)
                _viewModel.IsMiniMode = !_viewModel.IsMiniMode;
        };
        contextMenu.Items.Add(miniItem);
        
        contextMenu.Items.Add(new Separator());
        
        var exitItem = new MenuItem { Header = "종료" };
        exitItem.Click += (s, e) => Application.Current.Shutdown();
        contextMenu.Items.Add(exitItem);
        
        _trayIcon.ContextMenu = contextMenu;
        _trayIcon.TrayMouseDoubleClick += (s, e) => ShowMainWindow();
    }

    private void ShowMainWindow()
    {
        if (_viewModel.IsMiniMode)
        {
            _viewModel.IsMiniMode = false;
        }
        else
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }
    }
    #endregion

    #region 창 크기 저장
    private void LoadWindowSettings()
    {
        try
        {
            var settingsPath = GetWindowSettingsPath();
            if (!File.Exists(settingsPath)) return;
            
            var lines = File.ReadAllLines(settingsPath);
            foreach (var line in lines)
            {
                var parts = line.Split('=', 2);
                if (parts.Length != 2) continue;
                
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                
                switch (key)
                {
                    case "Width":
                        if (double.TryParse(value, out var w) && w >= MinWidth)
                            Width = w;
                        break;
                    case "Height":
                        if (double.TryParse(value, out var h) && h >= MinHeight)
                            Height = h;
                        break;
                }
            }
        }
        catch
        {
            // 설정 로드 실패 시 기본값 사용
        }
    }

    private void SaveWindowSettings()
    {
        try
        {
            var settingsPath = GetWindowSettingsPath();
            var dir = Path.GetDirectoryName(settingsPath);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            
            var lines = new[]
            {
                $"Width={Width}",
                $"Height={Height}"
            };
            
            File.WriteAllLines(settingsPath, lines);
        }
        catch
        {
            // 설정 저장 실패 무시
        }
    }

    private static string GetWindowSettingsPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(appData, "KeySoundChan", "window.txt");
    }
    #endregion

    #region 프리셋 콤보박스
    private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PresetComboBox.SelectedIndex <= 0) return;
        
        var selectedItem = PresetComboBox.SelectedItem as ComboBoxItem;
        if (selectedItem?.Content is string presetName)
        {
            _viewModel.ApplyPresetCommand.Execute(presetName);
        }
        
        PresetComboBox.SelectedIndex = 0;
    }
    #endregion

    #region 이미지 기능
    private void SelectImage_Click(object sender, RoutedEventArgs e)
    {
        var paths = _viewModel.SelectImageFiles();
        if (paths != null && paths.Length > 0)
        {
            _viewModel.SetImages(paths);
        }
    }

    private void ClearImage_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.ClearImage();
    }

    private void ImagePanel_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0 && IsSupportedImageFile(files[0]))
            {
                e.Effects = DragDropEffects.Copy;
                ImageDropHint.Visibility = Visibility.Visible;
                ImagePanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x1f, 0x1f, 0x23));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    private void ImagePanel_DragLeave(object sender, DragEventArgs e)
    {
        var surfaceBrush = (SolidColorBrush)FindResource("SurfaceBrush");
        ImageDropHint.Visibility = Visibility.Collapsed;
        ImagePanel.Background = surfaceBrush;
    }

    private void ImagePanel_Drop(object sender, DragEventArgs e)
    {
        var surfaceBrush = (SolidColorBrush)FindResource("SurfaceBrush");
        ImageDropHint.Visibility = Visibility.Collapsed;
        ImagePanel.Background = surfaceBrush;
        
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            // 이미지 파일만 필터링
            var imageFiles = files.Where(IsSupportedImageFile).ToArray();
            
            if (imageFiles.Length > 0)
            {
                _viewModel.SetImages(imageFiles);
            }
            else
            {
                MessageBox.Show("지원하지 않는 파일 형식입니다.\n(gif, png, jpg, bmp)", "오류",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    private static bool IsSupportedImageFile(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return Array.Exists(SupportedImageExtensions, e => e == ext);
    }
    #endregion

    #region 사운드 드래그 앤 드롭
    private void Panel_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0 && IsSupportedAudioFile(files[0]))
            {
                e.Effects = DragDropEffects.Copy;
                
                if (sender == KeyDownPanel)
                {
                    KeyDownDropHint.Visibility = Visibility.Visible;
                    KeyDownPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x1f, 0x1f, 0x23));
                }
                else if (sender == KeyUpPanel)
                {
                    KeyUpDropHint.Visibility = Visibility.Visible;
                    KeyUpPanel.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x1f, 0x1f, 0x23));
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    private void Panel_DragLeave(object sender, DragEventArgs e)
    {
        var surfaceBrush = (SolidColorBrush)FindResource("SurfaceBrush");
        
        if (sender == KeyDownPanel)
        {
            KeyDownDropHint.Visibility = Visibility.Collapsed;
            KeyDownPanel.Background = surfaceBrush;
        }
        else if (sender == KeyUpPanel)
        {
            KeyUpDropHint.Visibility = Visibility.Collapsed;
            KeyUpPanel.Background = surfaceBrush;
        }
    }

    private void KeyDownPanel_Drop(object sender, DragEventArgs e)
    {
        ResetPanelVisuals();
        
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                var file = files[0];
                if (IsSupportedAudioFile(file))
                {
                    _viewModel.SetKeyDownSound(file);
                }
                else
                {
                    ShowUnsupportedAudioFileMessage();
                }
            }
        }
    }

    private void KeyUpPanel_Drop(object sender, DragEventArgs e)
    {
        ResetPanelVisuals();
        
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                var file = files[0];
                if (IsSupportedAudioFile(file))
                {
                    _viewModel.SetKeyUpSound(file);
                }
                else
                {
                    ShowUnsupportedAudioFileMessage();
                }
            }
        }
    }

    private void ResetPanelVisuals()
    {
        var surfaceBrush = (SolidColorBrush)FindResource("SurfaceBrush");
        
        KeyDownDropHint.Visibility = Visibility.Collapsed;
        KeyDownPanel.Background = surfaceBrush;
        
        KeyUpDropHint.Visibility = Visibility.Collapsed;
        KeyUpPanel.Background = surfaceBrush;
    }

    private static bool IsSupportedAudioFile(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return Array.Exists(SupportedAudioExtensions, e => e == ext);
    }

    private static void ShowUnsupportedAudioFileMessage()
    {
        MessageBox.Show("지원하지 않는 파일 형식입니다.\n(wav, mp3, m4a, ogg, flac)", "오류",
            MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    #endregion
}

using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeySoundChan.Services;
using Microsoft.Win32;

namespace KeySoundChan.ViewModels;

public partial class MainViewModel : ObservableObject, IDisposable
{
    private readonly KeyboardHookService _keyboardHook;
    private readonly AudioService _audioService;
    
    [ObservableProperty]
    private bool _isEnabled = true;
    
    [ObservableProperty]
    private int _volume = 50;
    
    [ObservableProperty]
    private string _keyDownSoundName = "선택 안 됨";
    
    [ObservableProperty]
    private string _keyUpSoundName = "선택 안 됨";
    
    [ObservableProperty]
    private bool _hasKeyDownSound;
    
    [ObservableProperty]
    private bool _hasKeyUpSound;
    
    [ObservableProperty]
    private int _keyPressCount;
    
    [ObservableProperty]
    private bool _showVisualFeedback = true;
    
    [ObservableProperty]
    private double _feedbackOpacity;
    
    [ObservableProperty]
    private bool _minimizeToTray;
    
    // 이미지 관련
    [ObservableProperty]
    private bool _hasImage;
    
    [ObservableProperty]
    private string _imageInfo = "이미지 또는 GIF를 선택하세요";
    
    [ObservableProperty]
    private ImageSource? _currentImageSource;
    
    private List<BitmapSource>? _renderedFrames;
    private int _currentFrameIndex;
    private List<string>? _imagePaths;
    
    // 미니 모드
    [ObservableProperty]
    private bool _isMiniMode;
    
    [ObservableProperty]
    private int _miniImageOpacity = 100;

    // 내장 프리셋 사운드 경로들
    public Dictionary<string, string> SoundPresets { get; } = new()
    {
        { "🔔 벨", "bell.wav" },
        { "👏 박수", "clap.wav" },
        { "🥁 퍼커션", "percussion.wav" },
        { "🏓 탁구", "tabletennis.wav" },
        { "💥 샷", "shot.wav" },
        { "🐭 찍찍", "squeak.mp3" },
        { "💀 샌즈", "sans.wav" },
    };

    public MainViewModel()
    {
        _keyboardHook = new KeyboardHookService();
        _audioService = new AudioService();
        
        _keyboardHook.KeyDown += OnKeyDown;
        _keyboardHook.KeyUp += OnKeyUp;
        
        LoadSettings();
        _keyboardHook.Start();
    }

    partial void OnIsEnabledChanged(bool value)
    {
        _keyboardHook.IsEnabled = value;
    }

    partial void OnVolumeChanged(int value)
    {
        _audioService.Volume = value / 100f;
        SaveSettings();
    }
    
    partial void OnMinimizeToTrayChanged(bool value)
    {
        SaveSettings();
    }

    private void OnKeyDown(int keyCode)
    {
        if (!IsEnabled) return;
        
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            KeyPressCount++;
            
            if (ShowVisualFeedback)
            {
                FeedbackOpacity = 1.0;
                StartFeedbackFade();
            }
            
            // 이미지 프레임 전환
            if (_renderedFrames != null && _renderedFrames.Count > 0)
            {
                _currentFrameIndex = (_currentFrameIndex + 1) % _renderedFrames.Count;
                CurrentImageSource = _renderedFrames[_currentFrameIndex];
            }
        });
        
        _audioService.PlayKeyDown();
    }

    private void OnKeyUp(int keyCode)
    {
        if (!IsEnabled) return;
        _audioService.PlayKeyUp();
    }

    private System.Windows.Threading.DispatcherTimer? _feedbackTimer;
    
    private void StartFeedbackFade()
    {
        _feedbackTimer?.Stop();
        _feedbackTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        _feedbackTimer.Tick += (s, e) =>
        {
            FeedbackOpacity -= 0.08;
            if (FeedbackOpacity <= 0)
            {
                FeedbackOpacity = 0;
                _feedbackTimer?.Stop();
            }
        };
        _feedbackTimer.Start();
    }

    [RelayCommand]
    private void SelectKeyDownSound()
    {
        var path = ShowOpenFileDialog("사운드 파일|*.wav;*.mp3;*.m4a;*.ogg;*.flac|모든 파일|*.*", "사운드 파일 선택");
        if (path != null)
        {
            SetKeyDownSound(path);
        }
    }

    [RelayCommand]
    private void SelectKeyUpSound()
    {
        var path = ShowOpenFileDialog("사운드 파일|*.wav;*.mp3;*.m4a;*.ogg;*.flac|모든 파일|*.*", "사운드 파일 선택");
        if (path != null)
        {
            SetKeyUpSound(path);
        }
    }

    [RelayCommand]
    private void ClearKeyDownSound()
    {
        _audioService.SetKeyDownSound(null);
        KeyDownSoundName = "선택 안 됨";
        HasKeyDownSound = false;
        SaveSettings();
    }

    [RelayCommand]
    private void ClearKeyUpSound()
    {
        _audioService.SetKeyUpSound(null);
        KeyUpSoundName = "선택 안 됨";
        HasKeyUpSound = false;
        SaveSettings();
    }

    [RelayCommand]
    private void ApplyPreset(string presetName)
    {
        if (SoundPresets.TryGetValue(presetName, out var fileName))
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var soundPath = Path.Combine(basePath, "resources", fileName);
            
            if (File.Exists(soundPath))
            {
                SetKeyDownSound(soundPath);
            }
            else
            {
                MessageBox.Show($"사운드 파일을 찾을 수 없습니다: {fileName}", "오류", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    public void SetKeyDownSound(string path)
    {
        if (_audioService.SetKeyDownSound(path))
        {
            KeyDownSoundName = Path.GetFileName(path);
            HasKeyDownSound = true;
            SaveSettings();
        }
        else
        {
            MessageBox.Show("사운드 파일을 불러올 수 없습니다.", "오류",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void SetKeyUpSound(string path)
    {
        if (_audioService.SetKeyUpSound(path))
        {
            KeyUpSoundName = Path.GetFileName(path);
            HasKeyUpSound = true;
            SaveSettings();
        }
        else
        {
            MessageBox.Show("사운드 파일을 불러올 수 없습니다.", "오류",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    // 이미지 관련 메서드 - 단일 파일 (GIF 또는 이미지)
    public void SetImage(string path)
    {
        SetImages(new[] { path });
    }
    
    // 여러 이미지 파일을 스프라이트처럼 등록
    public void SetImages(string[] paths)
    {
        try
        {
            _imagePaths = paths.ToList();
            _renderedFrames = new List<BitmapSource>();
            _currentFrameIndex = 0;
            
            if (paths.Length == 1)
            {
                var path = paths[0];
                var ext = Path.GetExtension(path).ToLowerInvariant();
                
                if (ext == ".gif")
                {
                    // GIF 프레임을 올바르게 렌더링
                    ExtractGifFrames(path);
                    ImageInfo = $"{Path.GetFileName(path)} ({_renderedFrames.Count} 프레임)";
                }
                else
                {
                    // 일반 이미지
                    var bitmap = LoadBitmapImage(path);
                    if (bitmap != null)
                    {
                        _renderedFrames.Add(bitmap);
                        ImageInfo = Path.GetFileName(path);
                    }
                }
            }
            else
            {
                // 여러 이미지 파일 - 스프라이트 시트처럼 사용
                foreach (var path in paths.OrderBy(p => p))
                {
                    var ext = Path.GetExtension(path).ToLowerInvariant();
                    
                    if (ext == ".gif")
                    {
                        ExtractGifFrames(path);
                    }
                    else
                    {
                        var bitmap = LoadBitmapImage(path);
                        if (bitmap != null)
                        {
                            _renderedFrames.Add(bitmap);
                        }
                    }
                }
                ImageInfo = $"{paths.Length}개 이미지 ({_renderedFrames.Count} 프레임)";
            }
            
            if (_renderedFrames.Count > 0)
            {
                CurrentImageSource = _renderedFrames[0];
                HasImage = true;
                SaveSettings();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"이미지를 불러올 수 없습니다: {ex.Message}", "오류",
                MessageBoxButton.OK, MessageBoxImage.Error);
            ClearImage();
        }
    }
    
    private BitmapImage? LoadBitmapImage(string path)
    {
        try
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        catch
        {
            return null;
        }
    }
    
    private void ExtractGifFrames(string path)
    {
        var decoder = new GifBitmapDecoder(
            new Uri(path),
            BitmapCreateOptions.PreservePixelFormat,
            BitmapCacheOption.OnLoad);
        
        if (decoder.Frames.Count == 0) return;
        
        // 캔버스 크기 (첫 프레임 기준)
        int width = decoder.Frames[0].PixelWidth;
        int height = decoder.Frames[0].PixelHeight;
        
        // 메타데이터에서 실제 크기 가져오기
        var metadata = decoder.Metadata as BitmapMetadata;
        if (metadata != null)
        {
            try
            {
                var w = metadata.GetQuery("/logscrdesc/Width");
                var h = metadata.GetQuery("/logscrdesc/Height");
                if (w != null) width = (ushort)w;
                if (h != null) height = (ushort)h;
            }
            catch { }
        }
        
        // 이전 프레임을 유지할 캔버스
        WriteableBitmap? canvas = null;
        
        for (int i = 0; i < decoder.Frames.Count; i++)
        {
            var frame = decoder.Frames[i];
            var frameMetadata = frame.Metadata as BitmapMetadata;
            
            // 프레임 위치 및 disposal 정보
            int left = 0, top = 0;
            ushort disposal = 0;
            
            if (frameMetadata != null)
            {
                try
                {
                    var l = frameMetadata.GetQuery("/imgdesc/Left");
                    var t = frameMetadata.GetQuery("/imgdesc/Top");
                    var d = frameMetadata.GetQuery("/grctlext/Disposal");
                    
                    if (l != null) left = (ushort)l;
                    if (t != null) top = (ushort)t;
                    if (d != null) disposal = (ushort)(byte)d;
                }
                catch { }
            }
            
            // 새 캔버스 생성 또는 이전 캔버스 복사
            if (canvas == null || disposal == 2) // 2 = RestoreToBackground
            {
                canvas = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            }
            
            // 프레임을 캔버스에 그리기
            var convertedFrame = new FormatConvertedBitmap(frame, PixelFormats.Pbgra32, null, 0);
            int stride = convertedFrame.PixelWidth * 4;
            byte[] pixels = new byte[stride * convertedFrame.PixelHeight];
            convertedFrame.CopyPixels(pixels, stride, 0);
            
            // 투명 픽셀 처리 (알파가 0이 아닌 픽셀만 복사)
            canvas.Lock();
            try
            {
                unsafe
                {
                    byte* canvasPtr = (byte*)canvas.BackBuffer;
                    int canvasStride = canvas.BackBufferStride;
                    
                    for (int y = 0; y < convertedFrame.PixelHeight; y++)
                    {
                        for (int x = 0; x < convertedFrame.PixelWidth; x++)
                        {
                            int srcIdx = y * stride + x * 4;
                            int dstX = left + x;
                            int dstY = top + y;
                            
                            if (dstX >= width || dstY >= height) continue;
                            
                            int dstIdx = dstY * canvasStride + dstX * 4;
                            
                            byte alpha = pixels[srcIdx + 3];
                            if (alpha > 0)
                            {
                                canvasPtr[dstIdx] = pixels[srcIdx];         // B
                                canvasPtr[dstIdx + 1] = pixels[srcIdx + 1]; // G
                                canvasPtr[dstIdx + 2] = pixels[srcIdx + 2]; // R
                                canvasPtr[dstIdx + 3] = alpha;              // A
                            }
                        }
                    }
                }
                canvas.AddDirtyRect(new Int32Rect(0, 0, width, height));
            }
            finally
            {
                canvas.Unlock();
            }
            
            // 현재 상태를 저장
            var rendered = canvas.Clone();
            rendered.Freeze();
            _renderedFrames.Add(rendered);
            
            // disposal == 3 이면 이전 프레임으로 복원 (복잡한 케이스, 일단 스킵)
        }
    }
    
    public void ClearImage()
    {
        _renderedFrames = null;
        _imagePaths = null;
        _currentFrameIndex = 0;
        CurrentImageSource = null;
        HasImage = false;
        ImageInfo = "이미지 또는 GIF를 선택하세요";
        SaveSettings();
    }
    
    [RelayCommand]
    private void ToggleMiniMode()
    {
        IsMiniMode = !IsMiniMode;
    }
    
    partial void OnMiniImageOpacityChanged(int value) => SaveSettings();

    private string? ShowOpenFileDialog(string filter, string title)
    {
        var dialog = new OpenFileDialog
        {
            Filter = filter,
            Title = title
        };
        
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
    
    public string[]? SelectImageFiles()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "이미지 파일|*.gif;*.png;*.jpg;*.jpeg;*.bmp|GIF|*.gif|모든 파일|*.*",
            Title = "이미지/GIF 선택 (여러 개 선택 가능)",
            Multiselect = true
        };
        
        return dialog.ShowDialog() == true ? dialog.FileNames : null;
    }

    private void LoadSettings()
    {
        try
        {
            var settingsPath = GetSettingsPath();
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
                    case "Volume":
                        if (int.TryParse(value, out var vol))
                        {
                            Volume = vol;
                            _audioService.Volume = vol / 100f;
                        }
                        break;
                    case "KeyDownSound":
                        if (File.Exists(value))
                            SetKeyDownSound(value);
                        break;
                    case "KeyUpSound":
                        if (File.Exists(value))
                            SetKeyUpSound(value);
                        break;
                    case "ShowVisualFeedback":
                        ShowVisualFeedback = value == "true";
                        break;
                    case "MinimizeToTray":
                        MinimizeToTray = value == "true";
                        break;
                    case "ImagePaths":
                        if (!string.IsNullOrEmpty(value))
                        {
                            var paths = value.Split('|').Where(File.Exists).ToArray();
                            if (paths.Length > 0)
                                SetImages(paths);
                        }
                        break;
                    case "MiniImageOpacity":
                        if (int.TryParse(value, out var imgOp))
                            MiniImageOpacity = imgOp;
                        break;
                }
            }
        }
        catch
        {
            // 설정 로드 실패 시 기본값 사용
        }
    }

    private void SaveSettings()
    {
        try
        {
            var settingsPath = GetSettingsPath();
            var dir = Path.GetDirectoryName(settingsPath);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            
            var lines = new[]
            {
                $"Volume={Volume}",
                $"KeyDownSound={_audioService.KeyDownSoundPath ?? ""}",
                $"KeyUpSound={_audioService.KeyUpSoundPath ?? ""}",
                $"ShowVisualFeedback={(ShowVisualFeedback ? "true" : "false")}",
                $"MinimizeToTray={(MinimizeToTray ? "true" : "false")}",
                $"ImagePaths={(_imagePaths != null ? string.Join("|", _imagePaths) : "")}",
                $"MiniImageOpacity={MiniImageOpacity}"
            };
            
            File.WriteAllLines(settingsPath, lines);
        }
        catch
        {
            // 설정 저장 실패 무시
        }
    }

    private static string GetSettingsPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(appData, "KeySoundChan", "settings.txt");
    }

    public void Dispose()
    {
        SaveSettings();
        _keyboardHook.Dispose();
        _audioService.Dispose();
        GC.SuppressFinalize(this);
    }
}

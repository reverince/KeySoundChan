using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace KeySoundChan.Services;

public class AudioService : IDisposable
{
    private IWavePlayer? _outputDevice;
    private MixingSampleProvider? _mixer;
    private CachedSound? _keyDownSound;
    private CachedSound? _keyUpSound;
    private float _volume = 0.5f;
    private bool _isInitialized;

    public float Volume
    {
        get => _volume;
        set => _volume = Math.Clamp(value, 0f, 1f);
    }

    public string? KeyDownSoundPath { get; private set; }
    public string? KeyUpSoundPath { get; private set; }

    public AudioService()
    {
        InitializeAudio();
    }

    private void InitializeAudio()
    {
        try
        {
            // WASAPI Shared 모드 - 가장 낮은 레이턴시
            var wasapi = new WasapiOut(
                NAudio.CoreAudioApi.AudioClientShareMode.Shared,
                latency: 20); // 20ms 레이턴시
            
            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            {
                ReadFully = true
            };
            
            wasapi.Init(_mixer);
            wasapi.Play();
            _outputDevice = wasapi;
            _isInitialized = true;
        }
        catch
        {
            // WASAPI 실패 시 WaveOut 폴백 (저레이턴시 설정)
            try
            {
                var waveOut = new WaveOutEvent
                {
                    DesiredLatency = 50,      // 50ms
                    NumberOfBuffers = 2        // 버퍼 수 최소화
                };
                
                _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
                {
                    ReadFully = true
                };
                
                waveOut.Init(_mixer);
                waveOut.Play();
                _outputDevice = waveOut;
                _isInitialized = true;
            }
            catch
            {
                _isInitialized = false;
            }
        }
    }

    public bool SetKeyDownSound(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            _keyDownSound = null;
            KeyDownSoundPath = null;
            return true;
        }

        try
        {
            _keyDownSound = new CachedSound(filePath);
            KeyDownSoundPath = filePath;
            return true;
        }
        catch
        {
            _keyDownSound = null;
            KeyDownSoundPath = null;
            return false;
        }
    }

    public bool SetKeyUpSound(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            _keyUpSound = null;
            KeyUpSoundPath = null;
            return true;
        }

        try
        {
            _keyUpSound = new CachedSound(filePath);
            KeyUpSoundPath = filePath;
            return true;
        }
        catch
        {
            _keyUpSound = null;
            KeyUpSoundPath = null;
            return false;
        }
    }

    public void PlayKeyDown()
    {
        if (_keyDownSound != null && _isInitialized)
        {
            PlaySound(_keyDownSound);
        }
    }

    public void PlayKeyUp()
    {
        if (_keyUpSound != null && _isInitialized)
        {
            PlaySound(_keyUpSound);
        }
    }

    private void PlaySound(CachedSound sound)
    {
        if (_mixer == null) return;
        
        var provider = new CachedSoundSampleProvider(sound, _volume);
        _mixer.AddMixerInput(provider);
    }

    public void Dispose()
    {
        _outputDevice?.Stop();
        _outputDevice?.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// 메모리에 캐시된 사운드 데이터
/// </summary>
public class CachedSound
{
    public float[] AudioData { get; }
    public WaveFormat WaveFormat { get; }

    public CachedSound(string audioFileName)
    {
        using var audioFileReader = new AudioFileReader(audioFileName);
        
        var targetFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        ISampleProvider sampleProvider = audioFileReader;
        
        // 모노 -> 스테레오 변환
        if (audioFileReader.WaveFormat.Channels == 1)
        {
            sampleProvider = new MonoToStereoSampleProvider(sampleProvider);
        }
        
        // 샘플레이트가 다르면 리샘플링
        if (audioFileReader.WaveFormat.SampleRate != 44100)
        {
            sampleProvider = new WdlResamplingSampleProvider(sampleProvider, 44100);
        }
        
        WaveFormat = targetFormat;
        
        // 버퍼 크기 최적화
        var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
        var readBuffer = new float[4096];
        int samplesRead;
        
        while ((samplesRead = sampleProvider.Read(readBuffer, 0, readBuffer.Length)) > 0)
        {
            wholeFile.AddRange(readBuffer.Take(samplesRead));
        }
        
        AudioData = wholeFile.ToArray();
    }
}

/// <summary>
/// 캐시된 사운드를 재생하기 위한 샘플 프로바이더 (최적화)
/// </summary>
public class CachedSoundSampleProvider : ISampleProvider
{
    private readonly CachedSound _cachedSound;
    private readonly float _volume;
    private int _position;

    public WaveFormat WaveFormat => _cachedSound.WaveFormat;

    public CachedSoundSampleProvider(CachedSound cachedSound, float volume)
    {
        _cachedSound = cachedSound;
        _volume = volume;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        var audioData = _cachedSound.AudioData;
        var availableSamples = audioData.Length - _position;
        var samplesToCopy = Math.Min(availableSamples, count);
        
        if (samplesToCopy <= 0) return 0;

        // 볼륨 적용하며 복사 (최적화)
        var sourceSpan = audioData.AsSpan(_position, (int)samplesToCopy);
        var destSpan = buffer.AsSpan(offset, (int)samplesToCopy);
        
        for (int i = 0; i < samplesToCopy; i++)
        {
            destSpan[i] = sourceSpan[i] * _volume;
        }
        
        _position += (int)samplesToCopy;
        return (int)samplesToCopy;
    }
}

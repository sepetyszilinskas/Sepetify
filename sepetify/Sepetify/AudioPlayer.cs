using NAudio.Wave;

namespace Sepetify;

/// <summary>
/// Thin wrapper around NAudio: play/pause/stop, seek, volume and track end events.
/// </summary>
public sealed class AudioPlayer : IDisposable
{
    private WaveOut? _output;
    private AudioFileReader? _reader;
    private bool _disposed;

    public event EventHandler? PlaybackEnded;

    public string? CurrentPath { get; private set; }
    public TimeSpan Duration => _reader?.TotalTime ?? TimeSpan.Zero;
    public TimeSpan Position
    {
        get => _reader?.CurrentTime ?? TimeSpan.Zero;
        set
        {
            if (_reader is null) return;
            var target = value < TimeSpan.Zero ? TimeSpan.Zero : (value > Duration ? Duration : value);
            _reader.CurrentTime = target;
        }
    }

    public bool IsPlaying => _output?.PlaybackState == PlaybackState.Playing;
    public bool HasTrack => _reader is not null;

    public float Volume
    {
        get => _reader?.Volume ?? 1f;
        set { if (_reader is not null) _reader.Volume = Math.Clamp(value, 0f, 1f); }
    }

    /// <summary>Loads a file without starting playback.</summary>
    public void Load(string path, bool startPlaying)
    {
        Close();
        _reader = new AudioFileReader(path);
        _output = new WaveOut { BufferMilliseconds = 120 };
        _output.Init(_reader);
        _output.PlaybackStopped += OnPlaybackStopped;
        CurrentPath = path;
        if (startPlaying) _output.Play();
    }

    public void Play()
    {
        if (_output is null) return;
        if (_output.PlaybackState == PlaybackState.Stopped && _reader is not null &&
            _reader.CurrentTime >= _reader.TotalTime - TimeSpan.FromMilliseconds(200))
        {
            _reader.CurrentTime = TimeSpan.Zero;
        }
        _output.Play();
    }

    public void Pause() => _output?.Pause();

    public void TogglePlayPause()
    {
        if (IsPlaying) Pause(); else Play();
    }

    /// <summary>Stops and rewinds to the beginning.</summary>
    public void Stop()
    {
        if (_output is null) return;
        _output.Stop();
        if (_reader is not null) _reader.CurrentTime = TimeSpan.Zero;
    }

    private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
    {
        // Only report natural end-of-file, not manual stops.
        if (e.Exception is not null) return;
        if (_reader is null) return;
        if (_reader.CurrentTime < _reader.TotalTime - TimeSpan.FromMilliseconds(500)) return;

        PlaybackEnded?.Invoke(this, EventArgs.Empty);
    }

    public void Close()
    {
        if (_output is not null)
        {
            _output.PlaybackStopped -= OnPlaybackStopped;
            try { _output.Stop(); } catch { /* device may already be gone */ }
            _output.Dispose();
            _output = null;
        }
        _reader?.Dispose();
        _reader = null;
        CurrentPath = null;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Close();
    }
}

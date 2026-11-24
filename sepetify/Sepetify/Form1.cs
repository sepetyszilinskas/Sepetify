namespace Sepetify;

public partial class Form1 : Form
{
    private System.ComponentModel.IContainer components = null!;

    private Panel pnlSidebar = null!;
    private Panel pnlPlayer = null!;
    private Panel pnlDivider = null!;
    private ListView lstPlaylist = null!;
    private FlowLayoutPanel btnRow = null!;
    private TextBox txtSearch = null!;
    private Label lblStatus = null!;
    private Button btnAddFiles = null!;
    private Button btnAddFolder = null!;
    private Button btnClear = null!;
    private PictureBox picArtwork = null!;
    private Label lblTitle = null!;
    private Label lblArtist = null!;
    private Label lblCurrent = null!;
    private Label lblTotal = null!;
    private Label lblVolume = null!;
    private TrackBar trkSeek = null!;
    private TrackBar trkVolume = null!;
    private Button btnPlay = null!;
    private Button btnPrev = null!;
    private Button btnNext = null!;
    private Button btnStop = null!;
    private Button btnShuffle = null!;
    private Button btnRepeat = null!;
    private OpenFileDialog dlgFiles = null!;
    private FolderBrowserDialog dlgFolder = null!;
    private System.Windows.Forms.Timer tmrTick = null!;

    private readonly AudioPlayer _player = new();
    private readonly List<Track> _tracks = new();
    private List<Track> _visible = new();
    private int _currentIndex = -1;
    private bool _draggingSeek;
    private bool _shuffle;
    private RepeatMode _repeat = RepeatMode.All;

    private enum RepeatMode { Off, All, One }

    public Form1()
    {
        InitializeComponent();

        _player.PlaybackEnded += (_, _) => BeginInvoke(OnTrackEnded);
        _player.Volume = trkVolume.Value / 100f;

        KeyPreview = true;
        KeyDown += Form1_KeyDown;

        UpdatePlayButton();
        tmrTick.Start();
    }

    // ---------------- Playlist loading ----------------

    private void btnAddFiles_Click(object? sender, EventArgs e)
    {
        if (dlgFiles.ShowDialog(this) != DialogResult.OK) return;
        AddPaths(dlgFiles.FileNames);
    }

    private void btnAddFolder_Click(object? sender, EventArgs e)
    {
        if (dlgFolder.ShowDialog(this) != DialogResult.OK) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            var files = Directory.EnumerateFiles(dlgFolder.SelectedPath, "*.*", SearchOption.AllDirectories)
                                 .Where(Track.IsSupported)
                                 .OrderBy(p => p, StringComparer.OrdinalIgnoreCase);
            AddPaths(files);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Could not read that folder:\n" + ex.Message, "Sepetify",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void AddPaths(IEnumerable<string> paths)
    {
        var existing = new HashSet<string>(_tracks.Select(t => t.Path), StringComparer.OrdinalIgnoreCase);
        int added = 0;

        Cursor = Cursors.WaitCursor;
        try
        {
            foreach (var path in paths.Where(Track.IsSupported))
            {
                if (!existing.Add(path)) continue;
                _tracks.Add(Track.Load(path));
                added++;
            }
        }
        finally
        {
            Cursor = Cursors.Default;
        }

        if (added == 0 && _tracks.Count > 0) return;
        RefreshList();
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        _player.Stop();
        _player.Close();
        _tracks.Clear();
        _currentIndex = -1;
        lblTitle.Text = "Nothing playing";
        lblArtist.Text = "Add some music to get started";
        picArtwork.Image = null;
        lblCurrent.Text = "0:00";
        lblTotal.Text = "0:00";
        trkSeek.Value = 0;
        RefreshList();
        UpdatePlayButton();
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e) => RefreshList();

    private void RefreshList()
    {
        string query = txtSearch.Text.Trim();
        _visible = string.IsNullOrEmpty(query)
            ? _tracks.ToList()
            : _tracks.Where(t =>
                t.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                t.Artist.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                t.Album.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

        lstPlaylist.BeginUpdate();
        lstPlaylist.Items.Clear();
        for (int i = 0; i < _visible.Count; i++)
        {
            var t = _visible[i];
            var item = new ListViewItem((i + 1).ToString());
            item.SubItems.Add(t.Title);
            item.SubItems.Add(t.DisplayArtist);
            item.SubItems.Add(Track.FormatTime(t.Duration));
            item.Tag = t;
            if (ReferenceEquals(t, CurrentTrack)) item.BackColor = Color.FromArgb(38, 60, 46);
            lstPlaylist.Items.Add(item);
        }
        lstPlaylist.EndUpdate();

        lblStatus.Text = _tracks.Count == 0
            ? "No tracks loaded"
            : $"{_tracks.Count} track{(_tracks.Count == 1 ? "" : "s")}" +
              (_visible.Count != _tracks.Count ? $"  ·  {_visible.Count} shown" : "");

        trkSeek.Enabled = _player.HasTrack;
    }
    private Track? CurrentTrack =>
        _currentIndex >= 0 && _currentIndex < _tracks.Count ? _tracks[_currentIndex] : null;

    // ---------------- Playback ----------------

    private void lstPlaylist_DoubleClick(object? sender, EventArgs e)
    {
        if (lstPlaylist.SelectedItems.Count == 0) return;
        if (lstPlaylist.SelectedItems[0].Tag is Track track) PlayTrack(_tracks.IndexOf(track));
    }

    private void PlayTrack(int index)
    {
        if (index < 0 || index >= _tracks.Count) return;
        var track = _tracks[index];

        try
        {
            _player.Load(track.Path, startPlaying: true);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Could not play \"{track.FileName}\":\n{ex.Message}", "Sepetify",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _currentIndex = index;
        _player.Volume = trkVolume.Value / 100f;

        lblTitle.Text = track.Title;
        lblArtist.Text = string.IsNullOrWhiteSpace(track.Album)
            ? track.DisplayArtist
            : $"{track.DisplayArtist} — {track.Album}";
        Text = $"Sepetify — {track.Title}";

        picArtwork.Image?.Dispose();
        picArtwork.Image = track.LoadArtwork();

        lblCurrent.Text = "0:00";
        lblTotal.Text = Track.FormatTime(_player.Duration);
        trkSeek.Value = 0;
        trkSeek.Enabled = true;
        UpdatePlayButton();
        RefreshList();
    }

    private void OnTrackEnded()
    {
        if (_repeat == RepeatMode.One && _currentIndex >= 0)
        {
            PlayTrack(_currentIndex);
            return;
        }

        var next = NextIndex(manual: false);
        if (next is null)
        {
            _player.Stop();
            trkSeek.Value = 0;
            lblCurrent.Text = "0:00";
            UpdatePlayButton();
            return;
        }
        PlayTrack(next.Value);
    }

    /// <summary>Returns the index that should follow the current track, or null at the end of the playlist.</summary>
    private int? NextIndex(bool manual)
    {
        if (_tracks.Count == 0) return null;
        if (_currentIndex < 0) return 0;

        if (_shuffle)
        {
            if (_tracks.Count == 1) return manual ? 0 : null;
            int pick;
            do { pick = Random.Shared.Next(_tracks.Count); } while (pick == _currentIndex);
            return pick;
        }

        int next = _currentIndex + 1;
        if (next < _tracks.Count) return next;
        // Natural end with repeat off stops; manual "next" wraps around.
        if (_repeat == RepeatMode.All || manual) return 0;
        return null;
    }

    private void btnPlay_Click(object? sender, EventArgs e) => TogglePlay();

    private void TogglePlay()
    {
        if (!_player.HasTrack)
        {
            int start = lstPlaylist.SelectedItems.Count > 0 && lstPlaylist.SelectedItems[0].Tag is Track t
                ? _tracks.IndexOf(t)
                : (_currentIndex >= 0 ? _currentIndex : 0);
            if (_tracks.Count > 0) PlayTrack(Math.Max(0, start));
            return;
        }

        _player.TogglePlayPause();
        UpdatePlayButton();
    }

    private void btnNext_Click(object? sender, EventArgs e)
    {
        var idx = NextIndex(manual: true);
        if (idx is not null) PlayTrack(idx.Value);
    }

    private void btnPrev_Click(object? sender, EventArgs e)
    {
        if (_tracks.Count == 0) return;
        // Like most players: restart the song if we're more than 3 seconds in.
        if (_player.HasTrack && _player.Position > TimeSpan.FromSeconds(3))
        {
            _player.Position = TimeSpan.Zero;
            return;
        }
        int prev = _currentIndex <= 0 ? 0 : _currentIndex - 1;
        PlayTrack(prev);
    }

    private void btnStop_Click(object? sender, EventArgs e)
    {
        _player.Stop();
        trkSeek.Value = 0;
        lblCurrent.Text = "0:00";
        UpdatePlayButton();
    }

    private void UpdatePlayButton() => btnPlay.Text = _player.IsPlaying ? "❚❚" : "▶";

    // ---------------- Seek / volume / timer ----------------

    private void tmrTick_Tick(object? sender, EventArgs e)
    {
        if (!_player.HasTrack) return;

        if (!_draggingSeek && _player.Duration > TimeSpan.Zero)
        {
            double ratio = _player.Position.TotalMilliseconds / _player.Duration.TotalMilliseconds;
            trkSeek.Value = (int)Math.Clamp(ratio * trkSeek.Maximum, 0, trkSeek.Maximum);
            lblCurrent.Text = Track.FormatTime(_player.Position);
        }
        UpdatePlayButton();
    }

    private void trkSeek_MouseDown(object? sender, MouseEventArgs e) => _draggingSeek = true;
    private void trkSeek_MouseUp(object? sender, MouseEventArgs e)
    {
        _draggingSeek = false;
        ApplySeek();
    }

    private void trkSeek_Scroll(object? sender, EventArgs e)
    {
        if (_player.Duration <= TimeSpan.Zero) return;
        var target = TimeSpan.FromMilliseconds(_player.Duration.TotalMilliseconds * trkSeek.Value / trkSeek.Maximum);
        lblCurrent.Text = Track.FormatTime(target);
        if (!_draggingSeek) ApplySeek();
    }

    private void ApplySeek()
    {
        if (_player.Duration <= TimeSpan.Zero) return;
        _player.Position = TimeSpan.FromMilliseconds(
            _player.Duration.TotalMilliseconds * trkSeek.Value / trkSeek.Maximum);
    }

    private void trkVolume_Scroll(object? sender, EventArgs e) => _player.Volume = trkVolume.Value / 100f;

    private void btnShuffle_Click(object? sender, EventArgs e)
    {
        _shuffle = !_shuffle;
        btnShuffle.ForeColor = _shuffle ? Color.FromArgb(30, 215, 96) : Color.FromArgb(150, 150, 158);
        btnShuffle.FlatAppearance.BorderColor = _shuffle
            ? Color.FromArgb(30, 215, 96)
            : Color.FromArgb(60, 60, 66);
    }

    private void btnRepeat_Click(object? sender, EventArgs e)
    {
        _repeat = _repeat switch
        {
            RepeatMode.Off => RepeatMode.All,
            RepeatMode.All => RepeatMode.One,
            _ => RepeatMode.Off
        };

        btnRepeat.Text = _repeat == RepeatMode.One ? "⟳1" : "⟳";
        btnRepeat.Width = _repeat == RepeatMode.One ? 44 : 38;
        btnRepeat.ForeColor = _repeat == RepeatMode.Off
            ? Color.FromArgb(150, 150, 158)
            : Color.FromArgb(30, 215, 96);
        btnRepeat.FlatAppearance.BorderColor = _repeat == RepeatMode.Off
            ? Color.FromArgb(60, 60, 66)
            : Color.FromArgb(30, 215, 96);
    }

    // ---------------- Keyboard ----------------

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        // Let the search box handle its own typing.
        if (txtSearch.Focused && e.KeyCode != Keys.Space) return;

        switch (e.KeyCode)
        {
            case Keys.Space:
                if (!txtSearch.Focused) { TogglePlay(); e.Handled = true; }
                break;
            case Keys.Right when e.Control:
                btnNext_Click(sender, EventArgs.Empty);
                e.Handled = true;
                break;
            case Keys.Left when e.Control:
                btnPrev_Click(sender, EventArgs.Empty);
                e.Handled = true;
                break;
            case Keys.Up when e.Control:
                trkVolume.Value = Math.Min(trkVolume.Maximum, trkVolume.Value + 5);
                _player.Volume = trkVolume.Value / 100f;
                e.Handled = true;
                break;
            case Keys.Down when e.Control:
                trkVolume.Value = Math.Max(trkVolume.Minimum, trkVolume.Value - 5);
                _player.Volume = trkVolume.Value / 100f;
                e.Handled = true;
                break;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        tmrTick.Stop();
        _player.Dispose();
        base.OnFormClosing(e);
    }
}

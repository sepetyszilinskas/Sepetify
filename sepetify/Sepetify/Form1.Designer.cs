namespace Sepetify;

partial class Form1
{
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        var bg = Color.FromArgb(18, 18, 20);
        var panelBg = Color.FromArgb(28, 28, 32);
        var accent = Color.FromArgb(30, 215, 96);
        var text = Color.FromArgb(235, 235, 235);
        var muted = Color.FromArgb(150, 150, 158);
        var buttonBg = Color.FromArgb(36, 36, 42);

        pnlSidebar = new Panel();
        pnlPlayer = new Panel();
        pnlDivider = new Panel();
        lstPlaylist = new ListView();
        btnRow = new FlowLayoutPanel();
        txtSearch = new TextBox();
        lblStatus = new Label();
        btnAddFiles = new Button();
        btnAddFolder = new Button();
        btnClear = new Button();
        picArtwork = new PictureBox();
        lblTitle = new Label();
        lblArtist = new Label();
        lblCurrent = new Label();
        lblTotal = new Label();
        lblVolume = new Label();
        trkSeek = new TrackBar();
        trkVolume = new TrackBar();
        btnPlay = new Button();
        btnPrev = new Button();
        btnNext = new Button();
        btnStop = new Button();
        btnShuffle = new Button();
        btnRepeat = new Button();
        dlgFiles = new OpenFileDialog();
        dlgFolder = new FolderBrowserDialog();
        tmrTick = new System.Windows.Forms.Timer(components);

        SuspendLayout();
        pnlSidebar.SuspendLayout();
        pnlPlayer.SuspendLayout();
        btnRow.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picArtwork).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trkSeek).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trkVolume).BeginInit();

        // ---------------- Form ----------------
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = bg;
        ForeColor = text;
        Font = new Font("Segoe UI", 9.5f);
        ClientSize = new Size(960, 620);
        MinimumSize = new Size(800, 560);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sepetify";

        // ---------------- Left: playlist ----------------
        pnlSidebar.Dock = DockStyle.Fill;
        pnlSidebar.BackColor = panelBg;
        pnlSidebar.Padding = new Padding(12, 12, 12, 8);

        lstPlaylist.Dock = DockStyle.Fill;
        lstPlaylist.View = View.Details;
        lstPlaylist.FullRowSelect = true;
        lstPlaylist.HideSelection = false;
        lstPlaylist.MultiSelect = false;
        lstPlaylist.BorderStyle = BorderStyle.None;
        lstPlaylist.BackColor = Color.FromArgb(22, 22, 26);
        lstPlaylist.ForeColor = text;
        lstPlaylist.Font = new Font("Segoe UI", 9.5f);
        lstPlaylist.Columns.Add("#", 36, HorizontalAlignment.Right);
        lstPlaylist.Columns.Add("Title", 300, HorizontalAlignment.Left);
        lstPlaylist.Columns.Add("Artist", 200, HorizontalAlignment.Left);
        lstPlaylist.Columns.Add("Time", 70, HorizontalAlignment.Right);
        lstPlaylist.DoubleClick += lstPlaylist_DoubleClick;

        txtSearch.Dock = DockStyle.Top;
        txtSearch.Height = 30;
        txtSearch.PlaceholderText = "Search title, artist or album...";
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.BackColor = Color.FromArgb(22, 22, 26);
        txtSearch.ForeColor = text;
        txtSearch.TextChanged += txtSearch_TextChanged;

        lblStatus.Dock = DockStyle.Bottom;
        lblStatus.Height = 22;
        lblStatus.Text = "No tracks loaded";
        lblStatus.ForeColor = muted;
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;

        btnRow.Dock = DockStyle.Bottom;
        btnRow.Height = 42;
        btnRow.FlowDirection = FlowDirection.LeftToRight;
        btnRow.WrapContents = false;
        btnRow.BackColor = panelBg;
        btnRow.Padding = new Padding(0, 8, 0, 0);

        btnAddFiles.Text = "Add files";
        btnAddFiles.Width = 82;
        btnAddFiles.Height = 30;
        btnAddFiles.Margin = new Padding(0, 0, 6, 0);
        btnAddFiles.FlatStyle = FlatStyle.Flat;
        btnAddFiles.FlatAppearance.BorderSize = 1;
        btnAddFiles.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 66);
        btnAddFiles.BackColor = buttonBg;
        btnAddFiles.ForeColor = text;
        btnAddFiles.Click += btnAddFiles_Click;

        btnAddFolder.Text = "Add folder";
        btnAddFolder.Width = 92;
        btnAddFolder.Height = 30;
        btnAddFolder.Margin = new Padding(0, 0, 6, 0);
        btnAddFolder.FlatStyle = FlatStyle.Flat;
        btnAddFolder.FlatAppearance.BorderSize = 1;
        btnAddFolder.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 66);
        btnAddFolder.BackColor = buttonBg;
        btnAddFolder.ForeColor = text;
        btnAddFolder.Click += btnAddFolder_Click;

        btnClear.Text = "Clear";
        btnClear.Width = 64;
        btnClear.Height = 30;
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.FlatAppearance.BorderSize = 1;
        btnClear.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 66);
        btnClear.BackColor = buttonBg;
        btnClear.ForeColor = text;
        btnClear.Click += btnClear_Click;

        btnRow.Controls.Add(btnAddFiles);
        btnRow.Controls.Add(btnAddFolder);
        btnRow.Controls.Add(btnClear);

        // Docked controls are laid out in reverse add order.
        pnlSidebar.Controls.Add(lstPlaylist);
        pnlSidebar.Controls.Add(txtSearch);
        pnlSidebar.Controls.Add(btnRow);
        pnlSidebar.Controls.Add(lblStatus);

        // ---------------- Right: now playing ----------------
        pnlPlayer.Dock = DockStyle.Right;
        pnlPlayer.Width = 340;
        pnlPlayer.BackColor = bg;
        pnlPlayer.Padding = new Padding(20);

        pnlDivider.Dock = DockStyle.Right;
        pnlDivider.Width = 1;
        pnlDivider.BackColor = Color.FromArgb(45, 45, 50);

        picArtwork.Location = new Point(20, 20);
        picArtwork.Size = new Size(300, 300);
        picArtwork.SizeMode = PictureBoxSizeMode.Zoom;
        picArtwork.BackColor = Color.FromArgb(26, 26, 30);

        lblTitle.Location = new Point(20, 332);
        lblTitle.Size = new Size(300, 32);
        lblTitle.Font = new Font("Segoe UI Semibold", 13f);
        lblTitle.ForeColor = text;
        lblTitle.AutoEllipsis = true;
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblTitle.Text = "Nothing playing";

        lblArtist.Location = new Point(20, 364);
        lblArtist.Size = new Size(300, 22);
        lblArtist.ForeColor = muted;
        lblArtist.AutoEllipsis = true;
        lblArtist.TextAlign = ContentAlignment.MiddleLeft;
        lblArtist.Text = "Add some music to get started";

        trkSeek.Location = new Point(16, 392);
        trkSeek.Width = 308;
        trkSeek.Height = 28;
        trkSeek.AutoSize = false;
        trkSeek.Minimum = 0;
        trkSeek.Maximum = 1000;
        trkSeek.TickStyle = TickStyle.None;
        trkSeek.SmallChange = 10;
        trkSeek.LargeChange = 100;
        trkSeek.Scroll += trkSeek_Scroll;
        trkSeek.MouseDown += trkSeek_MouseDown;
        trkSeek.MouseUp += trkSeek_MouseUp;

        lblCurrent.Location = new Point(20, 420);
        lblCurrent.Size = new Size(80, 18);
        lblCurrent.ForeColor = muted;
        lblCurrent.Font = new Font("Segoe UI", 8.5f);
        lblCurrent.Text = "0:00";

        lblTotal.Location = new Point(260, 420);
        lblTotal.Size = new Size(60, 18);
        lblTotal.ForeColor = muted;
        lblTotal.Font = new Font("Segoe UI", 8.5f);
        lblTotal.TextAlign = ContentAlignment.TopRight;
        lblTotal.Text = "0:00";

        btnStop.Location = new Point(20, 456);
        btnStop.Size = new Size(42, 42);
        btnStop.Text = "■";
        btnStop.Font = new Font("Segoe UI", 10f);
        btnStop.FlatStyle = FlatStyle.Flat;
        btnStop.FlatAppearance.BorderSize = 0;
        btnStop.BackColor = buttonBg;
        btnStop.ForeColor = text;
        btnStop.Cursor = Cursors.Hand;
        btnStop.TabStop = false;
        btnStop.Click += btnStop_Click;

        btnPrev.Location = new Point(80, 452);
        btnPrev.Size = new Size(50, 50);
        btnPrev.Text = "◀◀";
        btnPrev.Font = new Font("Segoe UI", 10f);
        btnPrev.FlatStyle = FlatStyle.Flat;
        btnPrev.FlatAppearance.BorderSize = 0;
        btnPrev.BackColor = buttonBg;
        btnPrev.ForeColor = text;
        btnPrev.Cursor = Cursors.Hand;
        btnPrev.TabStop = false;
        btnPrev.Click += btnPrev_Click;

        btnPlay.Location = new Point(146, 446);
        btnPlay.Size = new Size(62, 62);
        btnPlay.Text = "▶";
        btnPlay.Font = new Font("Segoe UI", 15f);
        btnPlay.FlatStyle = FlatStyle.Flat;
        btnPlay.FlatAppearance.BorderSize = 0;
        btnPlay.BackColor = accent;
        btnPlay.ForeColor = Color.Black;
        btnPlay.Cursor = Cursors.Hand;
        btnPlay.TabStop = false;
        btnPlay.Click += btnPlay_Click;

        btnNext.Location = new Point(224, 452);
        btnNext.Size = new Size(50, 50);
        btnNext.Text = "▶▶";
        btnNext.Font = new Font("Segoe UI", 10f);
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.FlatAppearance.BorderSize = 0;
        btnNext.BackColor = buttonBg;
        btnNext.ForeColor = text;
        btnNext.Cursor = Cursors.Hand;
        btnNext.TabStop = false;
        btnNext.Click += btnNext_Click;

        lblVolume.Location = new Point(20, 528);
        lblVolume.Size = new Size(40, 22);
        lblVolume.Text = "VOL";
        lblVolume.ForeColor = muted;
        lblVolume.Font = new Font("Segoe UI", 8.5f);
        lblVolume.TextAlign = ContentAlignment.MiddleLeft;

        trkVolume.Location = new Point(58, 522);
        trkVolume.Width = 160;
        trkVolume.Height = 28;
        trkVolume.AutoSize = false;
        trkVolume.Minimum = 0;
        trkVolume.Maximum = 100;
        trkVolume.Value = 80;
        trkVolume.TickStyle = TickStyle.None;
        trkVolume.Scroll += trkVolume_Scroll;

        btnShuffle.Location = new Point(226, 526);
        btnShuffle.Size = new Size(38, 30);
        btnShuffle.Text = "⤨";
        btnShuffle.FlatStyle = FlatStyle.Flat;
        btnShuffle.FlatAppearance.BorderSize = 1;
        btnShuffle.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 66);
        btnShuffle.BackColor = buttonBg;
        btnShuffle.ForeColor = muted;
        btnShuffle.Cursor = Cursors.Hand;
        btnShuffle.TabStop = false;
        btnShuffle.Click += btnShuffle_Click;

        btnRepeat.Location = new Point(272, 526);
        btnRepeat.Size = new Size(38, 30);
        btnRepeat.Text = "⟳";
        btnRepeat.FlatStyle = FlatStyle.Flat;
        btnRepeat.FlatAppearance.BorderSize = 1;
        btnRepeat.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 66);
        btnRepeat.BackColor = buttonBg;
        btnRepeat.ForeColor = muted;
        btnRepeat.Cursor = Cursors.Hand;
        btnRepeat.TabStop = false;
        btnRepeat.Click += btnRepeat_Click;

        pnlPlayer.Controls.Add(picArtwork);
        pnlPlayer.Controls.Add(lblTitle);
        pnlPlayer.Controls.Add(lblArtist);
        pnlPlayer.Controls.Add(trkSeek);
        pnlPlayer.Controls.Add(lblCurrent);
        pnlPlayer.Controls.Add(lblTotal);
        pnlPlayer.Controls.Add(btnStop);
        pnlPlayer.Controls.Add(btnPrev);
        pnlPlayer.Controls.Add(btnPlay);
        pnlPlayer.Controls.Add(btnNext);
        pnlPlayer.Controls.Add(lblVolume);
        pnlPlayer.Controls.Add(trkVolume);
        pnlPlayer.Controls.Add(btnShuffle);
        pnlPlayer.Controls.Add(btnRepeat);

        Controls.Add(pnlSidebar);
        Controls.Add(pnlDivider);
        Controls.Add(pnlPlayer);

        // ---------------- Dialogs / timer ----------------
        dlgFiles.Title = "Add music files";
        dlgFiles.Multiselect = true;
        dlgFiles.Filter = "Audio files|*.mp3;*.wav;*.aiff;*.aif;*.m4a;*.aac;*.wma;*.flac;*.ogg|All files|*.*";

        dlgFolder.Description = "Choose a folder containing music";
        dlgFolder.UseDescriptionForTitle = true;

        tmrTick.Interval = 250;
        tmrTick.Tick += tmrTick_Tick;

        ((System.ComponentModel.ISupportInitialize)picArtwork).EndInit();
        ((System.ComponentModel.ISupportInitialize)trkSeek).EndInit();
        ((System.ComponentModel.ISupportInitialize)trkVolume).EndInit();
        btnRow.ResumeLayout(false);
        pnlPlayer.ResumeLayout(false);
        pnlSidebar.ResumeLayout(false);
        pnlSidebar.PerformLayout();
        ResumeLayout(false);
    }
}

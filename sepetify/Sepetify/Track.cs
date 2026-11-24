using System.Drawing;
using TagLib;

namespace Sepetify;

/// <summary>
/// A single song: its file path plus tags read from the file.
/// </summary>
public sealed class Track
{
    public string Path { get; }
    public string Title { get; private set; } = "";
    public string Artist { get; private set; } = "";
    public string Album { get; private set; } = "";
    public TimeSpan Duration { get; private set; }
    public byte[]? Artwork { get; private set; }

    private Track(string path) => Path = path;

    public string FileName => System.IO.Path.GetFileName(Path);
    public string DisplayArtist => string.IsNullOrWhiteSpace(Artist) ? "Unknown artist" : Artist;

    /// <summary>File types the player tries to open.</summary>
    public static readonly string[] SupportedExtensions =
        { ".mp3", ".wav", ".aiff", ".aif", ".m4a", ".aac", ".wma", ".flac", ".ogg" };

    public static bool IsSupported(string path) =>
        SupportedExtensions.Contains(System.IO.Path.GetExtension(path).ToLowerInvariant());

    /// <summary>Reads tags; falls back to the file name when the file has none.</summary>
    public static Track Load(string path)
    {
        var track = new Track(path);

        try
        {
            using var file = TagLib.File.Create(path);
            if (!string.IsNullOrWhiteSpace(file.Tag.Title)) track.Title = file.Tag.Title.Trim();
            if (!string.IsNullOrWhiteSpace(file.Tag.FirstPerformer)) track.Artist = file.Tag.FirstPerformer.Trim();
            if (!string.IsNullOrWhiteSpace(file.Tag.Album)) track.Album = file.Tag.Album.Trim();
            if (file.Properties?.Duration is TimeSpan d && d > TimeSpan.Zero) track.Duration = d;

            var picture = file.Tag.Pictures?.FirstOrDefault();
            if (picture is { Data.Count: > 0 })
            {
                track.Artwork = picture.Data.ToArray();
            }
        }
        catch
        {
            // Unreadable tags are not fatal; the file may still play.
        }

        if (string.IsNullOrWhiteSpace(track.Title))
        {
            track.Title = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        return track;
    }

    public Bitmap? LoadArtwork()
    {
        if (Artwork is null) return null;
        using var ms = new MemoryStream(Artwork);
        using var image = Image.FromStream(ms);
        return new Bitmap(image);
    }

    public static string FormatTime(TimeSpan time)
    {
        if (time < TimeSpan.Zero) time = TimeSpan.Zero;
        return time.TotalHours >= 1
            ? $"{(int)time.TotalHours}:{time.Minutes:00}:{time.Seconds:00}"
            : $"{time.Minutes}:{time.Seconds:00}";
    }
}

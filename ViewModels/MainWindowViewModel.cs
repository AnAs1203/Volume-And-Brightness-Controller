using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace VolumeBrightnessctl.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private double _volume = 23;
    [ObservableProperty]
    private double _brightness = 40;
    public bool Muted = false;
    public string VolumeText => $"Volume: {Volume:F0}%";
    public string BrightnessText => $"Brightness: {Brightness:F0}%";

    partial void OnVolumeChanged(double value)
    {
        OnPropertyChanged(nameof(VolumeText));
    }

    partial void OnBrightnessChanged(double value)
    {
        OnPropertyChanged(nameof(BrightnessText));
    }

    public static void runCommand(string argument)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{argument}\"",
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };
        using var process = Process.Start(processInfo);
        process?.WaitForExit();
    }

    public void MuteVolume()
    {
        runCommand($"pactl -- set-sink-mute 0 toggle");
    }

    public void SetVolume(double volume)
    {
        runCommand($"pactl -- set-sink-volume 0 {volume:F0}%");
    }

    public void SetBrightness(double brightness)
    {
        runCommand($"brightnessctl set {brightness}%");
    }

    public void NextMedia()
    {
        runCommand($"playerctl next");
    }

    public void PauseMedia()
    {
        runCommand($"playerctl play-pause");
    }

    public void PreviousMedia()
    {
        runCommand($"playerctl previous");
    }
}

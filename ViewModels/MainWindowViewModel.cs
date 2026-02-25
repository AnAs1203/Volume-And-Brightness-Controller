using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System;

namespace VolumeBrightnessctl.ViewModels;

public class Fetcher
{
    private readonly MainWindowViewModel _main;

    public Fetcher(MainWindowViewModel main)
    {
        _main = main;
    }

    public static ProcessStartInfo makeProcessInfo(string argument)
    {
        ProcessStartInfo processInfoToReturn = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{argument}\"",
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };
        return processInfoToReturn;
    }
    private static string? CatchOutput(ProcessStartInfo processInfo)
    {
        using var process = Process.Start(processInfo);
        var line = process?.StandardOutput.ReadToEnd();
        process?.WaitForExit();
        return line;
    }

    public static int FetchBrightness()
    {
        var line = CatchOutput(makeProcessInfo("brightnessctl"));
        try
        {
            var number = line.Split('(')[1].Split('%')[0];
            return int.Parse(number);
        }
        catch (System.NullReferenceException)
        {
            Console.WriteLine("Could not get current volume");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured: {ex.Message}");
        }
        return 0;
    }

    public static int FetchVolume()
    {
        var line = CatchOutput(makeProcessInfo("pactl -- get-sink-volume 0"));
        try
        {
            var number = line.Split('/')[1].Trim().Split('%')[0];
            return int.Parse(number);
        }
        catch (System.NullReferenceException)
        {
            Console.WriteLine("Could not get current volume");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured: {ex.Message}");
        }
        return 0;
    }
}

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private double _volume = Fetcher.FetchVolume();
    [ObservableProperty]
    private double _brightness = Fetcher.FetchBrightness();
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

    public void ExitApp()
    {
        Environment.Exit(0);
    }
}

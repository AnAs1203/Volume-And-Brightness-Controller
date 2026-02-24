using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using VolumeBrightnessctl.ViewModels;
using Avalonia.Interactivity;

namespace VolumeBrightnessctl.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        VolumeSlider.ValueChanged += OnVolumeSliderChanged;
        BrightnessSlider.ValueChanged += OnBrightnessSliderChanged;
    }

    private void OnVolumeSliderChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.SetVolume(e.NewValue);
        }
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.MuteVolume();
        }
    }

    private void OnButtonPrevious(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.PreviousMedia();
        }
    }

    private void OnButtonPause(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.PauseMedia();
        }
    }

    private void OnButtonNext(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.NextMedia();
        }
    }

    private void OnBrightnessSliderChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.SetBrightness(e.NewValue);
        }
    }
}

using CareerTracker.App.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using System.Diagnostics;

namespace CareerTracker.App.Views;

public abstract class BasePage<TViewModel>(TViewModel viewModel, bool shouldUseSafeArea = true) : BasePage(viewModel, shouldUseSafeArea)
    where TViewModel : ObservableObject
{
    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BasePage : ContentPage
{
    protected BasePage(object? viewModel = null, bool shouldUseSafeArea = true)
    {
        BindingContext = viewModel;

        On<iOS>().SetUseSafeArea(shouldUseSafeArea);

        if (string.IsNullOrWhiteSpace(Title)) Title = GetType().Name;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Debug.WriteLine($"OnAppearing: {Title}");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is IDisposable d) d.Dispose();
        Debug.WriteLine($"OnDisappearing: {Title}");
    }

    protected static async Task ProcessAction(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(ResourceHelper.GetText("ErrorTitle"),
                $" {ex.Message}", ResourceHelper.GetText("BtnOk"));
        }
    }
}
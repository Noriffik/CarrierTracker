using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace CareerTracker.App.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty] private string _login = string.Empty;
    [ObservableProperty] private string _password = string.Empty;

    protected LoginViewModel(ILogger logger) : base(logger)
    {
    }

    [RelayCommand]
    public override Task InitializeAsync()
    {
        return base.InitializeAsync();
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            await Task.CompletedTask;
        }
        catch(Exception ex) {
            Logger.LogError("Ошибка авторизации", ex);
        }
        finally
        {
            IsLoading = false;
        }
    }
}

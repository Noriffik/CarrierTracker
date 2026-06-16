using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CareerTracker.App.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty] private string _login = string.Empty;
    [ObservableProperty] private string _password = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        
    }
}
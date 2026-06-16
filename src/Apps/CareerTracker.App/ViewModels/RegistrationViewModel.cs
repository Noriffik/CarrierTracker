using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace CareerTracker.App.ViewModels;

public partial class RegistrationViewModel : ViewModelBase
{
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _confirmPassword = string.Empty;

    protected RegistrationViewModel(ILogger logger) : base(logger)
    {
    }
}

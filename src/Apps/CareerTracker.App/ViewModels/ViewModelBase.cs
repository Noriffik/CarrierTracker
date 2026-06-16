using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CareerTracker.App.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private bool _isInitialized;
    [ObservableProperty] private bool _isLoading = true;

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

}

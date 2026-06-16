using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace CareerTracker.App.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected ILogger Logger;
    private long _isBusy;


    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private bool _isInitialized;
    [ObservableProperty] private bool _isLoading = true;

    protected ViewModelBase(ILogger logger)
    {
        Logger = logger;

        InitializeAsyncCommand = new AsyncRelayCommand(
            async () =>
            {
                await IsBusyFor(InitializeAsync);
                IsInitialized = true;
            },
            options: AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
    }


    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;

    protected async Task IsBusyFor(Func<Task> unitOfWork)
    {
        if (unitOfWork is null) throw new ArgumentNullException(nameof(unitOfWork));

        try
        {
            Interlocked.Increment(ref _isBusy);
            OnPropertyChanged(nameof(IsBusy));
            IsLoading = true;

            await unitOfWork();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка в IsBusyFor: {ex.Message}");
            throw;
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            OnPropertyChanged(nameof(IsBusy));
            IsLoading = false;
        }
    }
}

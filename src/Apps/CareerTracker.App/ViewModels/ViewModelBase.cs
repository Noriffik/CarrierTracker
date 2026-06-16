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

    #region Navigation

    public virtual void NavigatedFrom() { }

    public virtual void NavigatedTo() { }

    public virtual void NavigatingFrom() { }

    public virtual bool BackButtonPressed() => false;

    public virtual void Initialize() { }

    public virtual void Appearing()
    {
        if (this is MainViewModel)
        {
            Shell.Current.BindingContext = this;
        }
        else
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        }

        if (!string.IsNullOrEmpty(Title))
        {
            var eventName = $"{Title.ToLower().Replace(" ", "_")}_page_appearing";
            Logger.LogEvent(eventName);
        }
    }

    public virtual void Disappearing() { }

    protected Task GoBackAsync(IDictionary<string, object> parameters = null) => GoToAsync("..", parameters);

    protected Task GoToAsync<TViewModel>(IDictionary<string, object> parameters = null, bool animate = true, bool isRoot = false)
        where TViewModel : BaseViewModel => GoToAsync(typeof(TViewModel).Name, parameters, animate, isRoot);

    protected async Task GoToAsync(IEnumerable<Type> fullRoute, IDictionary<string, object> parameters = null, bool animate = true, bool isRoot = false)
    {
        try
        {
            IsBusy = true;
            if (fullRoute == null || !fullRoute.Any()) return;
            if (fullRoute.Any(partialUri => !partialUri.IsAssignableFrom(typeof(BaseViewModel)))) return;

            var navigationUri = string.Join("/", fullRoute.Select(partialUri => partialUri.Name));
            await GoToAsync(navigationUri, parameters, animate, isRoot);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task GoToAsync(string navigationUri, IDictionary<string, object> parameters = null, bool animate = true, bool isRoot = false)
    {
        try
        {
            IsBusy = true;
            var finalNavigationUri = $"{(isRoot ? "//" : string.Empty)}{navigationUri}";

            if (parameters != null)
            {
                await Shell.Current.GoToAsync(navigationUri, animate, parameters);
            }
            else
            {
                await Shell.Current.GoToAsync(navigationUri, animate);
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [ICommand]
    private Task GoBack() => GoBackAsync();

    [ICommand]
    private async Task GoToAsync(Type type)
    {
        if (Shell.Current.FlyoutIsPresented) Shell.Current.FlyoutIsPresented = false;
        await GoToAsync(type.Name);
    }

    [ICommand]
    private void ToggleMenu()
    {
        Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
    }

    #endregion Navigation
}

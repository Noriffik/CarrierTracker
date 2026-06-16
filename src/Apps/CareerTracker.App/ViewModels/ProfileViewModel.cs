using Microsoft.Extensions.Logging;

namespace CareerTracker.App.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    protected ProfileViewModel(ILogger logger) : base(logger)
    {
    }
}
using CareerTracker.App.ViewModels;
using CareerTracker.App.Views;
using HorusStudio.Maui.MaterialDesignControls;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace CareerTracker.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMaterialDesignControls(options =>
            {
                options.EnableDebug();

                options.OnException((sender, exception) =>
                {
                    System.Diagnostics.Debug.WriteLine($"EXCEPTION ON LIBRARY: {sender} - {exception}");
                });
                options.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                    fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                }, new("RobotoRegular", "RobotoMedium", "RobotoRegular"));
            })
            .UseMauiCompatibility()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services
                .AutoConfigureViewModelsAndPages();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    static IServiceCollection AutoConfigureViewModelsAndPages(this IServiceCollection services)
    {
        var vmTypes = GetViewModelsToRegister();
        foreach (var vm in vmTypes)
        {
            services.AddTransient(vm);
        }

        var pageTypes = GetPagesToRegister(vmTypes);
        foreach (var page in pageTypes)
        {
            services.AddTransient(page);
        }

        return services;
    }

    public static IEnumerable<Type> GetViewModelsToRegister()
    {
        // Get ViewModel types that satisfy MyViewModel:BaseViewModel
        var currentNs = typeof(MauiProgram).Namespace;
        var types = typeof(MauiProgram).Assembly.GetTypes();
        var viewModels = types.Where(t => t.Namespace == $"{currentNs}.ViewModels" && t.BaseType == typeof(ViewModelBase));

        return viewModels;
    }

    public static IEnumerable<Type> GetPagesToRegister(IEnumerable<Type> viewModelTypes)
    {
        // Get ContentPage types that satisfy MyPage:BaseContentPage<MyPage>
        var currentNs = typeof(MauiProgram).Namespace;
        var types = typeof(MauiProgram).Assembly.GetTypes();
        var pages = types.Where(t => t.Namespace == $"{currentNs}.Pages" &&
            t.BaseType.Name.StartsWith(typeof(BasePage<>).Name) &&
            viewModelTypes.Any(vm => t.BaseType.FullName.Contains(vm.FullName)));

        return pages;
    }
}

using HorusStudio.Maui.MaterialDesignControls;
namespace CareerTracker.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MaterialDesignControls.InitializeComponents();

        MainPage = new AppShell();
    }
}

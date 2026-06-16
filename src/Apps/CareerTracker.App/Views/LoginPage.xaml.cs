using CareerTracker.App.ViewModels;

namespace CareerTracker.App.Views;

public partial class LoginPage : BasePage<LoginViewModel>
{
	public LoginPage(LoginViewModel vm) : base(vm)
	{
		InitializeComponent();
	}
}
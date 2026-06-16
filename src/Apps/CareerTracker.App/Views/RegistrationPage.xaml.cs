using CareerTracker.App.ViewModels;

namespace CareerTracker.App.Views;

public partial class RegistrationPage : BasePage<RegistrationViewModel>
{
	public RegistrationPage(RegistrationViewModel vm) : base(vm)
	{
		InitializeComponent();
	}
}
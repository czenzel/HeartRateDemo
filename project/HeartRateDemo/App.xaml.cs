using HeartRateDemo.Services;

namespace HeartRateDemo;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}

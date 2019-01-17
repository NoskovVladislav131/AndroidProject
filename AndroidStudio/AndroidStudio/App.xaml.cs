using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MasterDetailMenuV2
{
	public partial class App : Application
	{

        public static MasterDetailPage MasterDetail { get; set; }

        public async static Task NavigateMasterDetail(Page page)
        {
            App.MasterDetail.IsPresented = false;
            await App.MasterDetail.Detail.Navigation.PushAsync(page);
        }

        public App ()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

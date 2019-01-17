using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailMenuV2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Master : ContentPage
	{
        public Master()
        {
            InitializeComponent();

            ButtonNews.Clicked += async (sender, e) =>
            {
                await App.NavigateMasterDetail(new News());
            };

            

            ButtonPrice.Clicked += async (sender, e) => 
            {
                await App.NavigateMasterDetail(new Price());
            };

            ButtonIdeas.Clicked += async (sender, e) =>
            {
                await App.NavigateMasterDetail(new Ideas());
            };
                
           
}
	}
}
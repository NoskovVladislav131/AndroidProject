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

    public class PriceClass
    {
        public string Date { get; set; }
        public string Price { get; set; }
        //public string HrefNews { get; set; }
    }

    

    public partial class Price : ContentPage
	{
        //Создание экземпляра кнопки
        public Button headerbutton = new Button
        {
            Text = "Увеличить"
        };

        public int schstring;

        public List<PriceClass> PriceList { get; set; }

        public Price ()
		{
			InitializeComponent ();
            PriceList = new List<PriceClass> { };
            // растягиваем на два столбца
            //Content = grid;
            ListAddItems(PriceList);
            ListView(PriceList);
            //Обработка нажатия на кнопку
            headerbutton.Clicked += (sender, args) =>
            {
                ListAddItems(PriceList);
                ListView(PriceList);
            };
        }
        
        public List<PriceClass> ListAddItems(List<PriceClass> PriceList)
        {
            schstring++;
            PriceList.Add(
                new PriceClass() { Date = "Galaxy S8", Price = schstring .ToString()}
            );
            return PriceList;
        }

        public List<PriceClass> ListView(List<PriceClass> PriceList)
        {

            //Заполняем новый экземпляр ListViev
            ListView listView = new ListView
            {
                
                HasUnevenRows = true,
                ItemsSource = PriceList,
                SeparatorColor = Color.Red, //Цвет разделителя строк
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid
                    {
                        RowDefinitions =
                        {                            
                            new RowDefinition { Height = new GridLength(1,GridUnitType.Star) }
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(4,GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(3,GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(2,GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) }
                        }
                    };
                    //BoxView redBox = new BoxView { Color = Color.Red };
                    //BoxView blueBox = new BoxView { Color = Color.Blue };
                    //BoxView yellowBox = new BoxView { Color = Color.Yellow };
                    Label titlelabel1 = new Label { FontSize = 18, HorizontalOptions = LayoutOptions.Center };
                    titlelabel1.SetBinding(Label.TextProperty, "Date");
                    Label titlelabel2 = new Label { FontSize = 18, HorizontalOptions = LayoutOptions.Center };
                    titlelabel2.SetBinding(Label.TextProperty, "Price");
                    Label titlelabel3 = new Label { FontSize = 18, HorizontalOptions = LayoutOptions.Center , Text="%"};
                    //Image img = new Image { Source = "arrowdown.png" , Aspect = Aspect.AspectFill };
                    BoxView redBox = new BoxView { Color = Color.Red };
                    grid.Children.Add(titlelabel1, 0, 0);                    
                    grid.Children.Add(titlelabel2, 1, 0);
                    grid.Children.Add(titlelabel3, 2, 0);
                    grid.Children.Add(redBox, 3, 0);
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { grid }
                            //Children = { titlelabel1,companyLabel }//, priceLabel 
                        }
                    };
                })
            };
            //Контент страницы
            this.Content = new StackLayout { Children = { headerbutton, listView } };
            return PriceList;
        }
    }
}
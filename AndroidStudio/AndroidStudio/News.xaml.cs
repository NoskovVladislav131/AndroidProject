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

    public class NewsClass
    {
        public string Date { get; set; }
        public string TextNews { get; set; }
        public string HrefNews { get; set; }
    }

    public partial class News : ContentPage
	{

        public List<NewsClass> NewsList { get; set; }
        //Отображаемый Лист
        public List<NewsClass> NewsListViev { get; set; }
        //Массив из Json запроса
        string[,] NewsMass;
        //Таймер
        //Button timerButton;
        //Пока alive=true крутим таймер
        bool alive = true;

        public News ()
		{
			InitializeComponent ();
            //Инициализируем Лист новостей с нулевым значением
            NewsList = new List<NewsClass>
            {            };
            //Выводим его на печать
            NewsList = ListView(NewsList);
            //Добавляем к нулевому значению листа новую строку
            //NewsList = ListAddItems(NewsList);
            //Выводим её на печать
            //NewsList = ListView(NewsList);
            //Таймер, который выводит на экран информацию
            Device.StartTimer(TimeSpan.FromSeconds(20), callback: OnTimerTick);
            //Запуск таймера, который будет опрашивать базу каждыйе 60 секунд
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                //Асинхронная функция таймера по получения массива Json
                Task.Run(async () =>
                {
                    //Создаем экземпляр класса Json
                    JobJson JobJson = new JobJson();
                    //Придаем массиву значение из запроса к базе данных
                    NewsMass = await JobJson.JsonMassNewsReed(5);
                });
                return true;
            });

        }
        //Процедура по таймеру, которая выводит NewsList на экран 
        private bool OnTimerTick()
        {
            //добавляем новые записи из массив, полученного из Json запроса к базе, к уже сформированному NewsList
            NewsList = MassNewsToList(NewsList, NewsMass);
            //Выводим на экран новый лист
            NewsList = ListView(NewsList);
            return alive;
        }
        //Преобразовываем массив новостей в Лист
        private List<NewsClass> MassNewsToList(List<NewsClass> NewsList , string [,] MassNews)
        {
            NewsClass NewsListFirst = new NewsClass();
            if (NewsList.Count==0) {
                int i = 0;
                while (MassNews[i, 0]!= null)
                {
                    NewsList.Add(
                    new NewsClass() {
                        Date = MassNews[i, 0],
                        TextNews = MassNews[i, 1],
                        HrefNews = MassNews[i, 2] }
                    );
                    i++;
                }
                return NewsList;
            }
            else {
                NewsListFirst = NewsList.First();
                int i = 0;
                while ((NewsListFirst.TextNews != MassNews[i, 1])&&(MassNews[i, 1]!=null))
                {
                    NewsList.Insert(0,
                    new NewsClass() { Date = MassNews[i, 0], TextNews = MassNews[i, 1], HrefNews = MassNews[i, 2] });
                    i++;
                }
                return NewsList;
            }
            //NewsListFirst = NewsList.First();
            //int i = 0;
            //int j = 0;
            //while (NewsListFirst.TextNews != MassNews[i,j])
            //{
            //   NewsList.Add(
            //   new NewsClass() { Date = MassNews[i,0], TextNews = MassNews[i, 1], HrefNews = MassNews[i, 2] });
            //    i++;
            //}
            //return NewsList;
        }

        //Добавление строки в  Лист новостей для проверки
        public List<NewsClass> ListAddItems(List<NewsClass> NewsList)
        {
            NewsList.Add(
                new NewsClass() { Date = "Galaxy S8", TextNews = "Samsung8", HrefNews = "48000" }
                );
            return NewsList;
        }
        //Вывод Списка новостей на экран
        public List<NewsClass> ListView(List<NewsClass> NewsList)
        {
            //Отрисовываем шапку
            Label header = new Label
            {
                Text = "Список новостей",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            NewsListViev = NewsList;
            //NewsListViev = ListAddItems(NewsList);
            //Выводим на экран NewsList
            ListView listView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = NewsListViev,
                SeparatorColor = Color.Red, //Цвет разделителя строк
                ItemTemplate = new DataTemplate(() =>
                {
                    Label titlelabel1 = new Label { FontSize = 18 , HorizontalOptions = LayoutOptions.Center };
                    titlelabel1.SetBinding(Label.TextProperty, "Date");

                    Label companyLabel = new Label ();
                    companyLabel.SetBinding(Label.TextProperty, "TextNews");

                    //Label priceLabel = new Label();
                    //priceLabel.SetBinding(Label.TextProperty, "HrefNews");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            Children = { titlelabel1, companyLabel }//, priceLabel 
                        }
                    };
                })
            };
            this.Content = new StackLayout { Children = { header, listView } };
            return NewsList;
        }

        
    }
}
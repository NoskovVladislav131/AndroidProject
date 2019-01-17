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

    public class IdeasClass
    {
        public string Date { get; set; }
        public string IdeasText { get; set; }
    }

    public partial class Ideas : ContentPage
	{
        public List<IdeasClass> IdeasList { get; set; }
        //Отображаемый Лист
        public List<IdeasClass> IdeasListViev { get; set; }
        //Массив из Json запроса
        string[,] IdeasMass;
        //Таймер
        //Button timerButton;
        //Пока alive=true крутим таймер
        bool alive = true;

        public Ideas()
        {
            InitializeComponent();
            //Инициализируем Лист новостей с нулевым значением
            IdeasList = new List<IdeasClass>
            { };
            //Выводим его на печать
            IdeasList = ListView(IdeasList);
            Device.StartTimer(TimeSpan.FromSeconds(350), callback: OnTimerTick);
            //Запуск таймера, который будет опрашивать базу каждыйе 60 секунд
            Device.StartTimer(TimeSpan.FromSeconds(335), () =>
            {
                //Асинхронная функция таймера по получения массива Json
                Task.Run(async () =>
                {
                    //Создаем экземпляр класса Json
                    JobJson JobJson = new JobJson();
                    //Придаем массиву значение из запроса к базе данных
                    IdeasMass = await JobJson.JsonMassIdeasReed(10);
                });
                return true;
            });

        }
        //Процедура по таймеру, которая выводит IdeasList на экран 
        private bool OnTimerTick()
        {
            //добавляем новые записи из массив, полученного из Json запроса к базе, к уже сформированному IdeasList
            IdeasList = MassIdeasToList(IdeasList, IdeasMass);
            //Выводим на экран новый лист
            IdeasList = ListView(IdeasList);
            return alive;
        }
        //Преобразовываем массив новостей в Лист
        private List<IdeasClass> MassIdeasToList(List<IdeasClass> IdeasList, string[,] IdeasMass)
        {
            IdeasClass IdeasListFirst = new IdeasClass();
            if (IdeasList.Count == 0)
            {
                int i = 0;
                while (IdeasMass[i, 0] != null)
                {
                    IdeasList.Add(
                    new IdeasClass()
                    {
                        Date = IdeasMass[i, 0],
                        IdeasText = IdeasMass[i, 1],
                    }
                    );
                    i++;
                }
                return IdeasList;
            }
            else
            {
                IdeasListFirst = IdeasList.First();
                int i = 0;
                while ((IdeasListFirst.IdeasText != IdeasMass[i, 1]) && (IdeasMass[i, 1] != null))
                {
                    IdeasList.Insert(0,
                    new IdeasClass() { Date = IdeasMass[i, 0], IdeasText = IdeasMass[i, 1]});
                    i++;
                }
                return IdeasList;
            }
            //IdeasListFirst = IdeasList.First();
            //int i = 0;
            //int j = 0;
            //while (IdeasListFirst.TextIdeas != MassIdeas[i,j])
            //{
            //   IdeasList.Add(
            //   new IdeasClass() { Date = MassIdeas[i,0], TextIdeas = MassIdeas[i, 1], HrefIdeas = MassIdeas[i, 2] });
            //    i++;
            //}
            //return IdeasList;
        }

        //Добавление строки в  Лист новостей для проверки
        public List<IdeasClass> ListAddItems(List<IdeasClass> IdeasList)
        {
            IdeasList.Add(
                new IdeasClass() { Date = "Galaxy S8", IdeasText = "Samsung8"}
                );
            return IdeasList;
        }
        //Вывод Списка новостей на экран
        public List<IdeasClass> ListView(List<IdeasClass> IdeasList)
        {
            //Отрисовываем шапку
            Label header = new Label
            {
                Text = "Список идей",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            IdeasListViev = IdeasList;
            //IdeasListViev = ListAddItems(IdeasList);
            //Выводим на экран IdeasList
            ListView listView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = IdeasListViev,
                SeparatorColor = Color.Red, //Цвет разделителя строк
                ItemTemplate = new DataTemplate(() =>
                {
                    Label titlelabel1 = new Label { FontSize = 18, HorizontalOptions = LayoutOptions.Center };
                    titlelabel1.SetBinding(Label.TextProperty, "Date");

                    Label companyLabel = new Label();
                    companyLabel.SetBinding(Label.TextProperty, "IdeasText");

                    //Label priceLabel = new Label();
                    //priceLabel.SetBinding(Label.TextProperty, "HrefIdeas");

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
            return IdeasList;
        }
    }
}
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using Newtonsoft;
using Newtonsoft.Json;
using System.Net.Http;

namespace MasterDetailMenuV2
{
    class JobJson
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string NewsShow = "https://quicksitebyaleksandr.000webhostapp.com/NewsShow";
        private readonly string IdeasShow = "https://quicksitebyaleksandr.000webhostapp.com/IdeasShow";
        private string[,] JsonMass = new string[1000,20];
        private int i, j = 0;



        //Отправляем запрос на сервер и получаем ответ
        public async Task<string[,]> JsonMassNewsReed (int NewsSCH)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"NewsCNT" , NewsSCH.ToString() },
            };

            FormUrlEncodedContent form = new FormUrlEncodedContent(dict);
            HttpResponseMessage response = await client.PostAsync(NewsShow, form);
            string result = await response.Content.ReadAsStringAsync();
            //await DisplayAlert("Результат", result , "Ok");
            JsonMass = ReedResultToMass(result, JsonMass);
            return JsonMass;
        }

        //Отправляем запрос на сервер и получаем ответ 
        public async Task<string[,]> JsonMassIdeasReed(int IdeasSCH)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"IdeasCNT" , IdeasSCH.ToString() },
            };

            FormUrlEncodedContent form = new FormUrlEncodedContent(dict);
            HttpResponseMessage response = await client.PostAsync(IdeasShow, form);
            string result = await response.Content.ReadAsStringAsync();
            //await DisplayAlert("Результат", result , "Ok");
            JsonMass = ReedResultToMass(result, JsonMass);
            return JsonMass;
        }

        private string[,] ReedResultToMass(string result, string[,] JsonMassReed)
        {
            int q = 0;
            char Stopresult = '~';
            char Stopresult1 = '$';
            string StrOld = "";
            //i = 0;
            while (result.Length - 1 != q)
            {
                q++;
                if (result[q] == Stopresult)
                {
                    JsonMassReed[i, j] = StrOld;
                    StrOld = "";
                    i++;
                    j = 0 ;
                }
                else if (result[q] == Stopresult1)
                {
                    JsonMassReed[i, j] = StrOld;
                    StrOld = "";
                    j++;
                }
                else
                {
                    StrOld += result[q];
                }
            };
            return JsonMassReed;
        }

       


    }
}

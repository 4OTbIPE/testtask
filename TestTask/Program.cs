using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestTask
{
    class Program
    {
        static Root forecast { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Получение данных о погоде в городе Уфа...");
            try
            {
                GetData().Wait();
                Console.WriteLine("Данные получены!");
                GetMaxDaytime();
                GetTemperatureDiff();
            }
            catch
            {
                Console.WriteLine("Не удалось получить данные((");
            }
            Console.ReadLine();
        }


        public static async Task GetData()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://community-open-weather-map.p.rapidapi.com/forecast/daily?q=ufa%2Cru&cnt=10&units=metric&lang=ru"),
                Headers =
    {
        { "x-rapidapi-host", "community-open-weather-map.p.rapidapi.com" },
        { "x-rapidapi-key", "74f061f984msh2b8d80baf8b920bp193125jsn2637a1ac5ae5" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                forecast = JsonConvert.DeserializeObject<Root>(body);
            }
        }

        private static DateTime GetTime(int stamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds((double)stamp).ToLocalTime();
        }

        private static void GetMaxDaytime()
        {
            var maxDaytime = forecast.list.Take(5).Max((x) => x.sunset - x.sunrise);
            var maxDaytimeDate = forecast.list.Take(5).Where((x) => (x.sunset - x.sunrise) == maxDaytime).First().dt;
            Console.WriteLine("Самый длинный день из ближайших пяти: " + GetTime(maxDaytimeDate).ToString("dd.MM.yy") + " длится: " + GetTime(maxDaytime).ToString("HH:mm"));
        }

        private static void GetTemperatureDiff()
        {
            var minTempDiff = forecast.list.Min((x) => Math.Abs(x.feels_like.night - x.temp.night));
            var minTempDiffDate = forecast.list.Where((x) => Math.Abs(x.feels_like.night - x.temp.night) == minTempDiff).First().dt;
            Console.WriteLine("Мин. разница ощущаемой и реальной темп.ночью составляет: " + minTempDiff + "°С - " + GetTime(minTempDiffDate).ToString("dd.MM.yy") + " числа");
        }
    }
}

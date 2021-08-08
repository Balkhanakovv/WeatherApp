using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Forecast weather;

        public Forecast Weather
        {
            get { return weather; }
            set
            {
                weather = value;
                OnPropertyChanged("Weather");
            }
        }

        private Command berdsk;

        public Command Berdsk
        {
            get
            {
                return berdsk ??
                    (berdsk = new Command(obj =>
                    {
                        url = "https://api.openweathermap.org/data/2.5/weather?q=Berdsk&units=metric&appid=777c0b35616a7832e4d103e06717114c";
                        GetWeatherInfoAsync();
                    }));
            }
        }

        private Command novosibirsk;

        public Command Novosibirsk
        {
            get
            {
                return novosibirsk ??
                    (novosibirsk = new Command(obj =>
                    {
                        url = "https://api.openweathermap.org/data/2.5/weather?q=Novosibirsk&units=metric&appid=777c0b35616a7832e4d103e06717114c";
                        GetWeatherInfoAsync();
                    }));
            }
        }

        private string url = "https://api.openweathermap.org/data/2.5/weather?q=Novosibirsk&units=metric&appid=777c0b35616a7832e4d103e06717114c";

        public ViewModel()
        {
            GetWeatherInfoAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async void GetWeatherInfoAsync()
        {
            string weatherForecast;

            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                weatherForecast = await reader.ReadToEndAsync();
            }

            Weather = JsonConvert.DeserializeObject<Forecast>(weatherForecast);
            
            char symb = Weather.Main.Temp[Weather.Main.Temp.Length - 2];
            if (symb == '.')
                Weather.Main.Temp += "°С";
            else
                Weather.Main.Temp = Weather.Main.Temp.Remove(Weather.Main.Temp.Length - 1) + "°С";
        }
    }
}

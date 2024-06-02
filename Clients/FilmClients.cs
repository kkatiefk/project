using Newtonsoft.Json;
using project.Model;

namespace project.Clients
{
    public class FilmClients
    {
        private static string _address;
        private static string _apikey;
        private static string _apiHost;

        public FilmClients()
        {
            _address = Constants.Address;
            _apikey = Constants.ApiKey;
            _apiHost = Constants.ApiHost;
        }

        public async Task<Find> GetFilmDetails(string filmName)
        {
            var client = new HttpClient();
            var requestUri = $"{_address}&query={filmName}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
        {
            { "X-RapidAPI-Key", _apikey },
            { "X-RapidAPI-Host", _apiHost},
        },
            };


            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<Find>(body);
                return result;
            }
        }



        public async Task<Find> GetFilmDetailsByDate(string titleReleaseText)
        {
            var client = new HttpClient();
            var requestUri = $"{_address}&query={titleReleaseText}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
                {
                    { "X-RapidAPI-Key", _apikey },
                    { "X-RapidAPI-Host", _apiHost },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Find>(body);

                // Фільтруємо результати за роком випуску
                result.titleResults.results = result.titleResults.results
                    .Where(r => r.titleReleaseText == titleReleaseText)
                    .ToArray();

                return result;
            }
        }

        public async Task<Find> GetFilmDetailsByCredits(string topCredit)
        {
            var client = new HttpClient();
            var requestUri = $"{_address}&query={topCredit}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
                {
                    { "X-RapidAPI-Key", _apikey },
                    { "X-RapidAPI-Host", _apiHost },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Find>(body);

                // Фільтруємо результати за топ кредитом
                result.titleResults.results = result.titleResults.results
                    .Where(r => r.topCredits != null && r.topCredits.Contains(topCredit))
                    .ToArray();

                return result;
            }
        }

        public async Task<Result> GetRandomFilm()
        {
            var client = new HttpClient();
            var requestUri = $"{_address}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
                {
                    { "X-RapidAPI-Key", _apikey },
                    { "X-RapidAPI-Host", _apiHost },
                },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Find>(body);

                // Отримуємо випадковий фільм з результатів
                var random = new Random();
                var randomFilm = result.titleResults.results.OrderBy(x => random.Next()).FirstOrDefault();

                return randomFilm;
            }
        }
    }
}

using ReversiMvcV2.Models;
using System.Net.Http.Headers;

namespace ReversiMvcV2.Controllers
{
    public class ApiRequester
    {
        public static string api
        {
            get
            {
                return "https://localhost:7258/api/Spel";
            }
        }

        public static List<Spel>? GetAllSpellen()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content);
                    List<SpelJson>? spellenJson = response.Content.ReadAsAsync<List<SpelJson>>().Result;
                    List<Spel> spellen = spellenJson.Select(x => new Spel(x)).ToList();
                    return spellen;
                }
                return null;
            }
        }

        public static Spel? GetSpelByGuid(string guid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api + $"/Gametoken/{guid}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content);
                    SpelJson? spelJson = response.Content.ReadAsAsync<SpelJson>().Result;
                    if(spelJson is null) { return null; }
                    Spel spel = new Spel(spelJson);
                    return spel;
                }
                return null;
            }

        }
    }
}

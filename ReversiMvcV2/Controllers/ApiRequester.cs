using ReversiMvcV2.Models;
using ReversiMvcV2.Models.Request;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

        public static Spel? GetSpelByPlayerId(string guid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api + $"/Playertoken/{guid}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = null;
                try
                {
                    response = client.GetAsync(client.BaseAddress).Result;
                } catch(Exception ex)
                {

                }
                if (response != null && response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content);
                    SpelJson? spelJson = response.Content.ReadAsAsync<SpelJson>().Result;
                    if (spelJson is null) { return null; }
                    Spel spel = new Spel(spelJson);
                    return spel;
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

                HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
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

        public static void JoinSpel(string spelerId, string spelId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api + $"/Join");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var request = JsonSerializer.Serialize( new JoinSpelRequest() { SpelerId = spelerId, SpelId = spelId });
                var requestContent = new StringContent(request, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync(client.BaseAddress, requestContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var stringData = response.Content.ReadAsAsync<Object>();
                }
            }
        }

        public static void CreateSpel(string spelerId, string omschrijving)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api + $"/New");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var request = JsonSerializer.Serialize(new CreateSpelRequest() { spelerToken = spelerId, omschrijving = omschrijving });
                var requestContent = new StringContent(request, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(client.BaseAddress, requestContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var stringData = response.Content.ReadAsAsync<Object>();
                }
            }
        }
    }
}

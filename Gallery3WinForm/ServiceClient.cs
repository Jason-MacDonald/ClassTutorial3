using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gallery3WinForm
{
    public static class ServiceClient
    {
        /// <summary>
        /// Creates an HttpClient object which requests a JSON string from the specified URL and deserialises it into a .Net object i.e. a List<string>
        /// </summary>
        internal async static Task<List<string>> GetArtistNamesAsync()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<string>>
                    (await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtistNames/"));
        }

        internal async static Task<clsArtist> GetArtistAsync(string prArtistName)
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsArtist>
                    (await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtist?Name=" + prArtistName));

        }

        internal static async Task<string> InsertArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PostArtist", "POST");
        }

        internal static async Task<string> UpdateArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PutArtist", "PUT");
        }

        private async static Task<string> InsertOrUpdateAsync<TItem>(TItem prItem, string prUrl, string prRequest)
        {
            using (HttpRequestMessage lcReqMessage = new HttpRequestMessage(new HttpMethod(prRequest), prUrl))
            using (lcReqMessage.Content =
        new StringContent(JsonConvert.SerializeObject(prItem), Encoding.UTF8, "application/json"))
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.SendAsync(lcReqMessage);
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

    }
}
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CmApp.Domains;
using CmApp.Contracts;

namespace CmApp.Repositories
{
    public class ExchangeRatesRepository : IExchangeRatesRepository
    {
        //base currency allways EUR
        private readonly string Url = "https://api.exchangeratesapi.io/latest";
        public async Task<ExchangeRate> GetLatestForeignExchanges()
        {

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(Url)
            };

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(Url).Result;

            //Checks from API only countries which are in european union       
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ExchangeRate>(responseData);
                client.Dispose();
                return result;
            }
            else
            {
                client.Dispose();
                return null;
            }
        }

        public async Task<ExchangeRate> GetSelectedExchangeRate(string name)
        {
            var url = Url +"?symbols=" + name.ToUpper();
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(url).Result;

            //Checks from API only countries which are in european union       
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ExchangeRate>(responseData);
                client.Dispose();
                return result;
            }
            else
            {
                client.Dispose();
                return null;
            }
        }
    }
}

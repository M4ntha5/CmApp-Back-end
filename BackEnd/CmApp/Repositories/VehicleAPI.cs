using CmApp.Domains;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class VehicleAPI
    {
        private string Url = "https://vpic.nhtsa.dot.gov/api/vehicles/getmodelsformake/";
        private const string Format = "?format=json";

        public async Task<List<string>> GetAllMakeModels(string make)
        {
            Url += make + Format;

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(Url)
            };

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(Url).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CarMakesObject>(responseData);
                var models = result.Cars.Select(x => x.ModelName).ToList();
                client.Dispose();
                return models;
            }
            else
            {
                client.Dispose();
                return null;
            }
        }
    }
}

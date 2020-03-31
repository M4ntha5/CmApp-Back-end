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
        private readonly string Url = "https://vpic.nhtsa.dot.gov/api/vehicles/getallmakes?format=json";
        public async Task<CarMakesObject> GetAllMakes()
        {

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

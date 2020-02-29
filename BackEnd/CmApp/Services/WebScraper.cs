using CmApp.Contracts;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class WebScraper : IWebScraper
    {
        private readonly string Endpoint = "https://www.mdecoder.com/decode/";

        public Dictionary<string, string> GetVehicleInfo(string vin)
        {
            Dictionary<string, string> vehicleInfo = new Dictionary<string, string>();

            var table = SetUp(vin).Result[0];
            int info = 0;

            List<HtmlNode> toftitle2 = table.Descendants().Where
                                        (x => (x.Name == "tr")).ToList();
            foreach (var tr in toftitle2)
            {
                if (info == 0)
                {
                    info = 1;
                    continue;
                }
                var name = tr.ChildNodes[1].InnerText;
                var value = tr.ChildNodes[3].InnerText;
                vehicleInfo.Add(name, value);
            }
            return vehicleInfo;
        }

        public Dictionary<string, string> GetVehicleEquipment(string vin)
        {
            Dictionary<string, string> vehicleEquipment = new Dictionary<string, string>();

            var tables = SetUp(vin).Result;
            int eq = 0;

            foreach (var table in tables)
            {
                if (eq == 0)
                {
                    eq = 1;
                    continue;
                }
                List<HtmlNode> toftitle2 = table.Descendants().Where
                                        (x => (x.Name == "tr")).ToList();
                foreach (var tr in toftitle2)
                {
                    var name = tr.ChildNodes[0].InnerText;
                    var value = tr.ChildNodes[1].InnerText;
                    if (name != "-")
                        vehicleEquipment.Add(name, value);
                }
            }
            return vehicleEquipment;
        }

        private async Task<List<HtmlNode>> SetUp(string vin)
        {
            var website = Endpoint + vin;
            HttpClient http = new HttpClient();
            var response = await http.GetByteArrayAsync(website);
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument result = new HtmlDocument();
            result.LoadHtml(source);

            List<HtmlNode> tables = result.DocumentNode.Descendants().Where
            (x => (x.Name == "table" && x.Attributes["class"] != null &&
               x.Attributes["class"].Value.Contains("table , black table-striped"))).ToList();

            return tables;
        }

    }
}

using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Repositories;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class WebScraper : IWebScraper
    {
        private readonly string MDecoderEndpoint = "https://www.mdecoder.com/decode/";
        private readonly string AtlanticExpressEndpoint = "http://www.atlanticexpresscorp.com/services/tracking/?num=";

        public ICarRepository CarRepo { get; set; }
        public IFileRepository FileRepository { get; set; }
        public ITrackingRepository TrackingRepo { get; set; }

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
            var website = MDecoderEndpoint + vin;
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

        public async Task TrackingScraper(string vin)
        {
            var website = AtlanticExpressEndpoint + vin;
            HttpClient http = new HttpClient();
            var response = await http.GetByteArrayAsync(website);
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument result = new HtmlDocument();
            result.OptionFixNestedTags = true;
            result.LoadHtml(source);

            //table content
            List<HtmlNode> trs = result.DocumentNode.Descendants().Where
            (x => (x.Name == "tr")).ToList();

            var bookingNr = trs[11].ChildNodes[3].InnerText;
            var containerNr = trs[12].ChildNodes[3].InnerText;
            var trackingUrl = trs[13].ChildNodes[3].OuterHtml;

            var uri = trackingUrl.Split("\"")[1];
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match res = re.Match(containerNr);

            string alphaPart = res.Groups[1].Value;
            string numberPart = res.Groups[2].Value;

            var url = uri;// + "?container=" + alphaPart + "++" + numberPart;

            //insert other info here

            var car = await CarRepo.GetCarByVin(vin);

            var trackingEntity = new TrackingEntity()
            {
                BookingNumber = bookingNr,
                ContainerNumber = containerNr,
                Url = url,
                Car = car.Id,
                AuctionImages = new List<object>()
            };

            
            var tracking = await TrackingRepo.InsertTracking(trackingEntity);


            //all images
            var links = result.DocumentNode.SelectNodes("//img[@src]");
            List<string> imgLinks = new List<string>();

            foreach (var link in links)
            {
                if(link!= null)
                {
                    var lll = link.OuterHtml.Split("\"")[1];
                    if(lll.Contains("client") && lll.Contains("image"))
                    {
                        var newImg = lll.Replace("thumb", "full");
                        imgLinks.Add(newImg);
                    }
                }
            }

            var counter = 1;
            //downloads all auction images and inserts into tracking collection
            foreach (var img in imgLinks)
            {
                Image image = DownloadImageFromUrl(img.Trim());
                string imageName = counter+".png";
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Png);        
                var bytes = FileRepository.StreamToByteArray(stream);
                //need to insert here
                var imgResponse = await TrackingRepo.UploadImageToTracking(tracking.Id, bytes, imageName);
                counter++;
            }


        }

        public Image DownloadImageFromUrl(string imageUrl)
        {
            Image image;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                WebResponse webResponse = webRequest.GetResponse();

                Stream stream = webResponse.GetResponseStream();

                image = Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return null;
            }

            return image;
        }

    }
}

using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private readonly string MBDecoderEndpoint = "https://www.mbdecoder.com/decode/";
        private readonly string AtlanticExpressEndpoint = "http://www.atlanticexpresscorp.com/services/tracking/?num=";

        public ICarRepository CarRepo { get; set; }
        public IFileRepository FileRepository { get; set; }
        public ITrackingRepository TrackingRepo { get; set; }

        public Dictionary<string, string> GetVehicleInfo(string vin, string make)
        {
            try
            {
                Dictionary<string, string> vehicleInfo = new Dictionary<string, string>();
                HtmlNode table;
                if (make == "BMW")
                    table = SetUpBMW(vin).Result[0];
                else
                    table = SetUpMB(vin).Result[0];

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
            catch(Exception ex)
            {
                var error = new Dictionary<string, string>();
                error.Add("Error", ex.Message);
                return error;
            }
            
        }

        public Dictionary<string, string> GetVehicleEquipment(string vin, string make)
        {
            Dictionary<string, string> vehicleEquipment = new Dictionary<string, string>();

            List<HtmlNode> tables;
            if (make == "BMW")
                tables = SetUpBMW(vin).Result;
            else
                tables = SetUpMB(vin).Result;
            int eq = 0;
            if (tables == null || tables.Count < 1)
                throw new Exception("Failed to find any data by given vin. Try different vin number");

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

        private async Task<List<HtmlNode>> SetUpBMW(string vin)
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
        private async Task<List<HtmlNode>> SetUpMB(string vin)
        {
            var website = MBDecoderEndpoint + vin;
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

        public async Task<TrackingEntity> TrackingScraper(string vin)
        {
            var website = AtlanticExpressEndpoint + vin;
            HttpClient http = new HttpClient();

            var response = await http.GetByteArrayAsync(website);
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument result = new HtmlDocument();
            result.OptionFixNestedTags = true;
            result.LoadHtml(source);

            //check if captcha exists
            var dataSiteKey = result.DocumentNode.Descendants().Where
            (x => (x.Name == "div") && x.Attributes["class"] != null &&
               x.Attributes["class"].Value.Contains("g-recaptcha")).ToArray();

            //recaptcha bypass
            string recaptchaToken = null;
            if (dataSiteKey != null && dataSiteKey.Length > 0)
            {
                var split = dataSiteKey[0].OuterHtml.Split("\"");
                var key = split[3]; //sites recaptch key
                var repo = new Recaptcha2captcha();
                recaptchaToken = repo.Start(key, website);

                if (recaptchaToken != null)
                {
                    website += "&g-recaptcha-response=" + recaptchaToken;
                    response = null;
                    result = new HtmlDocument();
                    response = await http.GetByteArrayAsync(website);
                    source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                    source = WebUtility.HtmlDecode(source);
                    result.LoadHtml(source);
                }
            }

            //table content
            List<HtmlNode> trs = result.DocumentNode.Descendants().Where
            (x => (x.Name == "tr")).ToList();

            if (trs.Count == 0 || trs == null)
                throw new Exception("No tracking info for this car!");

            CarRepo = new CarRepository();
            FileRepository = new FileRepository();
            TrackingRepo = new TrackingRepository();

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
                var size = image.Size;

                var newImg = ResizeImage(image, new Size(1920, 1080));
                var newsize = newImg.Size;

                string imageName = tracking.Id + "_image" + counter + ".jpeg";
                var stream = new MemoryStream();

                newImg.Save(stream, ImageFormat.Jpeg);        
                var bytes = FileRepository.StreamToByteArray(stream);
                //insert here
                var imgResponse = await TrackingRepo.UploadImageToTracking(tracking.Id, bytes, imageName);
                var base64 = FileRepository.ByteArrayToBase64String(bytes);
                tracking.Base64images.Add(base64);

                counter++;
            }

            return tracking;
        }
        private Image ResizeImage(Image imgToResize, Size size)
        {
            var brush = new SolidBrush(Color.Black);

            var image = new Bitmap(imgToResize);
            
            float scale = Math.Min(size.Width / image.Width, size.Height / image.Height);

            var bmp = new Bitmap((int)size.Width, (int)size.Height);
            var graph = Graphics.FromImage(bmp);

            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, size.Width, size.Height));
            graph.DrawImage(image, ((int)size.Width - scaleWidth) / 2, ((int)size.Height - scaleHeight) / 2, scaleWidth, scaleHeight);

            graph.Dispose();

            return bmp;
        }

        private Image DownloadImageFromUrl(string imageUrl)
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

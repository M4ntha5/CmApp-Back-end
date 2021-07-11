using CmApp.BusinessLogic.Repositories;
using CmApp.Contracts;
using CmApp.Contracts.Models;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class ScraperService : IScraperService
    {
        private readonly string _mDecoderEndpoint = "https://www.mdecoder.com/decode/";
        private readonly string _mbDecoderEndpoint = "https://www.mbdecoder.com/decode/";
        private readonly string _atlanticExpressEndpoint = "https://www.atlanticexpresscorp.com/services/tracking/?num=";

        // private readonly IFileRepository FileRepository = new FileRepository();
        private readonly ITrackingRepository _trackingRepo;

        public ScraperService(ITrackingRepository trackingRepo)
        {
            _trackingRepo = trackingRepo;
        }

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
                if (table == null)
                    throw new BusinessException("Failed to find any data by given vin. Try different vin number");

                List<HtmlNode> toftitle2 = table.Descendants().Where
                                            (x => (x.Name == "tr")).ToList();

                if (toftitle2 == null || toftitle2.Count < 1)
                    throw new BusinessException("Failed to find any data by given vin. Try different vin number");

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
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
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
                throw new BusinessException("Failed to find any data by given vin. Try different vin number");

            foreach (var table in tables)
            {
                if (eq == 0)
                {
                    eq = 1;
                    continue;
                }
                List<HtmlNode> toftitle2 = table.Descendants().Where
                                        (x => (x.Name == "tr")).ToList();

                if (toftitle2 == null || toftitle2.Count < 1)
                    throw new BusinessException("Failed to find any data by given vin. Try different vin number");

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
            try
            {
                var website = _mDecoderEndpoint + vin;

                HttpClient http = new HttpClient();
                var response = await http.GetByteArrayAsync(website);
                String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                HtmlDocument result = new HtmlDocument();
                result.LoadHtml(source);

                List<HtmlNode> tables = result.DocumentNode.Descendants().Where
                (x => (x.Name == "table" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("table , black table-striped"))).ToList();

                if (tables.Count < 1 || tables == null)
                    throw new BusinessException("Failed to find any data by given vin. Try different vin number");

                return tables;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        private async Task<List<HtmlNode>> SetUpMB(string vin)
        {
            try
            {
                var website = _mbDecoderEndpoint + vin;

                HttpClient http = new HttpClient();
                var response = await http.GetByteArrayAsync(website);
                String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                HtmlDocument result = new HtmlDocument();
                result.LoadHtml(source);

                List<HtmlNode> tables = result.DocumentNode.Descendants().Where
                (x => (x.Name == "table" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("table , black table-striped"))).ToList();

                if (tables.Count < 1 || tables == null)
                    throw new BusinessException("Failed to find any data by given vin. Try different vin number");

                return tables;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public async Task<Tracking> TrackingScraper(Car car, int trackingId)
        {
            try
            {
                var website = _atlanticExpressEndpoint + car.Vin;
                var result = await GetPrimarySiteDocument(website);

                if (result == null)
                    throw new BusinessException("Error getting data from tracking page!");

                //check if captcha exists
                var dataSiteKey = result.DocumentNode.Descendants().Where
                (x => (x.Name == "div") && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("g-recaptcha")).ToArray();
                //------------------------------------------------------------
                //recaptcha bypass
                if (dataSiteKey != null && dataSiteKey.Length > 0)
                    result = await BypassRecaptcha(website, dataSiteKey);

                //table content
                List<HtmlNode> trs = result.DocumentNode.Descendants().Where
                (x => (x.Name == "tr")).ToList();

                if (trs.Count == 0 || trs == null)
                    throw new BusinessException("There is no tracking information for this car!");
                //------------------------------------------------------------

                var trackingUrl = trs[13].ChildNodes[3].OuterHtml;
                var url = trackingUrl.Split("\"")[1];

                //insert tracking info here          
                var trackingEntity = new Tracking()
                {
                    Vin = trs[0].ChildNodes[3].InnerText,
                    Year = int.Parse(trs[1].ChildNodes[3].InnerText),
                    Make = trs[2].ChildNodes[3].InnerText,
                    Model = trs[3].ChildNodes[3].InnerText,
                    Title = trs[4].ChildNodes[3].InnerText,
                    State = trs[5].ChildNodes[3].InnerText,
                    Status = trs[6].ChildNodes[3].InnerText,
                    DateReceived = DateTime.Parse(trs[7].ChildNodes[3].InnerText),
                    DateOrdered = DateTime.Parse(trs[8].ChildNodes[3].InnerText),
                    Branch = trs[9].ChildNodes[3].InnerText,
                    ShippingLine = trs[10].ChildNodes[3].InnerText,

                    BookingNumber = trs[11].ChildNodes[3].InnerText,
                    ContainerNumber = trs[12].ChildNodes[3].InnerText,
                    //Url = url,

                    FinalPort = trs[14].ChildNodes[3].InnerText,
                    DatePickedUp = DateTime.Parse(trs[15].ChildNodes[3].InnerText),
                    Color = trs[16].ChildNodes[3].InnerText,
                    Damage = trs[17].ChildNodes[3].InnerText,
                    Condition = trs[18].ChildNodes[3].InnerText,
                    Keys = trs[19].ChildNodes[3].InnerText,
                    Running = trs[20].ChildNodes[3].InnerText,
                    Wheels = trs[21].ChildNodes[3].InnerText,
                    AirBag = trs[22].ChildNodes[3].InnerText,
                    Radio = trs[23].ChildNodes[3].InnerText,

                    Car = car,
                };

                await _trackingRepo.UpdateCarTracking(trackingId, trackingEntity);
                var tracking = await _trackingRepo.GetTrackingByCar(car.Id);
                return tracking;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public async Task<List<string>> GetTrackingImagesUrls(Car car)
        {
            try
            {
                var website = _atlanticExpressEndpoint + car.Vin;
                var result = await GetPrimarySiteDocument(website);

                //check if captcha exists
                var dataSiteKey = result.DocumentNode.Descendants().Where
                (x => (x.Name == "div") && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("g-recaptcha")).ToArray();
                //------------------------------------------------------------
                //recaptcha bypass
                if (dataSiteKey != null && dataSiteKey.Length > 0)
                    result = await BypassRecaptcha(website, dataSiteKey);

                var links = result.DocumentNode.SelectNodes("//img[@src]");
                List<string> imgLinks = new List<string>();
                //changing image quality from thumbnail to full
                foreach (var link in links)
                {
                    if (link != null)
                    {
                        var url = link.OuterHtml.Split("\"")[1];
                        if (url.Contains("client") && url.Contains("image"))
                        {
                            var newImg = url.Replace("thumb", "full");
                            imgLinks.Add(newImg);
                        }
                    }
                }
                return imgLinks;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        //bring back if needed
        /*
        public async Task DownloadAllTrackingImages(TrackingEntity tracking, List<string> imageUrls)
        {
            try
            {
                var repo = new FileRepository();
                var trackingRepo = new TrackingRepository();

                if (tracking.Images.Count == imageUrls.Count)
                    return;

                //deletes form cloud
                await repo.DeleteFolder("/tracking/" + tracking.Id);
                //deletes from db
                await trackingRepo.DeleteTrackingImages(tracking.Id);

                var imgsList = new List<UploadImageRequest.File>();               

                var counter = 1;
                //downloads all auction images and puts to a list
                foreach (var img in imageUrls)
                {
                    var webClient = new WebClient();
                    var uri = new Uri(img.Trim());
                    byte[] bytes = webClient.DownloadData(uri);
                    var stream = new MemoryStream(bytes);

                    imgsList.Add(new UploadImageRequest.File()
                    {
                        Data = stream,
                        FileName = counter + ".jpeg"
                    });
                    counter++;
                }

                //inserts to cloud 
                //!!bring back if needed!!
                //var insertedUrls = await repo.InsertTrackingImages(tracking.Id, imgsList);
                //inserts to db
                await trackingRepo.UploadImageToTracking(tracking.Id, insertedUrls);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }*/

        private async Task<HtmlDocument> GetPrimarySiteDocument(string website)
        {
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient http = new HttpClient(clientHandler);
            var response = await http.GetByteArrayAsync(website);
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument result = new HtmlDocument
            {
                OptionFixNestedTags = true
            };
            result.LoadHtml(source);
            return result;
        }

        private async Task<HtmlDocument> BypassRecaptcha(string website, HtmlNode[] dataSiteKey)
        {
            HttpClient http = new HttpClient();
            HtmlDocument result = new HtmlDocument();
            string recaptchaToken;
            if (dataSiteKey != null && dataSiteKey.Length > 0)
            {
                var split = dataSiteKey[0].OuterHtml.Split("\"");
                var key = split[3]; //sites recaptch key
                var repo = new Recaptcha2captchaRepository();
                recaptchaToken = repo.Start(key, website);

                if (recaptchaToken != null)
                {
                    website += "&g-recaptcha-response=" + recaptchaToken;
                    var response = await http.GetByteArrayAsync(website);
                    var source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                    source = WebUtility.HtmlDecode(source);
                    result.LoadHtml(source);
                }
            }
            return result;
        }
    }
}

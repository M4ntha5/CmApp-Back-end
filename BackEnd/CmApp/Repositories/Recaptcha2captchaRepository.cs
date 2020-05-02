using System.IO;
using System;
using System.Net;
using System.Text;
using CmApp.Utils;

namespace CmApp.Domains
{
    public class Recaptcha2captchaRepository
    {
        private string Captcha_service_key;
        private string Site_key;
        private string Page_url;

        public void SetServiceKey(string service_key)
        {
            Captcha_service_key = service_key;
        }
        public void SetSiteKey(string site_key)
        {
            Site_key = site_key;
        }
        public void SetPageUrl(string page_url)
        {
            Page_url = page_url;
        }

        public string SendRequest() // HTTP POST
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)WebRequest.Create("http://2captcha.com/in.php");

                var postData = "key=" + Captcha_service_key + "&method=userrecaptcha&googlekey=" 
                    + Site_key + "&pageurl=" + Page_url;

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                    stream.Write(data, 0, data.Length);


                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                response.Close();

                if (responseString.Contains("OK|"))
                    return responseString;
                else
                    return "2captcha service return error. Error code:" + responseString;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public string SubmitForm(string RecaptchaResponseToken)  // HTTP POST
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)WebRequest.Create(Page_url);

                var postData = "submit=submin&g-recaptcha-response=" + RecaptchaResponseToken;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                    stream.Write(data, 0, data.Length);

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                response.Close();

                return responseString;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        // the request to retrieve g-recaptcha-response token from 2captcha service
        public string GetToken(string captcha_id)  // HTTP GET
        {
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("key", Captcha_service_key);
            webClient.QueryString.Add("action", "get");
            webClient.QueryString.Add("id", captcha_id);

            return webClient.DownloadString("http://2captcha.com/res.php");
        }
        // validate site with returned token thru proxy.php  
        public string GetValidate(string token)
        {
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("response", token);
            return webClient.DownloadString(Page_url);
        }

        public string Start(string siteKey, string url)
        {
            Recaptcha2captchaRepository service = new Recaptcha2captchaRepository();

            // we set 2captcha service key and target google site_key
            service.SetServiceKey(Settings.CaptchaApiKey);
            service.SetSiteKey(siteKey);
            service.SetPageUrl(url);

            var resp = service.SendRequest();
            var gcaptchaToken = "";
            if (resp.Contains("OK|"))
            {
                // loop till the service solves captcha and gets g-recaptcha-response token
                var i = 0;
                while (i++ <= 20)
                {
                    System.Threading.Thread.Sleep(5000); // sleep 5 seconds
                    gcaptchaToken = service.GetToken(resp[3..]);
                    if (gcaptchaToken.Contains("OK|"))
                        break;
                }

                if (gcaptchaToken.Contains("OK|"))
                {
                    var RecaptchaResponseToken = gcaptchaToken[3..];
                    // make google to validate g-recaptcha-response token 
                    _ = service.GetValidate(RecaptchaResponseToken);
                    // submit form to the target site
                    _ = service.SubmitForm(RecaptchaResponseToken);

                    return RecaptchaResponseToken;
                }
                else               
                    return null;

            }
            else
                return null;
        }
    }
    
}

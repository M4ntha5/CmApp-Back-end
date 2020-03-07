using System.IO;
using System;
using System.Net;
using System.Text;
using CmApp.Utils;

namespace CmApp.Domains
{
    public class Recaptcha2captcha
    {
        private static string captcha_service_key;
        private string site_key;
        private string page_url;

        public void setServiceKey(string service_key)
        {
            Recaptcha2captcha.captcha_service_key = service_key;
        }
        public void setSiteKey(string site_key)
        {
            this.site_key = site_key;
        }
        public void setPageUrl(string page_url)
        {
            this.page_url = page_url;
        }

        public string SendRequest() // HTTP POST
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)WebRequest.Create("http://2captcha.com/in.php");

                var postData = "key=" + Recaptcha2captcha.captcha_service_key + "&method=userrecaptcha&googlekey=" + this.site_key + "&pageurl=" + this.page_url;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                response.Close();

                if (responseString.Contains("OK|"))
                {
                    return responseString;
                }
                else
                {
                    return "2captcha service return error. Error code:" + responseString;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public string SubmitForm(string RecaptchaResponseToken)  // HTTP POST
        {
            // var page_url = "http://testing-ground.scraping.pro/recaptcha"; 
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)WebRequest.Create(this.page_url);

                var postData = "submit=submin&g-recaptcha-response=" + RecaptchaResponseToken;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

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
        public string getToken(string captcha_id)  // HTTP GET
        {
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("key", Recaptcha2captcha.captcha_service_key);
            webClient.QueryString.Add("action", "get");
            webClient.QueryString.Add("id", captcha_id);

            return webClient.DownloadString("http://2captcha.com/res.php");
        }
        // validate site with returned token thru proxy.php  
        public string getValidate(string token)
        {
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("response", token);
            return webClient.DownloadString(this.page_url);
        }

        public string Start(string siteKey, string url)
        {
            Recaptcha2captcha service = new Recaptcha2captcha();

            // we set 2captcha service key and target google site_key
            service.setServiceKey(Settings.CaptchaApiKey);
            service.setSiteKey(siteKey);
            service.setPageUrl(url);

            var resp = service.SendRequest();
            var gcaptchaToken = "";
            if (resp.Contains("OK|"))
            {
                // loop till the service solves captcha and gets g-recaptcha-response token
                var i = 0;
                while (i++ <= 20)
                {
                    System.Threading.Thread.Sleep(5000); // sleep 5 seconds
                    //Console.WriteLine("Captcha is being solved for {0} seconds", i * 5);
                    gcaptchaToken = service.getToken(resp.Substring(3, resp.Length - 3));
                    if (gcaptchaToken.Contains("OK|"))
                    {
                        break;
                    }
                }


                if (gcaptchaToken.Contains("OK|"))
                {
                    var RecaptchaResponseToken = gcaptchaToken.Substring(3, gcaptchaToken.Length - 3);
                    //Console.WriteLine("g-recaptcha-response token:  " + RecaptchaResponseToken);

                    // make google to validate g-recaptcha-response token 
                    var iSvalid = service.getValidate(RecaptchaResponseToken);
                    //Console.WriteLine("Token is validated by google: " + iSvalid);

                    // submit form to the target site
                    var SubmitFormResp = service.SubmitForm(RecaptchaResponseToken);
                    //Console.WriteLine("Submit form return: " + SubmitFormResp);
                    return gcaptchaToken;

                }
                else
                {
                    //Console.WriteLine("Captcha has not been solved. Error code: " + gcaptchaToken);
                    //Environment.Exit(0);
                    return null;
                }

            }
            else
            {
                //Console.WriteLine("Error: " + resp);
                //Environment.Exit(0);
                return null;
            }
        }
    }
    
}

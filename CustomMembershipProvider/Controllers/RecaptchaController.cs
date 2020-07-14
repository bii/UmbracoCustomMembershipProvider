using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using CustomMembershipProvider.Models;
using Umbraco.Web.WebApi;

namespace CustomMembershipProvider.Controllers
{
    public class RecaptchaController : UmbracoApiController
    {
        // umbraco/api/recaptcha/siteverify
        [HttpPost]
        public async Task<RecaptchaResponse> SiteVerify(string recaptchaResponse, string recaptchaAction)
        {
            var secret = System.Configuration.ConfigurationManager.AppSettings["Recaptcha:Secret"];

            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("Recaptcha secret key not defined in appSettings section");
            }
                            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"https://www.google.com/recaptcha/api/siteverify");

                var response = await client.GetStringAsync($"?response={recaptchaResponse}&secret={secret}&action={recaptchaAction}");
                var result = JsonConvert.DeserializeObject<RecaptchaResponse>(response);

                return result;
            }
        }
    }
}
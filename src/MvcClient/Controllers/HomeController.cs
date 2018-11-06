using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly URIConfig _config;

        public HomeController(IOptions<URIConfig> config)
        {
            _config = config.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secure()
        {
            ViewData["Message"] = "Secure page.";

            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> CallApiUsingClientCredentials()
        {
            var tokenClient = new TokenClient(_config.authority+"/connect/token", "mvc", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var content = await client.GetStringAsync(_config.api + "/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            //var content = await client.GetStringAsync(_config.api+"/");
            var xml = "<soapenv:Envelope xmlns:soapenv = \"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem = \"http://tempuri.org/\" >\n" +
   "<soapenv:Header />\n" +
   "<soapenv:Body >\n" +
      "<tem:SubmitRequest >\n" +
        "<tem:NonPosRequest CardNumber = \"9440385200600010995\" PwdHash = \"YQBhAGEAYQAxADEAMQAxAA==\" >\n" +
          "<tem:RequestRecords TableName = \"ClientsKids_VW\" Operation = \"SELECT\" />\n" +
        "</tem:NonPosRequest >\n" +
      "</tem:SubmitRequest >\n" +
   "</soapenv:Body >\n" +
"</soapenv:Envelope >";

            var body = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml");
            var content = await client.PostAsync(_config.api + "/PublicWebService.asmx", body);
            var xmlresult= await content.Content.ReadAsStringAsync();
            var xmldoc = System.Xml.Linq.XDocument.Parse(xmlresult);
            ViewBag.Xml = xmldoc;
            return View("Xml");
            //ViewBag.Json = JArray.Parse(content).ToString();
            //return View("Json");
        }
    }
}
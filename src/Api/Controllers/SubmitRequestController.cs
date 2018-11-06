using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Api.Controllers
{
    [Route("PublicWebService.asmx")]
    [Authorize]
    public class SubmitRequestController : Controller
    {
       
        private string target = "http://rimitest.new-vision.com/PublicWebService/PublicWebService.asmx";

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                var queryString = Request.QueryString;
                var response = await _httpClient.GetAsync(target+queryString.Value);
                var content = await response.Content.ReadAsStringAsync();

                Response.StatusCode = (int)response.StatusCode;
                Response.ContentType = response.Content.Headers.ContentType.ToString();
                Response.ContentLength = response.Content.Headers.ContentLength;

                return Content(content);

            }
            //return new JsonResult(from h in this.Request.Headers select new { h.Key, h.Value });
        }
        
            
        

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                using (StreamContent body = new StreamContent(Request.Body))
                {
                    body.Headers.ContentType = new MediaTypeHeaderValue(new System.Net.Mime.ContentType(Request.ContentType).MediaType);
                    var response = await _httpClient.PostAsync(target, body);
                    var content = await response.Content.ReadAsStringAsync();
                    Response.StatusCode = (int)response.StatusCode;
                    Response.ContentType = response.Content.Headers.ContentType?.ToString();
                    Response.ContentLength = response.Content.Headers.ContentLength;
                    return Content(content);

                }
            }
        }

    }
}
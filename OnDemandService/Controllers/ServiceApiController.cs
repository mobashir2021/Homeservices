using Newtonsoft.Json;
using OnDemandService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OnDemandService.Controllers
{
    public class ServiceApiController : ApiController
    {
        
        [HttpGet]
        public HttpResponseMessage GetCategories()
        {
            

            try
            {
                string data = "";
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(data));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        
    }
}

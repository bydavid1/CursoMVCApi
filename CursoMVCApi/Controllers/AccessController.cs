using CursoMVCApi.Models.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CursoMVCApi.Controllers
{
    public class AccessController : ApiController
    {
        // GET: Access
        [HttpGet]
        public Response Index()
        {
            Response response = new Response();
            response.result = 200;
            response.message = "Hello world";

            return response;
        }
    }
}
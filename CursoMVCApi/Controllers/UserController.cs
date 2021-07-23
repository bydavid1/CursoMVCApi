using CursoMVCApi.Models;
using CursoMVCApi.Models.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CursoMVCApi.Controllers
{
    public class UserController : ApiController
    {
        // GET: User
        [HttpGet]
        public HttpResponseMessage Index()
        {
            try
            {
                IEnumerable<user> users;

                using (var context = new DBModel())
                {
                    users = context.users.ToList(); 
                }

                if (users.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, users);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error en el servidor");
            }
        }

        [HttpPost]

        public HttpResponseMessage Login(user user)
        {
            try
            {
                //IQueryable<user> userToValidate;

                using (var context = new DBModel())
                {
                    var userToValidate = context.users.Where(model => model.email == user.email && model.password == user.password);
                    if (userToValidate.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.Accepted, Guid.NewGuid().ToString());
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Datos incorrectos");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error en el servidor");
            }
        }
    }
}
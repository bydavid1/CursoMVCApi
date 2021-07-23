using CursoMVCApi.Models.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CursoMVCApi.Models;
using System.Net.Http;
using System.Net;

namespace CursoMVCApi.Controllers
{
    public class CategoryController : ApiController
    {
        // GET: Category
        [HttpGet]
        public HttpResponseMessage Index()
        {
            try
            {
                
                using (var db = new DBModel())
                {
                    IQueryable<category> categories;

                    categories = db.categories;
                    if (categories.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, categories);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent);
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error en el servidor " + ex.Message);
            }
        }

        [HttpGet]
        public Response Find(int id)
        {
            try
            {
                Response response = new Response();
                using (var db = new DBModel())
                {
                    var category = db.categories.Find(id);

                    if (category == null)
                    {
                        response.result = 404;
                        response.message = "Category not found";
                    }
                    else
                    {
                        response.result = 200;
                        response.data = category;
                    }

                }

                return response;
            }
            catch (Exception)
            {
                Response response = new Response();
                response.result = 500;
                response.message = "Server error";
                return response;
            }
        }

        [HttpPost]
        public HttpResponseMessage Store(category category)
        {
            if (category == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "La categoria no esta disponible o no existe");
            }

            var db = new DBModel();

            db.categories.Add(category);
            var result = db.SaveChanges();

            if (result == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Creado con exito");
            } else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ocurrio un inconveniente al guardar");
            }
        }
    }
}
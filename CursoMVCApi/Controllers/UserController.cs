using CursoMVCApi.Models;
using CursoMVCApi.Models.WS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CursoMVCApi.Controllers
{
    public class UserController : ApiController
    {
        public string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=POS_SYS;Integrated Security=True";
        
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

        [HttpGet]
        public HttpResponseMessage GetByEmail(string email)
        {
            try
            {
                if (email == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No se ha especificado el nombre");
                }

                using(var context = new DBModel())
                {
                    IQueryable<user> user = context.users.Where(res => res.email == email);

                    if (user.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El email no existe");
                    }
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

        [Route("api/user/store")]
        [HttpPost]
        public HttpResponseMessage Store(user user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("StoreNewUser", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 20));
                    command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 20));
                    command.Parameters["@email"].Value = user.email;
                    command.Parameters["@password"].Value = user.password;

                    connection.Open();
                    var reader = command.ExecuteNonQuery();
                    connection.Close();

                    if (reader > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Usuario creado con exito");
                    }

                    return Request.CreateResponse(HttpStatusCode.NoContent, "El usuario pudo no haberse creado");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error en el servidor " + ex.Message);
            }
        }
    }
}
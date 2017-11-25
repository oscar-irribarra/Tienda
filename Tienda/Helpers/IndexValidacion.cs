using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tienda.Models;

namespace Tienda.Helpers
{
    public class IndexValidacion
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public static Respuesta SaveChanges(ApplicationDbContext db)
        {
            try
            {
                db.SaveChanges();
                return new Respuesta { Respuestaex = true };
            }
            catch (Exception ex)
            {
                var respuesta = new Respuesta { Respuestaex = false };
                if (ex.InnerException != null && ex.InnerException.InnerException != null
                 && ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    respuesta.Mensaje = "Ya existe un registro con este valor";
                }
                else
                {
                    respuesta.Mensaje = ex.Message;
                }

                return respuesta;
            }
        }
    }
}
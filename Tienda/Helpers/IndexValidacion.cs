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

        public static ResponseModel SaveChanges(ApplicationDbContext db)
        {
            var rm = new ResponseModel();

            try
            {
                db.SaveChanges();
                rm.Response = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null
                 && ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    rm.Message = "Ya existe un registro con este valor";
                }
                else
                {
                    rm.Message = ex.Message;
                }
                rm.Response = false;

            }
            return rm;
        }
    }
}
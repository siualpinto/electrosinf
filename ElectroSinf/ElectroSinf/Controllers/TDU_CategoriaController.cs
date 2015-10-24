using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElectroSinf.Lib_Primavera.Model;

namespace ElectroSinf.Controllers
{
    public class TDU_CategoriaController : ApiController
    {        
        public TDU_Categoria Get(int id)
        {
            Lib_Primavera.Model.TDU_Categoria categoria = Lib_Primavera.PriIntegration.GetCategoria(id);
            if (categoria == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return categoria;
            }
        }
    }
}

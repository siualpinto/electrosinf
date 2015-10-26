using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElectroSinf.Controllers
{
    public class TDU_TipoArtigobyCDU_CategoriaController : ApiController
    {
        public IEnumerable<Lib_Primavera.Model.TDU_TipoArtigobyCDU_Categoria> Get(int id)
        {
            IEnumerable<Lib_Primavera.Model.TDU_TipoArtigobyCDU_Categoria> tipos = Lib_Primavera.PriIntegration.ListaTiposArtigosbyCategoria(id);
            if (tipos == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return tipos;
            }
        }
    }
}

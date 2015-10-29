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
        public IEnumerable<Lib_Primavera.Model.TDU_Categoria> Get()
        {
            return Lib_Primavera.PriIntegration.ListaCategorias();
        }
        public IEnumerable<Lib_Primavera.Model.TDU_TipoArtigo> Get(int id)
        {
            IEnumerable<Lib_Primavera.Model.TDU_TipoArtigo> tipos = Lib_Primavera.PriIntegration.ListaTiposArtigosbyCategoria(id);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElectroSinf.Lib_Primavera.Model;

namespace ElectroSinf.Controllers
{
    public class TDU_TipoArtigoController : ApiController
    {
        public IEnumerable<Lib_Primavera.Model.TDU_TipoArtigo> Get()
        {
            IEnumerable<Lib_Primavera.Model.TDU_TipoArtigo> tipos = Lib_Primavera.PriIntegration.ListaTiposArtigos();
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
        public IEnumerable<Lib_Primavera.Model.Artigo> Get(int id)
        {
            IEnumerable<Lib_Primavera.Model.Artigo> tipos = Lib_Primavera.PriIntegration.ListaArtigosbyTipo(id);
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

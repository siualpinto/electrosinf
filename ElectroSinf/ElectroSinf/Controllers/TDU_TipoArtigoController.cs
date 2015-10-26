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
        public TDU_TipoArtigo Get(int id)
        {
            Lib_Primavera.Model.TDU_TipoArtigo tipoArtigo = Lib_Primavera.PriIntegration.GetTipoArtigo(id);
            if (tipoArtigo == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return tipoArtigo;
            }
        }
    }
}

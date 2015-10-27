using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElectroSinf.Lib_Primavera.Model;

namespace ElectroSinf.Controllers
{
    public class SearchController : ApiController
    {
        //
        // GET: /Artigos/

        public IEnumerable<Lib_Primavera.Model.Artigo> Get(string id)
        {
            IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.SearchArtigosNome(id);
            return artigos;
        }

        public IEnumerable<Lib_Primavera.Model.Artigo> Get()
        {
            IEnumerable<Lib_Primavera.Model.Artigo> artigos = Lib_Primavera.PriIntegration.SearchArtigosHome();
            return artigos;
        }


    }
}

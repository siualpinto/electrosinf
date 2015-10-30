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
        public IEnumerable<Lib_Primavera.Model.Artigo> Get(string search)
        {
            return  Lib_Primavera.PriIntegration.SearchArtigosNome(search.Replace("\"", ""));
        }

        public IEnumerable<Lib_Primavera.Model.Artigo> Get()
        {
            return Lib_Primavera.PriIntegration.SearchArtigosHome(); ;
        }
    }
}

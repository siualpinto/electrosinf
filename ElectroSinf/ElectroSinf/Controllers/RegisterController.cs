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
    public class Register : ApiController
    {
        public HttpResponseMessage Post(Lib_Primavera.Model.Register input)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.Register(input);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(HttpStatusCode.Created, input.cliente);
                string uri = Url.Link("DefaultApi", new { cliente = input.cliente });
                response.Headers.Location = new Uri(uri);
                response.Content = new StringContent(erro.Descricao);
                return response;
            }

            else
            {
                HttpResponseMessage error = Request.CreateResponse(HttpStatusCode.BadRequest);
                error.Content = new StringContent(erro.Descricao);
                return error;
            }
        }
    }
}
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
    public class DocVendaController : ApiController
    {
        public static Object _lock = new Object();
        public IEnumerable<Lib_Primavera.Model.DocVenda> Get()
        {
            return Lib_Primavera.PriIntegration.Encomendas_List();
        }

        /*obter faturas de um cliente e estado do pedido*/
        public List<DocVenda> Get(string id)
        {
            List<DocVenda> encomendasCliente = Lib_Primavera.PriIntegration.GET_Pedidos(id);
            if (encomendasCliente == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return encomendasCliente;
            }
        }

        public HttpResponseMessage Post(Lib_Primavera.Model.DocVenda dv)
        {
            lock (_lock)
            {
                Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
                erro = Lib_Primavera.PriIntegration.Encomendas_New(dv);

                if (erro.Erro == 0)
                {
                    var response = Request.CreateResponse(
                       HttpStatusCode.Created, dv.id);
                    string uri = Url.Link("DefaultApi", new { DocId = dv.id });
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
}

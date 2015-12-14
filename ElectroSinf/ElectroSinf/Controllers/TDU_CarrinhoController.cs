using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElectroSinf.Lib_Primavera.Model;

namespace ElectroSinf.Controllers
{
    public class TDU_CarrinhoController : ApiController
    {
        //GET http://localhost:49234/api/TDU_Carrinho/
        public IEnumerable<Lib_Primavera.Model.TDU_Carrinho> Get()
        {
            return Lib_Primavera.PriIntegration.ListaCarrinho();
        }

        //GET http://localhost:49234/api/TDU_Carrinho/MIGUEL
        public List<TDU_Carrinho> Get(string id)
        {
            List<TDU_Carrinho> carrinho = Lib_Primavera.PriIntegration.GetCarrinhoCliente(id);
            if (carrinho == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return carrinho;
            }
        }

        //POST http://localhost:49234/api/TDU_Carrinho/
        public HttpResponseMessage Post(TDU_Carrinho carrinhoLinha)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereCarrinhoObj(carrinhoLinha);
            if (erro.Erro == 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, erro.Descricao);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }
        //POST http://localhost:49234/api/TDU_Carrinho/
        public HttpResponseMessage Put(TDU_Carrinho carrinhoLinha)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.UpdateCarrinhoObj(carrinhoLinha);
            if (erro.Erro == 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, erro.Descricao);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }

        //DELETE http://localhost:49234/api/TDU_Carrinho/
        public HttpResponseMessage Delete(TDU_Carrinho carrinho)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            try
            {
                erro = Lib_Primavera.PriIntegration.DelArtigoCarrinho(carrinho);
                if (erro.Erro == 0)
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                else return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
            }
            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao + "|" + exc.Message);
            }
        }
    }
}

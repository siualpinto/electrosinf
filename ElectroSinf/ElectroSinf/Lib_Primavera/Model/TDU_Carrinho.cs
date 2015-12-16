using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS800;
using Interop.StdPlatBS800;
using Interop.StdBE800;
using Interop.GcpBE800;
using ADODB;
using Interop.IGcpBS800;

namespace ElectroSinf.Lib_Primavera.Model
{
    public class TDU_Carrinho
    {
        public string CDU_IdArtigo { get; set; }
        public string CDU_IdCliente { get; set; }
        public int CDU_Quantidade { get; set; }
        public int Stock { get; set; }
        public double PrecoTotal { set; get; }
        public string Nome { set; get; }
        //AQUI
        public string CDU_Armazem { set; get; }


        //ALTERADO PARA RECEBER ARMAZEM
        public static List<TDU_Carrinho> toCarrinhoList(Interop.StdBE800.StdBELista objList)
        {
            double pvp1 = 0, iva = 0;
            string idArtigo, descricao;
            List<Model.TDU_Carrinho> listCarrinho = new List<TDU_Carrinho>();
            while (!objList.NoFim())
            {
                idArtigo = objList.Valor("CDU_IdArtigo");
                descricao = PriEngine.Engine.Comercial.Artigos.DaValorAtributo(idArtigo, "Descricao");
                pvp1 = PriEngine.Engine.Comercial.ArtigosPrecos.DaPrecoArtigoMoeda(idArtigo, "EUR", "UN", "PVP1", false, 0);
                iva = Convert.ToDouble(PriEngine.Engine.Comercial.Artigos.DaValorAtributo(idArtigo, "IVA"));
                pvp1 = pvp1 * (1 + iva / 100.0);
                listCarrinho.Add(new TDU_Carrinho
                {
                    CDU_IdArtigo = idArtigo,
                    CDU_IdCliente = objList.Valor("CDU_IdCliente"),
                    CDU_Quantidade = objList.Valor("CDU_Quantidade"),
                    PrecoTotal = Math.Round((pvp1 * objList.Valor("CDU_Quantidade")),2),
                    Nome = descricao,
                    Stock = (int)PriEngine.Engine.Consulta("select SUM(StkActual) Stock from ArtigoArmazem where Artigo = '"+idArtigo+"';").Valor("Stock"),
                    CDU_Armazem = PriEngine.Engine.Consulta("select Descricao From Armazens where Armazem='"+objList.Valor("CDU_Armazem")+"'").Valor("Descricao")
                });
                objList.Seguinte();

            }
            return listCarrinho;
        }
    }
}
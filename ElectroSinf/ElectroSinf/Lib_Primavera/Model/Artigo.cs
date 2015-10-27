using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.StdBE800;

namespace ElectroSinf.Lib_Primavera.Model
{
    public class Artigo
    {
        public Artigo() { }
        public Artigo(Interop.GcpBE800.GcpBEArtigo objArtigo, double pvp1)
        {
            CodArtigo = objArtigo.get_Artigo();
            DescArtigo = objArtigo.get_Descricao();
            if (objArtigo.get_Marca() != "")
            {
                Marca = PriEngine.Engine.Comercial.Marcas.Edita(objArtigo.get_Marca()).get_Descricao();
            }
            if (objArtigo.get_Modelo() != "" && objArtigo.get_Marca() != "")
            {
                Modelo = PriEngine.Engine.Comercial.Modelos.Edita(objArtigo.get_Marca(), objArtigo.get_Modelo()).get_Descricao();
            }
            Preco = Math.Round(pvp1 * (1 + Convert.ToDouble(objArtigo.get_IVA()) / 100.0), 2);
            Especificacoes = PriIntegration.ListaEspecificacoesArtigo(CodArtigo);
            int cdu_tipo = PriEngine.Engine.Comercial.Artigos.DaValorAtributo(CodArtigo, "CDU_Tipo");
            Tipo = PriIntegration.GetTipoArtigo(cdu_tipo).CDU_TipoArtigo;
            StdBECamposChave cdu_tipo_chave = new StdBECamposChave();
            cdu_tipo_chave.AddCampoChave("CDU_IdTipo", cdu_tipo);
            int cdu_categoria = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_TipoArtigo", cdu_tipo_chave, "CDU_Categoria");
            StdBECamposChave cdu_categoria_chave = new StdBECamposChave();
            cdu_categoria_chave.AddCampoChave("CDU_IdCategoria", cdu_categoria);
            Categoria = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_Categoria", cdu_categoria_chave, "CDU_Categoria");
            Stock = objArtigo.get_StkActual();
        }
        public string CodArtigo { get; set; }
        public string DescArtigo { get; set; }
        public string Tipo { get; set; }
        public string Categoria { get; set; }
        public string Marca { set; get; }
        public string Modelo { set; get; }
        public double Preco { set; get; }
        public Double Stock { set; get; }
        public List<Model.TDU_Especificacao> Especificacoes { set; get; }
    }
}
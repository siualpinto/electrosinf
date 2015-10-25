using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroSinf.Lib_Primavera.Model
{
    public class Artigo
    {
        public Artigo() { }
        public Artigo(Interop.GcpBE800.GcpBEArtigo objArtigo, double pvp1)
        {
            CodArtigo = objArtigo.get_Artigo();
            DescArtigo = objArtigo.get_Descricao();
            Familia = PriEngine.Engine.Comercial.Familias.Edita(objArtigo.get_Familia()).get_Descricao();
            SubFamilia = PriEngine.Engine.Comercial.Familias.EditaSubFamilia(objArtigo.get_SubFamilia(), objArtigo.get_Familia()).get_Descricao();
            Peso = objArtigo.get_Peso();
            Volume = objArtigo.get_Volume();
            Marca = PriEngine.Engine.Comercial.Marcas.Edita(objArtigo.get_Marca()).get_Descricao();
            Dim1 = objArtigo.get_Dimensao1();
            Dim2 = objArtigo.get_Dimensao2();
            Dim3 = objArtigo.get_Dimensao3();
            Modelo = PriEngine.Engine.Comercial.Modelos.Edita(objArtigo.get_Marca(), objArtigo.get_Modelo()).get_Descricao();
            Preco = pvp1 * (1 + Convert.ToDouble(objArtigo.get_IVA()) / 100.0);
        }
        public string CodArtigo { get; set; }
        public string DescArtigo { get; set; }
        public string Familia { get; set; }
        public string SubFamilia { get; set; }
        public double Peso { set; get; }
        public double Volume { set; get; }
        public string Marca { set; get; }
        public string Dim1 { set; get; }
        public string Dim2 { set; get; }
        public string Dim3 { set; get; }
        public string Modelo { set; get; }
        public double Preco { set; get; }
        public string Disponibilidade { set; get; }
    }
}
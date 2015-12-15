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
using ElectroSinf.Lib_Primavera.Model;
//using Interop.StdBESql800;
//using Interop.StdBSSql800;

namespace ElectroSinf.Lib_Primavera
{
    public class PriIntegration
    {
        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {

            GcpBEArtigo objArtigo;
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                objArtigo = PriEngine.Engine.Comercial.Artigos.Consulta(codArtigo);
                if (objArtigo == null)
                    return null;
                double pvp1 = PriEngine.Engine.Comercial.ArtigosPrecos.DaPrecoArtigoMoeda(codArtigo, "EUR", "UN", "PVP1", false, 0);
                myArt = new Model.Artigo(objArtigo, pvp1);
                return myArt;
            }
            else
            {
                return null;
            }

        }

        public static List<Model.Artigo> ListaArtigos()
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("artigo");
                    art.DescArtigo = objList.Valor("descricao");

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }

        public static List<Artigo> getRelacionados(string id, int tipo, double pvp1)
        {
            List<Artigo> relacionados = new List<Artigo>();
            double precoMax = pvp1 * 1.25;
            string precoString = precoMax.ToString();
            precoString = precoString.Replace(",", ".");

            StdBELista lst = new StdBELista();
            lst = PriEngine.Engine.Consulta("IF(SELECT COUNT(DISTINCT Artigo) Total FROM LinhasDoc WHERE Data >= DATEADD(month,-3,GETDATE()) AND Artigo IN (SELECT Artigo FROM Artigo WHERE CDU_Tipo=" + tipo + ") AND LinhasDoc.PrecUnit <= " + precoString + ") >$4"
                                        + "BEGIN "
                                        + "	SELECT Art.Artigo, Descricao, PVP1, IVA, Total "
                                        + "	FROM "
                                        + "		(SELECT Artigo.Artigo, Descricao, PVP1, IVA "
                                        + "		FROM Artigo "
                                        + "		JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo"
                                        + "		WHERE CDU_Tipo=" + tipo + " AND PVP1 <= " + precoString + " AND Artigo.Artigo!='" + id + "' ) AS Art "
                                        + "		JOIN (SELECT TOP 5 Artigo, COUNT(*) Total "
                                        + "			FROM LinhasDoc "
                                        + "			WHERE Data >= DATEADD(month,-3,GETDATE())"
                                        + "			GROUP BY Artigo "
                                        + "			HAVING COUNT(*) > 1"
                                        + "			ORDER BY COUNT(*) DESC) AS Linhas ON Art.Artigo = Linhas.Artigo "
                                        + "END "
                                        + "ELSE "
                                        + "SELECT Artigo.Artigo, Descricao, PVP1, IVA "
                                        + "FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo "
                                        + "WHERE CDU_Tipo=" + tipo + " AND Artigo.Artigo!='" + id + "' "
                                        + "ORDER BY NEWID() "
                                        + "SET ROWCOUNT 5");

            Artigo art;
            while (!lst.NoFim())
            {
                art = new Artigo();
                art.CodArtigo = lst.Valor("artigo");
                art.DescArtigo = lst.Valor("descricao");
                art.Preco = Math.Round(lst.Valor("PVP1") * (1 + Convert.ToDouble(lst.Valor("IVA")) / 100.0), 2);
                relacionados.Add(art);
                lst.Seguinte();
            }
            return relacionados;
        }
        #endregion Artigo
        #region DocVenda
        public static Model.RespostaErro Encomendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();

            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();

            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Model.LinhaDocVenda> lstlindv = new List<Model.LinhaDocVenda>();

            try
            {
                if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                {
                    //atributos que por enquanto não são dinamicos:
                    string serie = "1";
                    double desconto = 0.0;
                    string armazem = "A1";

                    // Atribui valores ao cabecalho do doc
                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Serie(serie);
                    myEnc.set_Tipodoc(dv.DocType);
                    myEnc.set_TipoEntidade("C");
                    // Linhas do documento para a lista de linhas
                    //lstlindv = dv.LinhasDoc;
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    double pvp1 = 0, quantidade = 0;
                    string idArtigo = "";
                    int Stock = 0;
                    StdBELista carrinho = PriEngine.Engine.Consulta("SELECT * FROM TDU_Carrinho WHERE CDU_IdCliente='" + dv.Entidade + "'");
                    while (!carrinho.NoFim())
                    {
                        idArtigo = carrinho.Valor("CDU_IdArtigo");
                        quantidade = Convert.ToDouble(carrinho.Valor("CDU_Quantidade"));
                        armazem = carrinho.Valor("CDU_Armazem");
                        Stock = (int)PriEngine.Engine.Comercial.ArtigosArmazens.DaStockArtigo(idArtigo);
                        //ARMAZEM
                        if (quantidade > Stock)
                        {
                            erro.Erro = 1;
                            erro.Descricao = "quantidadeErrada";
                            return erro;
                        }
                        pvp1 = PriEngine.Engine.Comercial.ArtigosPrecos.DaPrecoArtigoMoeda(idArtigo, "EUR", "UN", "PVP1", false, 0);
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, idArtigo, quantidade, armazem, "", pvp1, desconto);
                        carrinho.Seguinte();
                    }

                    /*
                    foreach (Model.LinhaDocVenda lin in lstlindv)
                    {
                        pvp1 = PriEngine.Engine.Comercial.ArtigosPrecos.DaPrecoArtigoMoeda(lin.CodArtigo, "EUR", "UN", "PVP1", false, 0);
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, armazem, "", pvp1, desconto);
                    }
                    */
                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    carrinho.Inicio();
                    while (!carrinho.NoFim())
                    {
                        idArtigo = carrinho.Valor("CDU_IdArtigo");
                        StdBECamposChave tdu_carrinho = new StdBECamposChave();
                        tdu_carrinho.AddCampoChave("CDU_IdCliente", dv.Entidade);
                        tdu_carrinho.AddCampoChave("CDU_IdArtigo", idArtigo);
                        PriEngine.Engine.TabelasUtilizador.Remove("TDU_Carrinho", tdu_carrinho);
                        carrinho.Seguinte();
                    }
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                try
                {
                    PriEngine.Engine.DesfazTransaccao();
                }
                catch (Exception)
                {

                }
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                Console.Write(erro.Descricao);
                return erro;

            }
        }
        public static List<Model.DocVenda> Encomendas_List()
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='FA'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }
        public static List<Model.DocVenda> GET_Pedidos(string idCliente)
        {
            StdBELista objList, objListLin;
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();
            LinhaDocVenda lindv;

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("select Id,Data, Estado from CabecDoc JOIN CabecDocStatus ON CabecDoc.Id = CabecDocStatus.IdCabecDoc where TipoDoc = 'ECL' and Entidade = '" + idCliente + "'");
                while (!objList.NoFim())
                {
                    Model.DocVenda dv = new Model.DocVenda();
                    dv.id = objList.Valor("Id");
                    dv.Data = objList.Valor("Data");
                    if (objList.Valor("Estado") == "T")
                    {
                        dv.estado = "Pronto";
                    }
                    else if (objList.Valor("Estado") == "P")
                    {
                        dv.estado = "Em Espera";
                    }
                    else dv.estado = "Anulado";

                    objListLin = PriEngine.Engine.Consulta("SELECT Artigo,Descricao,Quantidade from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;

                    listdv.Add(dv);
                    objList.Seguinte();
                }
            }
            return listdv;
        }
        /*
        public static Model.DocVenda Encomenda_Get(string numdoc)
        {


            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {


                string st = "SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='FA' and NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dv = new Model.DocVenda();
                dv.id = objListCab.Valor("id");
                dv.Entidade = objListCab.Valor("Entidade");
                dv.NumDoc = objListCab.Valor("NumDoc");
                dv.Data = objListCab.Valor("Data");
                dv.TotalMerc = objListCab.Valor("TotalMerc");
                dv.Serie = objListCab.Valor("Serie");
                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                listlindv = new List<Model.LinhaDocVenda>();

               

                while (!objListLin.NoFim())
                {
                    lindv = new Model.LinhaDocVenda();
                    lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindv.CodArtigo = objListLin.Valor("Artigo");
                    lindv.DescArtigo = objListLin.Valor("Descricao");
                    lindv.Quantidade = objListLin.Valor("Quantidade");
                    lindv.Unidade = objListLin.Valor("Unidade");
                    lindv.Desconto = objListLin.Valor("Desconto1");
                    lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    listlindv.Add(lindv);
                    objListLin.Seguinte();
                }

                dv.LinhasDoc = listlindv;
                return dv;
            }
            return null;
        }
        */
        //public static Model.RespostaErro TransformaDoc(string id)
        //{

        //    Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
        //    GcpBEDocumentoVenda objFact = new GcpBEDocumentoVenda();
        //    GcpBEDocumentoVenda objRE = new GcpBEDocumentoVenda();
        //    GcpBELinhasDocumentoVenda objLinEnc = new GcpBELinhasDocumentoVenda();
        //    PreencheRelacaoVendas rl = new PreencheRelacaoVendas();

        //    GcpBELinhasDocumentoVenda lstlindc = new GcpBELinhasDocumentoVenda();

        //    try
        //    {
        //        if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
        //        {
        //            if (!PriEngine.Engine.Comercial.Vendas.ExisteID(id))
        //            {
        //                erro.Erro = 1;
        //                erro.Descricao = "Documento inexistente";
        //                return erro;
        //            }
        //            objFact = PriEngine.Engine.Comercial.Vendas.EditaID(id);

        //            // --- Criar os cabeçalhos da RE
        //            objRE.set_Entidade(objFact.get_Entidade());
        //            objRE.set_Serie("1");
        //            objRE.set_Tipodoc("ECL");
        //            objRE.set_TipoEntidade("C");

        //            objRE = PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(objRE, rl);

        //            lstlindc = objFact.get_Linhas();
        //            for (int i = 1; i <= lstlindc.NumItens; i++)
        //            {
        //                PriEngine.Engine.Comercial.Internos.CopiaLinha("V", objFact, "V", objRE, i,lstlindc[i].get_Quantidade());
        //            }
        //            PriEngine.Engine.IniciaTransaccao();
        //            PriEngine.Engine.Comercial.Vendas.Actualiza(objFact, "");
        //            PriEngine.Engine.Comercial.Vendas.Actualiza(objRE, "");
        //            PriEngine.Engine.TerminaTransaccao();

        //            erro.Erro = 0;
        //            erro.Descricao = "Sucesso";
        //            return erro;
        //        }
        //        else
        //        {
        //            erro.Erro = 1;
        //            erro.Descricao = "Erro ao abrir empresa";
        //            return erro;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            PriEngine.Engine.DesfazTransaccao();
        //        }catch { }
        //        erro.Erro = 1;
        //        erro.Descricao = ex.Message;
        //        return erro;
        //    }
        //}
        #endregion DocVenda
        #region TDU_Carrinho
        public static List<Model.TDU_Carrinho> ListaCarrinho()
        {
            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                return Model.TDU_Carrinho.toCarrinhoList(PriEngine.Engine.Consulta("SELECT * FROM  TDU_Carrinho"));
            }
            else return null;
        }
        public static Model.Cliente GetCarrinhoCliente(string codCliente)
        {
            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                GcpBECliente cli = PriEngine.Engine.Comercial.Clientes.Consulta(codCliente);
                if (cli == null)
                {
                    return null;
                }
                Cliente cliente = new Cliente();
                cliente.carrinho = TDU_Carrinho.toCarrinhoList(PriEngine.Engine.Consulta("SELECT * FROM TDU_Carrinho WHERE CDU_IdCliente='" + codCliente + "'"));
                cliente.CodPostal = cli.get_CodigoPostal();
                cliente.Distrito = PriEngine.Engine.Consulta("select Descricao from Distritos where Distrito=" + cli.get_Distrito() + ";").Valor("Descricao");
                cliente.Localidade = cli.get_Localidade();
                cliente.LocalidadeCodPostal = cli.get_LocalidadeCodigoPostal();
                cliente.Morada = cli.get_Morada();
                cliente.Pais = cli.get_Pais();
                cliente.NumContribuinte = cli.get_NumContribuinte();
                cliente.NumTelefone = cli.get_Telefone();
                return cliente;
            }

            return null;
        }
        public static Lib_Primavera.Model.RespostaErro DelArtigoCarrinho(Model.TDU_Carrinho carrinho)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_carrinho = new StdBECamposChave();

            try
            {
                if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                {
                    tdu_carrinho.AddCampoChave("CDU_IdCliente", carrinho.CDU_IdCliente);
                    tdu_carrinho.AddCampoChave("CDU_IdArtigo", carrinho.CDU_IdArtigo);
                    PriEngine.Engine.TabelasUtilizador.Remove("TDU_Carrinho", tdu_carrinho);
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        //MUDADO PARA RECEBER ARMAZEM

        public static Lib_Primavera.Model.RespostaErro UpdateCarrinhoObj(Model.TDU_Carrinho carrinho)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_carrinhoChaves = new StdBECamposChave();
            if (carrinho.CDU_Quantidade < 1)
            {
                erro.Erro = 1;
                erro.Descricao = "Quantidade errada";
                return erro;
            }
            try
            {
                if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                {
                    tdu_carrinhoChaves.AddCampoChave("CDU_IdCliente", carrinho.CDU_IdCliente);
                    tdu_carrinhoChaves.AddCampoChave("CDU_IdArtigo", carrinho.CDU_IdArtigo);

                    if (PriEngine.Engine.TabelasUtilizador.Existe("TDU_Carrinho", tdu_carrinhoChaves))
                    {
                        PriEngine.Engine.TabelasUtilizador.ActualizaValorAtributo("TDU_Carrinho", tdu_carrinhoChaves, "CDU_Quantidade", carrinho.CDU_Quantidade);
                    }
                    else
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Artigo não existe no carrinho";
                        return erro;
                    }
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

        public static Lib_Primavera.Model.RespostaErro InsereCarrinhoObj(Model.TDU_Carrinho carrinho)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            StdBECamposChave tdu_carrinhoChaves = new StdBECamposChave();
            StdBERegistoUtil tdu_carrinhoNovo = new StdBERegistoUtil();
            StdBECampos cmps = new StdBECampos();
            StdBECampo idCliente = new StdBECampo();
            StdBECampo idArtigo = new StdBECampo();
            StdBECampo quantidade = new StdBECampo();
            int quantidadeExistente = 0;
            //AQUI
            StdBECampo armazem = new StdBECampo();

            if (carrinho.CDU_Quantidade < 1)
            {
                erro.Erro = 1;
                erro.Descricao = "Quantidade errada";
                return erro;
            }

            try
            {
                if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                {

                    StdBELista objListCab;
                    string st = "SELECT Armazem From Armazens where Descricao='" + carrinho.CDU_Armazem + "'";
                    objListCab = PriEngine.Engine.Consulta(st);


                    tdu_carrinhoChaves.AddCampoChave("CDU_IdCliente", carrinho.CDU_IdCliente);
                    tdu_carrinhoChaves.AddCampoChave("CDU_IdArtigo", carrinho.CDU_IdArtigo);
                    //AQUI
                    tdu_carrinhoChaves.AddCampoChave("CDU_Armazem", objListCab.Valor("Armazem"));

                    if (PriEngine.Engine.TabelasUtilizador.Existe("TDU_Carrinho", tdu_carrinhoChaves))
                    {
                        quantidadeExistente = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_Carrinho", tdu_carrinhoChaves, "CDU_Quantidade");
                        quantidadeExistente += carrinho.CDU_Quantidade;
                        PriEngine.Engine.TabelasUtilizador.ActualizaValorAtributo("TDU_Carrinho", tdu_carrinhoChaves, "CDU_Quantidade", quantidadeExistente);
                    }
                    else
                    {

                        idCliente.Nome = "CDU_IdCliente";
                        idArtigo.Nome = "CDU_IdArtigo";
                        quantidade.Nome = "CDU_Quantidade";
                        idCliente.Valor = carrinho.CDU_IdCliente;
                        idArtigo.Valor = carrinho.CDU_IdArtigo;
                        quantidade.Valor = carrinho.CDU_Quantidade + quantidadeExistente;
                        //AQUI
                        armazem.Nome = "CDU_Armazem";


                       

                        armazem.Valor = objListCab.Valor("Armazem");

                        cmps.Insere(idCliente);
                        cmps.Insere(idArtigo);
                        cmps.Insere(quantidade);
                        //AQUI
                        cmps.Insere(armazem);

                        tdu_carrinhoNovo.set_Campos(cmps);
                        PriEngine.Engine.TabelasUtilizador.Actualiza("TDU_Carrinho", tdu_carrinhoNovo);
                    }
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }
        #endregion TDU_Carrinho
        #region Categoria
        public static Lib_Primavera.Model.TDU_Categoria GetCategoria(int id)
        {
            StdBECamposChave pk = new StdBECamposChave();
            //Primary Key of TDU_Categoria Table 
            pk.AddCampoChave("CDU_IdCategoria", id);
            Model.TDU_Categoria myCat = new Model.TDU_Categoria();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                if (PriEngine.Engine.TabelasUtilizador.Existe("TDU_Categoria", pk) == false)
                {
                    return null;
                }
                else
                {
                    myCat.CDU_IdCategoria = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_Categoria", pk, "CDU_IdCategoria");
                    myCat.CDU_Categoria = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_Categoria", pk, "CDU_Categoria");
                    return myCat;
                }

            }
            else
            {
                return null;
            }
        }
        public static List<Model.TDU_Categoria> ListaCategorias()
        {
            StdBELista objList;

            Model.TDU_Categoria categoria = new Model.TDU_Categoria();
            List<Model.TDU_Categoria> listCategorias = new List<Model.TDU_Categoria>();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT * FROM DBO.TDU_Categoria");

                while (!objList.NoFim())
                {
                    categoria = new Model.TDU_Categoria();
                    categoria.CDU_IdCategoria = objList.Valor("CDU_IdCategoria");
                    categoria.CDU_Categoria = objList.Valor("CDU_Categoria");

                    listCategorias.Add(categoria);
                    objList.Seguinte();
                }

                return listCategorias;

            }
            else
            {
                return null;

            }
        }
        #endregion Categoria
        #region TipoArtigo
        public static Lib_Primavera.Model.TDU_TipoArtigo GetTipoArtigo(int id)
        {
            StdBECamposChave pk = new StdBECamposChave();
            //Primary Key of TDU_TipoArtigo Table 
            pk.AddCampoChave("CDU_IdTipo", id);
            Model.TDU_TipoArtigo mytip = new Model.TDU_TipoArtigo();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                if (PriEngine.Engine.TabelasUtilizador.Existe("TDU_TipoArtigo", pk) == false)
                {
                    return null;
                }
                else
                {
                    mytip.CDU_IdTipo = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_TipoArtigo", pk, "CDU_idTipo");
                    mytip.CDU_TipoArtigo = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_TipoArtigo", pk, "CDU_TipoArtigo");
                    mytip.CDU_Categoria = PriEngine.Engine.TabelasUtilizador.DaValorAtributo("TDU_TipoArtigo", pk, "CDU_Categoria");
                    return mytip;
                }

            }
            else
            {
                return null;
            }
        }
        public static List<Model.TDU_TipoArtigo> ListaTiposArtigos()
        {
            StdBELista objList;

            Model.TDU_TipoArtigo tipo = new Model.TDU_TipoArtigo();
            List<Model.TDU_TipoArtigo> listTipos = new List<Model.TDU_TipoArtigo>();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT * FROM DBO.TDU_TipoArtigo");

                while (!objList.NoFim())
                {
                    tipo = new Model.TDU_TipoArtigo();
                    tipo.CDU_IdTipo = objList.Valor("CDU_IdTipo");
                    tipo.CDU_TipoArtigo = objList.Valor("CDU_TipoArtigo");
                    tipo.CDU_Categoria = objList.Valor("CDU_Categoria");

                    listTipos.Add(tipo);
                    objList.Seguinte();
                }

                return listTipos;

            }
            else
            {
                return null;

            }
        }
        public static List<Artigo> ListaArtigosbyTipo(int id)
        {
            List<Artigo> artigos = new List<Artigo>();
            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                StdBELista lst = PriEngine.Engine.Consulta("SELECT Artigo.Artigo,Descricao,IVA,PVP1 FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo WHERE CDU_Tipo=" + id);
                Artigo art;
                while (!lst.NoFim())
                {
                    art = new Artigo();
                    art.CodArtigo = lst.Valor("Artigo");
                    art.DescArtigo = lst.Valor("Descricao");
                    art.Preco = Math.Round(lst.Valor("PVP1") * (1 + Convert.ToDouble(lst.Valor("IVA")) / 100.0), 2);
                    artigos.Add(art);
                    lst.Seguinte();
                }
                return artigos;
            }
            else
            {
                return null;

            }

        }
        #endregion TipoArtigo
        #region TDU_TipoArtigobyCDU_Categoria
        public static List<Model.TDU_TipoArtigo> ListaTiposArtigosbyCategoria(int id)
        {
            StdBELista objList;

            Model.TDU_TipoArtigo tipoByCategoria = new Model.TDU_TipoArtigo();
            List<Model.TDU_TipoArtigo> listTipos = new List<Model.TDU_TipoArtigo>();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT * FROM DBO.TDU_TipoArtigo where CDU_Categoria=" + id);

                while (!objList.NoFim())
                {
                    tipoByCategoria = new Model.TDU_TipoArtigo();
                    tipoByCategoria.CDU_IdTipo = objList.Valor("CDU_IdTipo");
                    tipoByCategoria.CDU_TipoArtigo = objList.Valor("CDU_TipoArtigo");
                    tipoByCategoria.CDU_Categoria = objList.Valor("CDU_Categoria");

                    listTipos.Add(tipoByCategoria);
                    objList.Seguinte();
                }

                return listTipos;

            }
            else
            {
                return null;

            }
        }
        #endregion TDU_TipoArtigobyCDU_Categoria
        #region TDU_Especificacao
        public static List<Model.TDU_Especificacao> ListaEspecificacoesArtigo(string idArtigo)
        {

            StdBELista objList;
            List<Model.TDU_Especificacao> listEspecs = new List<Model.TDU_Especificacao>();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT * FROM  TDU_ArtigoEspecificacao JOIN TDU_Especificacao ON TDU_ArtigoEspecificacao.CDU_IdEspecificacao=TDU_Especificacao.CDU_idEspecificacao WHERE CDU_IdArtigo ='" + idArtigo + "'");
                while (!objList.NoFim())
                {
                    listEspecs.Add(new Model.TDU_Especificacao
                    {
                        CDU_Nome = objList.Valor("CDU_Nome"),
                        CDU_Valor = objList.Valor("CDU_Valor")
                    });
                    objList.Seguinte();

                }

                return listEspecs;
            }
            else return null;
        }
        #endregion TDU_Especificacao
        #region Search
        public static List<Model.Artigo> SearchArtigosNome(string id)
        {
            StdBELista objList;
            List<Model.Artigo> listArtigos = new List<Model.Artigo>();
            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT Artigo.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo  WHERE Descricao LIKE '%" + id + "%'");


                while (!objList.NoFim())
                {
                    listArtigos.Add(new Model.Artigo
                    {
                        CodArtigo = objList.Valor("Artigo"),
                        DescArtigo = objList.Valor("Descricao"),
                        Stock = Convert.ToDouble(objList.Valor("STKActual")),
                        Marca = objList.Valor("Marca"),
                        Preco = Math.Round((Convert.ToDouble(objList.Valor("PVP1")) * (1 + Convert.ToDouble(objList.Valor("IVA")) / 100.0)), 2)
                    });
                    objList.Seguinte();

                }

                return listArtigos;
            }
            else
                return null;
        }
        public static List<List<Artigo>> SearchArtigosHome()
        {
            List<List<Artigo>> artigosHome = new List<List<Artigo>>();
            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                artigosHome.Add(toArtigoList(PriEngine.Engine.Consulta("SELECT top 3 Artigo.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo order by Artigo DESC")));

                string queryContaTop = "select COUNT(DISTINCT Artigo) Total from LinhasDoc JOIN CabecDoc ON LinhasDoc.IdCabecDoc = CabecDoc.Id where CabecDoc.Data >= DATEADD(month,-3,GETDATE()) and TipoDoc = 'FA';";

                string queryGetTop = "select Art.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda, Total "
                                        + "from (SELECT Artigo.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda "
                                        + "FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo) as Art Join"
                                        + "(select top 10 Artigo, SUM(Quantidade) Total "
                                        + "from LinhasDoc JOIN CabecDoc ON LinhasDoc.IdCabecDoc = CabecDoc.Id "
                                        + "where CabecDoc.Data >= DATEADD(month,-3,GETDATE()) and TipoDoc = 'FA' and LinhasDoc.Artigo !=''"
                                        + "GROUP BY Artigo ORDER BY SUM(Quantidade) DESC) as Linhas on Art.Artigo = Linhas.Artigo ;";

                string queryGetRandom = "SELECT top 10 Artigo.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda "
                                            + " FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo order by Artigo DESC;";
                if (PriEngine.Engine.Consulta(queryContaTop).Valor("Total") > 9)
                {
                    artigosHome.Add(toArtigoList(PriEngine.Engine.Consulta(queryGetTop)));
                }
                else
                {
                    artigosHome.Add(toArtigoList(PriEngine.Engine.Consulta(queryGetRandom)));
                }
                return artigosHome;
            }
            else
                return null;
        }

        public static List<Artigo> toArtigoList(StdBELista objList)
        {
            List<Model.Artigo> listArtigos = new List<Model.Artigo>();
            while (!objList.NoFim())
            {
                listArtigos.Add(new Model.Artigo
                {
                    CodArtigo = objList.Valor("Artigo"),
                    DescArtigo = objList.Valor("Descricao"),
                    Stock = Convert.ToDouble(objList.Valor("STKActual")),
                    Marca = objList.Valor("Marca"),
                    Preco = Math.Round((Convert.ToDouble(objList.Valor("PVP1")) * (1 + Convert.ToDouble(objList.Valor("IVA")) / 100.0)), 2)
                });
                objList.Seguinte();
            }
            return listArtigos;
        }
        #endregion Search
        #region Cliente
        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente myCli = new GcpBECliente();
            StdBELista objList;
            try
            {
                objList = PriEngine.Engine.Consulta("SELECT * FROM PRIELECSINF.[dbo].[Clientes] where CDU_Email='" + cli.Email+"'");
                if (objList.Vazia())
                {
                    if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                    {
                        StdBELista lastClient = new StdBELista();
                        if (PriEngine.Engine.Comercial.Clientes.Existe("C001"))
                        {
                            lastClient = PriEngine.Engine.Consulta("SELECT top 1 Cliente FROM PRIELECSINF.[dbo].[Clientes] WHERE Cliente LIKE 'C%' ORDER BY Cliente DESC;");
                            string b = lastClient.Valor("Cliente");
                            b = b.Replace("C", "0");
                            int x = 0;
                            Int32.TryParse(b, out x);
                            x++;
                            string n = x.ToString();
                            string codCliente = "C";
                            for (int i = 0; i < (3 - n.Length); i++)
                            {
                                codCliente = string.Concat(codCliente, "0");
                            }
                            cli.CodCliente = string.Concat(codCliente, n);
                        }
                        else
                        {
                            cli.CodCliente = "C001";
                        }
                        StdBECampos cmps = new StdBECampos();
                        StdBECampo email = new StdBECampo();
                        StdBECampo pwd = new StdBECampo();
                        email.Nome = "CDU_Email";
                        pwd.Nome = "CDU_Password";
                        email.Valor = cli.Email;
                        pwd.Valor = PriEngine.Platform.Criptografia.Encripta(cli.Password, 50);
                        cmps.Insere(email);
                        cmps.Insere(pwd);
                        myCli.set_CamposUtil(cmps);
                        myCli.set_Cliente(cli.CodCliente);
                        myCli.set_Nome(cli.NomeCliente);
                        myCli.set_NomeFiscal(cli.NomeCliente);
                        myCli.set_NumContribuinte(cli.NumContribuinte);
                        myCli.set_Moeda(cli.Moeda);
                        myCli.set_Morada(cli.Morada);
                        myCli.set_Localidade(cli.Localidade);
                        myCli.set_CodigoPostal(cli.CodPostal);
                        myCli.set_Distrito(cli.Distrito);
                        myCli.set_Pais(cli.Pais);
                        myCli.set_Telefone(cli.NumTelefone);
                        myCli.set_LocalidadeCodigoPostal(cli.LocalidadeCodPostal);
                        myCli.set_ModoPag(cli.ModoPagamento);
                        myCli.set_CondPag(cli.CondicaoPagamento);
                        PriEngine.Engine.IniciaTransaccao();
                        PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);
                        PriEngine.Engine.TerminaTransaccao();
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                    else
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Erro ao abrir empresa";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = -1;
                    erro.Descricao = "Já existe pessoa com mesmo email";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }
        public static Lib_Primavera.Model.RespostaErro login(Model.Cliente cli)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            try
            {
                if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                {
                    StdBELista cliente = PriEngine.Engine.Consulta("SELECT Cliente,CDU_Password from Clientes where CDU_Email='" + cli.Email + "';");
                    if (cliente.Vazia())
                    {
                        erro.Erro = -1;
                        erro.Descricao = "Email Errado";
                    }
                    else
                    {
                        string inserida = PriEngine.Platform.Criptografia.Encripta(cli.Password, 50);
                        if (inserida == cliente.Valor("CDU_Password"))
                        {
                            erro.Erro = 0;
                            erro.Descricao = cliente.Valor("Cliente");
                        }
                        else
                        {
                            erro.Erro = -1;
                            erro.Descricao = "Password Errada";
                        }
                    }
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }
            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }
        #endregion Cliente


        internal static List<Armazem_stock> getStock_armazem(string CodArtigo)
        {
            List<Armazem_stock> stock_armazem = new List<Armazem_stock>();

            StdBELista lst = new StdBELista();
            lst = PriEngine.Engine.Consulta("SELECT ArtigoArmazem.Armazem,ArtigoArmazem.StkActual,Armazens.Descricao,Armazens.Morada,Armazens.Localidade,Armazens.Cp,Armazens.CpLocalidade FROM ArtigoArmazem JOIN Armazens ON ArtigoArmazem.Armazem = Armazens.Armazem WHERE Artigo ='" + CodArtigo + "'");

            Armazem_stock arm_stc;
            while (!lst.NoFim())
            {
                arm_stc = new Armazem_stock();
                arm_stc.Armazem_id = lst.Valor("Armazem");
                arm_stc.Stock_qtdd = lst.Valor("StkActual");
                arm_stc.Descricao = lst.Valor("Descricao");
                arm_stc.Morada = lst.Valor("Morada");
                arm_stc.Localidade = lst.Valor("Localidade");
                arm_stc.Cp = lst.Valor("Cp");
                arm_stc.CpLocalidade = lst.Valor("CpLocalidade");

                stock_armazem.Add(arm_stc);
                lst.Seguinte();
            }
            return stock_armazem;
        }
        }
    }

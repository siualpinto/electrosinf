﻿using System;
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

            GcpBEArtigo objArtigo = new GcpBEArtigo();
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    double pvp1 = PriEngine.Engine.Comercial.ArtigosPrecos.DaPrecoArtigoMoeda(codArtigo, "EUR", "UN", "PVP1", false, 0);
                    myArt = new Model.Artigo(objArtigo, pvp1);
                    return myArt;
                }

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
                        Stock = (int)PriEngine.Engine.Comercial.ArtigosArmazens.DaStockArtigo(idArtigo);
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
        public static List<Model.TDU_Carrinho> GetCarrinhoCliente(string codCliente)
        {
            if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
            {
                return Model.TDU_Carrinho.toCarrinhoList(PriEngine.Engine.Consulta("SELECT * FROM TDU_Carrinho WHERE CDU_IdCliente='" + codCliente + "'"));
            }
            else return null;
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

            try
            {
                if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                {
                    tdu_carrinhoChaves.AddCampoChave("CDU_IdCliente", carrinho.CDU_IdCliente);
                    tdu_carrinhoChaves.AddCampoChave("CDU_IdArtigo", carrinho.CDU_IdArtigo);

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

                        cmps.Insere(idCliente);
                        cmps.Insere(idArtigo);
                        cmps.Insere(quantidade);

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

                artigosHome.Add(toArtigoList(PriEngine.Engine.Consulta("SELECT top 3 Artigo.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo order by NEWID()")));
                artigosHome.Add(toArtigoList(PriEngine.Engine.Consulta("IF(select COUNT(DISTINCT Artigo) Total from LinhasDoc where Data >= DATEADD(month,-3,GETDATE()) ) >$9 BEGIN select Art.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda, Total from (SELECT Artigo.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo) as Art Join (select top 10 Artigo, SUM(Quantidade) Total from LinhasDoc where Data >= DATEADD(month,-3,GETDATE()) GROUP BY Artigo ORDER BY SUM(Quantidade) DESC) as Linhas on Art.Artigo = Linhas.Artigo END ELSE SELECT top 10 Artigo.Artigo, Descricao, STKActual, Marca, PVP1, IVA, Moeda FROM Artigo JOIN ArtigoMoeda ON Artigo.Artigo=ArtigoMoeda.Artigo order by NEWID()")));
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
        #region Register
        public static Model.RespostaErro Register(Model.Register input)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();     

            GcpBECliente cliente = new GcpBECliente();
            try
            {
                if (PriEngine.InitializeCompany(ElectroSinf.Properties.Settings.Default.Company.Trim(), ElectroSinf.Properties.Settings.Default.User.Trim(), ElectroSinf.Properties.Settings.Default.Password.Trim()) == true)
                {                    

                    cliente.set_Cliente(input.cliente);
                    cliente.set_Nome(input.nome);
                    cliente.set_NomeFiscal(input.nome_fiscal);
                    cliente.set_Morada(input.morada);
                    cliente.set_NumContribuinte(input.numContribuinte);
              
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
        #endregion Register

    }
}
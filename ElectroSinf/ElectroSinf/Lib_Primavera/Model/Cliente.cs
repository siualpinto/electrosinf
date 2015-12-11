using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroSinf.Lib_Primavera.Model
{
    public class Cliente
    {
        public string Email
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string CodCliente
        {
            get;
            set;
        }
        public string NomeCliente
        {
            get;
            set;
        }

        public string NumContribuinte
        {
            get;
            set;
        }
        public string Moeda
        {
            get;
            set;
        }
        public string Morada
        {
            get;
            set;
        }
        public string Localidade
        {
            get;
            set;
        }
        public string CodPostal
        {
            get;
            set;
        }
        public string Distrito 
        {
            get;
            set;
        }
        public string ModoPagamento
        {
            get;
            set;
        }
        public string CondicaoPagamento
        {
            get;
            set;
        }
        public string Pais //País
        {
            get;
            set;
        }
    }
}
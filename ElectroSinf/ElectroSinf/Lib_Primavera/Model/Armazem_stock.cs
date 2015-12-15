using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.StdBE800;

namespace ElectroSinf.Lib_Primavera.Model
{
    public class Armazem_stock
    {
        public String Armazem_id
        {
            get;
            set;
        }
        public double Stock_qtdd
        {
            get;
            set;
        }
        public String Descricao
        {
            get;
            set;
        }    
        public String Morada
        {
            get;
            set;
        }        
        public String Localidade
        {
            get;
            set;
        }     
        public String Cp
        {
            get;
            set;
        }
        public String CpLocalidade
        {
            get;
            set;
        }
    }
}
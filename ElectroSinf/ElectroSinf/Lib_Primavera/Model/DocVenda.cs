﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroSinf.Lib_Primavera.Model
{
    public class DocVenda
    {

        public string id
        {
            get;
            set;
        }

        public string Entidade
        {
            get;
            set;
        }

        public int NumDoc
        {
            get;
            set;
        }

        public DateTime Data
        {
            get;
            set;
        }

        public double TotalMerc
        {
            get;
            set;
        }

        public string Serie
        {
            get;
            set;
        }

        public List<Model.LinhaDocVenda> LinhasDoc
        {
            get;
            set;
        }
        public string DocType { set; get; }
        public string estado { set; get; }
        public DateTime DataLiq { set; get; }

    }
}
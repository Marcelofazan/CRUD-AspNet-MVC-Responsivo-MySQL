using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPSimplesLTE.Models
{
    public class Situacao
    {
        public int? Id { get; set; }
        public int? Valor { get; set; }
        public string Texto { get; set; }
        public string Parametro { get; set; }
        public string Observacao { get; set; }
        public List<SelectListItem> Situacoes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ERPSimplesLTE.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Senha { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string SenhaConfirmar { get; set; }
        public string Nome { get; set; }
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Informe um email válido.")]
        public string Email { get; set; }
        public string Celular { get; set; }
        public bool Supervisor { get; set; }
        public Situacao Situacao { get; set; }
        public Usuario()
        {
            Situacao = new Situacao();
        }
        public decimal TaxaPercentual { get; set; }
        public DateTime? DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataAlteracao { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public string Observacao { get; set; }
    }
}
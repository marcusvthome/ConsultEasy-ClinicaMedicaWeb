using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Models
{
    [Index(nameof(CPF), IsUnique = true)]

    public class Secretaria
    {
        [DisplayName("Identificador do Secretario(a)")]
        public int Id { get; set; }

        public int? LoginID { get; set; }

        public Login Login { get; set; }

        [Required(ErrorMessage = "O nome do Secretario(a) é obrigatório")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF do Secretario(a) é obrigatório")]
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Data de Nascimento obrigatória")]
        [DisplayName("Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public Status Status { get; set; }
    }
}

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
    [Index(nameof(CRM), IsUnique = true)]
    public class Medico
    {
        [DisplayName("Identificador do Médico(a)")]
        public int Id { get; set; }

        public int? LoginID { get; set; }

        public Login Login { get; set; }

        [Required(ErrorMessage = "O nome do Médico(a) é obrigatório")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF do Médico(a) é obrigatório")]
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O CRM do Médico(a) é obrigatório")]
        [DisplayName("CRM")]
        public string CRM { get; set; }

        [Required(ErrorMessage = "Data de Nascimento obrigatória")]
        [DisplayName("Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public Status Status { get; set; }
    }
}

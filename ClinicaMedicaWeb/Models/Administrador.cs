using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Models
{
    public class Administrador
    {
        [DisplayName("Identificador do Administrador")]
        public int Id { get; set; }

        public int? LoginID { get; set; }

        public Login Login { get; set; }

        [Required(ErrorMessage = "O nome do administrador é obrigatório")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        public Status Status { get; set; }
    }
}

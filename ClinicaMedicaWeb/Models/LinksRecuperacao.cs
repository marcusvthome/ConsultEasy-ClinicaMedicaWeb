using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaMedicaWeb.Models
{
    public class LinksRecuperacao
    {
        public string Email { get; set; }

        [NotMapped]
        public Login login {get; set;}

        public DateTime HoraRequisicao { get; set; }
    }
}

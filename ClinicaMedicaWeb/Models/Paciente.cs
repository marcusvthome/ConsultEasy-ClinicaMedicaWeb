using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Models
{
    public class Paciente
    {
        [DisplayName("Identificador do Paciente(a)")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do Paciente(a) é obrigatório")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF do Paciente(a) é obrigatório")]
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Data de Nascimento obrigatória")]
        [DisplayName("Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O Telefone do Paciente(a) é obrigatório")]
        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O Endereço do Paciente(a) é obrigatório")]
        [DisplayName("Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "O Email do Paciente(a) é obrigatório")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A profissão do Paciente(a) é obrigatório")]
        [DisplayName("Profissão")]
        public string Profissao { get; set; }

        [Required(ErrorMessage = "O sexo do Paciente(a) é obrigatório")]
        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }

        public Status Status { get; set; }
    }
}

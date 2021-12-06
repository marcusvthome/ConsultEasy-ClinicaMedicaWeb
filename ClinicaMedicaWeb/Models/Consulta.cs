using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Models
{
    public class Consulta
    {
        [DisplayName("Identificador da Consulta")]
        public int Id { get; set; }

        public int? PacienteId { get; set; }

        public Paciente Paciente { get; set; }

        public int? MedicoId { get; set; }

        public Medico Medico { get; set; }

        public int? SecretariaId { get; set; }

        public Secretaria Secretaria { get; set; }

        [Required(ErrorMessage = "Data da Consulta")]
        [DisplayName("Data da Consulta")]
        [DataType(DataType.Date)]
        public DateTime DataConsulta { get; set; }

        [Required(ErrorMessage = "Hora da Consulta")]
        [DisplayName("Hora da Consulta")]
        [DataType(DataType.Time)]
        public DateTime HoraConsulta { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCadastroConsulta { get; set; }

        [DataType(DataType.Time)]
        public DateTime HoraInicio { get; set; }

        [DataType(DataType.Time)]
        public DateTime HoraFim { get; set; }

        [Required(ErrorMessage = "Pagamento da Consulta")]
        [DisplayName("Pagamento da Consulta")]
        public string Pagamento { get; set; }

        public bool Presenca { get; set; }

        public string Observacao { get; set; }

        public StatusConsulta Status { get; set; }

        public string MotivoCancelamento { get; set; }

        public int? SecretariaIdCancelamento { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCancelamento { get; set; }

        [NotMapped]
        public string NomeMedico { get; set; }

        [NotMapped]
        public string NomePaciente { get; set; }

        [NotMapped]
        [MinLength(8, ErrorMessage = "Utilize 8 caracteres")]
        [MaxLength(8, ErrorMessage = "Utilize ao máximo 8 caracteres")]
        [DataType(DataType.Password)]
        public string Chave { get; set; }

        public bool Privado { get; set; }
    }
}
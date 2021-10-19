using ClinicaMedicaWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //Exemplo de criação de tabela
        //public DbSet<NomeDaModel> NomeDaTabela { get; set; }
        public DbSet<Login> TBLogin { get; set; }
        public DbSet<Administrador> TBAdministrador { get; set; }
        public DbSet<Medico> TBMedico { get; set; }
        public DbSet<Secretaria> TBSecretaria { get; set; }

    }
}

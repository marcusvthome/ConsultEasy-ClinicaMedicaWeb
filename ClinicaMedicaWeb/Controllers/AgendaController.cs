using ClinicaMedicaWeb.Data;
using ClinicaMedicaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Controllers
{
    [Authorize(Roles = "Medico")]
    public class AgendaController : Controller
    {
        private readonly DataContext dataContext;

        public AgendaController(DataContext dc)
        {
            dataContext = dc;
        }

        public string Encrypt(Consulta consulta)
        {
            try
            {
                string textToEncrypt = consulta.Observacao;
                string ToReturn = "";
                string publickey = consulta.Chave;
                string secretkey = consulta.Chave;
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message, ex.InnerException);
                return "Erro";
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Consulta consulta)
        {
            if (consulta == null)
                return RedirectToAction("Index");
            else
            {
                if (consulta.NomePaciente == null && consulta.NomeMedico == null)
                {
                    consulta.NomePaciente = "";
                    consulta.NomeMedico = "";
                    List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.Presenca == true).Where(x => x.DataConsulta == consulta.DataConsulta).OrderBy(a => a.DataConsulta).ToList();
                    return View(lista);
                }
                else
                {
                    if (consulta.NomePaciente != null && consulta.NomeMedico == null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Presenca == true).Where(x => x.Status != StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.NomePaciente != null && consulta.NomeMedico != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.Presenca == true).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.NomePaciente == null && consulta.NomeMedico != null)
                    {
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.Presenca == true).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date)
                    {
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.Presenca == true).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomePaciente != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.Presenca == true).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomeMedico != null)
                    {
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.Presenca == true).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomePaciente != null && consulta.NomeMedico != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.Presenca == true).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                }

                return View();
            }
        }

        public IActionResult Index()
        {
            DateTime dataHoje = DateTime.Now;
            int idMedico = int.Parse(@User.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.Sid).Value);
            List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.MedicoId == idMedico).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.Status != StatusConsulta.Finalizado).Where(x => x.DataConsulta == dataHoje.Date).Where(x => x.Presenca == true).OrderBy(a => a.DataConsulta).ToList();
            return View(lista);
        }

        public IActionResult DetalharConsulta(int? Id)
        {
            if (Id.HasValue)
            {
                Consulta consulta = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).FirstOrDefault(x => x.Id == Id);
                if (consulta != null)
                {
                    if (consulta.Medico.Id.ToString() != @User.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.Sid).Value)
                    {
                        TempData["TipoMensagem"] = "ERRO";
                        TempData["Mensagem"] = "Você não pode dar início a uma consulta que não é sua";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        consulta.HoraInicio = DateTime.Now;
                        consulta.Status = StatusConsulta.Consultando;

                        dataContext.TBConsulta.Update(consulta);
                        dataContext.SaveChanges();
                        return View(consulta);
                    }

                }

            }

            return NoContent();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetalharConsulta(Consulta consulta)
        {
            Consulta consulta2 = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).FirstOrDefault(x => x.Id == consulta.Id);

            consulta2.HoraFim = DateTime.Now;

            if(consulta.Chave != null)
            {
                string encrypted = Encrypt(consulta);
                consulta2.Observacao = encrypted;
                consulta2.Privado = true;
            }
            else
            {
                consulta2.Observacao = consulta.Observacao;
                consulta2.Privado = false;
            }

            consulta2.Status = StatusConsulta.Finalizado;
            consulta.Medico = null;
            consulta.Paciente = null;

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (dataContext.TBConsulta.Any(x => x.Id == consulta.Id))
                {
                    try
                    {
                        dataContext.TBConsulta.Update(consulta2);
                        dataContext.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "A consulta não pode ser encerrada";
                        return View();
                    }

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = "Consulta foi finalizada com sucesso";
                    return RedirectToAction("Index");
                }
            }
            return NoContent();
        }
    }
}


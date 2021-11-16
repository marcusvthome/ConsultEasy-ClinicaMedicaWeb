using ClinicaMedicaWeb.Data;
using ClinicaMedicaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Controllers
{
    [Authorize(Roles = "Secretaria")]
    public class ConsultaController : Controller
    {
        private readonly DataContext dataContext;

        public ConsultaController(DataContext dc)
        {
            dataContext = dc;
        }

        public IActionResult Create()
        {
            var listaMedico = dataContext.TBMedico.Where(x => x.Status == Status.Ativo).OrderBy(x => x.Nome).Select(m => new
            {
                MedicoId = m.Id,
                MedicoNome = m.Nome
            }).ToList();

            ViewBag.Medicos = new SelectList(listaMedico, "MedicoId", "MedicoNome");

            var listaPaciente = dataContext.TBPaciente.Where(x => x.Status == Status.Ativo).OrderBy(x => x.Nome).Select(m => new
            {
                PacienteId = m.Id,
                PacienteNome = m.Nome
            }).ToList();

            ViewBag.Pacientes = new SelectList(listaPaciente, "PacienteId", "PacienteNome");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Consulta consulta)
        {
            var listaMedico = dataContext.TBMedico.Where(x => x.Status == Status.Ativo).OrderBy(x => x.Nome).Select(m => new
            {
                MedicoId = m.Id,
                MedicoNome = m.Nome
            }).ToList();

            ViewBag.Medicos = new SelectList(listaMedico, "MedicoId", "MedicoNome");

            var listaPaciente = dataContext.TBPaciente.Where(x => x.Status == Status.Ativo).OrderBy(x => x.Nome).Select(m => new
            {
                PacienteId = m.Id,
                PacienteNome = m.Nome
            }).ToList();

            ViewBag.Pacientes = new SelectList(listaPaciente, "PacienteId", "PacienteNome");

            List<Consulta> lista = dataContext.TBConsulta.Where(x => x.Status != StatusConsulta.Cancelado).ToList();

            if (lista != null)
            {
                foreach (Consulta consulta2 in lista)
                {
                    if (consulta2.MedicoId == consulta.MedicoId)
                    {
                        if (consulta2.DataConsulta == consulta.DataConsulta && consulta2.HoraConsulta == consulta.HoraConsulta)
                        {
                            ViewBag.TipoMensagem = "ERRO";
                            ViewBag.Mensagem = "Há uma consulta agendada neste horário!";
                            return View();

                        }
                    }
                }
            }
            if (consulta.DataConsulta < DateTime.Now.Date)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A consulta não pode ser agendada em uma data ou hora passada!";
                return View();
            }

            consulta.Status = StatusConsulta.Agendado;
            consulta.DataCadastroConsulta = DateTime.Now;
            consulta.Presenca = false;
            consulta.Secretaria = null;

            dataContext.TBConsulta.Add(consulta);
            dataContext.SaveChanges();

            TempData["TipoMensagem"] = "SUCESSO";
            TempData["Mensagem"] = "Consulta agendada com sucesso";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Consulta consulta = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).FirstOrDefault(x => x.Id == Id);

                if (consulta != null)
                    return View(consulta);
            }

            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Consulta consulta)
        {
            Consulta consulta2 = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).FirstOrDefault(x => x.Id == consulta.Id);

            List<Consulta> lista = dataContext.TBConsulta.Where(x => x.Id != consulta.Id).Where(x => x.Status != StatusConsulta.Cancelado).ToList();

            if (lista != null)
            {
                foreach (Consulta consulta3 in lista)
                {
                    if (consulta3.MedicoId == consulta2.MedicoId)
                    {
                        if (consulta3.DataConsulta == consulta.DataConsulta && consulta3.HoraConsulta == consulta.HoraConsulta)
                        {
                            ViewBag.TipoMensagem = "ERRO";
                            ViewBag.Mensagem = "Há uma consulta agendada neste horário!";
                            return View();
                        }
                    }
                }
            }

            if (consulta.DataConsulta < DateTime.Now.Date)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A consulta não pode ser agendada em uma data ou hora passada!";
                return View();
            }
            else
            {
                consulta2.DataConsulta = consulta.DataConsulta;
                consulta2.HoraConsulta = consulta.HoraConsulta;
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
                            ViewBag.MensagemErro = "A consulta não pode ser atualizada";
                            return View();
                        }

                        TempData["TipoMensagem"] = "SUCESSO";
                        TempData["Mensagem"] = "Consulta atualizada com sucesso";
                        return RedirectToAction("Index");
                    }
                }
            }
            return NoContent();
        }

        public IActionResult Delete(int? ID)
        {
            if (ID.HasValue)
            {
                Consulta consulta = dataContext.TBConsulta.FirstOrDefault(x => x.Id == ID);

                if (consulta != null)
                    return View(consulta);
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Consulta consulta)
        {
            Consulta consulta2 = dataContext.TBConsulta.FirstOrDefault(x => x.Id == consulta.Id);

            if (consulta2 != null)
            {
                consulta2.Status = StatusConsulta.Cancelado;
                consulta2.SecretariaIdCancelamento = consulta.SecretariaIdCancelamento;
                consulta2.DataCancelamento = DateTime.Now;
                consulta2.MotivoCancelamento = consulta.MotivoCancelamento;

                dataContext.TBConsulta.Update(consulta2);
                dataContext.SaveChanges();

                TempData["TipoMensagem"] = "SUCESSO";
                TempData["Mensagem"] = $"A consulta foi cancelada com sucesso";
                return RedirectToAction("Index");
            }
            return NoContent();
        }

        public ActionResult ConfirmarPresenca(int? ID)
        {
            if (ID.HasValue)
            {
                Consulta consulta = dataContext.TBConsulta.FirstOrDefault(x => x.Id == ID);

                if (consulta != null)
                {
                    consulta.Presenca = true;

                    dataContext.TBConsulta.Update(consulta);
                    dataContext.SaveChanges();

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = $"A presença do paciente nesta consulta foi confirmada com sucesso";
                    return RedirectToAction("Index");
                }
            }
            return NoContent();
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
                    List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.DataConsulta == consulta.DataConsulta).OrderBy(a => a.DataConsulta).ToList();
                    return View(lista);
                }
                else
                {
                    if (consulta.NomePaciente != null && consulta.NomeMedico == null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Status != StatusConsulta.Cancelado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.NomePaciente != null && consulta.NomeMedico != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.Status != StatusConsulta.Cancelado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.NomePaciente == null && consulta.NomeMedico != null)
                    {
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.Status != StatusConsulta.Cancelado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date)
                    {
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomePaciente != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomeMedico != null)
                    {
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomePaciente != null && consulta.NomeMedico != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status != StatusConsulta.Cancelado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                }

                return View();
            }
        }

        public IActionResult Index()
        {
            List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Status != StatusConsulta.Cancelado).Where(x => x.DataConsulta == DateTime.Now.Date).OrderBy(a => a.DataConsulta).ToList();
            return View(lista);
        }
    }
}

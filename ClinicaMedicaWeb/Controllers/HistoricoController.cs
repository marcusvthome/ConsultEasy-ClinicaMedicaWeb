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
    public class HistoricoController : Controller
    {
        private readonly DataContext dataContext;

        public HistoricoController(DataContext dc)
        {
            dataContext = dc;
        }

        public string Decrypt(Consulta consulta)
        {
            try
            {
                string textToDecrypt = consulta.Observacao;
                string ToReturn = "";
                string publickey = consulta.Chave;
                string secretkey = consulta.Chave;
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                //throw new Exception(ae.Message, ae.InnerException);
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
                    List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Status == StatusConsulta.Finalizado).Where(x => x.DataConsulta == consulta.DataConsulta).ToList();
                    return View(lista);
                }
                else
                {
                    if (consulta.NomePaciente != null && consulta.NomeMedico == null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Status == StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.NomePaciente != null && consulta.NomeMedico != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.Status == StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.NomePaciente == null && consulta.NomeMedico != null)
                    {
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.Status == StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date)
                    {
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status == StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomePaciente != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status == StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomeMedico != null)
                    {
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status == StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                    else if (consulta.DataConsulta >= DateTime.Now.Date && consulta.NomePaciente != null && consulta.NomeMedico != null)
                    {
                        consulta.NomePaciente = consulta.NomePaciente.Trim().ToUpper();
                        consulta.NomeMedico = consulta.NomeMedico.Trim().ToUpper();
                        List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Paciente.Nome.ToUpper().Contains(consulta.NomePaciente)).Where(x => x.Medico.Nome.ToUpper().Contains(consulta.NomeMedico)).Where(x => x.DataConsulta == consulta.DataConsulta).Where(x => x.Status == StatusConsulta.Finalizado).OrderBy(a => a.DataConsulta).ToList();
                        return View(lista);
                    }
                }

                return View();
            }
        }

        public IActionResult Index()
        {
            DateTime dataHoje = DateTime.Now;
            List<Consulta> lista = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).Where(x => x.Status == StatusConsulta.Finalizado).Where(x => x.DataConsulta == dataHoje.Date).OrderBy(a => a.DataConsulta).ToList();
            return View(lista);
        }

        public IActionResult ExibirDetalhesConsulta(int? Id)
        {
            if (Id.HasValue)
            {
                Consulta consulta = dataContext.TBConsulta.Include(x => x.Paciente).Include(x => x.Medico).FirstOrDefault(x => x.Id == Id);
                if (consulta != null)
                {
                    if (consulta.Medico.Id.ToString() != @User.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.Sid).Value)
                    {
                        TempData["TipoMensagem"] = "ERRO";
                        TempData["Mensagem"] = "Você não visualizar os detalhes da consulta que não é sua";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Privado = consulta.Privado;

                        if(consulta.Privado == true)
                        {
                            ViewBag.Observacao = "Para visualizar as observações, é necessário inserir a senha desta consulta!";
                        }
                        else
                        {
                            ViewBag.Observacao = consulta.Observacao;
                        }
                        return View(consulta);
                    }

                }

            }

            return NoContent();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExibirDetalhesConsulta(Consulta consulta)
        {
            Consulta consulta2 = dataContext.TBConsulta.Include(x => x.Medico).FirstOrDefault(x => x.Id == consulta.Id);
            consulta2.Chave = consulta.Chave;

            if (consulta2.Privado == true)
            {
                if (consulta.Chave != null)
                {
                    string descrypt = Decrypt(consulta2);
                    consulta2.Observacao = descrypt;
                    ViewBag.Observacao = consulta2.Observacao;
                    return View(consulta2);
                }
                else
                {
                    ViewBag.TipoMensagem = "ERRO";
                    ViewBag.Mensagem = "Informe a chave caso queira visualizar as observações!";
                    ViewBag.Observacao = "Informe a chave caso queira visualizar as observações!";
                    return View();
                }
            }
            else
            {
                ViewBag.Observacao = consulta2.Observacao;
                return View();
            }
        }
    }
}

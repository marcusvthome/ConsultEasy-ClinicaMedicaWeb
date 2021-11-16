using ClinicaMedicaWeb.Data;
using ClinicaMedicaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Controllers
{
    [Authorize(Roles = "Secretaria")]
    public class PacienteController : Controller
    {
        private readonly DataContext dataContext;

        public PacienteController(DataContext dc)
        {
            dataContext = dc;
        }

        public bool ValidarCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma = 0;
            int resto = 0;
            cpf = cpf.Trim(); //removendo espaços do início e do fim
            cpf = cpf.Replace(".", "").Replace("-", ""); //tirando pontos e traços

            if (cpf.Length != 11) //verificando o tamanho
            {
                return false;
            }

            tempCpf = cpf.Substring(0, 9); //pegando o cpf sem o digito e armazenando em um cpf temporário

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i]; //fazendo a soma do 1ro digito
            }

            resto = soma % 11; //definindo o resto da divisão

            if (resto < 2)
            {
                resto = 0;
            }
            else
                resto = 11 - resto;

            digito = resto.ToString(); //definindo o primeiro digito

            tempCpf = tempCpf + digito; //adicionando o primeiro digito ao cpf temporário

            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i]; //fazendo a soma do 2do digito
            }

            resto = soma % 11; //definindo o resto da divisão

            if (resto < 2)
            {
                resto = 0;
            }
            else
                resto = 11 - resto;

            digito = digito + resto.ToString(); //atribuindo o segundo dígito junto ao primeiro
            return cpf.EndsWith(digito); //retorna se o cpf termina com o digito
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paciente paciente)
        {
            List<Paciente> lista = dataContext.TBPaciente.ToList();

            if (lista != null)
            {
                foreach (Paciente paciente2 in lista)
                {
                    if (paciente2.CPF == paciente.CPF || paciente2.CPF.Replace("-", "").Replace(".", "") == paciente.CPF.Replace("-", "").Replace(".", ""))
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CPF informado já existe!";
                        return View();
                    }
                    else if (paciente2.Email == paciente.Email)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O E-mail informado já existe!";
                        return View();
                    }
                }
            }

            if (ValidarCPF(paciente.CPF) == false)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "O CPF informado é inválido!";
                return View();
            }
            else if (paciente.DataNascimento >= DateTime.Now)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A data de nascimento informada não pode ser futura!";
                return View();
            }
            else
            {
                paciente.Status = Status.Ativo;

                dataContext.TBPaciente.Add(paciente);
                dataContext.SaveChanges();

                TempData["TipoMensagem"] = "SUCESSO";
                TempData["Mensagem"] = "Paciente cadastrado com sucesso";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue)
            {
                Paciente paciente = dataContext.TBPaciente.FirstOrDefault(x => x.Id == ID);

                if (paciente != null)
                {
                    paciente.Status = Status.Inativo;

                    dataContext.TBPaciente.Update(paciente);
                    dataContext.SaveChanges();

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = $"Cadastro de {paciente.Nome} removido com sucesso";
                    return RedirectToAction("Index");
                }
            }
            return NoContent();
        }

        public IActionResult Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Paciente paciente = dataContext.TBPaciente.FirstOrDefault(x => x.Id == Id);
                if (paciente != null)
                    return View(paciente);
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Paciente paciente)
        {
            Paciente paciente2 = dataContext.TBPaciente.FirstOrDefault(x => x.Id == paciente.Id);

            List<Paciente> lista = dataContext.TBPaciente.Where(x => x.Id != paciente.Id).ToList();

            if (lista != null)
            {
                foreach (Paciente paciente3 in lista)
                {
                    if (paciente3.CPF == paciente.CPF || paciente3.CPF.Replace("-", "").Replace(".", "") == paciente.CPF.Replace("-", "").Replace(".", ""))
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CPF informado já existe!";
                        return View();
                    }
                    else if (paciente3.Email == paciente.Email)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O E-mail informado já existe!";
                        return View();
                    }
                }
            }

            if (ValidarCPF(paciente.CPF) == false)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "O CPF informado é inválido!";
                return View();
            }
            else if (paciente.DataNascimento >= DateTime.Now)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A data de nascimento informada não pode ser futura!";
                return View();
            }
            else
            {
                paciente2.Nome = paciente.Nome;
                paciente2.CPF = paciente.CPF;
                paciente2.Email = paciente.Email;
                paciente2.DataNascimento = paciente.DataNascimento;
                paciente2.Telefone = paciente.Telefone;
                paciente2.Endereco = paciente.Endereco;
                paciente2.Profissao = paciente.Profissao;
                paciente2.Sexo = paciente.Sexo;
                paciente2.Observacao = paciente.Observacao;

                ModelState.Clear();

                if (ModelState.IsValid)
                {
                    if (dataContext.TBPaciente.Any(x => x.Id == paciente.Id))
                    {
                        try
                        {
                            dataContext.TBPaciente.Update(paciente2);
                            dataContext.SaveChanges();
                        }
                        catch
                        {
                            ViewBag.MensagemErro = "O Paciente não pode ser atualizado";
                            return View();
                        }

                        TempData["TipoMensagem"] = "SUCESSO";
                        TempData["Mensagem"] = "Paciente atualizado com sucesso";
                        return RedirectToAction("Index");
                    }
                }
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Paciente paciente)
        {
            if (paciente == null)
                return RedirectToAction("Index");
            else
            {
                if (paciente.Nome == null && paciente.CPF == null)
                {
                    paciente.Nome = "";
                    paciente.CPF = "";
                    List<Paciente> lista = dataContext.TBPaciente.Where(x => x.Status != Status.Inativo).ToList();
                    return View(lista);
                }
                else
                {
                    if (paciente.Nome != null && paciente.CPF == null)
                    {
                        paciente.Nome = paciente.Nome.Trim().ToUpper();
                        List<Paciente> lista = dataContext.TBPaciente.Where(x => x.Nome.ToUpper().Contains(paciente.Nome)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                    else if (paciente.Nome != null && paciente.CPF != null)
                    {
                        paciente.Nome = paciente.Nome.Trim().ToUpper();
                        paciente.CPF = paciente.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Paciente> lista = dataContext.TBPaciente.Where(x => x.Nome.ToUpper().Contains(paciente.Nome)).Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(paciente.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }

                    else if (paciente.Nome == null && paciente.CPF != null)
                    {
                        paciente.CPF = paciente.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Paciente> lista = dataContext.TBPaciente.Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(paciente.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                }

                return View();
            }
        }

        public IActionResult Index()
        {
            List<Paciente> lista = dataContext.TBPaciente.Where(x => x.Status != Status.Inativo).ToList();
            return View(lista);
        }
    }
}

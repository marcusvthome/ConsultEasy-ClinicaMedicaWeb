using ClinicaMedicaWeb.Data;
using ClinicaMedicaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class MedicoController : Controller
    {
        private readonly DataContext dataContext;

        public MedicoController(DataContext dc)
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

        public string CalculaHashDuplo(Login login)
        {
            int count = 0;
            do
            {
                string StringToByte = login.Senha; //salvando o valor do objeto na variável
                byte[] valor = Encoding.ASCII.GetBytes(StringToByte); // salvando a string em um vetor de bytes
                var sha1 = SHA256.Create(); // criando o SHA256
                byte[] x = sha1.ComputeHash(valor); // computando o hash do valor digitado e atribuindoà um vetor de byte
                string saida = BitConverter.ToString(x); // convertendo o vetor de byte para string
                string resultado = saida.Replace("-", ""); // removendo traços e atribuindo ao resultado
                login.Senha = resultado;
                count++;

            } while (count <= 1);


            string resultadoOk = login.Senha;
            return resultadoOk;

        }

        public string GerarSenhaUsuario(Login login)
        {
            string[] removeArroba = login.Email.Split("@"); //pegando só o que vem antes do @
            login.Senha = removeArroba[0]; //transformando o email sem @ na nova senha
            string hashDuplo = CalculaHashDuplo(login); //calculando a senha hash dupla
            //char[] array = yourStringVariable.Take(5).ToArray();
            string novaSenha = hashDuplo.Substring(0, 4) + "@Ab1";  //colocando os 4 primeiros primeiros números do hash e concatenando com @Ab1
            char[] stringArray = novaSenha.Take(8).ToArray(); //colocando a senha em 1 array para randomizar
            Random random = new Random(); // criando random
            stringArray = stringArray.OrderBy(x => random.Next()).ToArray(); //embaralhando as letras/números/caracteres
            string resultado = string.Join("", stringArray);
            //string resultado =ToString(stringArray); //convertendo o array embaralhado para string

            return resultado;
        }

        public bool SendMail(Login login, string senha)
        {
            try
            {
                // Estancia da Classe de Mensagem
                MailMessage _mailMessage = new MailMessage();
                // Remetente
                _mailMessage.From = new MailAddress("contatoconsulteasy@gmail.com");

                // Destinatario seta no metodo abaixo

                //Contrói o MailMessage
                _mailMessage.To.Add(login.Email);
                _mailMessage.Subject = "Primeiro Acesso - ConsultEasy";
                _mailMessage.IsBodyHtml = true;
                _mailMessage.Body = "<b>Olá, você foi cadastrado no sistema ConsultEasy como um usuário " + login.TipoUsuario + "<br> É necessário que você logue utilizando a seguinte senha temporária " + senha;

                //CONFIGURAÇÃO COM PORTA
                SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

                //CONFIGURAÇÃO SEM PORTA
                // SmtpClient _smtpClient = new SmtpClient(UtilRsource.ConfigSmtp);

                // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação)
                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential("contatoconsulteasy@gmail.com", "Contato@@123");

                _smtpClient.EnableSsl = true;

                _smtpClient.Send(_mailMessage);

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Medico medico)
        {
            List<Medico> lista = dataContext.TBMedico.Include(x => x.Login).ToList();

            if (lista != null)
            {
                foreach (Medico medico2 in lista)
                {
                    if (medico2.CRM == medico.CRM)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CRM informado já existe!";
                        return View();
                    }
                    else if (medico2.CPF == medico.CPF || medico2.CPF.Replace("-", "").Replace(".", "") == medico.CPF.Replace("-","").Replace(".", ""))
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CPF informado já existe!";
                        return View();
                    }
                    else if (medico2.Login.Email == medico.Login.Email)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O E-mail informado já existe!";
                        return View();
                    }
                }
            }

            if (ValidarCPF(medico.CPF) == false)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "O CPF informado é inválido!";
                return View();
            }
            else if (medico.DataNascimento >= DateTime.Now)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A data de nascimento informada não pode ser futura!";
                return View();
            }
            else
            {
                medico.Status = Status.PrimeiroAcesso;
                medico.Login.Senha = GerarSenhaUsuario(medico.Login);
                string senha = medico.Login.Senha;
                medico.Login.Senha = CalculaHashDuplo(medico.Login);
                medico.Login.TipoUsuario = "Medico";

                dataContext.TBMedico.Add(medico);
                dataContext.TBLogin.Add(medico.Login);
                dataContext.SaveChanges();

                TempData["TipoMensagem"] = "SUCESSO";
                TempData["Mensagem"] = "Médico cadastrado com sucesso";
                SendMail(medico.Login, senha);
                return RedirectToAction("Index");
            }

        }

        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue)
            {
                Medico medico = dataContext.TBMedico.FirstOrDefault(x => x.Id == ID);

                if (medico != null)
                {
                    medico.Status = Status.Inativo;

                    dataContext.TBMedico.Update(medico);
                    dataContext.SaveChanges();

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = $"Cadastro de {medico.Nome} removido com sucesso";
                    return RedirectToAction("Index");
                }
            }
            return NoContent();
        }

        public IActionResult Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Medico medico = dataContext.TBMedico.Include(x => x.Login).FirstOrDefault(x => x.Id == Id);
                if (medico != null)
                    return View(medico);
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Medico medico)
        {
            Medico medico2 = dataContext.TBMedico.Include(x => x.Login).FirstOrDefault(x => x.Id == medico.Id);
            medico.LoginID = medico2.LoginID;

            List<Medico> lista = dataContext.TBMedico.Include(x => x.Login).Where(x => x.Id != medico.Id).ToList();

            if (lista != null)
            {
                foreach (Medico medico3 in lista)
                {
                    if (medico3.CRM == medico.CRM)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CRM informado já existe!";
                        return View();
                    }
                    else if (medico3.CPF == medico.CPF || medico3.CPF.Replace("-", "").Replace(".", "") == medico.CPF.Replace("-", "").Replace(".", ""))
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CPF informado já existe!";
                        return View();
                    }
                    else if (medico3.Login.Email == medico.Login.Email)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O E-mail informado já existe!";
                        return View();
                    }
                }
            }

            if (ValidarCPF(medico.CPF) == false)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "O CPF informado é inválido!";
                return View();
            }
            else if (medico.DataNascimento >= DateTime.Now)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A data de nascimento informada não pode ser futura!";
                return View();
            }
            else
            {
                medico2.Login.Email = medico.Login.Email;
                medico2.Nome = medico.Nome;
                medico2.CPF = medico.CPF;
                medico2.CRM = medico.CRM;
                medico2.DataNascimento = medico.DataNascimento;

                ModelState.Clear();

                if (ModelState.IsValid)
                {
                    if (dataContext.TBMedico.Any(x => x.Id == medico.Id))
                    {
                        try
                        {
                            dataContext.TBMedico.Update(medico2);
                            dataContext.SaveChanges();
                            dataContext.TBLogin.Update(medico2.Login);
                            dataContext.SaveChanges();
                        }
                        catch
                        {
                            ViewBag.MensagemErro = "O médico não pode ser atualizado";
                            return View();
                        }

                        TempData["TipoMensagem"] = "SUCESSO";
                        TempData["Mensagem"] = "Médico atualizado com sucesso";
                        return RedirectToAction("Index");
                    }
                }
            }  
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Medico medico)
        {
            if (medico == null)
                return RedirectToAction("Index");
            else
            {
                if (medico.Nome == null && medico.CRM == null && medico.CPF == null)
                {
                    medico.Nome = "";
                    medico.CRM = "";
                    medico.CPF = "";
                    List<Medico> lista = dataContext.TBMedico.Where(x => x.Status != Status.Inativo).ToList();
                    return View(lista);
                }
                else
                {
                    if (medico.Nome != null && medico.CRM == null && medico.CPF == null)
                    {
                        medico.Nome = medico.Nome.Trim().ToUpper();
                        List<Medico> lista = dataContext.TBMedico.Where(x => x.Nome.ToUpper().Contains(medico.Nome)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                    else if (medico.Nome != null && medico.CRM != null && medico.CPF == null)
                    {
                        medico.Nome = medico.Nome.Trim().ToUpper();
                        medico.CRM = medico.CRM.Trim().ToUpper();
                        List<Medico> lista = dataContext.TBMedico.Where(x => x.Nome.ToUpper().Contains(medico.Nome)).Where(x => x.CRM.ToUpper().Contains(medico.CRM)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }

                    else if (medico.Nome != null && medico.CRM != null && medico.CPF != null)
                    {
                        medico.Nome = medico.Nome.Trim().ToUpper();
                        medico.CRM = medico.CRM.Trim().ToUpper();
                        medico.CPF = medico.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Medico> lista = dataContext.TBMedico.Where(x => x.Nome.ToUpper().Contains(medico.Nome)).Where(x => x.CRM.ToUpper().Contains(medico.CRM)).Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(medico.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }

                    else if (medico.Nome == null && medico.CRM != null && medico.CPF != null)
                    {
                        medico.CRM = medico.CRM.Trim().ToUpper();
                        medico.CPF = medico.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Medico> lista = dataContext.TBMedico.Where(x => x.CRM.ToUpper().Contains(medico.CRM)).Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(medico.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                    else if (medico.Nome != null && medico.CRM == null && medico.CPF != null)
                    {
                        medico.Nome = medico.Nome.Trim().ToUpper();
                        medico.CPF = medico.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Medico> lista = dataContext.TBMedico.Where(x => x.Nome.ToUpper().Contains(medico.Nome)).Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(medico.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                    else if (medico.Nome == null && medico.CRM != null && medico.CPF == null)
                    {
                        medico.CRM = medico.CRM.Trim().ToUpper();
                        List<Medico> lista = dataContext.TBMedico.Where(x => x.CRM.ToUpper().Contains(medico.CRM)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                    else if (medico.Nome == null && medico.CRM == null && medico.CPF != null)
                    {
                        medico.CPF = medico.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Medico> lista = dataContext.TBMedico.Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(medico.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                }

                return View();
            }
        }

        public IActionResult Index()
        {
            List<Medico> lista = dataContext.TBMedico.Where(x => x.Status != Status.Inativo).ToList();
            return View(lista);
        }
    }
}

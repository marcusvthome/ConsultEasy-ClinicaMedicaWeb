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
    public class SecretariaController : Controller
    {
        private readonly DataContext dataContext;

        public SecretariaController(DataContext dc)
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
                Console.WriteLine("Erro",
                Environment.NewLine, ex.StackTrace);
                return false;
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Secretaria secretaria)
        {
            List<Secretaria> lista = dataContext.TBSecretaria.Include(x => x.Login).ToList();

            if (lista != null)
            {
                foreach (Secretaria secretaria2 in lista)
                {
                    if (secretaria2.CPF == secretaria.CPF || secretaria2.CPF.Replace("-", "").Replace(".", "") == secretaria.CPF.Replace("-", "").Replace(".", ""))
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CPF informado já existe!";
                        return View();
                    }
                    else if (secretaria2.Login.Email == secretaria.Login.Email)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O E-mail informado já existe!";
                        return View();
                    }
                }
            }

            if (ValidarCPF(secretaria.CPF) == false)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "O CPF informado é inválido!";
                return View();
            }
            else if (secretaria.DataNascimento >= DateTime.Now)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A data de nascimento informada não pode ser futura!";
                return View();
            }
            else
            {
                secretaria.Status = Status.PrimeiroAcesso;
                secretaria.Login.Senha = GerarSenhaUsuario(secretaria.Login);
                string senha = secretaria.Login.Senha;
                secretaria.Login.Senha = CalculaHashDuplo(secretaria.Login);
                secretaria.Login.TipoUsuario = "Secretaria";

                dataContext.TBSecretaria.Add(secretaria);
                dataContext.TBLogin.Add(secretaria.Login);
                dataContext.SaveChanges();

                TempData["TipoMensagem"] = "SUCESSO";
                TempData["Mensagem"] = "Secretário(a) cadastrado com sucesso";
                SendMail(secretaria.Login, senha);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue)
            {
                Secretaria secretaria = dataContext.TBSecretaria.FirstOrDefault(x => x.Id == ID);

                if (secretaria != null)
                {
                    secretaria.Status = Status.Inativo;

                    dataContext.TBSecretaria.Update(secretaria);
                    dataContext.SaveChanges();

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = $"Cadastro de {secretaria.Nome} removido com sucesso";
                    return RedirectToAction("Index");
                }
            }
            return NoContent();
        }
        public IActionResult Edit(int? Id)
        {
            if (Id.HasValue)
            {
                Secretaria secretaria = dataContext.TBSecretaria.Include(x => x.Login).FirstOrDefault(x => x.Id == Id);
                if (secretaria != null)
                    return View(secretaria);
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Secretaria secretaria)
        {
            Secretaria secretaria2 = dataContext.TBSecretaria.Include(x => x.Login).FirstOrDefault(x => x.Id == secretaria.Id);
            secretaria.LoginID = secretaria2.LoginID;

            List<Secretaria> lista = dataContext.TBSecretaria.Include(x => x.Login).Where(x => x.Id != secretaria.Id).ToList();

            if (lista != null)
            {
                foreach (Secretaria secretaria3 in lista)
                {
                    if (secretaria3.CPF == secretaria.CPF || secretaria3.CPF.Replace("-", "").Replace(".", "") == secretaria.CPF.Replace("-", "").Replace(".", ""))
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O CPF informado já existe!";
                        return View();
                    }
                    else if (secretaria3.Login.Email == secretaria.Login.Email)
                    {
                        ViewBag.TipoMensagem = "ERRO";
                        ViewBag.Mensagem = "O E-mail informado já existe!";
                        return View();
                    }
                }
            }

            if (ValidarCPF(secretaria.CPF) == false)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "O CPF informado é inválido!";
                return View();
            }
            else if (secretaria.DataNascimento >= DateTime.Now)
            {
                ViewBag.TipoMensagem = "ERRO";
                ViewBag.Mensagem = "A data de nascimento informada não pode ser futura!";
                return View();
            }
            else
            {
                secretaria2.Login.Email = secretaria.Login.Email;
                secretaria2.Nome = secretaria.Nome;
                secretaria2.CPF = secretaria.CPF;
                secretaria2.DataNascimento = secretaria.DataNascimento;

                ModelState.Clear();

                if (ModelState.IsValid)
                {
                    if (dataContext.TBSecretaria.Any(x => x.Id == secretaria.Id))
                    {
                        try
                        {
                            dataContext.TBSecretaria.Update(secretaria2);
                            dataContext.SaveChanges();
                            dataContext.TBLogin.Update(secretaria2.Login);
                            dataContext.SaveChanges();
                        }
                        catch
                        {
                            ViewBag.MensagemErro = "O Secretário(a) não pode ser atualizado";
                            return View();
                        }

                        TempData["TipoMensagem"] = "SUCESSO";
                        TempData["Mensagem"] = "Secretário(a) atualizado com sucesso";
                        return RedirectToAction("Index");
                    }
                }
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Secretaria secretaria)
        {
            if (secretaria == null)
                return RedirectToAction("Index");
            else
            {
                if (secretaria.Nome == null && secretaria.CPF == null)
                {
                    secretaria.Nome = "";
                    secretaria.CPF = "";
                    List<Secretaria> lista = dataContext.TBSecretaria.Where(x => x.Status != Status.Inativo).ToList();
                    return View(lista);
                }
                else
                {
                    if (secretaria.Nome != null && secretaria.CPF == null)
                    {
                        secretaria.Nome = secretaria.Nome.Trim().ToUpper();
                        List<Secretaria> lista = dataContext.TBSecretaria.Where(x => x.Nome.ToUpper().Contains(secretaria.Nome)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                    else if (secretaria.Nome != null && secretaria.CPF != null)
                    {
                        secretaria.Nome = secretaria.Nome.Trim().ToUpper();
                        secretaria.CPF = secretaria.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Secretaria> lista = dataContext.TBSecretaria.Where(x => x.Nome.ToUpper().Contains(secretaria.Nome)).Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(secretaria.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }

                    else if (secretaria.Nome == null && secretaria.CPF != null)
                    {
                        secretaria.CPF = secretaria.CPF.Trim().Replace("-", "").Replace(".", "");
                        List<Secretaria> lista = dataContext.TBSecretaria.Where(x => x.CPF.Replace("-", "").Replace(".", "").Contains(secretaria.CPF)).Where(x => x.Status != Status.Inativo).ToList();
                        return View(lista);
                    }
                }

                return View();
            }
        }

        public IActionResult Index()
        {
            List<Secretaria> lista = dataContext.TBSecretaria.Where(x => x.Status != Status.Inativo).ToList();
            return View(lista);
        }
    }
}

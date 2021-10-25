using ClinicaMedicaWeb.Data;
using ClinicaMedicaWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext dataContext;

        public LoginController(DataContext dc)
        {
            dataContext = dc;
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

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult PrimeiroAcessoUsuario()
        {
            if (TempData["Id"] != null)
            {
                Login login = (Login)dataContext.TBLogin.FirstOrDefault(x => x.ID == (int)TempData["Id"]);
                return View(login);
            }
            return NoContent();
        }

        public IActionResult PrimeiroAcesso()
        {
            Login login = (Login)dataContext.TBLogin.FirstOrDefault();
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PrimeiroAcesso(Login login)
        {
            Login login2 = (Login)dataContext.TBLogin.FirstOrDefault(x => x.ID == login.ID);
            login.TipoUsuario = login2.TipoUsuario;
            login.Email = login2.Email;

            Administrador userAdmin = new Administrador();
            userAdmin = null;
            Medico userMedico = new Medico();
            userMedico = null;
            Secretaria userSecretaria = new Secretaria();
            userSecretaria = null;

            if (login.TipoUsuario == "Administrador")
            {
                userAdmin = dataContext.TBAdministrador.Include(x => x.Login).FirstOrDefault(x => x.LoginID == login.ID);
            }
            else if (login.TipoUsuario == "Medico")
            {
                userMedico = dataContext.TBMedico.Include(x => x.Login).FirstOrDefault(x => x.LoginID == login.ID);
            }
            else
            {
                userSecretaria = dataContext.TBSecretaria.Include(x => x.Login).FirstOrDefault(x => x.LoginID == login.ID);
            }

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (userAdmin != null)
                {
                    Login loginAuxiliar = new Login();
                    loginAuxiliar.Senha = login.Senha;
                    string novaSenha = CalculaHashDuplo(loginAuxiliar);
                    login2.Senha = novaSenha;
                    userAdmin.Status = Status.Ativo;
                    loginAuxiliar = null;

                    try
                    {
                        dataContext.TBAdministrador.Update(userAdmin);
                        dataContext.TBLogin.Update(login2);
                        dataContext.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "O administrador não pode ser atualizado";
                        return View();
                    }

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = "A senha do Administrador foi atualizada com sucesso";
                    return RedirectToAction("Index");
                }

                else if (userMedico != null)
                {
                    Login loginAuxiliar = new Login();
                    loginAuxiliar.Senha = login.Senha;
                    string novaSenha = CalculaHashDuplo(loginAuxiliar);
                    login2.Senha = novaSenha;
                    userMedico.Status = Status.Ativo;
                    loginAuxiliar = null;

                    try
                    {
                        dataContext.TBLogin.Update(login2);
                        dataContext.TBMedico.Update(userMedico);
                        dataContext.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "O médico não pode ser atualizado";
                        return View();
                    }

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = "A senha do médico foi atualizada com sucesso";
                    return RedirectToAction("Index");
                }

                else
                {
                    Login loginAuxiliar = new Login();
                    loginAuxiliar.Senha = login.Senha;
                    string novaSenha = CalculaHashDuplo(loginAuxiliar);
                    login2.Senha = novaSenha;
                    userSecretaria.Status = Status.Ativo;
                    loginAuxiliar = null;

                    try
                    {
                        dataContext.TBLogin.Update(login2);
                        dataContext.TBSecretaria.Update(userSecretaria);
                        dataContext.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "O secretário não pode ser atualizado";
                        return View();
                    }

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = "A senha do secretário foi atualizada com sucesso";
                    return RedirectToAction("Index");
                }
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Login login)
        {
            Login login2 = new Login();
            login2.Senha = login.Senha;
            login2.SenhaConfirmar = login.Senha;
            login = dataContext.TBLogin.FirstOrDefault(x => x.Email == login.Email);
            login.SenhaConfirmar = login.Senha;

            Administrador userAdmin = new Administrador();
            userAdmin = null;
            Medico userMedico = new Medico();
            userMedico = null;
            Secretaria userSecretaria = new Secretaria();
            userSecretaria = null;

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (login.TipoUsuario == "Administrador")
                {
                    userAdmin = dataContext.TBAdministrador.Include(x => x.Login).FirstOrDefault(x => x.Login.ID == login.ID);
                }
                else if (login.TipoUsuario == "Medico")
                {
                    userMedico = dataContext.TBMedico.Include(x => x.Login).FirstOrDefault(x => x.Login.ID == login.ID);
                }
                else
                {
                    userSecretaria = dataContext.TBSecretaria.Include(x => x.Login).FirstOrDefault(x => x.Login.ID == login.ID);
                }

                if (userAdmin != null)
                {
                    if (userAdmin.Status == Status.PrimeiroAcesso)
                    {
                        if (userAdmin.Login.Senha == login.Senha)
                        {
                            return RedirectToAction("PrimeiroAcesso");
                        }
                    }
                    else
                    {
                        login2.Senha = CalculaHashDuplo(login2);
                        Administrador userAdmin2 = dataContext.TBAdministrador.Include(x => x.Login).FirstOrDefault(x => x.Login.Email == login.Email && x.Login.Senha == login2.Senha);

                        if (userAdmin2 != null && userAdmin2.Status == Status.Inativo)
                        {
                            ViewBag.Erro = "Este usuário está Inativo, por favor contate o Administrador!";
                            return View();
                        }

                        else if (userAdmin2 != null)
                        {
                            List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Sid, userAdmin2.Id.ToString()),
                            new Claim(ClaimTypes.Name, userAdmin2.Nome),
                            new Claim(ClaimTypes.GroupSid, userAdmin2.LoginID.ToString()),
                            new Claim(ClaimTypes.Role, userAdmin2.Login.TipoUsuario),
                        };

                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            AuthenticationProperties authProperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                                IsPersistent = true,
                            };

                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                if (userMedico != null)
                {
                    if (userMedico.Status == Status.PrimeiroAcesso)
                    {
                        login.Senha = CalculaHashDuplo(login);
                        if (userMedico.Login.Senha == login.Senha)
                        {
                            TempData["ID"] = login.ID;
                            return RedirectToAction("PrimeiroAcessoUsuario");
                        }
                    }
                    else
                    {
                        login2.Senha = CalculaHashDuplo(login2);
                        Medico userMedico2 = dataContext.TBMedico.Include(x => x.Login).FirstOrDefault(x => x.Login.Email == login.Email && x.Login.Senha == login2.Senha);
                        
                        if (userMedico2 != null && userMedico2.Status == Status.Inativo)
                        {
                            ViewBag.Erro = "Este usuário está Inativo, por favor contate o Administrador!";
                            return View();
                        }

                        else if(userMedico2 != null)
                        {
                            List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Sid, userMedico2.Id.ToString()),
                            new Claim(ClaimTypes.Name, userMedico2.Nome),
                            new Claim(ClaimTypes.GroupSid, userMedico2.LoginID.ToString()),
                            new Claim(ClaimTypes.Role, userMedico2.Login.TipoUsuario),
                        };

                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            AuthenticationProperties authProperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                                IsPersistent = true,
                            };

                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                            return RedirectToAction("Index", "Home");
                        }

                    }

                }

                if (userSecretaria != null)
                {
                    if (userSecretaria.Status == Status.PrimeiroAcesso)
                    {
                        login.Senha = CalculaHashDuplo(login);
                        if (userSecretaria.Login.Senha == login.Senha)
                        {
                            TempData["ID"] = login.ID;
                            return RedirectToAction("PrimeiroAcessoUsuario");
                        }
                    }
                    else
                    {
                        login2.Senha = CalculaHashDuplo(login2);
                        Secretaria userSecretaria2 = dataContext.TBSecretaria.Include(x => x.Login).FirstOrDefault(x => x.Login.Email == login.Email && x.Login.Senha == login2.Senha);

                        if (userSecretaria2 != null && userSecretaria2.Status == Status.Inativo)
                        {
                            ViewBag.Erro = "Este usuário está Inativo, por favor contate o Administrador!";
                            return View();
                        }

                        else if(userSecretaria2 != null)
                        {
                            List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Sid, userSecretaria2.Id.ToString()),
                            new Claim(ClaimTypes.Name, userSecretaria2.Nome),
                            new Claim(ClaimTypes.GroupSid, userSecretaria2.LoginID.ToString()),
                            new Claim(ClaimTypes.Role, userSecretaria2.Login.TipoUsuario),
                        };

                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            AuthenticationProperties authProperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                                IsPersistent = true,
                            };

                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                            return RedirectToAction("Index", "Home");
                        }

                    }

                }
            }

            ViewBag.Erro = "Usuário e/ou senha inválidos";
            return View();
        }

        public IActionResult Logoff()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public bool LinkRecuperarSenha(Login login)
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

                string link = "<a href=\"" + "https://" + HttpContext.Request.Host + "/Login/AlterarSenha/" + login.ID + "\">Alterar Senha</a>";

                _mailMessage.Body = "<b>Aqui está o ink para a sua recuperação senha: " + link;

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

        public IActionResult AlterarSenha(int? Id)
        {
            if (Id.HasValue)
            {
                Login login = dataContext.TBLogin.FirstOrDefault(x => x.ID == Id);

                if (login != null)
                    return View(login);
            }

            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlterarSenha(Login login)
        {
            Login login2 = (Login)dataContext.TBLogin.FirstOrDefault(x => x.ID == login.ID);
            login.TipoUsuario = login2.TipoUsuario;
            login.Email = login2.Email;

            login.SenhaAtual = CalculaHashDuplo(login);
            login.Senha = login.SenhaConfirmar;

            if (login2.Senha == login.SenhaAtual)
            {
                ViewBag.Erro = "A nova senha não pode ser a mesma que a atual";
                return View();
            }

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (login != null)
                {
                    Login loginAuxiliar = new Login();
                    loginAuxiliar.Senha = login.Senha;
                    string novaSenha = CalculaHashDuplo(loginAuxiliar);
                    login2.Senha = novaSenha;
                    loginAuxiliar = null;

                    try
                    {
                        dataContext.TBLogin.Update(login2);
                        dataContext.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "A senha do usuário não pode ser atualizada";
                        return View();
                    }

                    TempData["TipoMensagem"] = "SUCESSO";
                    TempData["Mensagem"] = "A senha do usuário foi atualizada com sucesso";
                    Logoff();
                    return RedirectToAction("Index", "Login");

                }
            }

            return NoContent();
        }

        public IActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarSenha(Login login)
        {
            login = dataContext.TBLogin.FirstOrDefault(x => x.Email == login.Email);

            if (login != null)
            {
                LinkRecuperarSenha(login);

                ViewBag.Sucesso = "Se há um usuário existe, o link de recuperação de senha será enviado neste e-mail!";
                return View();
            }
            else
            {
                ViewBag.Erro = "Email não encontrado";

            }
            return View();
        }

    }
}

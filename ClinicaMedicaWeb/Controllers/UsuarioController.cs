using ClinicaMedicaWeb.Data;
using ClinicaMedicaWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedicaWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly DataContext dataContext;

        public UsuarioController(DataContext dc)
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
            return View();
        }

        public IActionResult Logoff()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        [Authorize]
        public IActionResult RedefinirSenha(int? Id)
        {
            if(Id.HasValue){
                Login login = dataContext.TBLogin.FirstOrDefault(x => x.ID == Id);

                if(login != null)
                    return View(login);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RedefinirSenha(Login login)
        {
            Login login2 = (Login)dataContext.TBLogin.FirstOrDefault(x => x.ID == login.ID);
            login.TipoUsuario = login2.TipoUsuario;
            login.Email = login2.Email;

            login.Senha = login.SenhaAtual;
            login2.SenhaAtual = CalculaHashDuplo(login);
            
            if (login2.SenhaAtual != login2.Senha)
            {
                ViewBag.Erro = "A senha atual está incorreta";
                return View();
            }

            else if (login.SenhaAtual == login.SenhaConfirmar)
            {
                ViewBag.Erro = "A nova senha não pode ser a mesma que a atual";
                return View();
            }

            if (ModelState.IsValid)
            {
                if (login != null)
                {
                    Login loginAuxiliar = new Login();
                    loginAuxiliar.Senha = login.SenhaConfirmar;
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

    }
}

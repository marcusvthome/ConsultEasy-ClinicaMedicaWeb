using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ClinicaMedicaWeb.Models;
using System.Text;
using System.Security.Claims;

namespace ClinicaMedicaWeb.Tokens
{
    public class Token
    {
        //Consult123!
        public static string Secret = "fb69f0a89aad1415baafa4968db4aad4688dd06611832a51d41da73b3b413c28";

        public static string GenerateToken(Login login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, login.Email),
                    new Claim(ClaimTypes.Role, login.TipoUsuario),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), 
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
}
}

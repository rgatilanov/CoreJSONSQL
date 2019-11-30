﻿namespace UserApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using UserApi.Models;
    using Api.Library.Interfaces;

    [Route("api/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //[HttpPost]
        //public Login Authenticate(UserMin usMin)
        //{
        //    // Integración a base de datos
        //    if (usMin.Nick == "rgatilanov" && usMin.Password == "96CAE35CE8A9B0244178BF28E4966C2CE1B8385723A96A6B838858CDD6CA0A1E") //SHA2
        //    {
        //        // Leemos el secret_key desde nuestro appseting
        //        var secretKey = _configuration.GetValue<string>("SecretKey");
        //        var key = Encoding.ASCII.GetBytes(secretKey);

        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            // Nuestro token va a durar un día
        //            Expires = DateTime.UtcNow.AddDays(1),
        //            // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //        };

        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var createdToken = tokenHandler.CreateToken(tokenDescriptor);

        //        return new Login()
        //        {
        //            ID = usMin.ID,
        //            Nick = usMin.Nick,
        //            Token = tokenHandler.WriteToken(createdToken),
        //        };
        //    }
        //    else
        //        return null;
        //}
        [HttpPost, Route("login")]
        //public IActionResult Login([FromBody]UserMin user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest("Invalid client request");
        //    }

        //    if (user.Nick == "rgatilanov" && user.Password == "4297f44b13955235245b2497399d7a93") //MD5 (123123)
        //    {
        //        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("maestria-mtwdm-2019"));
        //        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        //        var tokeOptions = new JwtSecurityToken(
        //            issuer: "http://localhost:44308",
        //            audience: "http://localhost:44308",
        //            claims: new List<System.Security.Claims.Claim>(),
        //            expires: DateTime.Now.AddMinutes(5),
        //            signingCredentials: signinCredentials
        //        );

        //        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        //        return Ok(new { Token = tokenString });
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}
        public Api.Library.Models.User Login([FromBody]Api.Library.Models.UserMin user)
        {
            var ConnectionStringLocal = _configuration.GetValue<string>("ConnectionStringLocal");
            var ConnectionStringAzure = _configuration.GetValue<string>("ConnectionStringAzure");
            using (ILogin Login = Factorizador.CrearConexionServicio(Api.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                Api.Library.Models.User objusr = Login.EstablecerLogin(user.Nick, user.Password);

                if (objusr.ID > 0)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("maestria-mtwdm-2019"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:44308",
                        audience: "http://localhost:44308",
                        claims: new List<System.Security.Claims.Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    objusr.JWT = tokenString;
                }
                return objusr;
            }            
        }
    }
}
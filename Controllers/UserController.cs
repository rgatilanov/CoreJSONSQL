using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using UserApi.Models;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Se hace el test así: https://localhost:44303/api/user
        [HttpGet]
        //[Authorize]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = new List<User>();
            users.Add(new Models.User()
            {
                CreateDate = DateTime.Now,
                ID = 1,
                Name = "Ramón Gerardo",
                Nick = "rgatilanov",
                Password = null,
                accountType = AccountType.Administrator
            });

            users.Add(new Models.User()
            {
                CreateDate = DateTime.Now,
                ID = 2,
                Name = "Juan Perez",
                Nick = "juan.perez",
                Password = null,
                accountType = AccountType.Basic,
            });

            return users;
        }

        //Se hace el test así: https://localhost:44303/api/user/1
        [HttpGet("{id}")]
        public ActionResult<User> GetUsers(int ID)
        {
            Models.User user = null;
            if (ID == 1)
                user = new Models.User()
                {
                    CreateDate = DateTime.Now,
                    ID = 1,
                    Nick = "rgatilanov",
                    Password = null,
                    Name = "Ramón Gerardo",
                    accountType = AccountType.Administrator
                };
            else
                user = new Models.User()
                {
                    CreateDate = DateTime.Now,
                    ID = 2,
                    Name = "Juan Perez",
                    Nick = "juan.perez",
                    Password = null,
                    accountType = AccountType.Basic,
                };

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"> {"id": 3,"nick": "leones2019","password": "123123","createDate": 2019-08-02T12:43:02.9396464-05:00"}
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<User> PostUser(User user)
        {
            /*Lógica a base de datos*/

            return user;
        }

        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUsers(int ID)
        {
            return new User();
        }
    }
}
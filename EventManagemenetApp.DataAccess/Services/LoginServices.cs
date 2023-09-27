using EventManagemenetApp.DataAccess.Interface;
using EventManagemenetApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagemenetApp.DataAccess.Services
{
    public class LoginServices:ILogin
    {
        private EventContext _context;

        public LoginServices(EventContext context)
        {
            _context = context;
        }

        public Registration Login(string userName, string passWord)
        {
            try
            {
                var validate = (from user in _context.Registration
                                where user.Username == userName && user.Password == passWord
                                select user).SingleOrDefault();

                return validate;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

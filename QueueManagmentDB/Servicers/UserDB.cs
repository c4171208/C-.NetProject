using Microsoft.EntityFrameworkCore;
using QueueManagmentDB.EF.Contexts;
using QueueManagmentDB.EF.Models;
using QueueManagmentDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentDB.Servicers
{
    public class UserDB : IUserDB

    {
        private readonly QueueManagmentContext _queueManagmentContext;
        public UserDB(QueueManagmentContext queueManagmentContext)
        {
            _queueManagmentContext = queueManagmentContext;
        }

        public void SighIn(User user)
        {
            //להוסיף וולידצית על ה-email   
            _queueManagmentContext.Users.Add(user);
            _queueManagmentContext.SaveChanges();
        }

        public User LogIn(string email)
        {
            User user = _queueManagmentContext.Users.AsNoTracking().Where(x => x.Email == email).FirstOrDefault();
            return user;
        }
        public User GetUser(int id)
        {
            return _queueManagmentContext.Users.AsNoTracking().Where(x => x.UserId == id).FirstOrDefault();
        }
    }
}

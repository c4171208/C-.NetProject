using QueueManagmentDB.EF.Contexts;
using QueueManagmentDB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentDB.Interfaces
{
    public interface IUserDB
    {
        void SighIn(User user);
         
        User LogIn(string email);

        User GetUser(int id);


    }
}

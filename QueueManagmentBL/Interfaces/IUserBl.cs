using QueueManagmentDB.EF.Contexts;
using QueueManagmentDB.EF.Models;
using QueueManagmentEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentBL.Interfaces
{
    public interface IUserBl
    {
        BaseResponse<User> SighIn(UserDTO user);

        BaseResponse<User> Login(string password, string email);

        BaseResponse<User> LogOut();
    }
}

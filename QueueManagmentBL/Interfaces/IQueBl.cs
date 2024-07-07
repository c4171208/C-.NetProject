using QueueManagmentDB.EF.Models;
using QueueManagmentEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentBL.Interfaces
{
    public interface IQueBl
    {
        BaseResponse<Queue> AddNewQue(QueueDTO queue);
        BaseResponse<Queue> RemoveQue(int id);
        BaseResponse<Queue> EditQueue(int id, QueueDTO queue);
        BaseResponse<List<Queue>> GetAllQues();
        int GetUserIdFromToken();

    }
}

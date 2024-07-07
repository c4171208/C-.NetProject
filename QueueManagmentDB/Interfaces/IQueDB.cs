using System;
using QueueManagmentDB.EF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentDB.Interfaces
{
    public interface IQueDB
    {
        void AddNewQue(Queue queue);
        void RemoveQue(int id);
        void EditQueue(int id,Queue queue);
        List<Queue> GetAllQues();
        Queue GetQue(int id);

    }
}

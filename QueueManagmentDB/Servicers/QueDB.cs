using Microsoft.EntityFrameworkCore;
using QueueManagmentDB.EF.Contexts;
using QueueManagmentDB.EF.Models;
using QueueManagmentDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentDB.Servicers
{
    public class QueDB : IQueDB
    {
        private readonly QueueManagmentContext _queueManagmentContext;
        public QueDB(QueueManagmentContext queueManagmentContext)
        {
            _queueManagmentContext = queueManagmentContext;
        }

        public void AddNewQue(Queue queue)
        {
            if (CheckIsAvailable(queue) == false)
            {
                throw new Exception("The date is not valid");
            }
            _queueManagmentContext.Queues.Add(queue);
            _queueManagmentContext.SaveChanges();
        }

        public void RemoveQue(int id)
        {
            Queue queue = GetQue(id);
            if (CheckIsAvailable(queue) == false)
            {
                throw new Exception("The date is not valid");
            }
            if (queue == null)
            {
                throw new Exception("Queue not found");
            }
            else
            {
                _queueManagmentContext.Queues.Remove(queue);
                _queueManagmentContext.SaveChanges();
            }
        }

        public void EditQueue(int id, Queue queue)
        {
            Queue oldQueue = GetQue(id);
            if (CheckIsAvailable(oldQueue) == false)
            {
                throw new Exception("The date is not valid");
            }
            if (oldQueue == null)
            {
                throw new Exception("Queue not found");
            }
            oldQueue.DesignatedTime = queue.DesignatedTime;
            _queueManagmentContext.SaveChanges();
        }

        public Queue GetQue(int id)
        {
            return _queueManagmentContext.Queues.AsNoTracking().FirstOrDefault(q => q.QueId == id);
        }


        public List<Queue> GetAllQues()
        {
            DateTime currentDateTime = DateTime.Now;
            return _queueManagmentContext.Queues.AsNoTracking().Where(q => q.DesignatedTime > currentDateTime).ToList();
        }

        public bool CheckIsAvailable(Queue queue1)
        {
            DateTime currentDateTime = DateTime.Now;
            if (queue1.DesignatedTime > currentDateTime)
                return true;
            return false;
        }


    }
}

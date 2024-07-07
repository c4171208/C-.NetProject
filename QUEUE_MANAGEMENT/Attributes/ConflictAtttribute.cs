//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using QueueManagmentDB.Interfaces;
//using QueueManagmentDB.Servicers;
//using QueueManagmentEntites;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Queue = QueueManagmentDB.EF.Models.Queue;

//namespace QueueManagmentApi.Attributes
//{
//    public class ConflictAttribute : ActionFilterAttribute
//    {
//        private readonly Queue _queue;
//        private readonly IQueDB _queDB;


//        public ConflictAttribute(Queue queue, IQueDB queDB)
//        {
//            _queDB = queDB;
//            _queue = queue;
//        }

//        public override async Task OnActionExecutionAsync(ActionExecutingContext _context, ActionExecutionDelegate _next)
//        {
//            List<Queue> queues = _queDB.GetAllQues();
//            foreach (Queue q in queues)
//            {
//                if (q.DesignatedTime == _queue.DesignatedTime)
//                {
//                    _context.Result = new ObjectResult(new BaseResponse<Queue>
//                    {
//                        IsSucsses = false,
//                        Message = "Queue already exists",
//                        StatusCode = 409
//                    })
//                    {
//                        StatusCode = 409
//                    };
//                }
//            }

//            await _next();
//        }
//    }
//}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QueueManagmentDB.Interfaces;
using QueueManagmentEntites;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Queue = QueueManagmentDB.EF.Models.Queue;

namespace QueueManagmentApi.Attributes
{
    public class ConflictAttribute : ActionFilterAttribute
    {
        private readonly IQueDB _queDB;

        public ConflictAttribute(IQueDB queDB)
        {
            _queDB = queDB;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext _context, ActionExecutionDelegate _next)
        {
            if (_context.ActionArguments.TryGetValue("queue", out var queueObj) && queueObj is QueueDTO queue)
            {
                List<Queue> queues = _queDB.GetAllQues();
                foreach (Queue q in queues)
                {
                    int result = DateTime.Compare((DateTime)(q.DesignatedTime), (DateTime)(queue.DesignatedTime));
                    if (result == 0)
                    {
                        _context.Result = new ObjectResult(new BaseResponse<Queue>
                        {
                            IsSucsses = false,
                            Message = "Queue already exists",
                            StatusCode = 409
                        })
                        {
                            StatusCode = 409
                        };
                        return;

                    }

                 
                }
                await _next();
            }
            else
            {
                _context.Result = new ObjectResult(new BaseResponse<Queue>
                {
                    IsSucsses = false,
                    Message = "internal server",
                    StatusCode = 500
                })
                {
                    StatusCode = 500
                };
                return;

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentEntites
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }

        public int StatusCode { get; set; }

        public bool IsSucsses { get; set; }

        public string Message { get; set; }
    }
}

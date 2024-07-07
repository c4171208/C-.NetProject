using AutoMapper;
using QueueManagmentDB.EF.Models;
using QueueManagmentEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentBL
{
    public class MannegerMap:Profile
    {
        public MannegerMap()
        {
            CreateMap<UserDTO, User>();
            CreateMap<QueueDTO, Queue>();
            CreateMap<QueueToRemoveDTO, Queue>();

        }
    }
}

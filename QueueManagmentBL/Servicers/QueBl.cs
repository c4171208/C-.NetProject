using AutoMapper;
using Microsoft.AspNetCore.Http;
using QueueManagmentBL.Interfaces;
using QueueManagmentDB.EF.Contexts;
using QueueManagmentDB.EF.Models;
using QueueManagmentDB.Interfaces;
using QueueManagmentEntites;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Queue = QueueManagmentDB.EF.Models.Queue;

namespace QueueManagmentBL.Servicers
{
    public class QueBl : IQueBl
    {
        private readonly IQueDB _queDB;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QueBl(IQueDB queDB, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _queDB = queDB;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        public BaseResponse<Queue> AddNewQue(QueueDTO queueFromClient)
        {

            int idFromToken = GetUserIdFromToken();
            queueFromClient.UserId = idFromToken;
            Queue queue = _mapper.Map<Queue>(queueFromClient);
            if (queue == null)
                return new BaseResponse<Queue>
                {
                    IsSucsses = false,
                    Message = "Add failed",
                    StatusCode = 500,

                };

            _queDB.AddNewQue(queue);
            return new BaseResponse<Queue>
            {
                Data = queue,
                IsSucsses = true,
                StatusCode = 200,
                Message = "The Data has been add."

            };
        }

        public BaseResponse<Queue> RemoveQue(int id)
        {
            Queue queue = _queDB.GetQue(id);

            if (queue == null)
                return new BaseResponse<Queue>
                {
                    IsSucsses = false,
                    Message = "Not fount ,The Data was not found",
                    StatusCode = 404,

                };

            else if (GetUserIdFromToken() != queue.UserId)
                return new BaseResponse<Queue>
                {
                    IsSucsses = false,
                    Message = "Access denied. You are not authorized",
                    StatusCode = 401,

                };
            _queDB.RemoveQue(id);
            return new BaseResponse<Queue>
            {
                Data = queue,
                IsSucsses = true,
                StatusCode = 200,
                Message = "The data is deleted",
            };


        }

        public BaseResponse<Queue> EditQueue(int id, QueueDTO queueToRemoveDTO)
        {
            Queue q = _queDB.GetQue(id);
            Queue queue = _mapper.Map<Queue>(queueToRemoveDTO);

            if (q == null)
                return new BaseResponse<Queue>
                {
                    IsSucsses = false,
                    Message = "Not fount ,The Data was not found",
                    StatusCode = 404,

                };

            else if (GetUserIdFromToken() != q.UserId)
                return new BaseResponse<Queue>
                {
                    IsSucsses = false,
                    Message = "Access denied. You are not authorized",
                    StatusCode = 401,

                };

            _queDB.EditQueue(id, queue);
            return new BaseResponse<Queue>
            {
                Data = queue,
                IsSucsses = true,
                StatusCode = 200,
                Message = "The data has been updated",

            };

        }
        public BaseResponse<List<Queue>> GetAllQues()
        {
            List<Queue> data = _queDB.GetAllQues();
            if (data == null || data.Count <= 0)
                return new BaseResponse<List<Queue>>
                {
                    Data = null,
                    IsSucsses = false,
                    StatusCode = 404,
                    Message = "The data Not found",

                };
            return new BaseResponse<List<Queue>>
            {
                Data = data,
                IsSucsses = true,
                StatusCode = 200,
                Message = "succsees to bring the data",

            };
        }

        public int GetUserIdFromToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies[CookiesKeys.AccessToken];
            if (token == null)
            {
                return -1;
                throw new Exception("Error");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            int userId;
            string userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value;
            if (userIdClaim != null)
            {
                if (int.TryParse(userIdClaim, out userId))
                {
                    return userId;
                }
                else
                {
                    return -1;
                    throw new Exception("Error");

                }
            }
            return -1;
            throw new Exception("Error");

        }




    }
}

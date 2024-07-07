using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QueueManagmentBL.Interfaces;
using QueueManagmentDB.EF.Models;
using QueueManagmentDB.Interfaces;
using QueueManagmentEntites;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagmentBL.Servicers
{
    public class UserBl : IUserBl
    {
        byte[] salt;
        private readonly IUserDB _userDB;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public UserBl(IUserDB userDB, IOptions<AppSettings> options, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userDB = userDB;
            _appSettings = options.Value;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public BaseResponse<User> SighIn(UserDTO userFromClient)
        {
            User user = _mapper.Map<User>(userFromClient);
            string hash = HashPasword.ToHashPasword(user.Password, out salt);
            user.Password = hash;
            user.Salt = salt;

            _userDB.SighIn(user);
            CreateUserToken(user);
            return new BaseResponse<User>
            {
                Data = user,
                IsSucsses = true,
                StatusCode = 200,
                Message = "The user is SighIn succsesfuly",

            };

        }

        public BaseResponse<User> Login(string password, string email)
        {
            User userFromDB = _userDB.LogIn(email);

            if (userFromDB != null)
            {
                if (HashPasword.VerifyPassword(password, userFromDB.Password, userFromDB.Salt))
                {
                    CreateUserToken(userFromDB);
                    return new BaseResponse<User>
                    {
                        Data = userFromDB,
                        IsSucsses = true,
                        StatusCode = 200,
                        Message = "The user is SighIn succsesfuly",

                    };
                }
            }
            return new BaseResponse<User>
            {
                Data = null,
                IsSucsses = false,
                StatusCode = 404,
                Message = "not found the user",

            }; ;
        }

        public BaseResponse<User> LogOut()
        {
            _httpContextAccessor.HttpContext.
              Response.Cookies.Delete(CookiesKeys.AccessToken);
            return new BaseResponse<User>
            {
                Data = null,
                IsSucsses = true,
                StatusCode = 200,
                Message = "The LogOut succsess",

            };
        }


        private void CreateUserToken(User user)
        {
            string newAccessToken = this.GenerateAccessToken(user);
            //להטמיע עוגיות
            CookieOptions cookieTokenOptions = new CookieOptions()
            {
                //לא יוכל לקרוא אותו js שרק השרת יוכל להגדיר אותו שקוד 
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(_appSettings.Jwt.ExpireMinutes)
            };
            //לגשת  לאוביקט של מי שניגש לקונטרולר של שכבת הקונטרולרים
            _httpContextAccessor.HttpContext.
                Response.Cookies.Append(CookiesKeys.AccessToken, newAccessToken, cookieTokenOptions);
        }

        private string GenerateAccessToken(User user)
        {

            var jwtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.SecretKey));
            var credentials = new SigningCredentials(jwtSecurityKey, SecurityAlgorithms.HmacSha256);
            //נתונים שאני רוצה להטמיע
            var claims = new[]
            {
                //new Claim(ClaimTypes.Name, user.FirstName+" "+user.LastName),
                //new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.UserId.ToString())
            };
            var token = new JwtSecurityToken(
                _appSettings.Jwt.Issuer,
                _appSettings.Jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_appSettings.Jwt.ExpireMinutes),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}


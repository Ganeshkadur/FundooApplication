using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.ResponseModel;
using ModelLayer.ResuestModels;
using Newtonsoft.Json.Linq;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace RepositoryLayer.Service
{
    public class Fandoo_Repository : IFandoo_Repository
    {
        private readonly Context _context;
       private readonly IDataProtector _protector;
        public readonly IConfiguration _config;
        private readonly ILogger<Fandoo_Repository> _logger;

        public Fandoo_Repository(Context context,IDataProtectionProvider provider, IConfiguration config,ILogger<Fandoo_Repository>logger)
        {
            _context = context;
            _protector = provider.CreateProtector("Encryption_key");
            _config = config;
            _logger = logger;
        }

        public UserRegistration Userregistation(UserRegisationModel request)
        {
           _logger.LogInformation("ENTER IN TO THE Userregistation METHOD in User_Repository with ", request);//-loger
            UserRegistration user = new UserRegistration();
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.Password = _protector.Protect(request.Password);

                _context.Add(user);
                _context.SaveChanges();

            _logger.LogInformation("Returned from Userregistation METHOD in User_Repository with " );
                return user;
            
        }

        public string UserLogin(UserLoginModel request)
        {
            _logger.LogInformation("ENTER IN TO THE Userregistation METHOD in User_Repository with ", request);//-logger
            var user = _context.usersRegistation.FirstOrDefault(u => u.Email == request.Email);

            if (user != null)
            {

                string userpass = _protector.Unprotect(user.Password);
                if (userpass == request.Password)
                {
                    var token = GenerateToken(user.UserId, request.Email);

                    //HttpContext.Session.SetInt32("UserId",user.UserId);

                    _logger.LogInformation("Returned from the Userregistation method in User_Repository with ", token);//-logger
                    return token;
                }
                else
                {
                    _logger.LogInformation("Returned from the Userregistation method because Email not matching the password ");//-logger
                    return null;
                }


            }
            else
            {

                _logger.LogInformation("Returned from the Userregistation method because the user not exist of this email");//-logger
                return null;
           
            }


        

        }

        public bool DeleteMyAccount(long userid)
        {
            var user = _context.usersRegistation.FirstOrDefault(x => x.UserId == userid);
            if (user != null)
            {
                _context.usersRegistation.Remove(user);
                return true;
            }
            else
            {
                return false;
            }

        }
     

        private string GenerateToken(long UserId, string userEmail)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
        new Claim("Email",userEmail),
        new Claim("UserId", UserId.ToString())
    };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }





        public ForgotPasswordModel ForgetPassword(String email)
        {
            try
            {
                var user = _context.usersRegistation.FirstOrDefault(x => x.Email == email);
                if (user != null)
                {
                    ForgotPasswordModel model = new ForgotPasswordModel();
                    model.Email = user.Email;
                    model.UserId = user.UserId;
                    model.Token = GenerateToken(user.UserId, user.Email);

                    return model;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Returned from the Userregistation method because Email not matching the password ",ex);//-logger

                throw ex;

            }
          

        }

        public bool Resetpassword(string email, string password, string ConfirmPasswor)
        {
            try
            {
               var user =_context.usersRegistation.FirstOrDefault(x => x.Email == email);
               if(password.Equals(ConfirmPasswor))
                {
                    user.Password = _protector.Protect(password);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                


            }catch (Exception ex)
            {
                throw ex;

            }
        }

        public bool UpdateUser(long userid,UserUpdateModel request)
        {
            var user=_context.usersRegistation.FirstOrDefault(y => y.UserId == userid);
            if(user != null)
            {
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                _context.SaveChanges();


                return true;
            }
            else
            {
                return false;
            }
        }

        public object GetMyDetails(long userid)
        {
            var user= _context.usersRegistation.FirstOrDefault(y=>y.UserId == userid);  
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }

        }

    }
}

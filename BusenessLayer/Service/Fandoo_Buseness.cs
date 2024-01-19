using BusenessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using ModelLayer.ResponseModel;
using ModelLayer.ResuestModels;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;


namespace BusenessLayer.Service
{
    public class Fandoo_Buseness : IFandoo_Buseness
    {

        private readonly IFandoo_Repository _repo;
        private readonly ILogger _logger;

        public Fandoo_Buseness(IFandoo_Repository repo,ILogger<Fandoo_Buseness>logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public UserRegistration Userregistation(UserRegisationModel request)
        {
            _logger.LogInformation("ENTER IN TO THE Userregistation METHOD in user Buseness with ", request);//-loger
            return _repo.Userregistation(request);
        }

        public string UserLogin(UserLoginModel request)
        {
            return _repo.UserLogin(request);
        }

      
        public ForgotPasswordModel ForgetPassword(String email)
        {
            return _repo.ForgetPassword(email);
        }

       public bool Resetpassword(string email, string password, string ConfirmPasswor)
        {
            return _repo.Resetpassword(email, password, ConfirmPasswor);
        }

       public bool DeleteMyAccount(long userid)
        {
            return _repo.DeleteMyAccount(userid);
        }

        public bool UpdateUser(long userid, UserUpdateModel request)
        {
            return _repo.UpdateUser(userid, request);
        }

        public object GetMyDetails(long userid)
        {
            return _repo.GetMyDetails(userid);
        }
    }
}

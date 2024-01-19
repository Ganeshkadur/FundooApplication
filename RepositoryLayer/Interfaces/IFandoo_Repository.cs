using ModelLayer.ResponseModel;
using ModelLayer.ResuestModels;
using RepositoryLayer.Entities;
using System;

namespace RepositoryLayer.Interfaces
{
    public interface IFandoo_Repository
    {
        UserRegistration Userregistation(UserRegisationModel request);
        string UserLogin(UserLoginModel request);
        bool DeleteMyAccount(long userid);
        ForgotPasswordModel ForgetPassword(String email);
        bool Resetpassword(string email, string password, string ConfirmPasswor);
        bool UpdateUser(long userid, UserUpdateModel request);
        object GetMyDetails(long userid);

    }
}
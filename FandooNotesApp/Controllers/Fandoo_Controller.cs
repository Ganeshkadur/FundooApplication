using BusenessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.ResponseModel;
using ModelLayer.ResuestModels;
using RepositoryLayer.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FandooNotesApp.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class Fandoo_Controller : ControllerBase
    {
        private readonly IFandoo_Buseness _buseness;
        private readonly ILogger<Fandoo_Controller> _logger;
        private readonly IBus bus;


        public Fandoo_Controller(IFandoo_Buseness buseness, IBus bus, ILogger<Fandoo_Controller> logger)
        {
            _buseness = buseness;
            _logger = logger;
            this.bus= bus;
        }

        [HttpPost]
        public IActionResult UserRegisration(UserRegisationModel request)
        {
            _logger.LogInformation("ENTER IN TO THE Userregistation METHOD with ",request);//-loger
            var result=_buseness.Userregistation(request);
            if(result == null)
            {
                return BadRequest(new ResponseModel<UserRegistration>() {Success=false ,Message="Regisation Unsucessfull", Data = result }) ;
            }
            else
            {
                return Ok(new ResponseModel<UserRegistration>() {Success=true,Message="Registration sucessfull",Data=result });
            }

        }


        [HttpPost]
        public IActionResult UserLogin(UserLoginModel request)
        {
            var result = _buseness.UserLogin(request);

            if(result == null)
            {
                return BadRequest(new ResponseModel<string>() { Message = "Login Failed...!", Success = false, Data = null });
            }
            else
            {
              //  HttpContext.Session.SetInt32("UserId",result.)
                return Ok(new ResponseModel<string>() { Message = "Login Sucess...!", Success = true, Data = result });
            }

           
        }

       


        [HttpPut]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                var result = _buseness.ForgetPassword(email);
                if (result != null)
                {

                    Send send = new Send();

                    send.SendingMail(result.Email, "Password is Trying to Changed is that you....! ");//result

                    Uri uri = new Uri("rabbitmq://localhost/NotesEmail_Queue");
                    var endPoint = await bus.GetSendEndpoint(uri);


                    return Ok(new ResponseModel<string>() { Message = "User Found", Success = true, Data = result.Token });
                }
                else
                {
                    return BadRequest(new ResponseModel<string>() { Success = false, Message = "User Not Found", Data = result.Token });
                }
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [Authorize]
        [HttpPost]
        public  IActionResult Resetpassword( string password, string ConfirmPasswor)
        { 
            string email=User.Claims.Where(x=>x.Type == "Email").FirstOrDefault().Value;

            var result= _buseness.Resetpassword(email, password,ConfirmPasswor);
            if (result)
            {
                return Ok(new ResponseModel<string>() { Success = true, Message = "Password Reset Sucessful.."});
            }
            else
            {
                return BadRequest(new ResponseModel<bool>() { Success = false, Message = "Password Reset Failed...!" });
            }

        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteMyAccount()
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=_buseness.DeleteMyAccount(userid);
            if(result)
            {
                return Ok(new ResponseModel<string>() { Success=true,Message="Account Deleted...!"});
            }
            else
            {
                return BadRequest(new ResponseModel<bool>() { Success = false, Message = "OOPS! something went wrong...!" });
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateUser(UserUpdateModel request)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=_buseness.UpdateUser(userid, request);
            if (result)
            {
                return Ok(new ResponseModel<string>() { Success = true, Message = "User Info Updated..." });
            }
            else
            {
                return BadRequest(new ResponseModel<bool>() { Success=false,Message="Some thing went Wrong...!"});
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMyDetails()
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
           var result= _buseness.GetMyDetails(userid);
            if (result!=null)
            {
                return Ok(new ResponseModel<object>() { Success = true,Message="here is your information...",Data=result });
            }
            else
            {
                return BadRequest(new ResponseModel<object>() { Success = false, Message = "No Information Found..!", Data = null });
            }

        }

    }
}

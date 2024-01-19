using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.ResuestModels
{
    public class ForgotPasswordModel
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}

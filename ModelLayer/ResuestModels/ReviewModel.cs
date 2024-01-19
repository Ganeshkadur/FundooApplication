using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.ResuestModels
{
    public class ReviewModel
    {
        [Required(ErrorMessage ="Comment Is A Requred Field")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "rating Is A Requred Field")]
        public string rating { get; set; }
    }
}

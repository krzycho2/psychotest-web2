using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace psychotest_web.Models
{
    public class UserIDModel
    {
        public string UserID { get; set; }

        public IdValidationResult idValidationResult { get; set; }
    }
}

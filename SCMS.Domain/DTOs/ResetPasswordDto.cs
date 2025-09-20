using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.Domain.DTOs
{
    public class ResetPasswordDto 
    {   [Required] public string Token 
        { get; set; } 
        [Required] public string Email 
        { get; set; } 
        [Required] public string Password 
        { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.Domain.DTOs
{
    public class ChangePasswordDto 
    {   [Required] public string OldPassword 
        { get; set; } 
        [Required] public string NewPassword 
        { get; set; } 
    }
}

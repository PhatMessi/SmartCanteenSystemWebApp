using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.Infrastructure.Services
{
    // Đây là file interface, định nghĩa chức năng cần có
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
        Task SendWelcomeEmailAsync(string toEmail, string temporaryPassword);
        Task SendParentLinkNotificationAsync(string parentEmail, string studentName);
    }
}

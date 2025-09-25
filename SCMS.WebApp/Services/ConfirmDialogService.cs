// SCMS.WebApp/Services/ConfirmDialogService.cs
using System;
using System.Threading.Tasks;

namespace SCMS.WebApp.Services
{
    public class ConfirmDialogService
    {
        public event Func<string, string, Task<bool>>? OnShow;

        public async Task<bool> Show(string title, string message)
        {
            if (OnShow != null)
            {
                return await OnShow.Invoke(title, message);
            }
            return false;
        }
    }
}
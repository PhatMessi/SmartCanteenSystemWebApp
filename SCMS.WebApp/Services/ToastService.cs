// File: SCMS.WebApp/Services/ToastService.cs
namespace SCMS.WebApp.Services
{
    public class ToastService : IDisposable
    {
        public event Action<string, ToastLevel> OnShow;
        public void ShowToast(string message, ToastLevel level = ToastLevel.Success)
        {
            OnShow?.Invoke(message, level);
        }
        public void Dispose() { }
    }
    public enum ToastLevel { Success, Danger }
}
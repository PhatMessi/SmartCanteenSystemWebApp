// File mới: SCMS.Application/BackgroundServices/PendingOrderCancellationService.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCMS.Application;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SCMS.Application.BackgroundServices
{
    public class PendingOrderCancellationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PendingOrderCancellationService> _logger;

        public PendingOrderCancellationService(IServiceProvider serviceProvider, ILogger<PendingOrderCancellationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Pending Order Cancellation Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Vì OrderService là scoped service, chúng ta cần tạo một scope mới
                    // để sử dụng nó trong một background service (là singleton)
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
                        await orderService.AutoCancelUnpaidOrdersAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while cancelling unpaid orders.");
                }

                // Chờ 1 phút trước khi lặp lại
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Pending Order Cancellation Service is stopping.");
        }
    }
}
// File: SCMS.API/Controllers/ReportsController.cs
using Microsoft.AspNetCore.Authorization; // Giữ lại using này
using Microsoft.AspNetCore.Mvc;
using SCMS.Application;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SCMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // THÊM DÒNG NÀY ĐỂ MỞ QUYỀN TRUY CẬP
    // [Authorize(Roles = "CanteenManager,SystemAdmin")] // VÔ HIỆU HÓA DÒNG NÀY
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("sales-summary")]
        public async Task<IActionResult> GetSalesSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var summary = await _reportService.GetSalesSummaryAsync(startDate, endDate);
            return Ok(summary);
        }
        [HttpGet("student-spending")]
        public async Task<IActionResult> GetStudentSpendingReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // CHUYỂN ĐỔI userId TỪ STRING SANG INT
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("User ID is invalid.");
            }

            var report = await _reportService.GetStudentSpendingReportAsync(userId, startDate, endDate);
            return Ok(report);
        }
    }
}
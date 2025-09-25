using System.ComponentModel.DataAnnotations;
namespace SCMS.Domain.DTOs
{
    public class TopUpRequestDto
    {
        [Required]
        [Range(10000, 5000000, ErrorMessage = "Số tiền nạp phải từ 10,000 đến 5,000,000 VND")]
        public decimal Amount { get; set; }
    }
}
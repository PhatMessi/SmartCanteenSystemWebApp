// File: SCMS.Application/Validators/CreateMenuItemDtoValidator.cs
using FluentValidation;
using SCMS.Domain.DTOs;

namespace SCMS.Application.Validators
{
    public class CreateMenuItemDtoValidator : AbstractValidator<CreateMenuItemDto>
    {
        public CreateMenuItemDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên món ăn không được để trống.")
                .MaximumLength(150).WithMessage("Tên món ăn không được vượt quá 150 ký tự.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Giá món ăn phải lớn hơn 0.");

            RuleFor(x => x.InventoryQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng tồn kho không được là số âm.");
        }
    }
}
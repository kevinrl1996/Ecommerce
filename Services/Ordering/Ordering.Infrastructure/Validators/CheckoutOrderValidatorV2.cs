using FluentValidation;
using Ordering.Core.DTOs;

namespace Ordering.Infrastructure.Validators
{
	public class CheckoutOrderValidatorV2 : AbstractValidator<CheckOutOrderDtoV2>
	{
		public CheckoutOrderValidatorV2()
		{
			RuleFor(o => o.UserName)
				.NotEmpty()
				.WithMessage("{UserName} es requerido.")
				.NotNull()
				.MaximumLength(70)
				.WithMessage("{UserName} no debe exceder los 70 caracteres.");
			RuleFor(o => o.TotalPrice)
				.NotEmpty()
				.WithMessage("{TotalPrice} es requerido.")
				.GreaterThan(-1)
				.WithMessage("{TotalPrice} no debe ser -ve.");
		}
	}
}

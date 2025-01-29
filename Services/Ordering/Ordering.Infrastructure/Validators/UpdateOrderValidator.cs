using FluentValidation;
using Ordering.Core.DTOs;

namespace Ordering.Infrastructure.Validators
{
	public class UpdateOrderValidator : AbstractValidator<UpdateOrderDto>
	{
		public UpdateOrderValidator()
		{
			RuleFor(o => o.Id)
				.NotEmpty()
				.NotNull()
				.WithMessage("{Id} es requerido.")
				.GreaterThan(0)
				.WithMessage("{Id} no puede ser -ve.");
			RuleFor(o => o.UserName)
				.NotEmpty()
				.WithMessage("{UserName} es requerido.")
				.NotNull()
				.MaximumLength(70)
				.WithMessage("{UserName} no puede exceder los 70 caracteres.");
			RuleFor(o => o.TotalPrice)
				.NotEmpty()
				.WithMessage("{TotalPrice} es requerido.")
				.GreaterThan(-1)
				.WithMessage("{TotalPrice} no puede ser -ve.");
			RuleFor(o => o.EmailAddress)
				.NotEmpty()
				.WithMessage("{EmailAddress} es requerido.");
			RuleFor(o => o.FirstName)
				.NotEmpty()
				.NotNull()
				.WithMessage("{FirstName} es requerido.");
			RuleFor(o => o.LastName)
				.NotEmpty()
				.NotNull()
				.WithMessage("{LastName} es requerido.");
		}
	}
}

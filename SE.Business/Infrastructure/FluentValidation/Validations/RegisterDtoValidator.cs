using FluentValidation;
using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.Infrastructure.FluentValidation.Validations
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleSet("all", () =>
            {
                RuleFor(d => d.Email).EmailAddress().WithMessage("Geçerli email adresi giriniz.");
                RuleFor(d => d.Email).NotNull().NotEmpty().WithMessage("Email alanı boş geçilemez.");
                RuleFor(d => d.Password).NotNull().NotEmpty().WithMessage("Şifre alanı boş geçilemez.");
                RuleFor(d => d.Phone).NotNull().NotEmpty().WithMessage("Telefon alanı boş geçilemez.");

            });
        }
    }
}

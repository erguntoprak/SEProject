using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Validations
{
    public class LoginModelValidator:AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(d => d.Email).EmailAddress().WithMessage("Geçerli mail adresi giriniz.");
            RuleFor(d => d.Email).NotNull().WithMessage("Email alanı boş geçilemez.");
            RuleFor(d => d.Email).NotEmpty().WithMessage("Email alanı boş geçilemez.");
        }
    }
}

﻿using FluentValidation;
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
            RuleFor(d => d.Email).EmailAddress().WithMessage("Geçerli email adresi giriniz.");
            RuleFor(d => d.Email).NotEmpty().WithMessage("Email alanı boş geçilemez.");
            RuleFor(d => d.Name).NotEmpty().WithMessage("Ad alanı boş geçilemez.");
            RuleFor(d => d.Surname).NotEmpty().WithMessage("Soyad alanı boş geçilemez.");
            RuleFor(d => d.Password).NotEmpty().WithMessage("Şifre alanı boş geçilemez.");
            RuleFor(d => d.Phone).NotEmpty().WithMessage("Telefon alanı boş geçilemez.");
        }
    }
}

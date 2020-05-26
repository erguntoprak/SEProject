﻿using FluentValidation;
using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.Infrastructure.FluentValidation.Validations
{
    public class BlogInsertDtoValidator : AbstractValidator<BlogInsertDto>
    {
        public BlogInsertDtoValidator()
        {
            RuleSet("all", () =>
            {
                RuleFor(x => x.Title).NotNull().WithMessage("Lütfen başlık giriniz.");
                RuleFor(x => x.UserId).NotNull().WithMessage("Kullanıcı boş geçilemez.");
                RuleFor(x => x.BlogItems).Must(x => x != null || x.Count >= 1).WithMessage("Lütfen içerik giriniz.");
            });
        }
    }
}
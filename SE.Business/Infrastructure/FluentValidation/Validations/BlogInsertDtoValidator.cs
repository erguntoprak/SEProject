using FluentValidation;
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

            RuleFor(x => x.Title).NotEmpty().WithMessage("Lütfen başlık giriniz.");
            RuleFor(x => x.MetaKeywords).NotEmpty().WithMessage("Lütfen meta anahtar kelime giriniz.");
            RuleFor(x => x.MetaTitle).NotEmpty().WithMessage("Lütfen meta başlık giriniz.");

            RuleFor(x => x.Title).MaximumLength(400).WithMessage("Başlık, 400 karakterden az olmalıdır.");
            RuleFor(x => x.MetaKeywords).MaximumLength(400).WithMessage("Meta Anahtar Kelime, 400 karakterden az olmalıdır.");
            RuleFor(x => x.MetaTitle).MaximumLength(400).WithMessage("Meta Başlık, 400 karakterden az olmalıdır.");

            RuleFor(x => x.FirstVisibleImageName).NotEmpty().WithMessage("Lütfen sayfalarda gösterilecek ilk görsel seçiniz.");


        }
    }
}


using FluentValidation;
using SE.Core.DTO;

namespace SE.Business.Infrastructure.FluentValidation.Validations
{
    public class EducationContactFormInsertDtoValidator : AbstractValidator<EducationContactFormInsertDto>
    {
        public EducationContactFormInsertDtoValidator()
        {
            RuleFor(d => d.Email).EmailAddress().WithMessage("Geçerli email adresi giriniz.");
            RuleFor(d => d.Email).NotEmpty().WithMessage("Email alanı boş geçilemez.");
            RuleFor(x => x.Email).MaximumLength(200).WithMessage("E-posta, 200 karakterden az olmalıdır.");

            RuleFor(d => d.NameSurname).NotEmpty().WithMessage("Ad Soyad alanı boş geçilemez.");
            RuleFor(x => x.NameSurname).MaximumLength(100).WithMessage("Ad Soyad, 100 karakterden az olmalıdır.");

            RuleFor(d => d.PhoneNumber).NotEmpty().WithMessage("Telefon alanı boş geçilemez.");
            RuleFor(x => x.PhoneNumber).MaximumLength(20).WithMessage("Telefon, 20 karakterden az olmalıdır.");

        }
    }
}

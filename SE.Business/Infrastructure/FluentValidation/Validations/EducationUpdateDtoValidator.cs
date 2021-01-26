using FluentValidation;
using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.Infrastructure.FluentValidation.Validations
{
    public class EducationUpdateDtoValidator : AbstractValidator<EducationUpdateDto>
    {
        public EducationUpdateDtoValidator()
        {
            RuleFor(x => x.AddressInformation.Address).NotEmpty().WithMessage("Lütfen adres giriniz.");
            RuleFor(x => x.AddressInformation.CityId).NotEmpty().Must(x => x != 0).WithMessage("Lütfen şehir seçiniz.");
            RuleFor(x => x.AddressInformation.DistrictId).NotEmpty().Must(x => x != 0).WithMessage("Lütfen ilçe seçiniz.");
            RuleFor(x => x.ContactInformation.AuthorizedEmail).NotEmpty().WithMessage("Lütfen yetkili email giriniz.");
            RuleFor(x => x.ContactInformation.AuthorizedName).NotEmpty().WithMessage("Lütfen yetkili adı giriniz.");
            RuleFor(x => x.ContactInformation.EducationEmail).NotEmpty().WithMessage("Lütfen eğitim emaili giriniz.");
            RuleFor(x => x.ContactInformation.EducationWebsite).NotEmpty().WithMessage("Lütfen eğitim sitesi giriniz.");
            RuleFor(x => x.ContactInformation.PhoneOne).NotEmpty().WithMessage("Lütfen telefon 1 giriniz.");
            RuleFor(x => x.ContactInformation.PhoneTwo).NotEmpty().WithMessage("Lütfen telefon 2 giriniz.");
            RuleFor(x => x.GeneralInformation.EducationName).NotEmpty().WithMessage("Lütfen eğitim adı giriniz.");
            RuleFor(x => x.GeneralInformation.EducationType).NotEmpty().Must(x => x != 0).WithMessage("Lütfen eğitim tipi giriniz.");
            RuleFor(x => x.Images).Must(x => x != null || x.Length >= 1).WithMessage("Lütfen eğitim görseli ekleyiniz.");
        }
    }
}

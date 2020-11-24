using FluentValidation;
using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.Infrastructure.FluentValidation.Validations
{
    public class EducationInsertDtoValidator : AbstractValidator<EducationInsertDto>
    {
        public EducationInsertDtoValidator()
        {
            RuleFor(x => x.AddressInformation.Address).NotNull().WithMessage("Lütfen adres giriniz.");
            RuleFor(x => x.AddressInformation.CityId).NotNull().Must(x => x != 0).WithMessage("Lütfen şehir seçiniz.");
            RuleFor(x => x.AddressInformation.DistrictId).NotNull().Must(x => x != 0).WithMessage("Lütfen ilçe seçiniz.");
            RuleFor(x => x.ContactInformation.AuthorizedEmail).NotNull().WithMessage("Lütfen yetkili email giriniz.");
            RuleFor(x => x.ContactInformation.AuthorizedName).NotNull().WithMessage("Lütfen yetkili adı giriniz.");
            RuleFor(x => x.ContactInformation.EducationEmail).NotNull().WithMessage("Lütfen eğitim emaili giriniz.");
            RuleFor(x => x.ContactInformation.EducationWebsite).NotNull().WithMessage("Lütfen eğitim sitesi giriniz.");
            RuleFor(x => x.ContactInformation.PhoneOne).NotNull().WithMessage("Lütfen telefon 1 giriniz.");
            RuleFor(x => x.ContactInformation.PhoneTwo).NotNull().WithMessage("Lütfen telefon 2 giriniz.");
            RuleFor(x => x.GeneralInformation.EducationName).NotNull().WithMessage("Lütfen eğitim adı giriniz.");
            RuleFor(x => x.GeneralInformation.EducationType).NotNull().Must(x => x != 0).WithMessage("Lütfen eğitim tipi giriniz.");
        }
    }
}

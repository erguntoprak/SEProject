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
            RuleFor(x => x.AddressInformation.Address).NotEmpty().WithMessage("Lütfen adres giriniz.");
            RuleFor(x => x.AddressInformation.CityId).NotEmpty().Must(x => x != 0).WithMessage("Lütfen şehir seçiniz.");
            RuleFor(x => x.AddressInformation.DistrictId).NotEmpty().Must(x => x != 0).WithMessage("Lütfen ilçe seçiniz.");
            RuleFor(x => x.ContactInformation.AuthorizedEmail).NotEmpty().WithMessage("Lütfen yetkili email giriniz.");
            RuleFor(x => x.ContactInformation.AuthorizedName).NotEmpty().WithMessage("Lütfen yetkili adı giriniz.");
            RuleFor(x => x.ContactInformation.EducationEmail).NotEmpty().WithMessage("Lütfen eğitim emaili giriniz.");
            RuleFor(x => x.ContactInformation.PhoneOne).NotEmpty().WithMessage("Lütfen telefon 1 giriniz.");
            RuleFor(x => x.GeneralInformation.EducationName).NotEmpty().WithMessage("Lütfen eğitim adı giriniz.");
            RuleFor(x => x.GeneralInformation.EducationType).NotEmpty().Must(x => x != 0).WithMessage("Lütfen eğitim tipi giriniz.");

            RuleFor(x => x.GeneralInformation.EducationName).MaximumLength(400).WithMessage("Eğitim Adı, 400 karakterden az olmalıdır.");

            RuleFor(x => x.ContactInformation.AuthorizedName).MaximumLength(100).WithMessage("Yetkili Adı, 100 karakterden az olmalıdır.");
            RuleFor(x => x.ContactInformation.AuthorizedEmail).MaximumLength(200).WithMessage("Yetkili Email, 200 karakterden az olmalıdır.");
            RuleFor(x => x.ContactInformation.PhoneOne).MaximumLength(20).WithMessage("Telefon, 20 karakterden az olmalıdır.");
            RuleFor(x => x.ContactInformation.PhoneTwo).MaximumLength(20).WithMessage("Telefon, 20 karakterden az olmalıdır.");
            RuleFor(x => x.ContactInformation.EducationEmail).MaximumLength(200).WithMessage("Kurum Email, 200 karakterden az olmalıdır.");
            RuleFor(x => x.ContactInformation.EducationWebsite).MaximumLength(200).WithMessage("Kurum Website, 200 karakterden az olmalıdır.");

            RuleFor(x => x.SocialInformation.YoutubeVideoOne).MaximumLength(300).WithMessage("Youtube Video Link 1, 300 karakterden az olmalıdır.");
            RuleFor(x => x.SocialInformation.YoutubeVideoTwo).MaximumLength(300).WithMessage("Youtube Video Link 2, 300 karakterden az olmalıdır.");
            RuleFor(x => x.SocialInformation.FacebookAccountUrl).MaximumLength(300).WithMessage("Facebook Link, 300 karakterden az olmalıdır.");
            RuleFor(x => x.SocialInformation.InstagramAccountUrl).MaximumLength(300).WithMessage("Instagram Link, 300 karakterden az olmalıdır.");
            RuleFor(x => x.SocialInformation.TwitterAccountUrl).MaximumLength(300).WithMessage("Twitter Link, 300 karakterden az olmalıdır.");
            RuleFor(x => x.SocialInformation.YoutubeAccountUrl).MaximumLength(300).WithMessage("Youtube Link, 300 karakterden az olmalıdır.");
            RuleFor(x => x.SocialInformation.MapCode).MaximumLength(1000).WithMessage("Google Map Kodu, 1000 karakterden az olmalıdır.");

        }
    }
}

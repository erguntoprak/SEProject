using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SE.Business.Helpers;
using SE.Core.Entities;
using SE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Extentions
{
    public static class SeedData
    {
        public static void SeedDataSave(EntitiesDbContext context)
        {
            context.Database.Migrate();
            if (!context.AttributeCategory.Any())
            {
                var physical = new AttributeCategory() { Name = "Fiziksel İmkanlar" };
                var language = new AttributeCategory() { Name = "Dil Olanakları" };
                var servicesProvided = new AttributeCategory() { Name = "Verdiği Hizmetler" };
                var sport = new AttributeCategory() { Name = "Sportif Olanaklar" };
                var art = new AttributeCategory() { Name = "Sanatsal Olanaklar" };
                var club = new AttributeCategory() { Name = "Kulüpler" };


                context.AttributeCategory.Add(physical);
                context.AttributeCategory.Add(language);
                context.AttributeCategory.Add(servicesProvided);
                context.AttributeCategory.Add(sport);
                context.AttributeCategory.Add(art);
                context.AttributeCategory.Add(club);
                context.SaveChanges();

                var attributes = new List<Core.Entities.Attribute>
                {
                  new Core.Entities.Attribute{ Name = "Uyku Odası", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Bilgisayar Laboratuvarı", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Konferans Salonu", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Kantin", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Oyun Alanı", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Lojman", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Sera", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Spor Alanı", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Yemekhane", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Kapalı Spor Salonu", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Laboratuvar", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Kütüphane", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Revir", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Akıllı Tahta", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "3D Odası", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Havuz", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Futbol Sahası", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Sanat Atölyesi", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Müzik Odası", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Bahçe", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Hayvanat Bahçesi", AttributeCategoryId=physical.Id},
                  new Core.Entities.Attribute{ Name = "Mutfak Atölyesi", AttributeCategoryId=physical.Id},


                  new Core.Entities.Attribute{ Name = "Çince", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "İngilizce", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "İbranice", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "Ermenice", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "Fransızca", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "İspanyolca", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "İtalyanca", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "Japonca", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "Almanca", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "Arapça", AttributeCategoryId=language.Id},
                  new Core.Entities.Attribute{ Name = "Rusça", AttributeCategoryId=language.Id},


                  new Core.Entities.Attribute{ Name = "Güvenlik", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Rehberlik", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Yaz Okulu", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Servis", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Haftasonu Eğitim", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Organik Beslenme", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Oyun Grubu", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Anne-Çocuk Oyun Grubu", AttributeCategoryId=servicesProvided.Id},
                  new Core.Entities.Attribute{ Name = "Dini Eğitim", AttributeCategoryId=servicesProvided.Id},


                  new Core.Entities.Attribute{ Name = "Futbol", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Voleybol", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Basketbol", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Judo", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Masa Tenisi", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Su Topu", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Yüzme", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Hentbol", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Tenis", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Eskrim", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Güreş", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Atletizm", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Okçuluk", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Jimnastik", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Yoga", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Badminton", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Beden Eğitimi", AttributeCategoryId=sport.Id},
                  new Core.Entities.Attribute{ Name = "Taekwondo", AttributeCategoryId=sport.Id},


                  new Core.Entities.Attribute{ Name = "Fotoğrafçılık", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Bale", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Origami", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Sinema", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Su Balesi", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Dekoratif Sanatlar", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Heykel", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Modern Dans", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Drama", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Görsel Sanatlar", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Tiyatro", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Ebru", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Buz Pateni", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Perküsyon", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Piyano", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "İngilizce Drama", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Orkestra", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Koro", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Halk Oyunları", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Dans", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "El Sanatları", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Orff", AttributeCategoryId=art.Id},
                  new Core.Entities.Attribute{ Name = "Gitar", AttributeCategoryId=art.Id},


                  new Core.Entities.Attribute{ Name = "Satranç", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Seramik Kulübü", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Yabancı Dil Kulübü", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Müzik Kulübü", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "İzcilik", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Binicilik Kulübü", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Proje Kulübü", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Gezi Kulübü", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Kaya Tırmanışı", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Bilişim Kulübü", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Gazetecilik", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Ekoloji", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Akıl ve Zeka Oyunları", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Değerler Eğitimi", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Planetarium - Gök Bilimi", AttributeCategoryId=club.Id},
                  new Core.Entities.Attribute{ Name = "Robotik ve Kodlama", AttributeCategoryId=club.Id},
                };
                context.Attribute.AddRange(attributes);

                var preSchool = new Category { Name = "Özel Anaokul", SeoUrl = UrlHelper.FriendlyUrl("Özel Anaokul") };
                var teachingCourse = new Category { Name = "Özel Öğretim Kursu", SeoUrl = UrlHelper.FriendlyUrl("Özel Öğretim Kursu") };
               
                context.Category.Add(preSchool);
                context.Category.Add(teachingCourse);
                context.SaveChanges();


                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                   CategoryId = preSchool.Id,
                   AttributeCategoryId = physical.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = preSchool.Id,
                    AttributeCategoryId = language.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = preSchool.Id,
                    AttributeCategoryId = servicesProvided.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = preSchool.Id,
                    AttributeCategoryId = sport.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = preSchool.Id,
                    AttributeCategoryId = art.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = preSchool.Id,
                    AttributeCategoryId = club.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = teachingCourse.Id,
                    AttributeCategoryId = physical.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = teachingCourse.Id,
                    AttributeCategoryId = language.Id
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    CategoryId = teachingCourse.Id,
                    AttributeCategoryId = servicesProvided.Id
                });
                var city = new City { Name = "İzmir" };
                context.City.Add(city);
                context.SaveChanges();

                context.District.Add(new District { Name = "Aliağa", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Aliağa") });
                context.District.Add(new District { Name = "Balçova", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Balçova") });
                context.District.Add(new District { Name = "Bayındır", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Bayındır") });
                context.District.Add(new District { Name = "Bayraklı", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Bayraklı") });
                context.District.Add(new District { Name = "Bergama", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Bergama") });
                context.District.Add(new District { Name = "Beydağ", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Beydağ") });
                context.District.Add(new District { Name = "Bornova", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Bornova") });
                context.District.Add(new District { Name = "Buca", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Buca") });
                context.District.Add(new District { Name = "Çeşme", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Çeşme") });
                context.District.Add(new District { Name = "Çiğli", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Çiğli") });
                context.District.Add(new District { Name = "Dikili", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Dikili") });
                context.District.Add(new District { Name = "Foça", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Foça") });
                context.District.Add(new District { Name = "Gaziemir", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Gaziemir") });
                context.District.Add(new District { Name = "Güzelbahçe", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Güzelbahçe") });
                context.District.Add(new District { Name = "Karabağlar", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Karabağlar") });
                context.District.Add(new District { Name = "Karaburun", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Karaburun") });
                context.District.Add(new District { Name = "Karşıyaka", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Karşıyaka") });
                context.District.Add(new District { Name = "Kemalpaşa", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Kemalpaşa") });
                context.District.Add(new District { Name = "Kınık", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Kınık") });
                context.District.Add(new District { Name = "Kiraz", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Kiraz") });
                context.District.Add(new District { Name = "Konak", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Konak") });
                context.District.Add(new District { Name = "Menderes", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Menderes") });
                context.District.Add(new District { Name = "Menemen", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Menemen") });
                context.District.Add(new District { Name = "Narlıdere", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Narlıdere") });
                context.District.Add(new District { Name = "Ödemiş", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Ödemiş") });
                context.District.Add(new District { Name = "Seferihisar", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Seferihisar") });
                context.District.Add(new District { Name = "Selçuk", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Selçuk") });
                context.District.Add(new District { Name = "Tire", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Tire") });
                context.District.Add(new District { Name = "Torbalı", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Torbalı") });
                context.District.Add(new District { Name = "Urla", CityId = city.Id, SeoUrl = UrlHelper.FriendlyUrl("Urla") });
                context.SaveChanges();
            }


        }
    }
}

using Microsoft.EntityFrameworkCore;
using SE.Core.Entities;
using SE.Data;
using SE.Web.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Extentions
{
    public static class SeedData
    {
        public static void SeedDataSaved(EntitiesDbContext context)
        {
            context.Database.Migrate();
            if (!context.AttributeCategory.Any())
            {
                var fizikselImkanlar = new AttributeCategory() { Name = "Fiziksel İmkanlar" };
                var hizmetler = new AttributeCategory() { Name = "Hizmetler" };
                var kulupler = new AttributeCategory() { Name = "Kulüpler" };

                context.AttributeCategory.Add(fizikselImkanlar);
                context.AttributeCategory.Add(hizmetler);
                context.AttributeCategory.Add(kulupler);

                var attributes = new List<Core.Entities.Attribute>
                {
                    new Core.Entities.Attribute { AttributeCategory= fizikselImkanlar, Name="Uyku Odası", Description="", DisplayOrder=0},
                    new Core.Entities.Attribute { AttributeCategory= fizikselImkanlar, Name="Yemekhane", Description="", DisplayOrder=1},
                    new Core.Entities.Attribute { AttributeCategory= fizikselImkanlar, Name="Havuz", Description="", DisplayOrder=2},
                    new Core.Entities.Attribute { AttributeCategory= fizikselImkanlar, Name="Oyun Alanı", Description="", DisplayOrder=3},
                    new Core.Entities.Attribute { AttributeCategory= fizikselImkanlar, Name="Bahçe", Description="", DisplayOrder=3},

                    new Core.Entities.Attribute { AttributeCategory= hizmetler, Name="Güvenlik", Description="", DisplayOrder=0},
                    new Core.Entities.Attribute { AttributeCategory= hizmetler, Name="Rehberlik", Description="", DisplayOrder=1},
                    new Core.Entities.Attribute { AttributeCategory= hizmetler, Name="Servis", Description="", DisplayOrder=2},
                    new Core.Entities.Attribute { AttributeCategory= hizmetler, Name="Oyun Grubu", Description="", DisplayOrder=3},
                    new Core.Entities.Attribute { AttributeCategory= hizmetler, Name="Organik Beslenme", Description="", DisplayOrder=3},

                    new Core.Entities.Attribute { AttributeCategory= kulupler, Name="Satranç", Description="", DisplayOrder=0},
                    new Core.Entities.Attribute { AttributeCategory= kulupler, Name="Rehberlik", Description="", DisplayOrder=1},
                    new Core.Entities.Attribute { AttributeCategory= kulupler, Name="Müzik Kulübü", Description="", DisplayOrder=2},
                    new Core.Entities.Attribute { AttributeCategory= kulupler, Name="Gezi Kulübü", Description="", DisplayOrder=3},
                    new Core.Entities.Attribute { AttributeCategory= kulupler, Name="Akıl ve Zeka Oyunları", Description="", DisplayOrder=3},

                };
                context.Attribute.AddRange(attributes);

                var categoryList = new List<Category>{
                    new Category { Name="Özel Anaokul",SeoUrl=UrlHelper.FriendlyUrl("Özel Anaokul") },
                    new Category { Name="Özel İlkokul",SeoUrl=UrlHelper.FriendlyUrl("Özel İlkokul") },
                    new Category { Name="Özel Ortaokul" ,SeoUrl=UrlHelper.FriendlyUrl("Özel Ortaokul")},
                    new Category { Name="Özel Lise",SeoUrl=UrlHelper.FriendlyUrl("Özel Lise") },
                    new Category { Name="Özel Anadolu Lisesi",SeoUrl=UrlHelper.FriendlyUrl("Özel Anadolu Lisesi") },
                    new Category { Name="Kolej",SeoUrl=UrlHelper.FriendlyUrl("Kolej") },
                    new Category { Name="Dil Okulu & Kursu",SeoUrl=UrlHelper.FriendlyUrl("Dil Okulu & Kursu") },
                    new Category { Name="Kişisel Gelişim Kursu",SeoUrl=UrlHelper.FriendlyUrl("Kişisel Gelişim Kursu") },
                    new Category { Name="Kurs Merkezi",SeoUrl=UrlHelper.FriendlyUrl("Kurs Merkezi") }
                };
                context.Category.AddRange(categoryList);
                context.SaveChanges();


                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    AttributeCategory = fizikselImkanlar,
                    CategoryId = 1
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    AttributeCategory = hizmetler,
                    CategoryId = 1
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    AttributeCategory = kulupler,
                    CategoryId = 1
                });

                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    AttributeCategory = fizikselImkanlar,
                    CategoryId = 2
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    AttributeCategory = hizmetler,
                    CategoryId = 2
                });
                context.CategoryAttributeCategory.Add(new CategoryAttributeCategory
                {
                    AttributeCategory = kulupler,
                    CategoryId = 3
                });

                context.City.Add(new City { Name = "İzmir" });
                context.SaveChanges();

                context.District.Add(new District { Name = "ALİAĞA", CityId = 1 });
                context.District.Add(new District { Name = "BALÇOVA", CityId = 1 });
                context.District.Add(new District { Name = "BAYINDIR", CityId = 1 });
                context.District.Add(new District { Name = "BAYRAKLI", CityId = 1 });
                context.District.Add(new District { Name = "BERGAMA", CityId = 1 });
                context.District.Add(new District { Name = "BEYDAĞ", CityId = 1 });
                context.District.Add(new District { Name = "BORNOVA", CityId = 1 });
                context.District.Add(new District { Name = "BUCA", CityId = 1 });
                context.District.Add(new District { Name = "ÇEŞME", CityId = 1 });
                context.District.Add(new District { Name = "ÇİĞLİ", CityId = 1 });
                context.District.Add(new District { Name = "DİKİLİ", CityId = 1 });
                context.District.Add(new District { Name = "FOÇA", CityId = 1 });
                context.District.Add(new District { Name = "GAZİEMİR", CityId = 1 });
                context.District.Add(new District { Name = "GÜZELBAHÇE", CityId = 1 });
                context.District.Add(new District { Name = "KARABAĞLAR", CityId = 1 });
                context.District.Add(new District { Name = "KARABURUN", CityId = 1 });
                context.District.Add(new District { Name = "KARŞIYAKA", CityId = 1 });
                context.District.Add(new District { Name = "KEMALPAŞA", CityId = 1 });
                context.District.Add(new District { Name = "KINIK", CityId = 1 });
                context.District.Add(new District { Name = "KİRAZ", CityId = 1 });
                context.District.Add(new District { Name = "KONAK", CityId = 1 });
                context.District.Add(new District { Name = "MENDERES", CityId = 1 });
                context.District.Add(new District { Name = "NARLIDERE", CityId = 1 });
                context.District.Add(new District { Name = "ÖDEMİŞ", CityId = 1 });
                context.District.Add(new District { Name = "SEFERİHİSAR", CityId = 1 });
                context.District.Add(new District { Name = "SELÇUK", CityId = 1 });
                context.District.Add(new District { Name = "TİRE", CityId = 1 });
                context.District.Add(new District { Name = "TORBALI", CityId = 1 });
                context.District.Add(new District { Name = "URLA", CityId = 1 });
                context.SaveChanges();
            }


        }
    }
}

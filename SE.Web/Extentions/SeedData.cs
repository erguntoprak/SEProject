using Microsoft.EntityFrameworkCore;
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
                    new Core.Entities.Attribute { AttributeCategoryId= fizikselImkanlar.Id, Name="Uyku Odası", Description="", DisplayOrder=0},
                    new Core.Entities.Attribute { AttributeCategoryId= fizikselImkanlar.Id, Name="Yemekhane", Description="", DisplayOrder=1},
                    new Core.Entities.Attribute { AttributeCategoryId= fizikselImkanlar.Id, Name="Havuz", Description="", DisplayOrder=2},
                    new Core.Entities.Attribute { AttributeCategoryId= fizikselImkanlar.Id, Name="Oyun Alanı", Description="", DisplayOrder=3},
                    new Core.Entities.Attribute { AttributeCategoryId= fizikselImkanlar.Id, Name="Bahçe", Description="", DisplayOrder=3},

                    new Core.Entities.Attribute { AttributeCategoryId= hizmetler.Id, Name="Güvenlik", Description="", DisplayOrder=0},
                    new Core.Entities.Attribute { AttributeCategoryId= hizmetler.Id, Name="Rehberlik", Description="", DisplayOrder=1},
                    new Core.Entities.Attribute { AttributeCategoryId= hizmetler.Id, Name="Servis", Description="", DisplayOrder=2},
                    new Core.Entities.Attribute { AttributeCategoryId= hizmetler.Id, Name="Oyun Grubu", Description="", DisplayOrder=3},
                    new Core.Entities.Attribute { AttributeCategoryId= hizmetler.Id, Name="Organik Beslenme", Description="", DisplayOrder=3},

                    new Core.Entities.Attribute { AttributeCategoryId= kulupler.Id, Name="Satranç", Description="", DisplayOrder=0},
                    new Core.Entities.Attribute { AttributeCategoryId= kulupler.Id, Name="Rehberlik", Description="", DisplayOrder=1},
                    new Core.Entities.Attribute { AttributeCategoryId= kulupler.Id, Name="Müzik Kulübü", Description="", DisplayOrder=2},
                    new Core.Entities.Attribute { AttributeCategoryId= kulupler.Id, Name="Gezi Kulübü", Description="", DisplayOrder=3},
                    new Core.Entities.Attribute { AttributeCategoryId= kulupler.Id, Name="Akıl ve Zeka Oyunları", Description="", DisplayOrder=3},

                };
                context.Attribute.AddRange(attributes);
                context.SaveChanges();
            }

            if (!context.Category.Any())
            {
                var categoryList = new List<Category>{
                    new Category { Name="Özel Anaokul" },
                    new Category { Name="Özel İlkokul" },
                    new Category { Name="Özel Ortaokul" },
                    new Category { Name="Özel Lise" },
                    new Category { Name="Özel Anadolu Lisesi" },
                    new Category { Name="Kolej" },
                    new Category { Name="Dil Okulu & Kursu" },
                    new Category { Name="Kişisel Gelişim Kursu" },
                    new Category { Name="Kurs Merkezi" }
                };
                context.Category.AddRange(categoryList);
                context.SaveChanges();
            }
        }
    }
}

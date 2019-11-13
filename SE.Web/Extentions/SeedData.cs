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
            if (!context.EducationAttributeCategory.Any())
            {
                var fizikselImkanlar = new EducationAttributeCategory() { Name = "Fiziksel İmkanlar" };
                var hizmetler = new EducationAttributeCategory() { Name = "Hizmetler" };
                var kulupler = new EducationAttributeCategory() { Name = "Kulüpler" };

                context.EducationAttributeCategory.Add(fizikselImkanlar);
                context.EducationAttributeCategory.Add(hizmetler);
                context.EducationAttributeCategory.Add(kulupler);

                var attributes = new List<EducationAttribute>
                {
                    new EducationAttribute {EducationAttributeCategoryId=fizikselImkanlar.Id, Name="Uyku Odası", Description="", DisplayOrder=0},
                    new EducationAttribute {EducationAttributeCategoryId=fizikselImkanlar.Id, Name="Yemekhane", Description="", DisplayOrder=1},
                    new EducationAttribute {EducationAttributeCategoryId=fizikselImkanlar.Id, Name="Havuz", Description="", DisplayOrder=2},
                    new EducationAttribute {EducationAttributeCategoryId=fizikselImkanlar.Id, Name="Oyun Alanı", Description="", DisplayOrder=3},
                    new EducationAttribute {EducationAttributeCategoryId=fizikselImkanlar.Id, Name="Bahçe", Description="", DisplayOrder=3},

                    new EducationAttribute {EducationAttributeCategoryId=hizmetler.Id, Name="Güvenlik", Description="", DisplayOrder=0},
                    new EducationAttribute {EducationAttributeCategoryId=hizmetler.Id, Name="Rehberlik", Description="", DisplayOrder=1},
                    new EducationAttribute {EducationAttributeCategoryId=hizmetler.Id, Name="Servis", Description="", DisplayOrder=2},
                    new EducationAttribute {EducationAttributeCategoryId=hizmetler.Id, Name="Oyun Grubu", Description="", DisplayOrder=3},
                    new EducationAttribute {EducationAttributeCategoryId=hizmetler.Id, Name="Organik Beslenme", Description="", DisplayOrder=3},

                    new EducationAttribute {EducationAttributeCategoryId=kulupler.Id, Name="Satranç", Description="", DisplayOrder=0},
                    new EducationAttribute {EducationAttributeCategoryId=kulupler.Id, Name="Rehberlik", Description="", DisplayOrder=1},
                    new EducationAttribute {EducationAttributeCategoryId=kulupler.Id, Name="Müzik Kulübü", Description="", DisplayOrder=2},
                    new EducationAttribute {EducationAttributeCategoryId=kulupler.Id, Name="Gezi Kulübü", Description="", DisplayOrder=3},
                    new EducationAttribute {EducationAttributeCategoryId=kulupler.Id, Name="Akıl ve Zeka Oyunları", Description="", DisplayOrder=3},

                };
                context.EducationAttribute.AddRange(attributes);
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

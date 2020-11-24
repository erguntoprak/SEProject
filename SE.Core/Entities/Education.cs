using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Education : BaseEntity
    {
        public Education()
        {
            AttributeEducations = new List<AttributeEducation>();
            EducationAddress = new EducationAddress();
            Images = new List<Image>();
            Questions = new List<Question>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public string AuthorizedName { get; set; }
        public string AuthorizedEmail { get; set; }
        public string PhoneOne { get; set; }
        public string PhoneTwo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string YoutubeVideoOne { get; set; }
        public string YoutubeVideoTwo { get; set; }
        public string FacebookAccountUrl { get; set; }
        public string InstagramAccountUrl { get; set; }
        public string TwitterAccountUrl { get; set; }
        public string YoutubeAccountUrl { get; set; }
        public string MapCode { get; set; }
        public string SeoUrl { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
        public ICollection<AttributeEducation> AttributeEducations { get; set; }
        public EducationAddress EducationAddress { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<EducationContactForm> EducationContactForms { get; set; }
    }
}

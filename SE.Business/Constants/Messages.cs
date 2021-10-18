using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.Constants
{
    public static class Messages
    {
        public static string CouldNotBeVerifyCid => "Kimlik No Doğrulamadı.";
        public static string YouNeedToVerifyYourEmailAddress => "E-posta adresinizi doğrulamanız gerekiyor, lütfen e-postanıza gelen bağlantıya tıklayın.";
        public static string AccountIsNotActive => "Üzgünüz, bu hesap aktif değil. Tarafınıza en kısa sürede dönüş sağlanacaktır. Bir sorun olduğunu düşünüyorsanız, lütfen iletişime geçin.";
        public static string SuccessfulLogin => "Başarılı giriş yapıldı.";
        public static string SuccessfulRegister => "Başarılı kayıt yapıldı.";
        public static string UserNotFound => "Kullanıcı Bulunamadı.";
        public static string EmailAndPasswordNotMatch => "E-posta adresiniz ve/veya şifreniz hatalı.";
        public static string CouldNotAddRoles => "Roller eklenemedi.";
        public static string CouldNotDeleteRoles => "Roller silinemedi.";
        public static string Added => "Başarıyla eklendi.";
        public static string Deleted => "Başarıyla silindi.";
        public static string Updated => "Başarıyla güncellendi.";
        public static string NotAdded => "Ekleme başarısız.";
        public static string NotDeleted => "Silme başarısız.";
        public static string NotUpdated => "Güncelleme başarısız.";
        public static string EmailSended => "E-posta gönderildi.";
        public static string EmailUsedBefore => "E-posta adresi daha önce kullanılmış.";
        public static string UserNameUsedBefore => "Kullanıcı adı daha önce kullanılmış.";
        public static string EmailAlreadyConfirmed => "E-posta adresiniz zaten onaylanmış.";
        public static string EmailConfirmed => "E-posta adresiniz onaylandı.";
        public static string EmailNotConfirmed => "E-posta adresiniz onaylanmadı.";
        public static string ObjectIsNull => "Talep edilen nesne bulunamadı.";
        public static string AlreadyRole => "Bu role daha önce tanımlanmış.";

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SE.Business.Helpers
{
    public static class UrlHelper
    {
        public static string FriendlyUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            url = url.ToLower();
            url = url.Trim();
            if (url.Length > 100)
            {
                url = url.Substring(0, 100);
            }
            url = url.Replace("İ", "i");
            url = url.Replace("ı", "i");
            url = url.Replace("ğ", "g");
            url = url.Replace("Ğ", "g");
            url = url.Replace("ç", "c");
            url = url.Replace("Ç", "c");
            url = url.Replace("ö", "o");
            url = url.Replace("Ö", "o");
            url = url.Replace("ş", "s");
            url = url.Replace("Ş", "s");
            url = url.Replace("ü", "u");
            url = url.Replace("Ü", "u");
            url = url.Replace("'", "");
            url = url.Replace("\"", "");
            char[] replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            for (int i = 0; i < replacerList.Length; i++)
            {
                string strChr = replacerList[i].ToString();
                if (url.Contains(strChr))
                {
                    url = url.Replace(strChr, string.Empty);
                }
            }
            Regex r = new Regex("[^a-zA-Z0-9_-]");
            url = r.Replace(url, "-");
            while (url.IndexOf("--") > -1)
                url = url.Replace("--", "-");
            return url;
        }
    }
}

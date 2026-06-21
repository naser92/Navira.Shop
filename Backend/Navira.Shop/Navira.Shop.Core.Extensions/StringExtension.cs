using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Navira.Shop.Core.Extensions
{
    public static class StringExtension
    {
        public const short RealPerson = 1;
        public const short Legal = 2;
        public const short Participation = 3;
        public const short Foreigne = 4;

        public static string TtmsHash(this string input)
        {
            var hash = BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(input))).Replace("-", string.Empty).ToUpper();
            return hash;
        }

        public static string SHA1(this string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
        public static string SHA512(this string input)
        {
            var hash = new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
        public static string AES(this string input, string key = "", string IV = "")
        {
            var keyByte = Encoding.UTF8.GetBytes(key);
            var IVByte = Encoding.UTF8.GetBytes(IV);
            byte[] encrypted;
            var encryptor = new AesManaged();
            encryptor.Mode = CipherMode.CBC;
            var ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(keyByte, IVByte), CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                    sw.Write(input);
                encrypted = ms.ToArray();
            }

            return BitConverter.ToString(encrypted).Replace("_", "");
        }
        public static string ToPascalCase(this string values)
        {
            return char.ToUpperInvariant(values[0]) + values.Substring(1);
        }
        public static string GetValue(this string values)
        {
            var result = string.Empty;

            foreach (var value in values)
                result += GetTryIntValue(value);


            return result;
        }

        public static int GetTryIntValue(this char ch) => char.IsDigit(ch) ?
            int.Parse(ch.ToString()) : ch;


        public static string AddLeftZero(string data, int size)
        {
            int countData = data.Length;
            int countZero = size - countData;
            return new string('0', countZero) + data;
        }
        public static bool IsMatch(this string value, string rege)
        {
            if (value == null) return false;
            return Regex.IsMatch(value, rege);
        }

        public static string DeleteSpaceOther(string str)
        {
            return Regex.Replace(str.Trim(), " {2,}", " ");
        }

        public static string Base64Encode(string clearText)
        {
            var clearTextBytes = System.Text.Encoding.UTF8.GetBytes(clearText);
            return System.Convert.ToBase64String(clearTextBytes);
        }

        public static Guid? ToGuid(this string value) => value.IsNullOrWhiteSpace() ? (Guid?)null : Guid.Parse(value);


        public static string Base64Decode(string base64EncodeText)
        {
            var base64EncodeTextBytes = System.Convert.FromBase64String(base64EncodeText);
            return System.Text.Encoding.UTF8.GetString(base64EncodeTextBytes);
        }

        public static string Md5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            var strBuilder = new StringBuilder();
            foreach (byte t in result)
            {
                strBuilder.Append(t.ToString("x2"));
            }
            return strBuilder.ToString();
        }
        public static long FromHexToLong(this string value)
        {
            if (value.IsNullOrEmpty())
                return 0;
            return Convert.ToInt64(value, 16);
        }
        public static string Join(this string[] values, string seperator)
        {
            return string.Join(seperator, values);
        }
        public static int EconomicCodeBuyerType(this string economicCode, string nationalCode)
        {
            return economicCode switch
            {
                { Length: 14 } when nationalCode.Length == 12 => Foreigne,
                { Length: 14 } when nationalCode.Length == 10 && economicCode[10] == '0' => RealPerson,
                { Length: 11 } when economicCode.StartsWith("6") && (nationalCode.Length == 11 || nationalCode.Length == 10) => Participation,
                { Length: 11 } when (economicCode.StartsWith("1") || economicCode.StartsWith("3")) && nationalCode.Length == 11 => Legal,
                _ => 0
            };
        }
    }
}

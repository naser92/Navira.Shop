using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Navira.Shop.Core.Extensions
{
    public static class FixValueExtension
    {
        public static string FixDateFormat(this string persianDate)
        {
            if (IsNullValue(persianDate))
                return string.Empty;

            persianDate = persianDate.Trim().FixNumberValue();

            if (persianDate.Length > 10 || persianDate.Length < 6)
                return string.Empty;

            var dateParts = persianDate.Split('/');

            if (dateParts.Length != 3)
                dateParts = persianDate.Split('-');

            if (dateParts.Length != 3)
                return string.Empty;

            if (dateParts[0].Length > 4)
                return string.Empty;

            if (dateParts[2].Length > 2)
                return string.Empty;

            if (dateParts[1].Length > 2)
                return string.Empty;

            if (!IsValidateDay(dateParts[2]))
                return string.Empty;

            if (!IsValidateMonth(dateParts[1]))
                return string.Empty;

            dateParts[0] = FixYear(dateParts[0]);
            dateParts[1] = PadLeft(dateParts[1], 2);
            dateParts[2] = PadLeft(dateParts[2], 2);

            return $"{dateParts[0]}/{dateParts[1]}/{dateParts[2]}";
        }
        public static bool IsTruePersianDate(this string persianDate)
        {
            return Regex.IsMatch(persianDate, @"^\d{4}\/\d{2}\/\d{2}$");
        }
        private static bool IsNullValue(string value)
        {
            return value == null ||
                   string.IsNullOrWhiteSpace(value);
        }

        private static string FixYear(string year)
        {
            var newYear = string.Empty;

            switch (year.Length)
            {
                case 4: newYear = year; break;
                case 2: newYear = GetPersianCenturies() + year; break;
                case 3: newYear = "1" + year; break;
            }

            return newYear;
        }

        private static int GetPersianCenturies()
        {
            var str = GetPersianDate();

            if (str.Length != 4)
                throw new Exception("GetYear -> pYear.Length != 4");

            return Convert.ToInt32(str.Substring(0, 2));
        }

        private static string GetPersianDate()
        {
            return new PersianCalendar()
                      .GetYear(DateTime.Now)
                      .ToString(CultureInfo.InvariantCulture);
        }



        private static string PadLeft(string value, int len)
        {
            return value.PadLeft(len, '0');
        }

        private static bool IsValidateDay(string day)
        {
            int.TryParse(day, out var result);

            return result > 0 && result <= 31;
        }

        private static bool IsValidateMonth(string month)
        {
            int.TryParse(month, out var result);

            return result > 0 && result <= 12;
        }

        public static string FixNumberValue(this string value)
        {
            if (value == null) return null;

            return value
                   .ReplacePersianNumbers()
                   .ReplaceArabicNumbers();
        }

        private static string ReplacePersianNumbers(this string data)
        {
            if (string.IsNullOrEmpty(data)) return data;

            var result = new StringBuilder(data);

            for (var index = 48; index < 58; ++index)
                result.Replace
                      (Convert.ToChar(1728 + index), Convert.ToChar(index));

            return result.ToString();
        }
        private static string ReplaceArabicNumbers(this string data)
        {
            return string.IsNullOrEmpty(data) ?
                data :
                (new StringBuilder(data).Replace("٠", "0")
                    .Replace("١", "1")
                    .Replace("٢", "2")
                    .Replace("٣", "3")
                    .Replace("٤", "4")
                    .Replace("٥", "5")
                    .Replace("٦", "6")
                    .Replace("٧", "7")
                    .Replace("٨", "8")
                    .Replace("٩", "9")).ToString();

        }

        public static string FixMoneyValue(this string data)
        {
            return data.FixNumberValue().Replace(",", "");

        }
        public static string FixBourseContractNumber(this string bourseContractNumber)
        {
            bourseContractNumber = bourseContractNumber.Replace("\\", "00");
            bourseContractNumber = bourseContractNumber.Replace("/", "00");

            if (bourseContractNumber.Length == 10)
                bourseContractNumber += "001";

            if (bourseContractNumber.Length == 11)
                bourseContractNumber = AddZeroBeforeLastBourseContractNumber(bourseContractNumber);

            return bourseContractNumber;
        }

        private static string AddZeroBeforeLastBourseContractNumber(string bourseContractNumber)
        {
            var lastChar = bourseContractNumber[bourseContractNumber.Length - 1];

            return bourseContractNumber
                       .Substring(0, bourseContractNumber.Length - 1) +
                   "00" +
                   lastChar;
        }
    }
}


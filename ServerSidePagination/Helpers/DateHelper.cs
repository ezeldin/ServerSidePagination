using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MOJ.Accounting.Domain.Helpers
{
    public static class DateHelper
    {
        public static string GeregorianToHijri(DateTime date)
        {
            DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo("ar-SA").DateTimeFormat;
            dateTimeFormatInfo.Calendar = new UmAlQuraCalendar();
            dateTimeFormatInfo.ShortDatePattern = "d/M/yyyy";
            return date.ToString("yyyy/M/d", dateTimeFormatInfo);
        }

        public static DateTime HijriToGeregorian(string date)
        {
            date = date.Replace('/', '-');
            string[] dateSplit = date.Split('-');
            if (dateSplit.Count() == 3)
            {
                int yearIndex = (dateSplit[2].Length == 4 ? 2 : 0);
                int dayIndex = (yearIndex == 2 ? 0 : 2);

                DateTime umAlqura = new DateTime(Convert.ToInt32(dateSplit[yearIndex]), Convert.ToInt32(dateSplit[1]), Convert.ToInt32(dateSplit[dayIndex]), new UmAlQuraCalendar());
                return umAlqura;
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static DateTime ParseStringDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static bool IsHijriDate(string date)
        {
            string[] value = date.Split('/', '-');
            int day = 0;
            int month = 0;
            int year = 0;

            int yearIndex = (value[2].Length == 4 ? 2 : 0);
            int dayIndex = (yearIndex == 2 ? 0 : 2);

            if (value.Count() == 3)
            {
                bool isDay = int.TryParse(value[dayIndex], out day);
                if (isDay)
                {
                    if (day > 0 && day < 31)
                    {
                        bool isMonth = int.TryParse(value[1], out month);
                        if (isMonth)
                        {
                            if (month > 0 && month < 13)
                            {
                                bool isYear = int.TryParse(value[yearIndex], out year);
                                if (isYear)
                                {
                                    if (year > 0 && year < 1536)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

    }
}

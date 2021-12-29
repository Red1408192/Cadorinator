using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cadorinator.Service.Helper
{
    public static class DatetimeHelper
    {
        /// <summary>
        /// Only for future dates
        /// </summary>
        public static DateTimeOffset ParseScheduleDate(string input)
        {
            try
            {
                var currentDay = DateTime.Now.Day;
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                if (!int.TryParse(Regex.Match(input, "[0-9]{2}(?=,)")?.Value, out var inputDay)) throw new InvalidOperationException();
                if (!int.TryParse(Regex.Match(input, "[0-9]{2}(?=:)")?.Value, out var inputHour)) throw new InvalidOperationException();
                if (!int.TryParse(Regex.Match(input, "[0-9]{2}(?!:)(?!,)")?.Value, out var inputMinute)) throw new InvalidOperationException();

                int inputMonth, inputYear;
                if (currentDay > inputDay)
                {
                    if (currentMonth == 12)
                    {
                        inputMonth = 1;
                        inputYear = currentYear + 1;
                    }
                    else
                    {
                        inputYear = currentYear;
                        inputMonth = currentMonth + 1;
                    }
                }
                else
                {
                    inputMonth = currentMonth;
                    inputYear = currentYear;
                }

                var result = new DateTime(inputYear, inputMonth, inputDay, inputHour, inputMinute, 0);
                return new DateTimeOffset(result).ToUniversalTime();
            }
            catch(Exception ex)
            {
                return DateTime.UtcNow;
            }
        }
    }
}

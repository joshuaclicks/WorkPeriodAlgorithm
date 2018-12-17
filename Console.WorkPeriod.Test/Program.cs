using System;
using System.Collections.Generic;
using WorkingPeriodDetector;

namespace Console.WorkPeriod.Test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            DateTime _startDate, _endDate;
            var publicHolidays = new List<DateTime>();

            publicHolidays.Add(new DateTime(2017, 10, 5));
            publicHolidays.Add(new DateTime(2017, 10, 9));
            publicHolidays.Add(new DateTime(2017, 10, 12));
            publicHolidays.Add(new DateTime(2017, 10, 16));
            OperationalTimeDetector.WorkPeriod(DateTime.Now, long.Parse("259200"), new WorkingPeriodDetector.Models.TimeComponent(){Hour=8,Minute=0,Second=0}, new WorkingPeriodDetector.Models.TimeComponent() { Hour = 16, Minute = 0, Second = 0 }, new WorkingPeriodDetector.Models.TimeComponent() { Hour = 0, Minute = 0, Second = 0 }, new WorkingPeriodDetector.Models.TimeComponent() { Hour =22, Minute = 59, Second = 59}, publicHolidays, out _startDate, out _endDate);

            System.Console.WriteLine(_startDate.ToString());

            System.Console.WriteLine(_endDate.ToString());

            System.Console.ReadKey();


        }
    }
}

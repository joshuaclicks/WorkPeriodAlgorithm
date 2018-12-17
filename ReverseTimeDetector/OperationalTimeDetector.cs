using System;
using System.Collections.Generic;
using System.Linq;
using ReverseTimeDetector.Models;

namespace ReverseTimeDetector
{
    /// <summary>
    /// Class contains methods that calculate the start date and duedates for operations depending on the configured working hours, days, 
    /// taking into consideration the weekend and Holidays as Non-Working days.
    /// </summary>
    public class OperationalTimeDetector
    {

        /// <summary>
        /// Examines the non-weekend last previous day/date when an action was initiated.
        /// </summary>
        /// <param name="startDate">Date when the action was initiated. This is a processed date, having being processed with the WorkDateTimme algorithm</param>
        /// <returns></returns>
        private static DateTime ReverseNonWeekendDay(DateTime startDate)
        {
            try
            {
                DateTime workStartDate = new DateTime();
                TimeSpan timeOfstartDay = startDate.TimeOfDay;
                string dayOfWeek = startDate.DayOfWeek.ToString();
                
                if (dayOfWeek.ToLower() == WeekDays.Friday.ToString().ToLower())
                {
                    // Adding zero because the datetime is neither a weekend date or an off-work datetime as the supplied datre is already a processed date.
                    workStartDate = startDate.AddDays(0);
                }
                else if (dayOfWeek.ToLower() == WeekDays.Saturday.ToString().ToLower())
                {

                    workStartDate = startDate.AddDays(-1);
                }
                else if (dayOfWeek.ToLower() == WeekDays.Sunday.ToString().ToLower())
                {
                    workStartDate = startDate.AddDays(-2);
                }
                else if (dayOfWeek.ToLower() == WeekDays.Monday.ToString().ToLower())
                {
                    workStartDate = startDate.AddDays(-3);
                }

                return workStartDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Examines the non-holiday last previous day/date when an action was initiated.
        /// </summary>
        /// <param name="startDate">Date when the action was initiated.</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <returns></returns>
        private static DateTime ReverseNonHolidayDay(DateTime startDate, List<DateTime> holidayDates)
        {
            try
            {
                DateTime nonHolidayDateTime = startDate;
                // The Ordering in acending order is set to ensure easy search.
                List<DateTime> holidays = holidayDates.OrderBy(a => a.Date).ToList();
                foreach (var holiday in holidays)
                {
                    if (holiday.Date == nonHolidayDateTime.Date)
                    {
                        nonHolidayDateTime = nonHolidayDateTime.AddDays(-1);
                    }
                }
                return nonHolidayDateTime;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        /// <summary>
        /// Determines whether the day and time supplied is neither an holiday date nor a weekend date. 
        /// If it is, it gives the nearest day and time that is a working time as described by the WorkStartTime & WorkClosingTime.
        /// </summary>
        /// <param name="startDate">Date when the action was initiated.</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <param name="numberOfDays">Number of working days to subtract backwards.</param>
        /// <returns></returns>
        public static DateTime ReverseWorkDateTime(DateTime startDate, List<DateTime> holidayDates, int numberOfDays)
        {
            try
            {
                DateTime unexaminedDateTime = startDate;
                if(numberOfDays > 0)
                {
                    // Loop through to find the time where the previous number of days (in hours) which is not a weekend and holiday date was.
                    for(int count = 0; count < numberOfDays; count++)
                    {
                        while (ReverseNonHolidayDay(unexaminedDateTime, holidayDates) != ReverseNonWeekendDay(unexaminedDateTime))
                        {
                            unexaminedDateTime = ReverseNonHolidayDay(unexaminedDateTime, holidayDates);
                            unexaminedDateTime = ReverseNonWeekendDay(unexaminedDateTime);
                        }
                    }
                }
                else
                {
                    while (ReverseNonHolidayDay(unexaminedDateTime, holidayDates) != ReverseNonWeekendDay(unexaminedDateTime))
                    {
                        unexaminedDateTime = ReverseNonHolidayDay(unexaminedDateTime, holidayDates);
                        unexaminedDateTime = ReverseNonWeekendDay(unexaminedDateTime);
                    }
                }

                return unexaminedDateTime;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

    }
}

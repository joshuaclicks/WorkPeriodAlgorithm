using System;
using System.Collections.Generic;
using System.Linq;
using WorkingPeriodDetector.Models;

namespace WorkingPeriodDetector
{
    /// <summary>
    /// Class contains methods that calculate the start date and duedates for operations depending on the configured working hours, days, 
    /// taking into consideration the weekend and Holidays as Non-Working days.
    /// </summary>
    public class OperationalTimeDetector
    {
        /// <summary>
        /// Determines the Period a task or operation is to be performed or carried out with the Weekend and Holiday Days Removed from the days.
        /// </summary>
        /// <param name="startDate">Date when the action was initiated.</param>
        /// <param name="timeInterval">The Time that an operation is meant to span through (in Seconds).</param>
        /// <param name="workStartTime">Time Work is to start on a daily basis.</param>
        /// <param name="workClosingTime">Time Work is to close on a daily basis.</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <param name="estimatedStartDate">Day and Time the operation or Work is expected to Start after weighing against the Holiday and Weekend Metrics.</param>
        /// <param name="estimatedDueDate">Day and Time the operation or Work is expected to End after weighing against the Holiday and Weekend Metrics.</param>
        public static void WorkPeriod(DateTime startDate, long timeInterval, TimeComponent workStartTime, TimeComponent workClosingTime, List<DateTime> holidayDates, out DateTime estimatedStartDate, out DateTime estimatedDueDate)
        {
            estimatedStartDate = EventStartDate(startDate, workStartTime, workClosingTime, holidayDates);
            estimatedDueDate = EventEndDate(estimatedStartDate, timeInterval, workStartTime, workClosingTime, holidayDates);
        }

        /// <summary>
        /// Determines when the Turn Around Time for a Ticket is to Start and be Due, with the Weekend and Holiday Days Removed from the days.
        /// </summary>
        /// <param name="logInitiationDate">Date when the action was initiated: when the Complaint was logged.</param>
        /// <param name="timeInterval">The Time that an operation is meant to span through (in Seconds).</param>
        /// <param name="loggingStartTime">Time Complaint Logging must start on a daily basis.</param>
        /// <param name="loggingClosingTime">Time Complaint Logging must end on a daily basis.</param>
        /// <param name="logResponseStartTime">Time Complaint Resolution/Activities must start on a daily basis. This is the start time of each work period, it should be 00:00:00 for 24 work period</param>
        /// <param name="logResponseClosingTime">Time Complaint Resolution/Activities must end on a daily basis. This is the end time of each work period, it should be 11:59:59 for 24 work period</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <param name="estimatedStartDate">Day and Time the Complaint Turn Around Time (TAT) is expected to Start reading after weighing against the Holiday and Weekend Metrics.</param>
        /// <param name="estimatedDueDate">Day and Time the Complaint Turn Around Time (TAT) is expected to be due after weighing against the Holiday and Weekend Metrics.</param>
        public static void WorkPeriod(DateTime logInitiationDate, long timeInterval, TimeComponent loggingStartTime, TimeComponent loggingClosingTime, TimeComponent logResponseStartTime, TimeComponent logResponseClosingTime, List<DateTime> holidayDates, out DateTime estimatedStartDate, out DateTime estimatedDueDate)
        {
            estimatedStartDate = EventStartDate(logInitiationDate, loggingStartTime, loggingClosingTime, holidayDates);
            estimatedDueDate = EventEndDate(estimatedStartDate, timeInterval, logResponseStartTime, logResponseClosingTime, holidayDates);
        }

        /// <summary>
        /// Examines that the day an action was initiated is not a weekend.
        /// </summary>
        /// <param name="startDate">Date when the action was initiated.</param>
        /// <param name="workStartTime">Time Work is to start on a daily basis.</param>
        /// <param name="workClosingTime">Time Work is to close on a daily basis.</param>
        /// <returns></returns>
        private static DateTime NonWeekendDay(DateTime startDate, TimeComponent workStartTime, TimeComponent workClosingTime)
        {
            try
            {
                DateTime workStartDate;
                DateTime monday;
                DateTime nextDay;

                TimeSpan timeOfstartDay = startDate.TimeOfDay;
                TimeSpan resumptionTime = new TimeSpan(workStartTime.Hour, workStartTime.Minute, workStartTime.Second);
                TimeSpan closingTime = new TimeSpan(workClosingTime.Hour, workClosingTime.Minute, workClosingTime.Second);

                string dayOfWeek = startDate.DayOfWeek.ToString();

                if (dayOfWeek.ToLower() == WeekDays.Friday.ToString().ToLower())
                {
                    //try
                    //{
                    if (timeOfstartDay < resumptionTime)
                    {
                        workStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, resumptionTime.Hours, resumptionTime.Minutes, resumptionTime.Seconds, DateTimeKind.Local);
                    }
                    else if (timeOfstartDay >= resumptionTime && timeOfstartDay <= closingTime)
                    {
                        workStartDate = startDate;
                    }
                    else
                    {
                        monday = startDate.AddDays(3);
                        workStartDate = new DateTime(monday.Year, monday.Month, monday.Day, resumptionTime.Hours, resumptionTime.Minutes, resumptionTime.Hours, DateTimeKind.Local);
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }
                else if (dayOfWeek.ToLower() == WeekDays.Saturday.ToString().ToLower())
                {
                    //try
                    //{
                    monday = startDate.AddDays(2);
                    workStartDate = new DateTime(monday.Year, monday.Month, monday.Day, resumptionTime.Hours, resumptionTime.Minutes, resumptionTime.Seconds, DateTimeKind.Local);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }
                else if (dayOfWeek.ToLower() == WeekDays.Sunday.ToString().ToLower())
                {
                    //try
                    //{
                    monday = startDate.AddDays(1);
                    workStartDate = new DateTime(monday.Year, monday.Month, monday.Day, resumptionTime.Hours, resumptionTime.Minutes, resumptionTime.Seconds, DateTimeKind.Local);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }
                else
                {
                    //try
                    //{
                    if (timeOfstartDay < resumptionTime)
                    {
                        workStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, resumptionTime.Hours, resumptionTime.Minutes, resumptionTime.Seconds, DateTimeKind.Local);
                    }
                    else if (timeOfstartDay >= resumptionTime && timeOfstartDay <= closingTime)
                    {
                        workStartDate = startDate;
                    }
                    else
                    {
                        nextDay = startDate.AddDays(1);
                        workStartDate = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, resumptionTime.Hours, resumptionTime.Minutes, resumptionTime.Seconds, DateTimeKind.Local);
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }

                return workStartDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Examines that the day an action was initiated is not a Holiday.
        /// </summary>
        /// <param name="startDate">Date when the action was initiated.</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <returns></returns>
        private static DateTime NonHolidayDay(DateTime startDate, List<DateTime> holidayDates)
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
                        nonHolidayDateTime = nonHolidayDateTime.AddDays(1);
                        nonHolidayDateTime = new DateTime(nonHolidayDateTime.Year, nonHolidayDateTime.Month, nonHolidayDateTime.Day, 0, 0, 0);
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
        /// <param name="workStartTime">Time Work is to start on a daily basis.</param>
        /// <param name="workClosingTime">Time Work is to close on a daily basis.</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <returns></returns>
        public static DateTime WorkDateTime(DateTime startDate, TimeComponent workStartTime, TimeComponent workClosingTime, List<DateTime> holidayDates)
        {
            try
            {
                DateTime unexaminedDateTime = startDate;
                while (NonHolidayDay(unexaminedDateTime, holidayDates) != NonWeekendDay(unexaminedDateTime, workStartTime, workClosingTime))
                {
                    unexaminedDateTime = NonHolidayDay(unexaminedDateTime, holidayDates);
                    unexaminedDateTime = NonWeekendDay(unexaminedDateTime, workStartTime, workClosingTime);
                }

                return unexaminedDateTime;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Finds the Start Date for an Event which has no holiday or Weekend in it.
        /// </summary>
        /// <param name="startDate">Date when the action was initiated.</param>
        /// <param name="workStartTime">Time Work is to start on a daily basis.</param>
        /// <param name="workClosingTime">Time Work is to close on a daily basis.</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <returns></returns>
        private static DateTime EventStartDate(DateTime startDate, TimeComponent workStartTime, TimeComponent workClosingTime, List<DateTime> holidayDates)
        {
            try
            {
                return WorkDateTime(startDate, workStartTime, workClosingTime, holidayDates);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Finds the End Date for an Event with Holidays and Weekends Adequately Catered for.
        /// </summary>
        /// <param name="startDate">Expected Date when an Event is to Start, taking into consideration the Weekend and Holiday Constraints</param>
        /// <param name="operationsInterval">The Time that an operation is meant to span through (in Seconds)</param>
        /// <param name="workStartTime">Time Work is to start on a daily basis.</param>
        /// <param name="workClosingTime">Time Work is to close on a daily basis.</param>
        /// <param name="holidayDates">List of holiday dates in a year for a specific locations as determined by the internal/external system.</param>
        /// <returns></returns>
        private static DateTime EventEndDate(DateTime startDate, long operationsInterval, TimeComponent workStartTime, TimeComponent workClosingTime, List<DateTime> holidayDates)
        {
            TimeSpan resumptionTime = new TimeSpan(workStartTime.Hour, workStartTime.Minute, workStartTime.Second);
            TimeSpan closingTime = new TimeSpan(workClosingTime.Hour, workClosingTime.Minute, workClosingTime.Second);
            DateTime availableOperationDay = startDate;
            long overallOperationsTimeLeft = operationsInterval;

            // Operational Time Left
            DateTime normalEnd = availableOperationDay.AddSeconds(operationsInterval);

            // To check whether the operational timespan allows for it to be completed on the same day or not.
            if (normalEnd.Date > availableOperationDay || normalEnd.TimeOfDay > closingTime)
            {
                while (overallOperationsTimeLeft > 0)
                {
                    long remainingTimeSpan = overallOperationsTimeLeft;
                    // Calculating the Operational time left on the same day the operation was initiated, labelled by StartDate
                    TimeSpan end = closingTime - availableOperationDay.TimeOfDay;
                    long initialTimeElapsed = Convert.ToInt64(end.TotalSeconds);
                    overallOperationsTimeLeft -= initialTimeElapsed;

                    if (overallOperationsTimeLeft > 0)
                    {
                        // The supposed next day after the operation had started. This is performed because the expected timespan for the operation has not expired,
                        //      but the operational working time on the first day [startdate] is up - meaning it's passed close of work.
                        availableOperationDay = availableOperationDay.AddDays(1);
                        availableOperationDay = new DateTime(availableOperationDay.Year, availableOperationDay.Month, availableOperationDay.Day, 0, 0, 0);
                        // To determine whether the supposed next day is neither a weekend nor a holiday date
                        availableOperationDay = WorkDateTime(availableOperationDay, workStartTime, workClosingTime, holidayDates);
                    }
                    else
                    {
                        //availableOperationDay = new DateTime(availableOperationDay.Year, availableOperationDay.Month, availableOperationDay.Day, 0, 0, 0);
                        availableOperationDay = availableOperationDay.AddSeconds(remainingTimeSpan);
                    }

                    continue;
                }

                return availableOperationDay;
            }
            // Return the end date when the operational timespan is meant to expire on the same day.
            return normalEnd;
        }
    }
}

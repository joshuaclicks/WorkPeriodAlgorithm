namespace ReverseTimeDetector.Models
{

    /// <summary>
    /// Time Component Class
    /// </summary>
    public class TimeComponent
    {
        /// <summary>
        /// Hour of the day: between 0 and 23 where 0 is 12am and 23 is 11pm
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// Minute of the day: between 0 and 59.
        /// </summary>
        public int Minute { get; set; }
        /// <summary>
        /// Second of the day: between 0 and 59.
        /// </summary>
        public int Second { get; set; }
    }

    public enum WeekDays
    {
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    }
}

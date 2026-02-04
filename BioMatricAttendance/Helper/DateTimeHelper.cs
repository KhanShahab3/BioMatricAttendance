namespace BioMatricAttendance.Helper
{
    public static class DateTimeHelper
    {
        private const int PakistanUtcOffset = 5;

        public static (DateTime StartUtc, DateTime EndUtc) GetUtcRangeForPakistanDate(DateTime? startDatePk, DateTime? endDatePk)
        {
            var todayPk = DateTime.UtcNow.AddHours(PakistanUtcOffset);

            var startPk = (startDatePk ?? todayPk);
            var endPk = (endDatePk ?? todayPk.Date.AddDays(1)).Date; 

            var startUtc = DateTime.SpecifyKind(startPk.Date, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(endPk.Date, DateTimeKind.Utc);
            if (startUtc.Date == endUtc.Date)
            {
               endUtc= endUtc.Date.AddDays(1);
            }

            return (startUtc, endUtc);
        }





    }
}

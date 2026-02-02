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

            var startUtc = DateTime.SpecifyKind(startPk, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(endPk, DateTimeKind.Utc);

            return (startUtc, endUtc);
        }





    }
}

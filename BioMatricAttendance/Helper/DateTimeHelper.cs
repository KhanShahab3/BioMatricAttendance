namespace BioMatricAttendance.Helper
{
    public static class DateTimeHelper
    {
        private const int PakistanUtcOffset = 5;

        public static (DateTime StartUtc, DateTime EndUtc) GetUtcRangeForPakistanDate(DateTime? startDatePk, DateTime? endDatePk)
        {
            var todayPk = DateTime.UtcNow.AddHours(PakistanUtcOffset).Date;

            var startPk = (startDatePk ?? todayPk).Date;
            var endPk = (endDatePk ?? todayPk).Date.AddDays(1); 

            var startUtc = DateTime.SpecifyKind(startPk.AddHours(-PakistanUtcOffset), DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(endPk.AddHours(-PakistanUtcOffset), DateTimeKind.Utc);

            return (startUtc, endUtc);
        }





    }
}

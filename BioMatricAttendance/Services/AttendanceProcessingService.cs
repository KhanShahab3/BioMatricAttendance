using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Repositories;
using BioMatricAttendance.Services;

public class AttendanceProcessingService : IAttendanceProcessingService
{
    private readonly IAttendanceProcessingRepository _attendanceProcessingRepository;

    public AttendanceProcessingService(IAttendanceProcessingRepository attendanceProcessingRepository)
    {
        _attendanceProcessingRepository = attendanceProcessingRepository;
    }

    public async Task<List<AttendanceReportDto>> ProcessFacultyAttendanceAsync(
        DateTime startDate, DateTime endDate, int instituteId)
    {
        return await _attendanceProcessingRepository.ProcessAttendance(startDate, endDate, instituteId);
    }
}
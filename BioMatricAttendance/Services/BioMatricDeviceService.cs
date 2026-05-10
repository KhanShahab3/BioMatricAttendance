using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Services
{
    public class BioMatricDeviceService:IBioMatricDeviceService
    {
        private readonly IBioMatricDeviceRepository _deviceRepository;
        private readonly AppDbContext _context;
        public BioMatricDeviceService(IBioMatricDeviceRepository deviceRepository, AppDbContext context)
        {
            _deviceRepository = deviceRepository;
            _context = context;
        }

        public async Task UnassignDevices(int deviceId)
        {
            if (deviceId == null ||deviceId==0 ) return;

            var devices = await _context.BiomatricDevices
                .Where(d => deviceId==d.Id)
                .ToListAsync();

            foreach (var d in devices)
            {
                d.InstituteId = 0;
                d.isRegistered = false;
            }
           

            await _context.SaveChangesAsync();
        }
        public async Task<BiomatricDevice> CreateDevice(BiomatricDevice device)
        {
            return await _deviceRepository.AddDevice(device);
        }
        public async Task<BiomatricDevice> GetDevice(int id)
        {
            return await _deviceRepository.GetDeviceById(id);
        }
        public async Task<List<GetDeviceNameDto>> GetDevices()
        {
            var devices = await _deviceRepository.GetAllDevices();
            return devices.Select(d => new GetDeviceNameDto
            {
                Id = d.Id,
                DeviceId = d.DeviceId,
                SessionId = d.SessionId,
                isRegistered = d.isRegistered,
                CreatedAt=d.CreatedAt
            }).ToList();
        }
        public async Task<List<GetDeviceNameDto>> GetUnassignDevice()
        {
            var devices = await _deviceRepository.GetUnassignDevices();
            return devices.Select(d => new GetDeviceNameDto
            {
                Id = d.Id,
                DeviceId = d.DeviceId,
                SessionId = d.SessionId,
                isRegistered = d.isRegistered,
                CreatedAt = d.CreatedAt
            }).ToList();
        }
        public async Task<BiomatricDevice> UpdateDevice(BiomatricDevice device)
        {
            return await _deviceRepository.UpdateDevice(device);
        }
        public async Task<bool> RemoveDevice(int id)
        {
            return await _deviceRepository.DeleteDevice(id);
        }
    }
}

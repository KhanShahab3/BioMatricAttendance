using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class BioMatricDeviceService:IBioMatricDeviceService
    {
        private readonly IBioMatricDeviceRepository _deviceRepository;
        public BioMatricDeviceService(IBioMatricDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
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

using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IBioMatricDeviceService
    {
        public Task<BiomatricDevice> CreateDevice(BiomatricDevice device);
        public Task<BiomatricDevice> GetDevice(int id);
        public Task<List<BiomatricDevice>> GetDevices();
        public Task<BiomatricDevice> UpdateDevice(BiomatricDevice device);
        public Task<bool> RemoveDevice(int id);
    }
}

using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IBioMatricDeviceRepository
    {
        public Task<BiomatricDevice> AddDevice(BiomatricDevice device);
        public Task<BiomatricDevice> GetDeviceById(int id);
        public Task<List<BiomatricDevice>> GetAllDevices();
        public Task<BiomatricDevice> UpdateDevice(BiomatricDevice device);
        public Task<bool> DeleteDevice(int id);

    }
}

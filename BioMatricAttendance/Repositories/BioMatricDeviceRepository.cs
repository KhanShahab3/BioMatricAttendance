using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class BioMatricDeviceRepository:IBioMatricDeviceRepository
    {
       private readonly AppDbContext _appContext;
        public BioMatricDeviceRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }
        public async Task<BiomatricDevice> AddDevice(BiomatricDevice device)
        {
            await _appContext.BiomatricDevices.AddAsync(device);
            await _appContext.SaveChangesAsync();
            return device;
        }
        public async Task<BiomatricDevice> GetDeviceById(int id)
        {
            var device = await _appContext.BiomatricDevices.FindAsync(id);
            return device;
        }
        public async Task<List<BiomatricDevice>> GetAllDevices()
        {
            var devices = await _appContext.BiomatricDevices.ToListAsync();
            return devices;
        }
        public async Task<BiomatricDevice> UpdateDevice(BiomatricDevice device)
        {
            var isDevice = await _appContext.BiomatricDevices.FindAsync(device.Id);
            if (isDevice != null)
            {
                isDevice.DeviceName = device.DeviceName;
                isDevice.IPAddress = device.IPAddress;
                isDevice.Location = device.Location;
                await _appContext.SaveChangesAsync();
            }
            return device;
        }
        public async Task<bool> DeleteDevice(int id)
        {
            var isDevice = await _appContext.BiomatricDevices.FindAsync(id);
            if (isDevice != null)
            {
                _appContext.Remove(isDevice);
                await _appContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}

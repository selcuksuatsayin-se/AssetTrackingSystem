using AssetTrackingSystem.Data;
using AssetTrackingSystem.Models;
using AssetTrackingSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AssetTrackingSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AssetTrackingDbContext _context;

        public DashboardService(AssetTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var viewModel = new DashboardViewModel
            {
                // Cihaz sayıları
                TotalComputers = await _context.Computers.CountAsync(),
                TotalDisplayMonitors = await _context.DisplayMonitors.CountAsync(),
                TotalMobilePhones = await _context.MobilePhones.CountAsync(),
                TotalJabraHeadsets = await _context.JabraHeadsets.CountAsync(),
                TotalPrinters = await _context.Printers.CountAsync(),

                // Depo bilgileri
                TotalWarehouseDevices = await GetWarehouseDeviceCountAsync(),

                // Son eklenen cihazlar
                RecentComputers = await GetRecentDevicesByTypeAsync<Computer>("Computer", 3),
                RecentDisplayMonitors = await GetRecentDevicesByTypeAsync<DisplayMonitor>("DisplayMonitor", 3),
                RecentMobilePhones = await GetRecentDevicesByTypeAsync<MobilePhone>("MobilePhone", 3),
                RecentJabraHeadsets = await GetRecentDevicesByTypeAsync<JabraHeadset>("JabraHeadset", 3),
                RecentPrinters = await GetRecentDevicesByTypeAsync<Printer>("Printer", 3),

                // İstatistikler
                FaultyDevicesCount = await GetFaultyDeviceCountAsync(),
                OldDevicesCount = await GetOldDeviceCountAsync(),
                NewDevicesThisMonth = await GetNewDevicesThisMonthAsync(),
                MovedToWarehouseThisMonth = await GetMovedToWarehouseThisMonthAsync()
            };

            // Kullanımda olan cihaz sayısı (toplam - depo - arızalı)
            viewModel.DevicesInUseCount = viewModel.TotalAllDevices -
                                        viewModel.TotalWarehouseDevices -
                                        viewModel.FaultyDevicesCount;

            return viewModel;
        }

        public async Task<int> GetDeviceCountAsync(string deviceType)
        {
            return deviceType.ToLower() switch
            {
                "computer" => await _context.Computers.CountAsync(),
                "displaymonitor" => await _context.DisplayMonitors.CountAsync(),
                "mobilephone" => await _context.MobilePhones.CountAsync(),
                "jabraheadset" => await _context.JabraHeadsets.CountAsync(),
                "printer" => await _context.Printers.CountAsync(),
                _ => 0
            };
        }

        public async Task<int> GetWarehouseDeviceCountAsync()
        {
            var computerCount = await _context.Computers.CountAsync(c => c.IsInWarehouse);
            var monitorCount = await _context.DisplayMonitors.CountAsync(d => d.IsInWarehouse);
            var phoneCount = await _context.MobilePhones.CountAsync(m => m.IsInWarehouse);
            var headsetCount = await _context.JabraHeadsets.CountAsync(j => j.IsInWarehouse);
            var printerCount = await _context.Printers.CountAsync(p => p.IsInWarehouse);

            return computerCount + monitorCount + phoneCount + headsetCount + printerCount;
        }

        public async Task<List<RecentDeviceInfo>> GetRecentDevicesAsync()
        {
            var recentDevices = new List<RecentDeviceInfo>();

            // Her kategoriden son 3 cihazı al
            recentDevices.AddRange(await GetRecentDevicesByTypeAsync<Computer>("Computer", 3));
            recentDevices.AddRange(await GetRecentDevicesByTypeAsync<DisplayMonitor>("DisplayMonitor", 3));
            recentDevices.AddRange(await GetRecentDevicesByTypeAsync<MobilePhone>("MobilePhone", 3));
            recentDevices.AddRange(await GetRecentDevicesByTypeAsync<JabraHeadset>("JabraHeadset", 3));
            recentDevices.AddRange(await GetRecentDevicesByTypeAsync<Printer>("Printer", 3));

            // Tarihe göre sırala ve son 10 tanesini al
            return recentDevices
                .OrderByDescending(d => d.CreatedDate)
                .Take(10)
                .ToList();
        }

        public async Task<int> GetFaultyDeviceCountAsync()
        {
            var computerCount = await _context.Computers.CountAsync(c => c.IsFaulty);
            var monitorCount = await _context.DisplayMonitors.CountAsync(d => d.IsFaulty);
            var phoneCount = await _context.MobilePhones.CountAsync(m => m.IsFaulty);
            var headsetCount = await _context.JabraHeadsets.CountAsync(j => j.IsFaulty);
            var printerCount = await _context.Printers.CountAsync(p => p.IsFaulty);

            return computerCount + monitorCount + phoneCount + headsetCount + printerCount;
        }

        public async Task<int> GetOldDeviceCountAsync()
        {
            var fiveYearsAgo = DateTime.Now.AddYears(-5);

            var computerCount = await _context.Computers.CountAsync(c => c.PurchaseDate.HasValue && c.PurchaseDate <= fiveYearsAgo);
            var monitorCount = await _context.DisplayMonitors.CountAsync(d => d.PurchaseDate.HasValue && d.PurchaseDate <= fiveYearsAgo);
            var phoneCount = await _context.MobilePhones.CountAsync(m => m.PurchaseDate.HasValue && m.PurchaseDate <= fiveYearsAgo);
            var headsetCount = await _context.JabraHeadsets.CountAsync(j => j.PurchaseDate.HasValue && j.PurchaseDate <= fiveYearsAgo);
            var printerCount = await _context.Printers.CountAsync(p => p.PurchaseDate.HasValue && p.PurchaseDate <= fiveYearsAgo);

            return computerCount + monitorCount + phoneCount + headsetCount + printerCount;
        }

        public async Task<int> GetNewDevicesThisMonthAsync()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var computerCount = await _context.Computers.CountAsync(c => c.CreatedDate >= startOfMonth && c.CreatedDate < endOfMonth);
            var monitorCount = await _context.DisplayMonitors.CountAsync(d => d.CreatedDate >= startOfMonth && d.CreatedDate < endOfMonth);
            var phoneCount = await _context.MobilePhones.CountAsync(m => m.CreatedDate >= startOfMonth && m.CreatedDate < endOfMonth);
            var headsetCount = await _context.JabraHeadsets.CountAsync(j => j.CreatedDate >= startOfMonth && j.CreatedDate < endOfMonth);
            var printerCount = await _context.Printers.CountAsync(p => p.CreatedDate >= startOfMonth && p.CreatedDate < endOfMonth);

            return computerCount + monitorCount + phoneCount + headsetCount + printerCount;
        }

        public async Task<int> GetMovedToWarehouseThisMonthAsync()
        {
            // Bu metod için CreatedDate yerine ayrı bir WarehouseMovementDate field'ı olması ideal
            // Şimdilik IsInWarehouse = true olan ve bu ay oluşturulan cihazları sayacağız
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var computerCount = await _context.Computers.CountAsync(c => c.IsInWarehouse && c.CreatedDate >= startOfMonth && c.CreatedDate < endOfMonth);
            var monitorCount = await _context.DisplayMonitors.CountAsync(d => d.IsInWarehouse && d.CreatedDate >= startOfMonth && d.CreatedDate < endOfMonth);
            var phoneCount = await _context.MobilePhones.CountAsync(m => m.IsInWarehouse && m.CreatedDate >= startOfMonth && m.CreatedDate < endOfMonth);
            var headsetCount = await _context.JabraHeadsets.CountAsync(j => j.IsInWarehouse && j.CreatedDate >= startOfMonth && j.CreatedDate < endOfMonth);
            var printerCount = await _context.Printers.CountAsync(p => p.IsInWarehouse && p.CreatedDate >= startOfMonth && p.CreatedDate < endOfMonth);

            return computerCount + monitorCount + phoneCount + headsetCount + printerCount;
        }

        #region Private Helper Methods

        private async Task<List<RecentDeviceInfo>> GetRecentDevicesByTypeAsync<T>(string deviceType, int count) where T : class
        {
            var recentDevices = new List<RecentDeviceInfo>();

            switch (deviceType)
            {
                case "Computer":
                    var computers = await _context.Computers
                        .OrderByDescending(c => c.CreatedDate)
                        .Take(count)
                        .ToListAsync();

                    recentDevices = computers.Select(c => new RecentDeviceInfo
                    {
                        Id = c.Id,
                        DeviceType = "Computer",
                        UserName = c.UserName,
                        UserSurname = c.UserSurname,
                        Model = c.Model,
                        SerialNumber = c.SerialNumber,
                        CreatedDate = c.CreatedDate
                    }).ToList();
                    break;

                case "DisplayMonitor":
                    var monitors = await _context.DisplayMonitors
                        .OrderByDescending(d => d.CreatedDate)
                        .Take(count)
                        .ToListAsync();

                    recentDevices = monitors.Select(d => new RecentDeviceInfo
                    {
                        Id = d.Id,
                        DeviceType = "DisplayMonitor",
                        UserName = d.UserName,
                        UserSurname = d.UserSurname,
                        Model = d.Model,
                        SerialNumber = d.SerialNumber,
                        CreatedDate = d.CreatedDate
                    }).ToList();
                    break;

                case "MobilePhone":
                    var phones = await _context.MobilePhones
                        .OrderByDescending(m => m.CreatedDate)
                        .Take(count)
                        .ToListAsync();

                    recentDevices = phones.Select(m => new RecentDeviceInfo
                    {
                        Id = m.Id,
                        DeviceType = "MobilePhone",
                        UserName = m.UserName,
                        UserSurname = m.UserSurname,
                        Model = m.Model,
                        SerialNumber = m.IMEI, // MobilePhone için IMEI kullanıyoruz
                        CreatedDate = m.CreatedDate
                    }).ToList();
                    break;

                case "JabraHeadset":
                    var headsets = await _context.JabraHeadsets
                        .OrderByDescending(j => j.CreatedDate)
                        .Take(count)
                        .ToListAsync();

                    recentDevices = headsets.Select(j => new RecentDeviceInfo
                    {
                        Id = j.Id,
                        DeviceType = "JabraHeadset",
                        UserName = j.UserName,
                        UserSurname = j.UserSurname,
                        Model = j.Model,
                        SerialNumber = j.SerialNumber,
                        CreatedDate = j.CreatedDate
                    }).ToList();
                    break;

                case "Printer":
                    var printers = await _context.Printers
                        .OrderByDescending(p => p.CreatedDate)
                        .Take(count)
                        .ToListAsync();

                    recentDevices = printers.Select(p => new RecentDeviceInfo
                    {
                        Id = p.Id,
                        DeviceType = "Printer",
                        Location = p.Location, // Printer için Location kullanıyoruz
                        Model = p.Model,
                        SerialNumber = p.SerialNumber,
                        CreatedDate = p.CreatedDate
                    }).ToList();
                    break;
            }

            return recentDevices;
        }

        #endregion
    }
}
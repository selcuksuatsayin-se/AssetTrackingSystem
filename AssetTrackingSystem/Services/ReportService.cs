using AssetTrackingSystem.Data;
using AssetTrackingSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AssetTrackingSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly AssetTrackingDbContext _context;

        public ReportService(AssetTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseReportViewModel> GetWarehouseReportAsync()
        {
            var report = new WarehouseReportViewModel
            {
                ReportTitle = "Depodaki Cihazlar Raporu",
                ReportDescription = "Şu anda depoda bekleyen tüm cihazların listesi",
                GeneratedDate = DateTime.Now,

                WarehouseComputers = await _context.Computers
                    .Where(c => c.IsInWarehouse)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync(),

                WarehouseDisplayMonitors = await _context.DisplayMonitors
                    .Where(d => d.IsInWarehouse)
                    .OrderByDescending(d => d.CreatedDate)
                    .ToListAsync(),

                WarehouseMobilePhones = await _context.MobilePhones
                    .Where(m => m.IsInWarehouse)
                    .OrderByDescending(m => m.CreatedDate)
                    .ToListAsync(),

                WarehouseJabraHeadsets = await _context.JabraHeadsets
                    .Where(j => j.IsInWarehouse)
                    .OrderByDescending(j => j.CreatedDate)
                    .ToListAsync(),

                WarehousePrinters = await _context.Printers
                    .Where(p => p.IsInWarehouse)
                    .OrderByDescending(p => p.CreatedDate)
                    .ToListAsync()
            };

            report.TotalRecords = report.TotalWarehouseDevices;
            return report;
        }

        public async Task<UserDevicesReportViewModel> GetUserDevicesReportAsync(string userName, string userSurname)
        {
            var normalizedName = userName?.Trim().ToLower() ?? string.Empty;
            var normalizedSurname = userSurname?.Trim().ToLower() ?? string.Empty;

            var viewModel = new UserDevicesReportViewModel
            {
                SearchUserName = userName,
                SearchUserSurname = userSurname,
                ReportDescription = $"{userName} {userSurname} kullanıcısına zimmetli tüm cihazlar"
            };

            try
            {
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userSurname))
                {
                    viewModel.ErrorMessage = "En az bir arama kriteri giriniz (ad veya soyad)";
                    return viewModel;
                }

                // Kullanıcı adı ve soyadı kontrolü
                bool hasName = !string.IsNullOrEmpty(normalizedName);
                bool hasSurname = !string.IsNullOrEmpty(normalizedSurname);

                // Computers - NULL kontrolü 
                viewModel.UserComputers = await _context.Computers
                    .Where(c =>
                        (!hasName || (c.UserName != null && c.UserName.Trim().ToLower().Contains(normalizedName))) &&
                        (!hasSurname || (c.UserSurname != null && c.UserSurname.Trim().ToLower().Contains(normalizedSurname)))
                    )
                    .OrderBy(c => c.Model)
                    .ToListAsync();

                // Display Monitors - NULL kontrolü
                viewModel.UserDisplayMonitors = await _context.DisplayMonitors
                    .Where(d =>
                        (!hasName || (d.UserName != null && d.UserName.Trim().ToLower().Contains(normalizedName))) &&
                        (!hasSurname || (d.UserSurname != null && d.UserSurname.Trim().ToLower().Contains(normalizedSurname)))
                    )
                    .OrderBy(d => d.Model)
                    .ToListAsync();

                // Mobile Phones - NULL kontrolü
                viewModel.UserMobilePhones = await _context.MobilePhones
                    .Where(m =>
                        (!hasName || (m.UserName != null && m.UserName.Trim().ToLower().Contains(normalizedName))) &&
                        (!hasSurname || (m.UserSurname != null && m.UserSurname.Trim().ToLower().Contains(normalizedSurname)))
                    )
                    .OrderBy(m => m.Model)
                    .ToListAsync();

                // Jabra Headsets - NULL kontrolü
                viewModel.UserJabraHeadsets = await _context.JabraHeadsets
                    .Where(j =>
                        (!hasName || (j.UserName != null && j.UserName.Trim().ToLower().Contains(normalizedName))) &&
                        (!hasSurname || (j.UserSurname != null && j.UserSurname.Trim().ToLower().Contains(normalizedSurname)))
                    )
                    .OrderBy(j => j.Model)
                    .ToListAsync();

                // İlk bulunan kullanıcının adı alınır
                var firstComputer = viewModel.UserComputers.FirstOrDefault();
                var firstMonitor = viewModel.UserDisplayMonitors.FirstOrDefault();
                var firstPhone = viewModel.UserMobilePhones.FirstOrDefault();
                var firstHeadset = viewModel.UserJabraHeadsets.FirstOrDefault();

                if (firstComputer != null)
                {
                    viewModel.FullUserName = $"{firstComputer.UserName} {firstComputer.UserSurname}";
                }
                else if (firstMonitor != null)
                {
                    viewModel.FullUserName = $"{firstMonitor.UserName} {firstMonitor.UserSurname}";
                }
                else if (firstPhone != null)
                {
                    viewModel.FullUserName = $"{firstPhone.UserName} {firstPhone.UserSurname}";
                }
                else if (firstHeadset != null)
                {
                    viewModel.FullUserName = $"{firstHeadset.UserName} {firstHeadset.UserSurname}";
                }
                else
                {
                    viewModel.ErrorMessage = "Kullanıcı bulunamadı";
                    return viewModel;
                }

                viewModel.TotalUserDevices = viewModel.UserComputers.Count +
                                           viewModel.UserDisplayMonitors.Count +
                                           viewModel.UserMobilePhones.Count +
                                           viewModel.UserJabraHeadsets.Count;

                viewModel.TotalRecords = viewModel.TotalUserDevices;

                return viewModel;
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
                return viewModel;
            }
        }

        public async Task<OldDevicesReportViewModel> GetOldDevicesReportAsync(int thresholdYears = 5)
        {
            var cutoffDate = DateTime.Now.AddYears(-thresholdYears);

            var report = new OldDevicesReportViewModel
            {
                ReportTitle = "Eski Cihazlar",
                ReportDescription = $"{cutoffDate:dd.MM.yyyy} tarihinden önce satın alınan cihazlar ({thresholdYears}+ yaş)",
                GeneratedDate = DateTime.Now,
                ThresholdYears = thresholdYears,

                OldComputers = await _context.Computers
                    .Where(c => c.PurchaseDate.HasValue && c.PurchaseDate <= cutoffDate)
                    .OrderBy(c => c.PurchaseDate)
                    .ToListAsync(),

                OldDisplayMonitors = await _context.DisplayMonitors
                    .Where(d => d.PurchaseDate.HasValue && d.PurchaseDate <= cutoffDate)
                    .OrderBy(d => d.PurchaseDate)
                    .ToListAsync(),

                OldMobilePhones = await _context.MobilePhones
                    .Where(m => m.PurchaseDate.HasValue && m.PurchaseDate <= cutoffDate)
                    .OrderBy(m => m.PurchaseDate)
                    .ToListAsync(),

                OldJabraHeadsets = await _context.JabraHeadsets
                    .Where(j => j.PurchaseDate.HasValue && j.PurchaseDate <= cutoffDate)
                    .OrderBy(j => j.PurchaseDate)
                    .ToListAsync(),

                OldPrinters = await _context.Printers
                    .Where(p => p.PurchaseDate.HasValue && p.PurchaseDate <= cutoffDate)
                    .OrderBy(p => p.PurchaseDate)
                    .ToListAsync()
            };

            report.TotalRecords = report.TotalOldDevices;
            return report;
        }

        public async Task<FaultyDevicesReportViewModel> GetFaultyDevicesReportAsync()
        {
            var report = new FaultyDevicesReportViewModel
            {
                ReportTitle = "Arızalı Cihazlar Raporu",
                ReportDescription = "Arızalı olarak işaretlenmiş tüm cihazların listesi",
                GeneratedDate = DateTime.Now,

                FaultyComputers = await _context.Computers
                    .Where(c => c.IsFaulty)
                    .OrderByDescending(c => c.CreatedDate)
                    .ToListAsync(),

                FaultyDisplayMonitors = await _context.DisplayMonitors
                    .Where(d => d.IsFaulty)
                    .OrderByDescending(d => d.CreatedDate)
                    .ToListAsync(),

                FaultyMobilePhones = await _context.MobilePhones
                    .Where(m => m.IsFaulty)
                    .OrderByDescending(m => m.CreatedDate)
                    .ToListAsync(),

                FaultyJabraHeadsets = await _context.JabraHeadsets
                    .Where(j => j.IsFaulty)
                    .OrderByDescending(j => j.CreatedDate)
                    .ToListAsync(),

                FaultyPrinters = await _context.Printers
                    .Where(p => p.IsFaulty)
                    .OrderByDescending(p => p.CreatedDate)
                    .ToListAsync()
            };

            report.TotalRecords = report.TotalFaultyDevices;
            return report;
        }

        public async Task<UnusedDevicesReportViewModel> GetUnusedDevicesReportAsync(DateTime cutoffDate)
        {
            var report = new UnusedDevicesReportViewModel
            {
                ReportTitle = "Kullanılmayan Cihazlar",
                ReportDescription = $"{cutoffDate:dd.MM.yyyy} tarihinden bu yana kullanılmayan cihazlar",
                GeneratedDate = DateTime.Now,
                CutoffDate = cutoffDate,

                UnusedComputers = await _context.Computers
                    .Where(c => c.LastUsedDate.HasValue && c.LastUsedDate <= cutoffDate)
                    .OrderBy(c => c.LastUsedDate)
                    .ToListAsync(),

                UnusedDisplayMonitors = await _context.DisplayMonitors
                    .Where(d => d.LastUsedDate.HasValue && d.LastUsedDate <= cutoffDate)
                    .OrderBy(d => d.LastUsedDate)
                    .ToListAsync(),

                UnusedMobilePhones = await _context.MobilePhones
                    .Where(m => m.LastUsedDate.HasValue && m.LastUsedDate <= cutoffDate)
                    .OrderBy(m => m.LastUsedDate)
                    .ToListAsync(),

                UnusedJabraHeadsets = await _context.JabraHeadsets
                    .Where(j => j.LastUsedDate.HasValue && j.LastUsedDate <= cutoffDate)
                    .OrderBy(j => j.LastUsedDate)
                    .ToListAsync(),

                UnusedPrinters = await _context.Printers
                    .Where(p => p.LastUsedDate.HasValue && p.LastUsedDate <= cutoffDate)
                    .OrderBy(p => p.LastUsedDate)
                    .ToListAsync()
            };

            report.TotalRecords = report.TotalUnusedDevices;
            return report;
        }

        public async Task<DeviceSearchViewModel> SearchDevicesAsync(DeviceSearchViewModel searchModel)
        {
            var results = new List<DeviceSearchResult>();

            // Arama kriterleri
            var modelSearch = searchModel.ModelSearch?.Trim().ToLower();
            var brandSearch = searchModel.BrandSearch?.Trim().ToLower();
            var serialSearch = searchModel.SerialNumberSearch?.Trim().ToLower();
            var userNameSearch = searchModel.UserNameSearch?.Trim().ToLower();
            var userSurnameSearch = searchModel.UserSurnameSearch?.Trim().ToLower();

            // Bilgisayar arama
            if (searchModel.SearchComputers)
            {
                var computers = await _context.Computers
                    .Where(c => (string.IsNullOrEmpty(modelSearch) || c.Model.ToLower().Contains(modelSearch)) &&
                               (string.IsNullOrEmpty(brandSearch) || c.Model.ToLower().Contains(brandSearch)) &&
                               (string.IsNullOrEmpty(serialSearch) || c.SerialNumber.ToLower().Contains(serialSearch)) &&
                               (string.IsNullOrEmpty(userNameSearch) || c.UserName.ToLower().Contains(userNameSearch)) &&
                               (string.IsNullOrEmpty(userSurnameSearch) || c.UserSurname.ToLower().Contains(userSurnameSearch)) &&
                               (!searchModel.OnlyWarehouseDevices || c.IsInWarehouse) &&
                               (!searchModel.OnlyFaultyDevices || c.IsFaulty) &&
                               (!searchModel.OnlyActiveDevices || (!c.IsInWarehouse && !c.IsFaulty)))
                    .ToListAsync();

                results.AddRange(computers.Select(c => new DeviceSearchResult
                {
                    Id = c.Id,
                    DeviceType = "Computer",
                    Model = c.Model,
                    SerialNumber = c.SerialNumber,
                    UserName = c.UserName,
                    UserSurname = c.UserSurname,
                    IsInWarehouse = c.IsInWarehouse,
                    IsFaulty = c.IsFaulty,
                    FaultNote = c.FaultNote,
                    PurchaseDate = c.PurchaseDate,
                    LastUsedDate = c.LastUsedDate,
                    CreatedDate = c.CreatedDate
                }));
            }

            // Monitör arama
            if (searchModel.SearchDisplayMonitors)
            {
                var monitors = await _context.DisplayMonitors
                    .Where(d => (string.IsNullOrEmpty(modelSearch) || d.Model.ToLower().Contains(modelSearch)) &&
                               (string.IsNullOrEmpty(brandSearch) || d.Model.ToLower().Contains(brandSearch)) &&
                               (string.IsNullOrEmpty(serialSearch) || d.SerialNumber.ToLower().Contains(serialSearch)) &&
                               (string.IsNullOrEmpty(userNameSearch) || d.UserName.ToLower().Contains(userNameSearch)) &&
                               (string.IsNullOrEmpty(userSurnameSearch) || d.UserSurname.ToLower().Contains(userSurnameSearch)) &&
                               (!searchModel.OnlyWarehouseDevices || d.IsInWarehouse) &&
                               (!searchModel.OnlyFaultyDevices || d.IsFaulty) &&
                               (!searchModel.OnlyActiveDevices || (!d.IsInWarehouse && !d.IsFaulty)))
                    .ToListAsync();

                results.AddRange(monitors.Select(d => new DeviceSearchResult
                {
                    Id = d.Id,
                    DeviceType = "DisplayMonitor",
                    Model = d.Model,
                    SerialNumber = d.SerialNumber,
                    UserName = d.UserName,
                    UserSurname = d.UserSurname,
                    IsInWarehouse = d.IsInWarehouse,
                    IsFaulty = d.IsFaulty,
                    FaultNote = d.FaultNote,
                    PurchaseDate = d.PurchaseDate,
                    LastUsedDate = d.LastUsedDate,
                    CreatedDate = d.CreatedDate
                }));
            }

            // Cep telefonu arama
            if (searchModel.SearchMobilePhones)
            {
                var phones = await _context.MobilePhones
                    .Where(m => (string.IsNullOrEmpty(modelSearch) || m.Model.ToLower().Contains(modelSearch)) &&
                               (string.IsNullOrEmpty(brandSearch) || m.Model.ToLower().Contains(brandSearch)) &&
                               (string.IsNullOrEmpty(serialSearch) || m.IMEI.ToLower().Contains(serialSearch)) &&
                               (string.IsNullOrEmpty(userNameSearch) || m.UserName.ToLower().Contains(userNameSearch)) &&
                               (string.IsNullOrEmpty(userSurnameSearch) || m.UserSurname.ToLower().Contains(userSurnameSearch)) &&
                               (!searchModel.OnlyWarehouseDevices || m.IsInWarehouse) &&
                               (!searchModel.OnlyFaultyDevices || m.IsFaulty) &&
                               (!searchModel.OnlyActiveDevices || (!m.IsInWarehouse && !m.IsFaulty)))
                    .ToListAsync();

                results.AddRange(phones.Select(m => new DeviceSearchResult
                {
                    Id = m.Id,
                    DeviceType = "MobilePhone",
                    Model = m.Model,
                    SerialNumber = m.IMEI,
                    UserName = m.UserName,
                    UserSurname = m.UserSurname,
                    IsInWarehouse = m.IsInWarehouse,
                    IsFaulty = m.IsFaulty,
                    FaultNote = m.FaultNote,
                    PurchaseDate = m.PurchaseDate,
                    LastUsedDate = m.LastUsedDate,
                    CreatedDate = m.CreatedDate
                }));
            }

            // Kulaklık arama
            if (searchModel.SearchJabraHeadsets)
            {
                var headsets = await _context.JabraHeadsets
                    .Where(j => (string.IsNullOrEmpty(modelSearch) || j.Model.ToLower().Contains(modelSearch)) &&
                               (string.IsNullOrEmpty(brandSearch) || j.Model.ToLower().Contains(brandSearch)) &&
                               (string.IsNullOrEmpty(serialSearch) || j.SerialNumber.ToLower().Contains(serialSearch)) &&
                               (string.IsNullOrEmpty(userNameSearch) || j.UserName.ToLower().Contains(userNameSearch)) &&
                               (string.IsNullOrEmpty(userSurnameSearch) || j.UserSurname.ToLower().Contains(userSurnameSearch)) &&
                               (!searchModel.OnlyWarehouseDevices || j.IsInWarehouse) &&
                               (!searchModel.OnlyFaultyDevices || j.IsFaulty) &&
                               (!searchModel.OnlyActiveDevices || (!j.IsInWarehouse && !j.IsFaulty)))
                    .ToListAsync();

                results.AddRange(headsets.Select(j => new DeviceSearchResult
                {
                    Id = j.Id,
                    DeviceType = "JabraHeadset",
                    Model = j.Model,
                    SerialNumber = j.SerialNumber,
                    UserName = j.UserName,
                    UserSurname = j.UserSurname,
                    IsInWarehouse = j.IsInWarehouse,
                    IsFaulty = j.IsFaulty,
                    FaultNote = j.FaultNote,
                    PurchaseDate = j.PurchaseDate,
                    LastUsedDate = j.LastUsedDate,
                    CreatedDate = j.CreatedDate
                }));
            }

            // Yazıcı arama
            if (searchModel.SearchPrinters)
            {
                var printers = await _context.Printers
                    .Where(p => (string.IsNullOrEmpty(modelSearch) || p.Model.ToLower().Contains(modelSearch)) &&
                               (string.IsNullOrEmpty(brandSearch) || p.Model.ToLower().Contains(brandSearch)) &&
                               (string.IsNullOrEmpty(serialSearch) || p.SerialNumber.ToLower().Contains(serialSearch)) &&
                               (!searchModel.OnlyWarehouseDevices || p.IsInWarehouse) &&
                               (!searchModel.OnlyFaultyDevices || p.IsFaulty) &&
                               (!searchModel.OnlyActiveDevices || (!p.IsInWarehouse && !p.IsFaulty)))
                    .ToListAsync();

                results.AddRange(printers.Select(p => new DeviceSearchResult
                {
                    Id = p.Id,
                    DeviceType = "Printer",
                    Model = p.Model,
                    SerialNumber = p.SerialNumber,
                    Location = p.Location,
                    IsInWarehouse = p.IsInWarehouse,
                    IsFaulty = p.IsFaulty,
                    FaultNote = p.FaultNote,
                    PurchaseDate = p.PurchaseDate,
                    LastUsedDate = p.LastUsedDate,
                    CreatedDate = p.CreatedDate
                }));
            }

            // Sonuçları sırala
            searchModel.Results.AllResults = results
                .OrderByDescending(r => r.CreatedDate)
                .ToList();

            return searchModel;
        }

        public async Task<WarehouseViewModel> GetWarehouseViewModelAsync(WarehouseFilter filter = null)
        {
            filter ??= new WarehouseFilter();

            var viewModel = new WarehouseViewModel
            {
                Filter = filter,
                Stats = new WarehouseStats()
            };

            // Filtrelenmiş verileri getir
            var computersQuery = _context.Computers.Where(c => c.IsInWarehouse);
            var monitorsQuery = _context.DisplayMonitors.Where(d => d.IsInWarehouse);
            var phonesQuery = _context.MobilePhones.Where(m => m.IsInWarehouse);
            var headsetsQuery = _context.JabraHeadsets.Where(j => j.IsInWarehouse);
            var printersQuery = _context.Printers.Where(p => p.IsInWarehouse);

            // Cihaz tipi filtresi
            if (filter.DeviceType != "All")
            {
                computersQuery = filter.DeviceType == "Computer" ? computersQuery : computersQuery.Where(c => false);
                monitorsQuery = filter.DeviceType == "DisplayMonitor" ? monitorsQuery : monitorsQuery.Where(d => false);
                phonesQuery = filter.DeviceType == "MobilePhone" ? phonesQuery : phonesQuery.Where(m => false);
                headsetsQuery = filter.DeviceType == "JabraHeadset" ? headsetsQuery : headsetsQuery.Where(j => false);
                printersQuery = filter.DeviceType == "Printer" ? printersQuery : printersQuery.Where(p => false);
            }

            // Durum filtresi
            if (filter.Status != "All")
            {
                bool isFaulty = filter.Status == "Faulty";
                computersQuery = computersQuery.Where(c => c.IsFaulty == isFaulty);
                monitorsQuery = monitorsQuery.Where(d => d.IsFaulty == isFaulty);
                phonesQuery = phonesQuery.Where(m => m.IsFaulty == isFaulty);
                headsetsQuery = headsetsQuery.Where(j => j.IsFaulty == isFaulty);
                printersQuery = printersQuery.Where(p => p.IsFaulty == isFaulty);
            }

            // Model filtresi
            if (!string.IsNullOrEmpty(filter.ModelFilter))
            {
                var modelFilter = filter.ModelFilter.ToLower();
                computersQuery = computersQuery.Where(c => c.Model.ToLower().Contains(modelFilter));
                monitorsQuery = monitorsQuery.Where(d => d.Model.ToLower().Contains(modelFilter));
                phonesQuery = phonesQuery.Where(m => m.Model.ToLower().Contains(modelFilter));
                headsetsQuery = headsetsQuery.Where(j => j.Model.ToLower().Contains(modelFilter));
                printersQuery = printersQuery.Where(p => p.Model.ToLower().Contains(modelFilter));
            }

            // Tarih aralığı filtresi
            if (filter.DateFrom.HasValue)
            {
                computersQuery = computersQuery.Where(c => c.CreatedDate >= filter.DateFrom.Value);
                monitorsQuery = monitorsQuery.Where(d => d.CreatedDate >= filter.DateFrom.Value);
                phonesQuery = phonesQuery.Where(m => m.CreatedDate >= filter.DateFrom.Value);
                headsetsQuery = headsetsQuery.Where(j => j.CreatedDate >= filter.DateFrom.Value);
                printersQuery = printersQuery.Where(p => p.CreatedDate >= filter.DateFrom.Value);
            }

            if (filter.DateTo.HasValue)
            {
                var endDate = filter.DateTo.Value.AddDays(1);
                computersQuery = computersQuery.Where(c => c.CreatedDate < endDate);
                monitorsQuery = monitorsQuery.Where(d => d.CreatedDate < endDate);
                phonesQuery = phonesQuery.Where(m => m.CreatedDate < endDate);
                headsetsQuery = headsetsQuery.Where(j => j.CreatedDate < endDate);
                printersQuery = printersQuery.Where(p => p.CreatedDate < endDate);
            }

            // Sıralama
            switch (filter.SortBy)
            {
                case "Model":
                    computersQuery = filter.SortOrder == "asc" ?
                        computersQuery.OrderBy(c => c.Model) :
                        computersQuery.OrderByDescending(c => c.Model);
                    monitorsQuery = filter.SortOrder == "asc" ?
                        monitorsQuery.OrderBy(d => d.Model) :
                        monitorsQuery.OrderByDescending(d => d.Model);
                    phonesQuery = filter.SortOrder == "asc" ?
                        phonesQuery.OrderBy(m => m.Model) :
                        phonesQuery.OrderByDescending(m => m.Model);
                    headsetsQuery = filter.SortOrder == "asc" ?
                        headsetsQuery.OrderBy(j => j.Model) :
                        headsetsQuery.OrderByDescending(j => j.Model);
                    printersQuery = filter.SortOrder == "asc" ?
                        printersQuery.OrderBy(p => p.Model) :
                        printersQuery.OrderByDescending(p => p.Model);
                    break;

                default: // CreatedDate
                    computersQuery = filter.SortOrder == "asc" ?
                        computersQuery.OrderBy(c => c.CreatedDate) :
                        computersQuery.OrderByDescending(c => c.CreatedDate);
                    monitorsQuery = filter.SortOrder == "asc" ?
                        monitorsQuery.OrderBy(d => d.CreatedDate) :
                        monitorsQuery.OrderByDescending(d => d.CreatedDate);
                    phonesQuery = filter.SortOrder == "asc" ?
                        phonesQuery.OrderBy(m => m.CreatedDate) :
                        phonesQuery.OrderByDescending(m => m.CreatedDate);
                    headsetsQuery = filter.SortOrder == "asc" ?
                        headsetsQuery.OrderBy(j => j.CreatedDate) :
                        headsetsQuery.OrderByDescending(j => j.CreatedDate);
                    printersQuery = filter.SortOrder == "asc" ?
                        printersQuery.OrderBy(p => p.CreatedDate) :
                        printersQuery.OrderByDescending(p => p.CreatedDate);
                    break;
            }

            // Verileri getir
            viewModel.WarehouseComputers = await computersQuery.ToListAsync();
            viewModel.WarehouseDisplayMonitors = await monitorsQuery.ToListAsync();
            viewModel.WarehouseMobilePhones = await phonesQuery.ToListAsync();
            viewModel.WarehouseJabraHeadsets = await headsetsQuery.ToListAsync();
            viewModel.WarehousePrinters = await printersQuery.ToListAsync();

            // İstatistikleri hesapla
            viewModel.Stats.ComputerCount = viewModel.WarehouseComputers.Count;
            viewModel.Stats.DisplayMonitorCount = viewModel.WarehouseDisplayMonitors.Count;
            viewModel.Stats.MobilePhoneCount = viewModel.WarehouseMobilePhones.Count;
            viewModel.Stats.JabraHeadsetCount = viewModel.WarehouseJabraHeadsets.Count;
            viewModel.Stats.PrinterCount = viewModel.WarehousePrinters.Count;
            viewModel.Stats.FaultyDevicesInWarehouse = viewModel.WarehouseComputers.Count(c => c.IsFaulty) +
                                                     viewModel.WarehouseDisplayMonitors.Count(d => d.IsFaulty) +
                                                     viewModel.WarehouseMobilePhones.Count(m => m.IsFaulty) +
                                                     viewModel.WarehouseJabraHeadsets.Count(j => j.IsFaulty) +
                                                     viewModel.WarehousePrinters.Count(p => p.IsFaulty);

            // Son hareketleri getir
            viewModel.RecentMovements = await GetRecentWarehouseMovementsAsync(10);

            return viewModel;
        }

        public async Task<bool> ToggleWarehouseStatusAsync(int deviceId, string deviceType, bool moveToWarehouse, string note = null)
        {
            try
            {
                switch (deviceType.ToLower())
                {
                    case "computer":
                        var computer = await _context.Computers.FindAsync(deviceId);
                        if (computer != null)
                        {
                            computer.IsInWarehouse = moveToWarehouse;
                            if (moveToWarehouse) computer.LastUsedDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                            await AddWarehouseMovement(computer.Id, "Computer", computer.Model, computer.SerialNumber,
                                computer.UserName, computer.UserSurname, moveToWarehouse, note);
                            return true;
                        }
                        break;

                    case "displaymonitor":
                        var monitor = await _context.DisplayMonitors.FindAsync(deviceId);
                        if (monitor != null)
                        {
                            monitor.IsInWarehouse = moveToWarehouse;
                            if (moveToWarehouse) monitor.LastUsedDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                            await AddWarehouseMovement(monitor.Id, "DisplayMonitor", monitor.Model, monitor.SerialNumber,
                                monitor.UserName, monitor.UserSurname, moveToWarehouse, note);
                            return true;
                        }
                        break;

                    case "mobilephone":
                        var phone = await _context.MobilePhones.FindAsync(deviceId);
                        if (phone != null)
                        {
                            phone.IsInWarehouse = moveToWarehouse;
                            if (moveToWarehouse) phone.LastUsedDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                            await AddWarehouseMovement(phone.Id, "MobilePhone", phone.Model, phone.IMEI,
                                phone.UserName, phone.UserSurname, moveToWarehouse, note);
                            return true;
                        }
                        break;

                    case "jabraheadset":
                        var headset = await _context.JabraHeadsets.FindAsync(deviceId);
                        if (headset != null)
                        {
                            headset.IsInWarehouse = moveToWarehouse;
                            if (moveToWarehouse) headset.LastUsedDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                            await AddWarehouseMovement(headset.Id, "JabraHeadset", headset.Model, headset.SerialNumber,
                                headset.UserName, headset.UserSurname, moveToWarehouse, note);
                            return true;
                        }
                        break;

                    case "printer":
                        var printer = await _context.Printers.FindAsync(deviceId);
                        if (printer != null)
                        {
                            printer.IsInWarehouse = moveToWarehouse;
                            if (moveToWarehouse) printer.LastUsedDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                            await AddWarehouseMovement(printer.Id, "Printer", printer.Model, printer.SerialNumber,
                                null, null, moveToWarehouse, note, printer.Location);
                            return true;
                        }
                        break;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<WarehouseMovement>> GetRecentWarehouseMovementsAsync(int count = 10)
        {
            var movements = new List<WarehouseMovement>();

            // Son değiştirilen bilgisayarlar
            var recentComputers = await _context.Computers
                .Where(c => c.IsInWarehouse)
                .OrderByDescending(c => c.CreatedDate)
                .Take(count)
                .ToListAsync();

            movements.AddRange(recentComputers.Select(c => new WarehouseMovement
            {
                DeviceId = c.Id,
                DeviceType = "Computer",
                Model = c.Model,
                SerialNumber = c.SerialNumber,
                UserName = c.UserName,
                UserSurname = c.UserSurname,
                MovementDate = c.CreatedDate,
                MovedToWarehouse = true,
                MovementNote = "Depoya gönderildi"
            }));

            // Diğer cihaz türleri için benzer sorgular eklenebilir

            return movements
                .OrderByDescending(m => m.MovementDate)
                .Take(count)
                .ToList();
        }

        private async Task AddWarehouseMovement(int deviceId, string deviceType, string model, string serialNumber,
            string userName, string userSurname, bool movedToWarehouse, string note, string location = null)
        {
            await Task.CompletedTask;
        }

        public async Task<WarrantyReportViewModel> GetWarrantyStatusReportAsync(int warningThresholdMonths = 3)
        {
            var report = new WarrantyReportViewModel
            {
                ReportTitle = "Garanti Durumu",
                ReportDescription = $"Garanti süresi bitmek üzere olan cihazlar ({warningThresholdMonths} ay eşik değeri)",
                GeneratedDate = DateTime.Now,
                WarningThresholdMonths = warningThresholdMonths
            };

            // Tüm cihaz türlerini kontrol edilir ve garanti durumu hesaplanır
            var devices = new List<WarrantyDeviceInfo>();

            // Bilgisayarlar
            var computers = await _context.Computers
                .Where(c => c.PurchaseDate.HasValue)
                .ToListAsync();

            devices.AddRange(computers.Select(c => CreateWarrantyDeviceInfo(c, "Computer")));

            // Monitörler
            var monitors = await _context.DisplayMonitors
                .Where(d => d.PurchaseDate.HasValue)
                .ToListAsync();

            devices.AddRange(monitors.Select(d => CreateWarrantyDeviceInfo(d, "DisplayMonitor")));

            // Telefonlar
            var phones = await _context.MobilePhones
                .Where(m => m.PurchaseDate.HasValue)
                .ToListAsync();

            devices.AddRange(phones.Select(m => CreateWarrantyDeviceInfo(m, "MobilePhone")));

            // Kulaklıklar
            var headsets = await _context.JabraHeadsets
                .Where(j => j.PurchaseDate.HasValue)
                .ToListAsync();

            devices.AddRange(headsets.Select(j => CreateWarrantyDeviceInfo(j, "JabraHeadset")));

            // Yazıcılar
            var printers = await _context.Printers
                .Where(p => p.PurchaseDate.HasValue)
                .ToListAsync();

            devices.AddRange(printers.Select(p => CreateWarrantyDeviceInfo(p, "Printer")));

            // Garanti bitiş tarihine göre sırala ve filtrele
            report.Devices = devices
                .Where(d => d.RemainingDays <= (warningThresholdMonths * 30)) // 3 ay (90 gün) kalanlar
                .OrderBy(d => d.RemainingDays)
                .ToList();

            report.TotalRecords = report.Devices.Count;

            return report;
        }

        private WarrantyDeviceInfo CreateWarrantyDeviceInfo(dynamic device, string deviceType)
        {
            var purchaseDate = (DateTime)device.PurchaseDate;
            var warrantyEndDate = purchaseDate.AddYears(3);
            var remainingDays = (warrantyEndDate - DateTime.Now).Days;

            var statusInfo = remainingDays switch
            {
                < 0 => ("Garanti Süresi Dolmuş", "text-danger"),
                < 30 => ("Garanti Bitmek Üzere", "text-warning"),
                _ => ("Garanti Kapsamında", "text-success")
            };

            string fullUserName;
            if (deviceType == "Printer")
            {
                fullUserName = device.Location ?? "Konum Belirtilmemiş";
            }
            else
            {
                fullUserName = !string.IsNullOrEmpty(device.UserName) && !string.IsNullOrEmpty(device.UserSurname)
                    ? $"{device.UserName} {device.UserSurname}"
                    : "Atanmamış";
            }

            return new WarrantyDeviceInfo
            {
                Id = device.Id,
                DeviceType = deviceType,
                Model = device.Model,
                SerialNumber = deviceType == "MobilePhone" ? device.IMEI : device.SerialNumber,
                UserName = deviceType != "Printer" ? device.UserName : null,
                UserSurname = deviceType != "Printer" ? device.UserSurname : null,
                Location = deviceType == "Printer" ? device.Location : null,
                PurchaseDate = purchaseDate,
                WarrantyEndDate = warrantyEndDate,
                RemainingDays = remainingDays,
                Status = statusInfo.Item1,
                StatusClass = statusInfo.Item2
            };
        }
    }
}
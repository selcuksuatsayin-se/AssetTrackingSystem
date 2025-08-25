using AssetTrackingSystem.Models;
using AssetTrackingSystem.Models.ViewModels;
using AssetTrackingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetTrackingSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // ---- DEPO RAPORU ----
        public async Task<IActionResult> Warehouse()
        {
            var report = await _reportService.GetWarehouseViewModelAsync();
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleWarehouseStatus(int deviceId, string deviceType, bool moveToWarehouse)
        {
            var result = await _reportService.ToggleWarehouseStatusAsync(deviceId, deviceType, moveToWarehouse, "Depo işlemi");
            return Json(new { success = result });
        }

        // ---- KİŞİYE AİT CİHAZLAR ----
        [HttpGet]
        public IActionResult UserDevices()
        {
            // return View(new UserDevicesReportViewModel());
            return View(); // _Layout otomatik olarak kullanılacak
        }

        [HttpPost]
        public async Task<IActionResult> UserDevices(UserDevicesReportViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var report = await _reportService.GetUserDevicesReportAsync(model.SearchUserName, model.SearchUserSurname);
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> UserDevicesSearch([FromBody] UserDevicesSearchRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserName) && string.IsNullOrEmpty(request.UserSurname))
                {
                    return Json(new { success = false, message = "En az bir arama kriteri giriniz (ad veya soyad)" });
                }

                var report = await _reportService.GetUserDevicesReportAsync(request.UserName, request.UserSurname);

                return Json(new
                {
                    success = true,
                    fullUserName = report.FullUserName,
                    totalUserDevices = report.TotalUserDevices,
                    userComputers = report.UserComputers,
                    userDisplayMonitors = report.UserDisplayMonitors,
                    userMobilePhones = report.UserMobilePhones,
                    userJabraHeadsets = report.UserJabraHeadsets
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        public class UserDevicesSearchRequest
        {
            public string UserName { get; set; }
            public string UserSurname { get; set; }
        }

        // ---- ESKİ CİHAZLAR ----
        //[HttpGet]
        //public async Task<IActionResult> OldDevices(int? thresholdYears)
        //{
        //    var threshold = thresholdYears ?? 5; // Varsayılan 5 yıl
        //    var report = await _reportService.GetOldDevicesReportAsync(threshold);
        //    return View(report);
        //}

        // ---- ESKİ CİHAZLAR ----
        // ---- ESKİ CİHAZLAR ----
        [HttpGet]
        public async Task<IActionResult> OldDevices(int? thresholdYears, string deviceType = null)
        {
            var threshold = thresholdYears ?? 5; // Varsayılan 5 yıl
            var report = await _reportService.GetOldDevicesReportAsync(threshold);

            // Cihaz tipi filtresi uygula
            if (!string.IsNullOrEmpty(deviceType))
            {
                switch (deviceType)
                {
                    case "Computer":
                        report.OldDisplayMonitors = new List<DisplayMonitor>();
                        report.OldMobilePhones = new List<MobilePhone>();
                        report.OldJabraHeadsets = new List<JabraHeadset>();
                        report.OldPrinters = new List<Printer>();
                        break;
                    case "DisplayMonitor":
                        report.OldComputers = new List<Computer>();
                        report.OldMobilePhones = new List<MobilePhone>();
                        report.OldJabraHeadsets = new List<JabraHeadset>();
                        report.OldPrinters = new List<Printer>();
                        break;
                    case "MobilePhone":
                        report.OldComputers = new List<Computer>();
                        report.OldDisplayMonitors = new List<DisplayMonitor>();
                        report.OldJabraHeadsets = new List<JabraHeadset>();
                        report.OldPrinters = new List<Printer>();
                        break;
                    case "JabraHeadset":
                        report.OldComputers = new List<Computer>();
                        report.OldDisplayMonitors = new List<DisplayMonitor>();
                        report.OldMobilePhones = new List<MobilePhone>();
                        report.OldPrinters = new List<Printer>();
                        break;
                    case "Printer":
                        report.OldComputers = new List<Computer>();
                        report.OldDisplayMonitors = new List<DisplayMonitor>();
                        report.OldMobilePhones = new List<MobilePhone>();
                        report.OldJabraHeadsets = new List<JabraHeadset>();
                        break;
                }
            }

            return View(report);
        }

        // ---- ARIZALI CİHAZLAR ----
        public async Task<IActionResult> FaultyDevices()
        {
            var report = await _reportService.GetFaultyDevicesReportAsync();
            return View(report);
        }

        // ---- CİHAZ ARAMA ----
        [HttpGet]
        public IActionResult DeviceSearch()
        {
            return View(new DeviceSearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> DeviceSearch(DeviceSearchViewModel model)
        {
            var results = await _reportService.SearchDevicesAsync(model);
            return View(results);
        }

        // ---- KULLANILMAYAN CİHAZLAR ----
        public async Task<IActionResult> UnusedDevices()
        {
            // Varsayılan olarak 6 ay öncesini kullan
            var sixMonthsAgo = DateTime.Now.AddMonths(-3);
            var report = await _reportService.GetUnusedDevicesReportAsync(sixMonthsAgo);
            return View(report);
        }

        [HttpGet]
        public async Task<IActionResult> WarrantyStatus(int? warningThresholdMonths)
        {
            var threshold = warningThresholdMonths ?? 3; // Varsayılan 3 ay
            var report = await _reportService.GetWarrantyStatusReportAsync(threshold);
            return View(report);
        }
    }
}
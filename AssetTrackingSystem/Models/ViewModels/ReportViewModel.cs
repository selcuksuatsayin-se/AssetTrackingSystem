using System.ComponentModel.DataAnnotations;

namespace AssetTrackingSystem.Models.ViewModels
{
    // Base ViewModel for all reports
    public class ReportViewModel
    {
        public string ReportTitle { get; set; }
        public string ReportDescription { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public int TotalRecords { get; set; }
    }

    // Depodaki cihazlar raporu için
    public class WarehouseReportViewModel : ReportViewModel
    {
        public List<Computer> WarehouseComputers { get; set; } = new List<Computer>();
        public List<DisplayMonitor> WarehouseDisplayMonitors { get; set; } = new List<DisplayMonitor>();
        public List<MobilePhone> WarehouseMobilePhones { get; set; } = new List<MobilePhone>();
        public List<JabraHeadset> WarehouseJabraHeadsets { get; set; } = new List<JabraHeadset>();
        public List<Printer> WarehousePrinters { get; set; } = new List<Printer>();

        public int TotalWarehouseDevices => WarehouseComputers.Count + WarehouseDisplayMonitors.Count +
                                          WarehouseMobilePhones.Count + WarehouseJabraHeadsets.Count +
                                          WarehousePrinters.Count;
    }

    // Kişiye ait cihazlar raporu için
    public class UserDevicesReportViewModel
    {
        public string ReportTitle { get; set; } = "Kişiye Ait Cihazlar Raporu";
        public string ReportDescription { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        public string SearchUserName { get; set; }
        public string SearchUserSurname { get; set; }

        // Yeni eklenen property'ler
        public string FullUserName { get; set; }
        public int TotalUserDevices { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalRecords { get; set; }

        public List<Computer> UserComputers { get; set; } = new List<Computer>();
        public List<DisplayMonitor> UserDisplayMonitors { get; set; } = new List<DisplayMonitor>();
        public List<MobilePhone> UserMobilePhones { get; set; } = new List<MobilePhone>();
        public List<JabraHeadset> UserJabraHeadsets { get; set; } = new List<JabraHeadset>();
    }

    // Eski cihazlar raporu için (eşik değeri ile)
    public class OldDevicesReportViewModel : ReportViewModel
    {
        public List<Computer> OldComputers { get; set; } = new List<Computer>();
        public List<DisplayMonitor> OldDisplayMonitors { get; set; } = new List<DisplayMonitor>();
        public List<MobilePhone> OldMobilePhones { get; set; } = new List<MobilePhone>();
        public List<JabraHeadset> OldJabraHeadsets { get; set; } = new List<JabraHeadset>();
        public List<Printer> OldPrinters { get; set; } = new List<Printer>();

        [Display(Name = "Eşik Değeri (Yıl)")]
        [Range(1, 50, ErrorMessage = "1-50 yıl arasında bir değer giriniz")]
        public int ThresholdYears { get; set; } = 5; // Varsayılan 5 yıl

        public DateTime CutoffDate => DateTime.Now.AddYears(-ThresholdYears);
        public int TotalOldDevices => OldComputers.Count + OldDisplayMonitors.Count +
                                    OldMobilePhones.Count + OldJabraHeadsets.Count +
                                    OldPrinters.Count;
    }

    // Arızalı cihazlar raporu için
    public class FaultyDevicesReportViewModel : ReportViewModel
    {
        public List<Computer> FaultyComputers { get; set; } = new List<Computer>();
        public List<DisplayMonitor> FaultyDisplayMonitors { get; set; } = new List<DisplayMonitor>();
        public List<MobilePhone> FaultyMobilePhones { get; set; } = new List<MobilePhone>();
        public List<JabraHeadset> FaultyJabraHeadsets { get; set; } = new List<JabraHeadset>();
        public List<Printer> FaultyPrinters { get; set; } = new List<Printer>();

        public int TotalFaultyDevices => FaultyComputers.Count + FaultyDisplayMonitors.Count +
                                       FaultyMobilePhones.Count + FaultyJabraHeadsets.Count +
                                       FaultyPrinters.Count;
    }



    // Kullanılmayan cihazlar raporu için
    public class UnusedDevicesReportViewModel : ReportViewModel
    {
        [Display(Name = "Tarih Seçin")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Tarih seçimi gereklidir")]
        public DateTime? CutoffDate { get; set; }

        public List<Computer> UnusedComputers { get; set; } = new List<Computer>();
        public List<DisplayMonitor> UnusedDisplayMonitors { get; set; } = new List<DisplayMonitor>();
        public List<MobilePhone> UnusedMobilePhones { get; set; } = new List<MobilePhone>();
        public List<JabraHeadset> UnusedJabraHeadsets { get; set; } = new List<JabraHeadset>();
        public List<Printer> UnusedPrinters { get; set; } = new List<Printer>();

        public int TotalUnusedDevices => UnusedComputers.Count + UnusedDisplayMonitors.Count +
                                       UnusedMobilePhones.Count + UnusedJabraHeadsets.Count +
                                       UnusedPrinters.Count;
    }
}
using System.ComponentModel.DataAnnotations;

namespace AssetTrackingSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        // Cihaz Sayıları
        public int TotalComputers { get; set; }
        public int TotalDisplayMonitors { get; set; }
        public int TotalMobilePhones { get; set; }
        public int TotalJabraHeadsets { get; set; }
        public int TotalPrinters { get; set; }

        // Depo Bilgileri
        public int TotalWarehouseDevices { get; set; }

        // Toplam Cihaz Sayısı
        public int TotalAllDevices => TotalComputers + TotalDisplayMonitors +
                                    TotalMobilePhones + TotalJabraHeadsets + TotalPrinters;

        // Son Eklenen Cihazlar (Her kategoriden en son 3'er)
        public List<RecentDeviceInfo> RecentComputers { get; set; } = new List<RecentDeviceInfo>();
        public List<RecentDeviceInfo> RecentDisplayMonitors { get; set; } = new List<RecentDeviceInfo>();
        public List<RecentDeviceInfo> RecentMobilePhones { get; set; } = new List<RecentDeviceInfo>();
        public List<RecentDeviceInfo> RecentJabraHeadsets { get; set; } = new List<RecentDeviceInfo>();
        public List<RecentDeviceInfo> RecentPrinters { get; set; } = new List<RecentDeviceInfo>();

        // Özet İstatistikler
        public int FaultyDevicesCount { get; set; }
        public int DevicesInUseCount { get; set; }
        public int OldDevicesCount { get; set; } // 5 yıldan eski

        // Bu ayın istatistikleri (opsiyonel)
        public int NewDevicesThisMonth { get; set; }
        public int MovedToWarehouseThisMonth { get; set; }
    }

    // Son eklenen cihazlar için helper class
    public class RecentDeviceInfo
    {
        public int Id { get; set; }
        public string DeviceType { get; set; } // "Computer", "Monitor", etc.
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public DateTime CreatedDate { get; set; }

        // Display için
        public string FullUserName => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserSurname)
            ? $"{UserName} {UserSurname}" : "Atanmamış";

        public string DisplayText => $"{Model} - {FullUserName}";

        // Printer için location bilgisi
        public string Location { get; set; }
        public string DisplayLocation => !string.IsNullOrEmpty(Location) ? Location : "Konum Belirtilmemiş";
    }
}
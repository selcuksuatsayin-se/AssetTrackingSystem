using System.ComponentModel.DataAnnotations;

namespace AssetTrackingSystem.Models.ViewModels
{
    public class WarehouseViewModel
    {
        // Depodaki Cihazlar - Kategorilere Göre
        public List<Computer> WarehouseComputers { get; set; } = new List<Computer>();
        public List<DisplayMonitor> WarehouseDisplayMonitors { get; set; } = new List<DisplayMonitor>();
        public List<MobilePhone> WarehouseMobilePhones { get; set; } = new List<MobilePhone>();
        public List<JabraHeadset> WarehouseJabraHeadsets { get; set; } = new List<JabraHeadset>();
        public List<Printer> WarehousePrinters { get; set; } = new List<Printer>();

        // İstatistikler
        public WarehouseStats Stats { get; set; } = new WarehouseStats();

        // Filtreleme ve Sıralama
        public WarehouseFilter Filter { get; set; } = new WarehouseFilter();

        // Son İşlemler (Depoya giren/çıkan son cihazlar)
        public List<WarehouseMovement> RecentMovements { get; set; } = new List<WarehouseMovement>();

        // Depo özeti
        public bool HasDevicesInWarehouse => Stats.TotalDevices > 0;
        public string LastUpdateText => $"Son güncelleme: {DateTime.Now:dd.MM.yyyy HH:mm}";
    }

    // Depo istatistikleri
    public class WarehouseStats
    {
        public int ComputerCount { get; set; }
        public int DisplayMonitorCount { get; set; }
        public int MobilePhoneCount { get; set; }
        public int JabraHeadsetCount { get; set; }
        public int PrinterCount { get; set; }

        public int TotalDevices => ComputerCount + DisplayMonitorCount +
                                 MobilePhoneCount + JabraHeadsetCount + PrinterCount;

        public int FaultyDevicesInWarehouse { get; set; }
        public int WorkingDevicesInWarehouse => TotalDevices - FaultyDevicesInWarehouse;

        // Kategori dağılımı yüzdesi
        public double ComputerPercentage => TotalDevices > 0 ? (double)ComputerCount / TotalDevices * 100 : 0;
        public double DisplayMonitorPercentage => TotalDevices > 0 ? (double)DisplayMonitorCount / TotalDevices * 100 : 0;
        public double MobilePhonePercentage => TotalDevices > 0 ? (double)MobilePhoneCount / TotalDevices * 100 : 0;
        public double JabraHeadsetPercentage => TotalDevices > 0 ? (double)JabraHeadsetCount / TotalDevices * 100 : 0;
        public double PrinterPercentage => TotalDevices > 0 ? (double)PrinterCount / TotalDevices * 100 : 0;
    }

    // Depo filtreleme seçenekleri
    public class WarehouseFilter
    {
        [Display(Name = "Cihaz Tipi")]
        public string DeviceType { get; set; } = "All"; // All, Computer, DisplayMonitor, etc.

        [Display(Name = "Durum")]
        public string Status { get; set; } = "All"; // All, Faulty, Working

        [Display(Name = "Model")]
        public string ModelFilter { get; set; }

        [Display(Name = "Depoya Giriş Tarihi - Başlangıç")]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Depoya Giriş Tarihi - Bitiş")]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [Display(Name = "Sıralama")]
        public string SortBy { get; set; } = "CreatedDate"; // CreatedDate, Model, DeviceType

        [Display(Name = "Sıralama Yönü")]
        public string SortOrder { get; set; } = "desc"; // asc, desc

        // Filtreleme yapılıp yapılmadığını kontrol et
        public bool IsFiltered => DeviceType != "All" ||
                                Status != "All" ||
                                !string.IsNullOrEmpty(ModelFilter) ||
                                DateFrom.HasValue ||
                                DateTo.HasValue;

        // Filtreyi temizle
        public void ClearFilter()
        {
            DeviceType = "All";
            Status = "All";
            ModelFilter = string.Empty;
            DateFrom = null;
            DateTo = null;
            SortBy = "CreatedDate";
            SortOrder = "desc";
        }
    }

    // Depo hareketleri (giren/çıkan cihazlar) için
    public class WarehouseMovement
    {
        public int DeviceId { get; set; }
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Location { get; set; } // Printer için
        public DateTime MovementDate { get; set; }
        public bool MovedToWarehouse { get; set; } // true = depoya gitti, false = depodan çıktı
        public string MovementNote { get; set; }

        // Display Properties
        public string FullUserName => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserSurname)
            ? $"{UserName} {UserSurname}" : (!string.IsNullOrEmpty(Location) ? Location : "Atanmamış");

        public string DeviceTypeDisplayName => DeviceType switch
        {
            "Computer" => "Bilgisayar",
            "DisplayMonitor" => "Monitör",
            "MobilePhone" => "Cep Telefonu",
            "JabraHeadset" => "Kulaklık",
            "Printer" => "Yazıcı",
            _ => DeviceType
        };

        public string MovementText => MovedToWarehouse ? "Depoya Gönderildi" : "Depodan Çıkarıldı";
        public string MovementClass => MovedToWarehouse ? "text-warning" : "text-success";
        public string MovementIcon => MovedToWarehouse ? "bi-box-arrow-in-down" : "bi-box-arrow-up";

        public string DisplayText => $"{DeviceTypeDisplayName} - {Model} ({FullUserName}) {MovementText}";
        public string TimeAgo => GetTimeAgo(MovementDate);

        private string GetTimeAgo(DateTime date)
        {
            var timeSpan = DateTime.Now - date;

            return timeSpan.TotalDays switch
            {
                >= 365 => $"{(int)(timeSpan.TotalDays / 365)} yıl önce",
                >= 30 => $"{(int)(timeSpan.TotalDays / 30)} ay önce",
                >= 7 => $"{(int)(timeSpan.TotalDays / 7)} hafta önce",
                >= 1 => $"{(int)timeSpan.TotalDays} gün önce",
                _ => timeSpan.TotalHours >= 1
                    ? $"{(int)timeSpan.TotalHours} saat önce"
                    : $"{(int)timeSpan.TotalMinutes} dakika önce"
            };
        }
    }

    // Depo operasyonları için request model
    public class WarehouseActionRequest
    {
        public int DeviceId { get; set; }
        public string DeviceType { get; set; }
        public bool MoveToWarehouse { get; set; } // true = depoya gönder, false = depodan çıkar
        public string Note { get; set; }
        public string ReturnUrl { get; set; } // İşlem sonrası dönülecek sayfa
    }
}
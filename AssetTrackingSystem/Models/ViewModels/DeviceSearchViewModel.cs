using System.ComponentModel.DataAnnotations;

namespace AssetTrackingSystem.Models.ViewModels
{
    public class DeviceSearchViewModel
    {
        // Arama Kriterleri
        [Display(Name = "Model Adı")]
        public string ModelSearch { get; set; }

        [Display(Name = "Marka")]
        public string BrandSearch { get; set; }

        [Display(Name = "Seri Numarası")]
        public string SerialNumberSearch { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserNameSearch { get; set; }

        [Display(Name = "Kullanıcı Soyadı")]
        public string UserSurnameSearch { get; set; }

        // Cihaz Tipi Filtreleri
        [Display(Name = "Bilgisayar")]
        public bool SearchComputers { get; set; } = true;

        [Display(Name = "Monitör")]
        public bool SearchDisplayMonitors { get; set; } = true;

        [Display(Name = "Cep Telefonu")]
        public bool SearchMobilePhones { get; set; } = true;

        [Display(Name = "Kulaklık")]
        public bool SearchJabraHeadsets { get; set; } = true;

        [Display(Name = "Yazıcı")]
        public bool SearchPrinters { get; set; } = true;

        // Durum Filtreleri
        [Display(Name = "Sadece Depodaki Cihazlar")]
        public bool OnlyWarehouseDevices { get; set; } = false;

        [Display(Name = "Sadece Arızalı Cihazlar")]
        public bool OnlyFaultyDevices { get; set; } = false;

        [Display(Name = "Sadece Aktif Cihazlar")]
        public bool OnlyActiveDevices { get; set; } = false;

        // Arama Sonuçları
        public SearchResults Results { get; set; } = new SearchResults();

        // Arama yapılıp yapılmadığını kontrol et
        public bool IsSearchPerformed => !string.IsNullOrEmpty(ModelSearch) ||
                                       !string.IsNullOrEmpty(BrandSearch) ||
                                       !string.IsNullOrEmpty(SerialNumberSearch) ||
                                       !string.IsNullOrEmpty(UserNameSearch) ||
                                       !string.IsNullOrEmpty(UserSurnameSearch);

        // Arama kriterlerini temizle
        public void ClearSearch()
        {
            ModelSearch = string.Empty;
            BrandSearch = string.Empty;
            SerialNumberSearch = string.Empty;
            UserNameSearch = string.Empty;
            UserSurnameSearch = string.Empty;
            OnlyWarehouseDevices = false;
            OnlyFaultyDevices = false;
            OnlyActiveDevices = false;
            SearchComputers = SearchDisplayMonitors = SearchMobilePhones =
            SearchJabraHeadsets = SearchPrinters = true;
            Results = new SearchResults();
        }
    }

    // Arama sonuçları için helper class
    public class SearchResults
    {
        public List<DeviceSearchResult> AllResults { get; set; } = new List<DeviceSearchResult>();

        // Kategorilere göre sonuçlar
        public List<DeviceSearchResult> ComputerResults =>
            AllResults.Where(r => r.DeviceType == "Computer").ToList();

        public List<DeviceSearchResult> DisplayMonitorResults =>
            AllResults.Where(r => r.DeviceType == "DisplayMonitor").ToList();

        public List<DeviceSearchResult> MobilePhoneResults =>
            AllResults.Where(r => r.DeviceType == "MobilePhone").ToList();

        public List<DeviceSearchResult> JabraHeadsetResults =>
            AllResults.Where(r => r.DeviceType == "JabraHeadset").ToList();

        public List<DeviceSearchResult> PrinterResults =>
            AllResults.Where(r => r.DeviceType == "Printer").ToList();

        public int TotalResults => AllResults.Count;
        public bool HasResults => AllResults.Any();
    }

    // Arama sonucu için tek bir cihaz bilgisi
    public class DeviceSearchResult
    {
        public int Id { get; set; }
        public string DeviceType { get; set; } // "Computer", "DisplayMonitor", etc.
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Location { get; set; } // Printer için
        public bool IsInWarehouse { get; set; }
        public bool IsFaulty { get; set; }
        public string FaultNote { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        // Display Properties
        public string FullUserName => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserSurname)
            ? $"{UserName} {UserSurname}" : "Atanmamış";

        public string DeviceTypeDisplayName => DeviceType switch
        {
            "Computer" => "Bilgisayar",
            "DisplayMonitor" => "Monitör",
            "MobilePhone" => "Cep Telefonu",
            "JabraHeadset" => "Kulaklık",
            "Printer" => "Yazıcı",
            _ => DeviceType
        };

        public string StatusText
        {
            get
            {
                var statuses = new List<string>();
                if (IsInWarehouse) statuses.Add("Depoda");
                if (IsFaulty) statuses.Add("Arızalı");
                if (!IsInWarehouse && !IsFaulty) statuses.Add("Aktif");
                return string.Join(", ", statuses);
            }
        }

        public string StatusClass => IsFaulty ? "text-danger" :
                                   IsInWarehouse ? "text-warning" : "text-success";

        // View için edit/details link'leri
        public string ControllerName => DeviceType switch
        {
            "Computer" => "Computer",
            "DisplayMonitor" => "DisplayMonitor",
            "MobilePhone" => "MobilePhone",
            "JabraHeadset" => "JabraHeadset",
            "Printer" => "Printer",
            _ => "Home"
        };
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetTrackingSystem.Models.ViewModels
{
    public class WarrantyReportViewModel : ReportViewModel
    {
        public List<WarrantyDeviceInfo> Devices { get; set; } = new List<WarrantyDeviceInfo>();

        [Display(Name = "Uyarı Eşiği (Ay)")]
        [Range(1, 36, ErrorMessage = "1-36 ay arasında bir değer giriniz")]
        public int WarningThresholdMonths { get; set; } = 3;
    }

    public class WarrantyDeviceInfo
    {
        public int Id { get; set; }
        public string DeviceType { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Location { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public int RemainingDays { get; set; }
        public string Status { get; set; }
        public string StatusClass { get; set; }

        // Display Properties
        public string FullUserName => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserSurname)
                  ? $"{UserName} {UserSurname}"
                  : (!string.IsNullOrEmpty(Location) ? Location : "Atanmamış");

        public string DeviceTypeDisplayName => DeviceType switch
        {
            "Computer" => "Bilgisayar",
            "DisplayMonitor" => "Monitör",
            "MobilePhone" => "Cep Telefonu",
            "JabraHeadset" => "Kulaklık",
            "Printer" => "Yazıcı",
            _ => DeviceType
        };
    }
}
using System.ComponentModel.DataAnnotations;

namespace AssetTrackingSystem.Models
{
    public class Printer
    {
        public int Id { get; set; }

        [Display(Name = "Konum")]
        public string Location { get; set; }

        [Display(Name = "Host Adı")]
        public string HostName { get; set; }

        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "IP Adresi")]
        public string IP { get; set; }

        [Display(Name = "Seri No")]
        public string SerialNumber { get; set; }

        [Display(Name = "Not")]
        public string Note { get; set; }

        // Tarih alanları
        [Display(Name = "Satın Alma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Fatura Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? InvoiceStartDate { get; set; }

        [Display(Name = "Fatura Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? InvoiceEndDate { get; set; }

        [Display(Name = "Son Kullanım Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? LastUsedDate { get; set; }

        // Depo durumu
        [Display(Name = "Depoda mı?")]
        public bool IsInWarehouse { get; set; } = false;

        // Arıza bilgileri
        [Display(Name = "Arıza Durumu")]
        [Required]
        public bool IsFaulty { get; set; } = false;

        [Display(Name = "Arıza Notu")]
        public string FaultNote { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
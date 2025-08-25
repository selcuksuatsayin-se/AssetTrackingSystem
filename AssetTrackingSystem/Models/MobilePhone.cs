using System.ComponentModel.DataAnnotations;

namespace AssetTrackingSystem.Models
{
    public class MobilePhone
    {
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Kullanıcı Soyadı")]
        public string UserSurname { get; set; }

        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "IMEI")]
        public string IMEI { get; set; }

        [Display(Name = "Hafıza")]
        public string Memory { get; set; }

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
using AssetTrackingSystem.Models.ViewModels;

namespace AssetTrackingSystem.Services
{
    /// <summary>
    /// Raporlama ve arama işlemleri için servis arayüzü
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Depodaki tüm cihazları getirir
        /// </summary>
        /// <returns>Depo raporu view modeli</returns>
        Task<WarehouseReportViewModel> GetWarehouseReportAsync();

        /// <summary>
        /// Belirli bir kullanıcıya ait tüm cihazları getirir
        /// </summary>
        /// <param name="userName">Kullanıcı adı (büyük/küçük harf duyarsız)</param>
        /// <param name="userSurname">Kullanıcı soyadı (büyük/küçük harf duyarsız)</param>
        /// <returns>Kullanıcı cihazları raporu</returns>
        Task<UserDevicesReportViewModel> GetUserDevicesReportAsync(string userName, string userSurname);

        /// <summary>
        /// Belirtilen eşik değerinden (yıl) daha eski cihazları getirir
        /// </summary>
        /// <param name="thresholdYears">Eşik değeri (yıl), 1–50 arası, , varsayılan: 5</param>
        /// <returns>Eski cihazlar raporu</returns>
        Task<OldDevicesReportViewModel> GetOldDevicesReportAsync(int thresholdYears = 5);

        /// <summary>
        /// Arızalı olarak işaretlenmiş tüm cihazları getirir
        /// </summary>
        /// <returns>Arızalı cihazlar raporu</returns>
        Task<FaultyDevicesReportViewModel> GetFaultyDevicesReportAsync();

        /// <summary>
        /// Belirli bir tarihten sonra kullanılmayan cihazları getirir
        /// </summary>
        /// <param name="cutoffDate">Kesim tarihi (bu tarihten sonra kullanılmayanlar)</param>
        /// <returns>Kullanılmayan cihazlar raporu</returns>
        Task<UnusedDevicesReportViewModel> GetUnusedDevicesReportAsync(DateTime cutoffDate);

        /// <summary>
        /// Çoklu kriterlere göre cihaz arama işlemi yapar
        /// </summary>
        /// <param name="searchModel">Arama kriterleri ve filtreler</param>
        /// <returns>Arama sonuçları</returns>
        Task<DeviceSearchViewModel> SearchDevicesAsync(DeviceSearchViewModel searchModel);

        /// <summary>
        /// Depo yönetimi için filtreli verileri getirir
        /// </summary>
        /// <param name="filter">Filtreleme ve sıralama kriterleri</param>
        /// <returns>Depo view model</returns>
        Task<WarehouseViewModel> GetWarehouseViewModelAsync(WarehouseFilter filter = null);

        /// <summary>
        /// Cihazın depo durumunu değiştirir (depoya gönder/depodan çıkar)
        /// </summary>
        /// <param name="deviceId">Cihaz ID</param>
        /// <param name="deviceType">Cihaz türü</param>
        /// <param name="moveToWarehouse">true: depoya gönder, false: depodan çıkar</param>
        /// <param name="note">İşlem notu</param>
        /// <returns>İşlem başarı durumu</returns>
        Task<bool> ToggleWarehouseStatusAsync(int deviceId, string deviceType, bool moveToWarehouse, string note = null);

        /// <summary>
        /// Son depo hareketlerini getirir
        /// </summary>
        /// <param name="count">Getirilecek kayıt sayısı, varsayılan: 10</param>
        /// <returns>Son hareketler listesi</returns>
        Task<List<WarehouseMovement>> GetRecentWarehouseMovementsAsync(int count = 10);

        /// <summary>
        /// Garanti durumu raporu getirir
        /// </summary>
        /// <param name="warningThresholdMonths">Uyarı eşiği (ay), varsayılan: 3</param>
        /// <returns>Garanti durum raporu</returns>
        Task<WarrantyReportViewModel> GetWarrantyStatusReportAsync(int warningThresholdMonths = 3);
    }
}
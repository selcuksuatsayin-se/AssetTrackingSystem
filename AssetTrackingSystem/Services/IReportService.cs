using AssetTrackingSystem.Models.ViewModels;

namespace AssetTrackingSystem.Services
{
    public interface IReportService
    {
        /// <summary>
        /// Depodaki tüm cihazları getirir
        /// </summary>
        /// <returns>Depo raporu</returns>
        Task<WarehouseReportViewModel> GetWarehouseReportAsync();

        /// <summary>
        /// Belirli bir kullanıcıya ait tüm cihazları getirir
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="userSurname">Kullanıcı soyadı</param>
        /// <returns>Kullanıcı cihazları raporu</returns>
        Task<UserDevicesReportViewModel> GetUserDevicesReportAsync(string userName, string userSurname);

        /// <summary>
        /// Belirtilen eşik değerinden (yıl) daha eski cihazları getirir.
        /// Varsayılan değer 5 yıldır.
        /// </summary>
        /// <param name="thresholdYears">Eşik değeri (yıl), 1–50 arası</param>
        /// <returns>Eski cihazlar raporu</returns>
        Task<OldDevicesReportViewModel> GetOldDevicesReportAsync(int thresholdYears = 5);

        /// <summary>
        /// Arızalı cihazları getirir
        /// </summary>
        /// <returns>Arızalı cihazlar raporu</returns>
        Task<FaultyDevicesReportViewModel> GetFaultyDevicesReportAsync();

        /// <summary>
        /// Belirli bir tarihten sonra kullanılmayan cihazları getirir
        /// </summary>
        /// <param name="cutoffDate">Kesim tarihi</param>
        /// <returns>Kullanılmayan cihazlar raporu</returns>
        Task<UnusedDevicesReportViewModel> GetUnusedDevicesReportAsync(DateTime cutoffDate);

        /// <summary>
        /// Cihaz arama işlemi yapar
        /// </summary>
        /// <param name="searchModel">Arama kriterleri</param>
        /// <returns>Arama sonuçları</returns>
        Task<DeviceSearchViewModel> SearchDevicesAsync(DeviceSearchViewModel searchModel);

        /// <summary>
        /// Depo operasyonları için WarehouseViewModel getirir
        /// </summary>
        /// <param name="filter">Filtreleme kriterleri</param>
        /// <returns>Depo view model</returns>
        Task<WarehouseViewModel> GetWarehouseViewModelAsync(WarehouseFilter filter = null);

        /// <summary>
        /// Cihazı depoya gönderir veya depodan çıkarır
        /// </summary>
        /// <param name="deviceId">Cihaz ID</param>
        /// <param name="deviceType">Cihaz türü</param>
        /// <param name="moveToWarehouse">true: depoya gönder, false: depodan çıkar</param>
        /// <param name="note">İşlem notu</param>
        /// <returns>İşlem başarılı mı</returns>
        Task<bool> ToggleWarehouseStatusAsync(int deviceId, string deviceType, bool moveToWarehouse, string note = null);

        /// <summary>
        /// Son depo hareketlerini getirir
        /// </summary>
        /// <param name="count">Getirilecek kayıt sayısı</param>
        /// <returns>Son hareketler listesi</returns>
        Task<List<WarehouseMovement>> GetRecentWarehouseMovementsAsync(int count = 10);

        /// <summary>
        /// Garanti durumu raporu getirir
        /// </summary>
        /// <param name="warningThresholdMonths">Uyarı eşiği (ay)</param>
        /// <returns>Garanti durum raporu</returns>
        Task<WarrantyReportViewModel> GetWarrantyStatusReportAsync(int warningThresholdMonths = 3);

    }
}
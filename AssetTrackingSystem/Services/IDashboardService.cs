using AssetTrackingSystem.Models.ViewModels;

namespace AssetTrackingSystem.Services
{
    /// <summary>
    /// Dashboard için istatistiksel veri sağlayan servis arayüzü
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Dashboard için tüm istatistikleri getirir
        /// </summary>
        /// <returns>Dashboard ViewModel</returns>
        Task<DashboardViewModel> GetDashboardDataAsync();

        /// <summary>
        /// Belirli bir cihaz türünün sayısını getirir
        /// </summary>
        /// <param name="deviceType">Cihaz türü (computer, displaymonitor, mobilephone, jabraheadset, printer)</param>
        /// <returns>Cihaz sayısı</returns>
        Task<int> GetDeviceCountAsync(string deviceType);

        /// <summary>
        /// Depodaki toplam cihaz sayısını getirir
        /// </summary>
        /// <returns>Depo cihaz sayısı</returns>
        Task<int> GetWarehouseDeviceCountAsync();

        /// <summary>
        /// Son eklenen cihazları getirir (her kategoriden 3'er adet)
        /// </summary>
        /// <returns>Son eklenen cihazlar listesi</returns>
        Task<List<RecentDeviceInfo>> GetRecentDevicesAsync();

        /// <summary>
        /// Arızalı olarak işaretlenmiş cihaz sayısını getirir
        /// </summary>
        /// <returns>Arızalı cihaz sayısı</returns>
        Task<int> GetFaultyDeviceCountAsync();

        /// <summary>
        /// 5 yıldan eski cihaz sayısını getirir
        /// </summary>
        /// <returns>Eski cihaz sayısı</returns>
        Task<int> GetOldDeviceCountAsync();

        /// <summary>
        /// Bu ay eklenen cihaz sayısını getirir
        /// </summary>
        /// <returns>Bu ay eklenen cihaz sayısı</returns>
        Task<int> GetNewDevicesThisMonthAsync();

        /// <summary>
        /// Bu ay depoya gönderilen cihaz sayısını getirir
        /// </summary>
        /// <returns>Bu ay depoya gönderilen cihaz sayısı</returns>
        Task<int> GetMovedToWarehouseThisMonthAsync();
    }
}
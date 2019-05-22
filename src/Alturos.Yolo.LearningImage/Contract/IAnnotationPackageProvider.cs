using Alturos.Yolo.LearningImage.Model;
using System.Threading.Tasks;

namespace Alturos.Yolo.LearningImage.Contract
{
    public interface IAnnotationPackageProvider
    {
        Task SetAnnotationConfigAsync(AnnotationConfig config);
        Task<AnnotationConfig> GetAnnotationConfigAsync();

        Task<AnnotationPackage[]> GetPackagesAsync(bool annotated);
        Task<AnnotationPackage[]> GetPackagesAsync(AnnotationPackageTag[] tags);

        Task<AnnotationPackage> RefreshPackageAsync(AnnotationPackage package);
        Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package);
        Task UploadPackageAsync(string packagePath);

        bool IsSyncing { get; set; }
        Task SyncPackagesAsync(AnnotationPackage[] packages);
        double GetSyncProgress();
    }
}

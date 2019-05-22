using Alturos.Yolo.LearningImage.Model;
using System.Threading.Tasks;

namespace Alturos.Yolo.LearningImage.Contract
{
    public interface IAnnotationPackageProvider
    {
        bool IsSyncing { get; set; }

        Task SetAnnotationConfig(AnnotationConfig config);
        Task<AnnotationConfig> GetAnnotationConfig();
        Task<AnnotationPackage[]> GetPackagesAsync(bool annotated);
        Task<AnnotationPackage> RefreshPackageAsync(AnnotationPackage package);
        Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package);
        Task UploadPackageAsync(string packagePath);
        Task SyncPackagesAsync(AnnotationPackage[] packages);
        double GetSyncProgress();
    }
}

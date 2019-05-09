using Alturos.Yolo.LearningImage.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alturos.Yolo.LearningImage.Contract
{
    public interface IAnnotationPackageProvider
    {
        bool IsSyncing { get; set; }

        AnnotationPackage[] GetPackages();
        Task<AnnotationPackage> RefreshPackageAsync(AnnotationPackage package);
        Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package);
        Task SyncPackagesAsync(AnnotationPackage[] packages);
        double GetSyncProgress();
    }
}

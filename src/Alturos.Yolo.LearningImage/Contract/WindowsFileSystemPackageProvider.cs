using Alturos.Yolo.LearningImage.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alturos.Yolo.LearningImage.Contract
{
    public class WindowsFileSystemPackageProvider : IAnnotationPackageProvider
    {
        public bool IsSyncing { get; set; }

        public AnnotationPackage[] GetPackages()
        {
            using (var openFolderDialog = new CommonOpenFileDialog())
            {
                openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                openFolderDialog.IsFolderPicker = true;

                if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var path = openFolderDialog.FileName;
                    var directories = Directory.GetDirectories(path).ToList();
                    directories.Add(path);

                    var packages = new List<AnnotationPackage>();
                    foreach (var directory in directories)
                    {
                        if (Directory.GetFiles(directory).Length == 0)
                        {
                            continue;
                        }

                        packages.Add(new AnnotationPackage
                        {
                            Extracted = true,
                            PackagePath = directory,
                            DisplayName = Path.GetFileNameWithoutExtension(directory),
                            Info = new AnnotationPackageInfo()
                        });
                    }

                    return packages.ToArray();
                }

                return new AnnotationPackage[0];
            }
        }

        public AnnotationPackage RefreshPackage(AnnotationPackage package)
        {
            return package;
        }

        public AnnotationPackage DownloadPackage(AnnotationPackage package)
        {
            return package;
        }

        public Task SyncPackages(AnnotationPackage[] packages)
        {
            throw new NotImplementedException();
        }

        public double GetSyncProgress()
        {
            throw new NotImplementedException();
        }
    }
}

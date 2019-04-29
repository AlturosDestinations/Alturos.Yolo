using Alturos.Yolo.LearningImage.Model;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage.CustomControls
{
    public partial class TagListControl : UserControl
    {
        public TagListControl()
        {
            this.InitializeComponent();
        }

        public void LoadFromExcel(AnnotationPackage[] packages)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Excel files|*.xlsx;*.xls;*.csv";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var indexTagDict = new Dictionary<int, string>();
            
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = File.Open(fileDialog.FileName, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[0];

                    // Create tags
                    var firstRow = table.Rows[0];
                    for (var i = 1; i < firstRow.ItemArray.Length; i++)
                    {
                        var tag = firstRow.ItemArray[i].ToString();
                        if (!string.IsNullOrEmpty(tag))
                        {
                            indexTagDict[i] = tag;
                        }
                    }

                    // Apply tags
                    foreach (DataRow row in table.Rows)
                    {
                        var package = packages.FirstOrDefault(o => o.DisplayName == Path.GetFileNameWithoutExtension(row[0].ToString()));
                        if (package == null)
                        {
                            continue;
                        }

                        foreach (var kvp in indexTagDict)
                        {
                            var tag = row[kvp.Key].ToString();
                            if (!string.IsNullOrEmpty(tag))
                            {
                                package.Info.GetType().GetProperty(kvp.Value)?.SetValue(package.Info, tag);
                            }
                        }
                    }
                }
            }
        }

        public void SetTags(AnnotationPackageInfo info)
        {
            if (info == null)
            {
                return;
            }
            var properties = info.GetType().GetProperties();
            this.dataGridView1.DataSource = properties.Select(o => new { Key = o.Name, Value = o.GetValue(o) });
        }
    }
}

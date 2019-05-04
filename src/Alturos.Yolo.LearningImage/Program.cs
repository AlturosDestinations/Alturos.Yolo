using Alturos.Yolo.LearningImage.Contract;
using SimpleInjector;
using System;
using System.Windows.Forms;

namespace Alturos.Yolo.LearningImage
{
    static class Program
    {
        private static Container _container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Bootstrap();
            Application.Run(_container.GetInstance<Main>());
        }

        private static void Bootstrap()
        {
            _container = new Container();
            _container.Register<IBoundingBoxReader, YoloBoundingBoxReader>(Lifestyle.Singleton);
            _container.Verify();
        }
    }
}

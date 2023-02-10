using System.Windows;
using SixLabors.ImageSharp;

namespace Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string PicName = @"output.bmp";
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var image = Core.GenerateImage())
            {
                image.SaveAsBmp(PicName);
            }
        }
    }
}

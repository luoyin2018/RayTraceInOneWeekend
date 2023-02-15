using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.Combine(Environment.CurrentDirectory, App.PicName));
            bitmap.EndInit();

            image.Source = bitmap;
        }
    }
}

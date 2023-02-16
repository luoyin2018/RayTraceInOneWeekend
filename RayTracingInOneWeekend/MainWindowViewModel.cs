using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Windows.Media.Imaging;
using RayTracingInOneWeekend;
using SixLabors.ImageSharp;
using System.Reflection;

namespace Viewer
{
    class ImageData
    {
        public string Name { get; }
        public BitmapImage Image { get; }

        public ImageData(string name, BitmapImage image)
        {
            Name = name;
            Image = image;
        }
    }

    class MainWindowViewModel : INotifyPropertyChanged
    {
        private static readonly string ImageFolder = "images";

        public ObservableCollection<ImageData> ImageCollection { get; } = new ObservableCollection<ImageData>();

        private ImageData _image;
        public ImageData SelectedImage
        {
            get => _image;
            set
            {
                _image = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task GenerateAllPicture()
        {
            if (!Directory.Exists(ImageFolder))
            {
                Directory.CreateDirectory(ImageFolder);
            }

            Assembly asm = Assembly.GetExecutingAssembly();

            var generatorInterface = typeof(IImageGenerator);
            var generatorTypes = asm.GetTypes()
                .Where(t => t.IsClass && generatorInterface.IsAssignableFrom(t))
                .OrderBy(t => t.Name);

            foreach (var gType in generatorTypes)
            {
                var imageName = @$"{ImageFolder}\{gType.Name}.jpg";
                await Task.Run(() =>
                {
                    var imageGenerator = Activator.CreateInstance(gType) as IImageGenerator;
                    using (var image = imageGenerator.GenerateImage())
                    {
                        image.SaveAsJpeg(imageName);
                    }
                });

                var imagePath = Path.Combine(Environment.CurrentDirectory, imageName);
                if (File.Exists(imagePath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.EndInit();

                    ImageData imageData = new ImageData(imageName, bitmap);
                    ImageCollection.Add(imageData);
                    SelectedImage = imageData;
                }
            }
        }
    }
}

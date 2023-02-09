﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Core.Run();
            InitializeComponent();

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.Combine(Environment.CurrentDirectory, Core.picname));
            bitmap.EndInit();

            image.Source = bitmap;
        }
    }
}

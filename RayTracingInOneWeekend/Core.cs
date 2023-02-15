﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Viewer.BookCode;

namespace Viewer
{
    public static class Core
    {
        public static Image<Rgba32> GenerateImage()
        {
            return Chapter7_2_RefineEffect.GenerateImage();
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GeneticAlgo.Shared;

namespace GeneticAlgo.GraphicalInterface.Tools
{
    public class PixelDrawer : IPixelDrawer
    {
        private int Size => _fieldSize * _scaleSize;
        private int ArraySize => Size * Size * 4;

        private readonly int _fieldSize;
        private readonly int _scaleSize;

        private readonly WriteableBitmap _writableBitmap;

        public PixelDrawer(Image image, int fieldSize, int scaleSize)
        {
            _fieldSize = fieldSize;
            _scaleSize = scaleSize;
            image.Height = Size;
            image.Width = Size;

            _writableBitmap = new WriteableBitmap(
                Size,
                Size,
                12,
                12,
                PixelFormats.Bgr32,
                null);

            image.Source = _writableBitmap;
        }

        public void DrawPoints((int X, int Y)[] points)
        {
            var pixels = new byte[ArraySize];
            PrintBackgroundWithBlack(pixels);

            foreach (var point in points)
                PutCell(pixels, point);
            
            PrintPixels(pixels);
        }

        private void PutCell(byte[] pixels, (int X, int Y) points)
        {
            for (var addX = 0; addX < _scaleSize; addX++)
                for (var addY = 0; addY < _scaleSize; addY++)
                    PutPixel(pixels,
                        points.X * _scaleSize + addX,
                        points.Y * _scaleSize + addY);
        }

        private void PutPixel(byte[] pixels, int positionX, int positionY)
        {
            Color color = Colors.Blue;
            var arrayPosition = (positionY * Size + positionX) * 4;

            pixels[arrayPosition] = color.B;
            pixels[++arrayPosition] = color.G;
            pixels[++arrayPosition] = color.R;

        }

        private void PrintBackgroundWithBlack(byte[] pixels)
        {
            var size = ArraySize;

            for (var i = 3; i < size; i += 4)
                pixels[i] = 255;
            
        }

        private void PrintPixels(byte[] pixels)
        {
            var rect = new Int32Rect(0, 0, Size, Size);
            int stride = 4 * Size;

            _writableBitmap.WritePixels(rect, pixels, stride, 0);
        }
    }
}
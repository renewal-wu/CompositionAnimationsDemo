using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace CompositionAnimationsSample.DemoItems
{
    public class DemoColorItems
    {
        public List<ColorItem> Items { get; set; }

        public DemoColorItems()
        {
            Items = new List<ColorItem>();
            for (int i = 0; i < 10; i++)
            {
                Items.Add(new ColorItem(i));
            }
        }
    }

    public class ColorItem
    {
        public SolidColorBrush Foreground { get; set; }
        public SolidColorBrush Background { get; set; }
        public string Content { get; set; }

        public ColorItem(int index)
        {
            Content = $"DemoIndex {index}";

            if (index % 3 == 0)
            {
                Foreground = GenerateTransparentSolidColorBrush();
                Background = GenerateTransparentSolidColorBrush();
            }
            else
            {
                Foreground = GenerateRandomSolidColorBrush(index);
                Background = GenerateRandomSolidColorBrush(index);
            }
        }

        private SolidColorBrush GenerateTransparentSolidColorBrush()
        {
            return new SolidColorBrush(Colors.Transparent);
        }

        private SolidColorBrush GenerateRandomSolidColorBrush(int index)
        {
            var random = new Random(index);
            var r = (byte)random.Next(0, 255);
            var g = (byte)random.Next(0, 255);
            var b = (byte)random.Next(0, 255);

            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }
    }
}

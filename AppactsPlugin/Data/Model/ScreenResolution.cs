using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AppactsPlugin.Data.Model
{
    public class ScreenResolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public ScreenResolution(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace textimage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        BitmapImage bi = null;
        //DrawingVisual dv = new DrawingVisual();

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == false) return;
            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(ofd.FileName);
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();

            imgShow.Source = bi;

        }

        private void imgShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (bi == null) return;
            var img = imgShow.Source;

            var text = "abcdefg";
            var tsiz = 36;

            // 设计要绘制的文本图像
            var fmtText = new FormattedText(
                text,
                new System.Globalization.CultureInfo("zh-cn"),
                FlowDirection.LeftToRight,
                new Typeface("SimSun"),
                tsiz,
                Brushes.Red);

            var centx = fmtText.Width / 2;
            var centy = fmtText.Height / 2;

            // 获取点击座标并转换为图像上的真实值 (即绘制座标)
            Point p = e.GetPosition(imgShow);
            double DrawX = p.X * bi.PixelWidth / imgShow.ActualWidth - centx;
            double DrawY = p.Y * bi.PixelHeight / imgShow.ActualHeight - centy;

           var dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
            dc.DrawImage(imgShow.Source, new Rect(0, 0, bi.PixelWidth, bi.PixelHeight));
            dc.DrawText(fmtText, new Point(DrawX, DrawY));
            }

            var bmp = new RenderTargetBitmap(bi.PixelWidth, bi.PixelHeight, 0, 0, PixelFormats.Default);
            bmp.Render(dv);

            imgShow.Source = bmp;
        }

        private void wdMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double h = grid.ActualHeight - 20;
            double w = grid.ActualWidth - 20;

            double imgw = h * 7 / 10;
            //w = w - imgw - 2;

            if (imgw < 1) imgw = 1;
            if (h < 1) h = 1;

            bdrImage.Height = h;
            bdrImage.Width = imgw;
        }
    }
}

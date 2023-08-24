using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CanvasTest
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

        public bool IsDraging
        {
            get { return (bool)GetValue(IsDragingProperty); }
            set { SetValue(IsDragingProperty, value); }
        }
        public static readonly DependencyProperty IsDragingProperty =
            DependencyProperty.Register("IsDraging", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        Point lastPoint = default;
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDraging = true;
            (sender as Rectangle)?.CaptureMouse();
            lastPoint = e.GetPosition(canvas);
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDraging = false;
            (sender as Rectangle)?.ReleaseMouseCapture();
            Point nowPoint = e.GetPosition(canvas);
            int index_X = (int)nowPoint.X / 50;
            int index_Y = (int)nowPoint.Y / 50;

            Point target = new Point(index_X * 50, index_Y * 50);

            Canvas.SetLeft(sender as Rectangle, target.X);
            Canvas.SetTop(sender as Rectangle, target.Y);
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                Panel.SetZIndex(canvas.Children[i], i);
            }

            Panel.SetZIndex(sender as Rectangle, canvas.Children.Count);
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || !IsDraging)
            {
                return;
            }

            var nowPoint = e.GetPosition(canvas);
            var rect = (sender as Rectangle);
            Panel.SetZIndex(rect, 999);
            Point offset = new Point(nowPoint.X - lastPoint.X, nowPoint.Y - lastPoint.Y);

            Canvas.SetLeft(rect, Canvas.GetLeft(rect) + offset.X);
            Canvas.SetTop(rect, Canvas.GetTop(rect) + offset.Y);
            lastPoint = nowPoint;
        }
    }
}

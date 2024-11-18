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
using ZividCapture.App.Events;

namespace ZividCapture.App.Views
{
    /// <summary>
    /// FrameViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FrameViewer : UserControl
    {
        public FrameViewer(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.GetEvent<FrameSavedEvent>().Subscribe((paths) =>
            {
                rgb.Source = new BitmapImage(new Uri(paths.RGBImgPath));
                depth.Source = new BitmapImage(new Uri(paths.DepthImgPath));
                normals.Source = new BitmapImage(new Uri(paths.NormalsImgPath));
            });
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in dc.DockItems)
                item.Show(dc);
        }
    }
}

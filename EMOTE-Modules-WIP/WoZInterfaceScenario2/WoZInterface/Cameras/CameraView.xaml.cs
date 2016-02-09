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

using WebcamControl;

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for CameraView.xaml
    /// </summary>
    public partial class CameraView : UserControl
    {

        public CameraManager Manager { get { return MyCamControl; } }
        private CameraManager MyCamControl = null;

        public CameraView()
        {
            InitializeComponent();
            MyCamControl = new CameraManager(1, holder);
        }
        public CameraView(CameraManager camControl)
        {
            InitializeComponent();
            this.AdjustSize();

            MyCamControl = camControl;
        }

        void AdjustSize()
        {
            this.Height = double.NaN;
            this.Width = double.NaN;
        }

        public void SetupCamera()
        {
        }

        public void ShowSettings(object sender, MouseButtonEventArgs e)
        {
            MyCamControl.ChangeViewTo(VIEWTYPE.SETTINGS);
            MyCamControl.StopRecording();
            MyCamControl.StopCapture();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

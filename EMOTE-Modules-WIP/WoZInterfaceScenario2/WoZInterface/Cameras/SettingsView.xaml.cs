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

using Microsoft.Expression.Encoder.Devices;

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public Binding mVideoBinding;
        public Binding mAudioBinding;

        private CameraManager MyCamControl = null;

        public SettingsView()
        {
            InitializeComponent();
        }
        public SettingsView(CameraManager camControl)
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

        public void FindDevices()
        {
            var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            var audDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);
            
            foreach (EncoderDevice dvc in vidDevices)
            {
                VideoDevices.Items.Add(dvc.Name);
            }

            foreach (EncoderDevice dvc in audDevices)
            {
                AudioDevices.Items.Add(dvc.Name);
            }
            
            VideoDevices.SelectedIndex = 0;
            AudioDevices.SelectedIndex = 0;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyCamControl.ChangeViewTo(VIEWTYPE.CAMERA);
        }    
    }


}

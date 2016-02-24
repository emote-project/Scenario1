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

using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for GazeControl.xaml
    /// </summary>
    public partial class GazeControl : UserControl
    {
        public WOZManager mWOZManager;

        public GlanceDirection mGlanceDirection = GlanceDirection.PARTICIPANT2TABLE;
        public GazeDirection mGazeDirection = GazeDirection.PARTICIPANT;
        public PointDirection mPointDirection = PointDirection.TOOLS;

        public GazeControl(WOZManager wozManager)
        {
            mWOZManager = wozManager;

            InitializeComponent();
        }


        private void UpdateGlance(object sender, RoutedEventArgs e)
        {
            if (GlanceARB.IsChecked == true)
                mGlanceDirection = GlanceDirection.PARTICIPANT2TABLE;
            else
                mGlanceDirection = GlanceDirection.TABLE2PARTICIPANT;
        }

        private void UpdateGaze(object sender, RoutedEventArgs e)
        {
            if (GazeARB.IsChecked == true)
                mGazeDirection = GazeDirection.PARTICIPANT;
            else if (GazeBRB.IsChecked == true)
                mGazeDirection = GazeDirection.CLICKS;
            else if (GazeCRB.IsChecked == true)
                mGazeDirection = GazeDirection.RANDOM;
            else
                mGazeDirection = GazeDirection.SYMBOL;
        }

        private void UpdatePoint(object sender, RoutedEventArgs e)
        {
            if (PointARB.IsChecked == true)
                mPointDirection = PointDirection.TOOLS;
            else
                mPointDirection = PointDirection.SYMBOL;
        }

        
        private void OverideGaze(object sender, RoutedEventArgs e)
        {
            int i = 0;
        }

        private void OveridePoint(object sender, RoutedEventArgs e)
        {
            int i = 0;
        }

        private void OverideGlance(object sender, RoutedEventArgs e)
        {

        }

        
    }
}

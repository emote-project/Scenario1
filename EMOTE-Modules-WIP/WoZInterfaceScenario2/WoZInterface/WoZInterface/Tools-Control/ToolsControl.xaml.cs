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

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for ToolsControl.xaml
    /// </summary>
    public partial class ToolsControl : UserControl
    {
        public WOZManager mWOZManager;

        public ToolsControl(WOZManager wozManager)
        {
            mWOZManager = wozManager;
            InitializeComponent();
        }

        private void DistanceOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.DistanceShow();
        }

        private void DistanceClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.DistanceHide();
        }

        private void DirectionOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.CompassShow();
        }

        private void DirectionClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.CompassHide();
        }

        private void SymbolOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.MapKeyShow();
        }
        
        private void SymbolClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.MapKeyHide();
        }

        private void ScrapBookOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.ToolAction("scrapBook", "show");
        }

        private void ScrapBookClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.ToolAction("scrapBook", "hide");
        }

        
    }
}

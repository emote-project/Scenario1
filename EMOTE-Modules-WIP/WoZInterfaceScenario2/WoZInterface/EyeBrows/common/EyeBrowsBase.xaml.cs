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

namespace WOZInterface.EyeBrows.common
{
    public class EyeBrowsViewModel : ViewModelBase
    {
        private int MULTIPLIER = 10;
        private double outerBrowL = 5;
        public double OuterBrowL
        {
            get
            {
                return outerBrowL;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < -1) value = -1;
                outerBrowL = value * MULTIPLIER;
                NotifyPropertyChanged("OuterBrowL");
            }
        }
        private double innerBrowL = 5;
        public double InnerBrowL
        {
            get
            {
                return innerBrowL;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < -1) value = -1;
                innerBrowL = value * MULTIPLIER;
                NotifyPropertyChanged("InnerBrowL");
                NotifyPropertyChanged("MidPositionL");
            }
        }
        public double MidPositionL
        {
            get
            {
                return (innerBrowL - 5) / 2;
            }
        }
        private double outerBrowR = 5;
        public double OuterBrowR
        {
            get
            {
                return outerBrowR;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < -1) value = -1;
                outerBrowR = value * MULTIPLIER;
                NotifyPropertyChanged("OuterBrowR");
            }
        }
        private double innerBrowR = 5;
        public double InnerBrowR
        {
            get
            {
                return innerBrowR;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < -1) value = -1;
                innerBrowR = value * MULTIPLIER;
                NotifyPropertyChanged("InnerBrowR");
                NotifyPropertyChanged("MidPositionR");
            }
        }
        public double MidPositionR
        {
            get
            {
                return (innerBrowR - 5) / 2;
            }
        }
    }

    public partial class EyeBrowsBase : UserControl
    {
        public EyeBrowsViewModel BrowsDefinition
        {
            get
            {
                return this.DataContext as EyeBrowsViewModel;
            }
        }

        public EyeBrowsBase()
        {
            InitializeComponent();
        }
    }
}
